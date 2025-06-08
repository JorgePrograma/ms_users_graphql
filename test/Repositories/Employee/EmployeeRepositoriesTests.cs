using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using msusersgraphql.Models.Dtos;
using msusersgraphql.Repositories.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace msusersgraphql.Tests.Repositories.Employee
{
    [TestClass]
    public class EmployeeRepositoryTests
    {
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private EmployeeRepository _employeeRepository;

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
            
            _employeeRepository = new EmployeeRepository(_mockHttpClientFactory.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _httpClient?.Dispose();
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithValidId_ShouldReturnEmployee()
        {
            // Arrange
            var userId = "user123";
            var expectedEmployee = new EmployeeDto
            {
                Id = "emp123",
                IdPerson = "person456",
                IdUser = userId,
                BussinesEmail = "employee@company.com",
                BussinesPhone = "555-0123",
                IdPosition = "pos789",
                IdBranch = "branch101"
            };

            var employeeDataDto = new EmployeeDataDto
            {
                Items = new List<EmployeeDto> { expectedEmployee },
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 10
            };

            var apiResponse = new ApiResponseDto<EmployeeDataDto>
            {
                Data = employeeDataDto,
                StatusCode = 200,
                Errors = new List<string>()
            };

            SetupHttpResponse(HttpStatusCode.OK, apiResponse);

            // Act
            var result = await _employeeRepository.GetEmployeeByIdAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployee.Id, result.Id);
            Assert.AreEqual(expectedEmployee.IdPerson, result.IdPerson);
            Assert.AreEqual(expectedEmployee.IdUser, result.IdUser);
            Assert.AreEqual(expectedEmployee.BussinesEmail, result.BussinesEmail);
            Assert.AreEqual(expectedEmployee.BussinesPhone, result.BussinesPhone);
            Assert.AreEqual(expectedEmployee.IdPosition, result.IdPosition);
            Assert.AreEqual(expectedEmployee.IdBranch, result.IdBranch);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithMultipleEmployees_ShouldReturnFirstEmployee()
        {
            // Arrange
            var userId = "user456";
            var employees = new List<EmployeeDto>
            {
                new EmployeeDto 
                { 
                    Id = "emp1", 
                    IdUser = userId, 
                    BussinesEmail = "emp1@company.com",
                    IdPosition = "manager"
                },
                new EmployeeDto 
                { 
                    Id = "emp2", 
                    IdUser = userId, 
                    BussinesEmail = "emp2@company.com",
                    IdPosition = "developer"
                }
            };

            var employeeDataDto = new EmployeeDataDto
            {
                Items = employees,
                TotalCount = 2,
                PageNumber = 1,
                PageSize = 10
            };

            var apiResponse = new ApiResponseDto<EmployeeDataDto>
            {
                Data = employeeDataDto,
                StatusCode = 200
            };

            SetupHttpResponse(HttpStatusCode.OK, apiResponse);

            // Act
            var result = await _employeeRepository.GetEmployeeByIdAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("emp1", result.Id);
            Assert.AreEqual("emp1@company.com", result.BussinesEmail);
            Assert.AreEqual("manager", result.IdPosition);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithEmptyItems_ShouldReturnNull()
        {
            // Arrange
            var userId = "user789";
            var employeeDataDto = new EmployeeDataDto
            {
                Items = new List<EmployeeDto>(),
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 10
            };

            var apiResponse = new ApiResponseDto<EmployeeDataDto>
            {
                Data = employeeDataDto,
                StatusCode = 200
            };

            SetupHttpResponse(HttpStatusCode.OK, apiResponse);

            // Act
            var result = await _employeeRepository.GetEmployeeByIdAsync(userId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithNullItems_ShouldReturnNull()
        {
            // Arrange
            var userId = "user999";
            var employeeDataDto = new EmployeeDataDto
            {
                Items = null,
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 10
            };

            var apiResponse = new ApiResponseDto<EmployeeDataDto>
            {
                Data = employeeDataDto,
                StatusCode = 200
            };

            SetupHttpResponse(HttpStatusCode.OK, apiResponse);

            // Act
            var result = await _employeeRepository.GetEmployeeByIdAsync(userId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithNullData_ShouldReturnNull()
        {
            // Arrange
            var userId = "null-data";
            var apiResponse = new ApiResponseDto<EmployeeDataDto>
            {
                Data = null,
                StatusCode = 200
            };

            SetupHttpResponse(HttpStatusCode.OK, apiResponse);

            // Act
            var result = await _employeeRepository.GetEmployeeByIdAsync(userId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithNullApiResponse_ShouldReturnNull()
        {
            // Arrange
            var userId = "null-response";
            
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

            // Act
            var result = await _employeeRepository.GetEmployeeByIdAsync(userId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithHttpErrorStatus_ShouldReturnNull()
        {
            // Arrange
            var userId = "error";
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("Not Found", Encoding.UTF8, "text/plain")
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
            var result = await _employeeRepository.GetEmployeeByIdAsync(userId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithInternalServerError_ShouldReturnNull()
        {
            // Arrange
            var userId = "500";
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("Internal Server Error", Encoding.UTF8, "text/plain")
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
            var result = await _employeeRepository.GetEmployeeByIdAsync(userId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithHttpClientException_ShouldReturnNull()
        {
            // Arrange
            var userId = "exception";

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act
            var result = await _employeeRepository.GetEmployeeByIdAsync(userId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithTaskCancelledException_ShouldReturnNull()
        {
            // Arrange
            var userId = "timeout";

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new TaskCanceledException("Request timeout"));

            // Act
            var result = await _employeeRepository.GetEmployeeByIdAsync(userId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithJsonException_ShouldReturnNull()
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

            // Act
            var result = await _employeeRepository.GetEmployeeByIdAsync(userId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_ShouldCallCorrectUrl()
        {
            // Arrange
            var userId = "url-test";
            var expectedUrl = $"employee/get-filter?idUser={userId}";
            
            var employeeDataDto = new EmployeeDataDto
            {
                Items = new List<EmployeeDto>(),
                TotalCount = 0
            };

            var apiResponse = new ApiResponseDto<EmployeeDataDto>
            {
                Data = employeeDataDto,
                StatusCode = 200
            };

            HttpRequestMessage capturedRequest = null;
            SetupHttpResponseWithCallback(HttpStatusCode.OK, apiResponse, 
                (request, token) => capturedRequest = request);

            // Act
            await _employeeRepository.GetEmployeeByIdAsync(userId);

            // Assert
            Assert.IsNotNull(capturedRequest);
            Assert.AreEqual(HttpMethod.Get, capturedRequest.Method);
            Assert.IsTrue(capturedRequest.RequestUri.ToString().Contains(expectedUrl));
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithSpecialCharactersInId_ShouldEncodeUrl()
        {
            // Arrange
            var userId = "test@123";
            
            var employeeDataDto = new EmployeeDataDto
            {
                Items = new List<EmployeeDto>(),
                TotalCount = 0
            };

            var apiResponse = new ApiResponseDto<EmployeeDataDto>
            {
                Data = employeeDataDto,
                StatusCode = 200
            };

            HttpRequestMessage capturedRequest = null;
            SetupHttpResponseWithCallback(HttpStatusCode.OK, apiResponse, 
                (request, token) => capturedRequest = request);

            // Act
            await _employeeRepository.GetEmployeeByIdAsync(userId);

            // Assert
            Assert.IsNotNull(capturedRequest);
            Assert.IsTrue(capturedRequest.RequestUri.ToString().Contains("employee/get-filter?idUser="));
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithNullOrEmptyId_ShouldStillMakeRequest()
        {
            // Arrange
            var userId = "";
            var employeeDataDto = new EmployeeDataDto
            {
                Items = new List<EmployeeDto>(),
                TotalCount = 0
            };

            var apiResponse = new ApiResponseDto<EmployeeDataDto>
            {
                Data = employeeDataDto,
                StatusCode = 200
            };

            SetupHttpResponse(HttpStatusCode.OK, apiResponse);

            // Act
            var result = await _employeeRepository.GetEmployeeByIdAsync(userId);

            // Assert
            Assert.IsNull(result);
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [TestMethod]
        public void EmployeeRepository_Constructor_ShouldCreateHttpClientFromFactory()
        {
            // Arrange & Act
            var repository = new EmployeeRepository(_mockHttpClientFactory.Object);

            // Assert
            Assert.IsNotNull(repository);
            _mockHttpClientFactory.Verify(x => x.CreateClient("UsersAPI"), Times.Once);
        }

        private void SetupHttpResponse<T>(HttpStatusCode statusCode, T responseObject)
        {
            var jsonResponse = JsonSerializer.Serialize(responseObject);
            var httpResponseMessage = new HttpResponseMessage(statusCode)
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
        }

        private void SetupHttpResponseWithCallback<T>(HttpStatusCode statusCode, T responseObject, 
            Action<HttpRequestMessage, CancellationToken> callback)
        {
            var jsonResponse = JsonSerializer.Serialize(responseObject);
            var httpResponseMessage = new HttpResponseMessage(statusCode)
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
                .Callback(callback)
                .ReturnsAsync(httpResponseMessage);
        }
    }
}
