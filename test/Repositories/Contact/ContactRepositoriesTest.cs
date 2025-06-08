using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using msusersgraphql.Models.Dtos;
using msusersgraphql.Repositories.Contact;
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

namespace msusersgraphql.Tests.Repositories.Contact
{
    [TestClass]
    public class ContactRepositoryTests
    {
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private ContactRepository _contactRepository;
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
            
            _contactRepository = new ContactRepository(_mockHttpClientFactory.Object);

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

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WithValidId_ShouldReturnContactAndLogCorrectly()
        {
            // Arrange
            var personId = "123";
            var expectedContact = new ContactDto
            {
                Id = "contact1",
                IdPerson = personId,
                Email = "test@example.com",
                Phone = "555-1234",
                Address = "123 Main St",
                CellPhone = "300-123-4567",
                Country = "Colombia",
                Department = "Antioquia",
                City = "Medellín"
            };

            var apiResponse = new ApiResponseDtoList<ContactDto>
            {
                Data = new List<ContactDto> { expectedContact },
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
                        req.RequestUri.ToString().Contains($"contact-details/get-by-filter?idPerson={personId}")
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _contactRepository.GetContactByIdPersonAsync(personId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedContact.Id, result.Id);
            Assert.AreEqual(expectedContact.IdPerson, result.IdPerson);
            Assert.AreEqual(expectedContact.Email, result.Email);
            Assert.AreEqual(expectedContact.Phone, result.Phone);
            Assert.AreEqual(expectedContact.Address, result.Address);

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains($"Request URL: contact-details/get-by-filter?idPerson={personId}"));
            Assert.IsTrue(consoleOutput.Contains("HTTP Status Code: OK"));
            Assert.IsTrue(consoleOutput.Contains("Raw API Response:"));
            Assert.IsTrue(consoleOutput.Contains("Deserialized Data:"));
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WithMultipleContacts_ShouldReturnFirstContactAndLog()
        {
            // Arrange
            var personId = "456";
            var contacts = new List<ContactDto>
            {
                new ContactDto { Id = "contact1", IdPerson = personId, Email = "first@example.com" },
                new ContactDto { Id = "contact2", IdPerson = personId, Email = "second@example.com" }
            };

            var apiResponse = new ApiResponseDtoList<ContactDto>
            {
                Data = contacts,
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
            var result = await _contactRepository.GetContactByIdPersonAsync(personId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("contact1", result.Id);
            Assert.AreEqual("first@example.com", result.Email);

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains("HTTP Status Code: OK"));
            Assert.IsTrue(consoleOutput.Contains("Deserialized Data:"));
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WithEmptyResponse_ShouldReturnNullAndLogWarning()
        {
            // Arrange
            var personId = "789";
            var apiResponse = new ApiResponseDtoList<ContactDto>
            {
                Data = new List<ContactDto>(),
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
            var result = await _contactRepository.GetContactByIdPersonAsync(personId);

            // Assert
            Assert.IsNull(result);

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains("HTTP Status Code: OK"));
            Assert.IsTrue(consoleOutput.Contains("Warning: API response data is null or empty"));
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WithNullData_ShouldReturnNullAndLogWarning()
        {
            // Arrange
            var personId = "999";
            var apiResponse = new ApiResponseDtoList<ContactDto>
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

            // Act
            var result = await _contactRepository.GetContactByIdPersonAsync(personId);

            // Assert
            Assert.IsNull(result);

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains("Warning: API response data is null or empty"));
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WithHttpErrorStatus_ShouldReturnNullAndLogError()
        {
            // Arrange
            var personId = "error";
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
            var result = await _contactRepository.GetContactByIdPersonAsync(personId);

            // Assert
            Assert.IsNull(result);

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains("HTTP Status Code: NotFound"));
            Assert.IsTrue(consoleOutput.Contains("Error: HTTP request failed with status NotFound"));
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WithInternalServerError_ShouldReturnNullAndLogError()
        {
            // Arrange
            var personId = "500";
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
            var result = await _contactRepository.GetContactByIdPersonAsync(personId);

            // Assert
            Assert.IsNull(result);

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains("HTTP Status Code: InternalServerError"));
            Assert.IsTrue(consoleOutput.Contains("Error: HTTP request failed with status InternalServerError"));
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WithHttpClientException_ShouldReturnNullAndLogException()
        {
            // Arrange
            var personId = "exception";

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act
            var result = await _contactRepository.GetContactByIdPersonAsync(personId);

            // Assert
            Assert.IsNull(result);

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains("Error in GetContactByIdPersonAsync:"));
            Assert.IsTrue(consoleOutput.Contains("Network error"));
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WithTaskCancelledException_ShouldReturnNullAndLogException()
        {
            // Arrange
            var personId = "timeout";

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new TaskCanceledException("Request timeout"));

            // Act
            var result = await _contactRepository.GetContactByIdPersonAsync(personId);

            // Assert
            Assert.IsNull(result);

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains("Error in GetContactByIdPersonAsync:"));
            Assert.IsTrue(consoleOutput.Contains("Request timeout"));
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WithInvalidJson_ShouldReturnNullAndLogException()
        {
            // Arrange
            var personId = "invalidjson";
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
            var result = await _contactRepository.GetContactByIdPersonAsync(personId);

            // Assert
            Assert.IsNull(result);

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains("HTTP Status Code: OK"));
            Assert.IsTrue(consoleOutput.Contains("Raw API Response: { invalid json }"));
            Assert.IsTrue(consoleOutput.Contains("Error in GetContactByIdPersonAsync:"));
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_ShouldCallCorrectUrlAndLogIt()
        {
            // Arrange
            var personId = "url-test";
            var expectedUrl = $"contact-details/get-by-filter?idPerson={personId}";
            
            var apiResponse = new ApiResponseDtoList<ContactDto>
            {
                Data = new List<ContactDto>(),
                StatusCode = 200
            };

            var jsonResponse = JsonSerializer.Serialize(apiResponse);
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
            await _contactRepository.GetContactByIdPersonAsync(personId);

            // Assert
            Assert.IsNotNull(capturedRequest);
            Assert.AreEqual(HttpMethod.Get, capturedRequest.Method);
            Assert.IsTrue(capturedRequest.RequestUri.ToString().Contains(expectedUrl));

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains($"Request URL: {expectedUrl}"));
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WithNullOrEmptyId_ShouldStillMakeRequestAndLog()
        {
            // Arrange
            var personId = "";
            var apiResponse = new ApiResponseDtoList<ContactDto>
            {
                Data = new List<ContactDto>(),
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
            var result = await _contactRepository.GetContactByIdPersonAsync(personId);

            // Assert
            Assert.IsNull(result);
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );

            // Verificar logs
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains("Request URL: contact-details/get-by-filter?idPerson="));
            Assert.IsTrue(consoleOutput.Contains("Warning: API response data is null or empty"));
        }

        [TestMethod]
        public void ContactRepository_Constructor_ShouldCreateHttpClientFromFactory()
        {
            // Arrange & Act
            var repository = new ContactRepository(_mockHttpClientFactory.Object);

            // Assert
            Assert.IsNotNull(repository);
            _mockHttpClientFactory.Verify(x => x.CreateClient("UsersAPI"), Times.Once);
        }
    }
}
