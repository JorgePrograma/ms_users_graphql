using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using msusersgraphql.Models.Dtos;
using msusersgraphql.Repositories.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace msusersgraphql.Tests.Repositories.User
{
    [TestClass]
    public class UserRepositoryTests
    {
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private UserRepository _userRepository;
        private StringWriter _consoleOutput;
        private TextWriter _originalConsoleOut;

        [TestInitialize]
        public void Setup()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://api.test.com/")
            };
            
            _mockHttpClientFactory
                .Setup(x => x.CreateClient("UsersAPI"))
                .Returns(_httpClient);
            
            _userRepository = new UserRepository(_mockHttpClientFactory.Object);

            // Capturar salida de Console para verificar logs
            _consoleOutput = new StringWriter();
            _originalConsoleOut = Console.Out;
            Console.SetOut(_consoleOutput);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _httpClient?.Dispose();
            Console.SetOut(_originalConsoleOut);
            _consoleOutput?.Dispose();
        }

        #region GetUserByIdAsync Tests

        [TestMethod]
        public async Task GetUserByIdAsync_WithValidId_ShouldReturnUser()
        {
            // Arrange
            var userId = "123";
            var expectedUser = new UserDto
            {
                Id = userId,
                IdUserIDCS = "idcs123",
                UserName = "testuser",
                State = "Active",
                CreationDate = DateTime.UtcNow,
                AvatarPath = "/avatars/user123.jpg",
                Roles = new List<UserRoleDto>
                {
                    new UserRoleDto { Id = "1", Name = "Admin" }
                }
            };

            var jsonResponse = JsonSerializer.Serialize(expectedUser);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => 
                        req.Method == HttpMethod.Get && 
                        req.RequestUri.ToString().Contains($"user/{userId}")
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _userRepository.GetUserByIdAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUser.Id, result.Id);
            Assert.AreEqual(expectedUser.IdUserIDCS, result.IdUserIDCS);
            Assert.AreEqual(expectedUser.UserName, result.UserName);
            Assert.AreEqual(expectedUser.State, result.State);
            Assert.AreEqual(expectedUser.AvatarPath, result.AvatarPath);
            Assert.AreEqual(1, result.Roles.Count);
            Assert.AreEqual("Admin", result.Roles.First().Name);
        }

        [TestMethod]
        public async Task GetUserByIdAsync_WithNullResponse_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var userId = "notfound";

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => 
                        req.Method == HttpMethod.Get && 
                        req.RequestUri.ToString().Contains($"user/{userId}")
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("null", Encoding.UTF8, "application/json")
                });

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => _userRepository.GetUserByIdAsync(userId));
            
            Assert.IsTrue(exception.Message.Contains($"User with id '{userId}' was not found."));
        }

        [TestMethod]
        public async Task GetUserByIdAsync_WithHttpRequestException_ShouldThrowAndLogException()
        {
            // Arrange
            var userId = "error";

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act & Assert
            await Assert.ThrowsExceptionAsync<HttpRequestException>(
                () => _userRepository.GetUserByIdAsync(userId));

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains("Error in GetUserByIdAsync: Network error"));
        }

        [TestMethod]
        public async Task GetUserByIdAsync_WithJsonException_ShouldThrowAndLogException()
        {
            // Arrange
            var userId = "invalidjson";
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{ invalid json }", Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<JsonException>(
                () => _userRepository.GetUserByIdAsync(userId));

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains("Error in GetUserByIdAsync:"));
        }

        [TestMethod]
        public async Task GetUserByIdAsync_ShouldCallCorrectUrl()
        {
            // Arrange
            var userId = "url-test";
            var expectedUser = new UserDto { Id = userId, UserName = "test" };

            var jsonResponse = JsonSerializer.Serialize(expectedUser);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            };

            HttpRequestMessage capturedRequest = null;
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Callback<HttpRequestMessage, CancellationToken>((request, token) =>
                {
                    capturedRequest = request;
                })
                .ReturnsAsync(httpResponseMessage);

            // Act
            await _userRepository.GetUserByIdAsync(userId);

            // Assert
            Assert.IsNotNull(capturedRequest);
            Assert.AreEqual(HttpMethod.Get, capturedRequest.Method);
            Assert.IsTrue(capturedRequest.RequestUri.ToString().Contains($"user/{userId}"));
        }

        #endregion

        #region GetUsersAsync Tests

        [TestMethod]
        public async Task GetUsersAsync_WithDefaultParameters_ShouldReturnUserListAndLog()
        {
            // Arrange
            var users = new List<UserDto>
            {
                new UserDto { Id = "1", UserName = "user1", State = "Active" },
                new UserDto { Id = "2", UserName = "user2", State = "Inactive" }
            };

            var userDataDto = new UserDataDto
            {
                Items = users,
                TotalCount = 2,
                PageNumber = 1,
                PageSize = 10
            };

            var apiResponse = new ApiResponseDto<UserDataDto>
            {
                Data = userDataDto,
                StatusCode = 200,
                Errors = new List<string>()
            };

            var jsonResponse = JsonSerializer.Serialize(apiResponse);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => 
                        req.Method == HttpMethod.Get && 
                        req.RequestUri.ToString().Contains("user/get-by-filter?pageNumber=1&pageSize=10")
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _userRepository.GetUsersAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Data.Count);
            Assert.AreEqual(2, result.TotalCount);
            Assert.AreEqual(1, result.PageNumber);
            Assert.AreEqual(10, result.PageSize);
            Assert.AreEqual("user1", result.Data.First().UserName);

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains("Calling URL:"));
            Assert.IsTrue(consoleOutput.Contains("Response Status: OK"));
            Assert.IsTrue(consoleOutput.Contains("Response Content:"));
        }

        [TestMethod]
        public async Task GetUsersAsync_WithCustomParameters_ShouldReturnUserListAndLog()
        {
            // Arrange
            var pageNumber = 2;
            var pageSize = 5;
            var users = new List<UserDto>
            {
                new UserDto { Id = "3", UserName = "user3" }
            };

            var userDataDto = new UserDataDto
            {
                Items = users,
                TotalCount = 15,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var apiResponse = new ApiResponseDto<UserDataDto>
            {
                Data = userDataDto,
                StatusCode = 200
            };

            var jsonResponse = JsonSerializer.Serialize(apiResponse);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _userRepository.GetUsersAsync(pageNumber, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Data.Count);
            Assert.AreEqual(15, result.TotalCount);
            Assert.AreEqual(pageNumber, result.PageNumber);
            Assert.AreEqual(pageSize, result.PageSize);

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains($"user/get-by-filter?pageNumber={pageNumber}&pageSize={pageSize}"));
        }

        [TestMethod]
        public async Task GetUsersAsync_WithNullData_ShouldThrowInvalidOperationExceptionAndLog()
        {
            // Arrange
            var apiResponse = new ApiResponseDto<UserDataDto>
            {
                Data = null,
                StatusCode = 200
            };

            var jsonResponse = JsonSerializer.Serialize(apiResponse);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => _userRepository.GetUsersAsync());
            
            Assert.IsTrue(exception.Message.Contains("Failed to deserialize response or no users found."));

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains("Error in GetUsersAsync:"));
        }

        [TestMethod]
        public async Task GetUsersAsync_WithNullApiResponse_ShouldThrowInvalidOperationExceptionAndLog()
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("null", Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => _userRepository.GetUsersAsync());

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains("Error in GetUsersAsync:"));
        }

        [TestMethod]
        public async Task GetUsersAsync_WithHttpErrorStatus_ShouldThrowHttpRequestExceptionAndLog()
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("Internal Server Error", Encoding.UTF8, "text/plain"),
                ReasonPhrase = "Internal Server Error"
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<HttpRequestException>(
                () => _userRepository.GetUsersAsync());
            
            Assert.IsTrue(exception.Message.Contains("HTTP Error: InternalServerError"));

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains("Response Status: InternalServerError"));
            Assert.IsTrue(consoleOutput.Contains("Error in GetUsersAsync:"));
        }

        [TestMethod]
        public async Task GetUsersAsync_WithHttpClientException_ShouldThrowAndLogException()
        {
            // Arrange
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act & Assert
            await Assert.ThrowsExceptionAsync<HttpRequestException>(
                () => _userRepository.GetUsersAsync());

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains("Error in GetUsersAsync: Network error"));
        }

        [TestMethod]
        public async Task GetUsersAsync_ShouldCallCorrectUrlAndLog()
        {
            // Arrange
            var pageNumber = 3;
            var pageSize = 15;
            var expectedUrl = $"user/get-by-filter?pageNumber={pageNumber}&pageSize={pageSize}";
            
            var userDataDto = new UserDataDto
            {
                Items = new List<UserDto>(),
                TotalCount = 0,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var apiResponse = new ApiResponseDto<UserDataDto>
            {
                Data = userDataDto,
                StatusCode = 200
            };

            HttpRequestMessage capturedRequest = null;
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Callback<HttpRequestMessage, CancellationToken>((request, token) =>
                {
                    capturedRequest = request;
                })
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(apiResponse), Encoding.UTF8, "application/json")
                });

            // Act
            await _userRepository.GetUsersAsync(pageNumber, pageSize);

            // Assert
            Assert.IsNotNull(capturedRequest);
            Assert.AreEqual(HttpMethod.Get, capturedRequest.Method);
            Assert.IsTrue(capturedRequest.RequestUri.ToString().Contains(expectedUrl));

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains(expectedUrl));
        }

        [TestMethod]
        public async Task GetUsersAsync_ShouldLogResponseContentTruncated()
        {
            // Arrange
            var longContent = new string('x', 300); // Contenido más largo que 200 caracteres
            var userDataDto = new UserDataDto
            {
                Items = new List<UserDto> { new UserDto { Id = "1", UserName = longContent } },
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 10
            };

            var apiResponse = new ApiResponseDto<UserDataDto>
            {
                Data = userDataDto,
                StatusCode = 200
            };

            var jsonResponse = JsonSerializer.Serialize(apiResponse);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage);

            // Act
            await _userRepository.GetUsersAsync();

            // Assert
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains("Response Content:"));
            Assert.IsTrue(consoleOutput.Contains("...")); // Verificar que se trunca
        }

        #endregion

        #region Constructor Tests

        [TestMethod]
        public void UserRepository_Constructor_ShouldCreateHttpClientFromFactory()
        {
            // Arrange & Act
            var repository = new UserRepository(_mockHttpClientFactory.Object);

            // Assert
            Assert.IsNotNull(repository);
            _mockHttpClientFactory.Verify(x => x.CreateClient("UsersAPI"), Times.Once);
        }

        #endregion
    }
}
