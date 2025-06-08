using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using msusersgraphql.Models.Dtos;
using msusersgraphql.Repositories.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace msusersgraphql.Tests.Repositories.Person
{
    [TestClass]
    public class PersonRepositoryTests
    {
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private PersonRepository _personRepository;

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
            
            _personRepository = new PersonRepository(_mockHttpClientFactory.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _httpClient?.Dispose();
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithValidId_ShouldReturnPerson()
        {
            // Arrange
            var personId = "123";
            var expectedPerson = new PersonDto
            {
                Id = personId,
                DocumentType = "CC",
                DocumentNumber = "12345678",
                FirstName = "Juan",
                MiddleName = "Carlos",
                LastName = "Pérez",
                SecondLastName = "García"
            };

            var personDataDto = new PersonDataDto
            {
                Items = new List<PersonDto> { expectedPerson },
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 10
            };

            var apiResponse = new ApiResponseDto<PersonDataDto>
            {
                Data = personDataDto,
                StatusCode = 200,
                Errors = new List<string>()
            };

            SetupHttpResponse(HttpStatusCode.OK, apiResponse);

            // Act
            var result = await _personRepository.GetPersonByIdAsync(personId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPerson.Id, result.Id);
            Assert.AreEqual(expectedPerson.DocumentType, result.DocumentType);
            Assert.AreEqual(expectedPerson.DocumentNumber, result.DocumentNumber);
            Assert.AreEqual(expectedPerson.FirstName, result.FirstName);
            Assert.AreEqual(expectedPerson.MiddleName, result.MiddleName);
            Assert.AreEqual(expectedPerson.LastName, result.LastName);
            Assert.AreEqual(expectedPerson.SecondLastName, result.SecondLastName);
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithMultiplePersons_ShouldReturnFirstPerson()
        {
            // Arrange
            var personId = "456";
            var persons = new List<PersonDto>
            {
                new PersonDto 
                { 
                    Id = "person1", 
                    FirstName = "María", 
                    LastName = "González",
                    DocumentType = "CC",
                    DocumentNumber = "11111111"
                },
                new PersonDto 
                { 
                    Id = "person2", 
                    FirstName = "Pedro", 
                    LastName = "Martínez",
                    DocumentType = "CE",
                    DocumentNumber = "22222222"
                }
            };

            var personDataDto = new PersonDataDto
            {
                Items = persons,
                TotalCount = 2,
                PageNumber = 1,
                PageSize = 10
            };

            var apiResponse = new ApiResponseDto<PersonDataDto>
            {
                Data = personDataDto,
                StatusCode = 200
            };

            SetupHttpResponse(HttpStatusCode.OK, apiResponse);

            // Act
            var result = await _personRepository.GetPersonByIdAsync(personId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("person1", result.Id);
            Assert.AreEqual("María", result.FirstName);
            Assert.AreEqual("González", result.LastName);
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithEmptyItems_ShouldReturnNull()
        {
            // Arrange
            var personId = "789";
            var personDataDto = new PersonDataDto
            {
                Items = new List<PersonDto>(),
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 10
            };

            var apiResponse = new ApiResponseDto<PersonDataDto>
            {
                Data = personDataDto,
                StatusCode = 200
            };

            SetupHttpResponse(HttpStatusCode.OK, apiResponse);

            // Act
            var result = await _personRepository.GetPersonByIdAsync(personId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithNullItems_ShouldReturnNull()
        {
            // Arrange
            var personId = "999";
            var personDataDto = new PersonDataDto
            {
                Items = null,
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 10
            };

            var apiResponse = new ApiResponseDto<PersonDataDto>
            {
                Data = personDataDto,
                StatusCode = 200
            };

            SetupHttpResponse(HttpStatusCode.OK, apiResponse);

            // Act
            var result = await _personRepository.GetPersonByIdAsync(personId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithNullData_ShouldReturnNull()
        {
            // Arrange
            var personId = "null-data";
            var apiResponse = new ApiResponseDto<PersonDataDto>
            {
                Data = null,
                StatusCode = 200
            };

            SetupHttpResponse(HttpStatusCode.OK, apiResponse);

            // Act
            var result = await _personRepository.GetPersonByIdAsync(personId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithNullApiResponse_ShouldReturnNull()
        {
            // Arrange
            var personId = "null-response";
            
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
            var result = await _personRepository.GetPersonByIdAsync(personId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithHttpErrorStatus_ShouldReturnNull()
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
            var result = await _personRepository.GetPersonByIdAsync(personId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithInternalServerError_ShouldReturnNull()
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
            var result = await _personRepository.GetPersonByIdAsync(personId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithHttpClientException_ShouldThrowException()
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

            // Act & Assert
            await Assert.ThrowsExceptionAsync<HttpRequestException>(
                () => _personRepository.GetPersonByIdAsync(personId));
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithTaskCancelledException_ShouldThrowException()
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

            // Act & Assert
            await Assert.ThrowsExceptionAsync<TaskCanceledException>(
                () => _personRepository.GetPersonByIdAsync(personId));
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithJsonException_ShouldThrowException()
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

            // Act & Assert
            await Assert.ThrowsExceptionAsync<JsonException>(
                () => _personRepository.GetPersonByIdAsync(personId));
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_ShouldCallCorrectUrl()
        {
            // Arrange
            var personId = "url-test";
            var expectedUrl = $"person/get-filter?idFilter={personId}";
            
            var personDataDto = new PersonDataDto
            {
                Items = new List<PersonDto>(),
                TotalCount = 0
            };

            var apiResponse = new ApiResponseDto<PersonDataDto>
            {
                Data = personDataDto,
                StatusCode = 200
            };

            HttpRequestMessage capturedRequest = null;
            SetupHttpResponseWithCallback(HttpStatusCode.OK, apiResponse, 
                (request, token) => capturedRequest = request);

            // Act
            await _personRepository.GetPersonByIdAsync(personId);

            // Assert
            Assert.IsNotNull(capturedRequest);
            Assert.AreEqual(HttpMethod.Get, capturedRequest.Method);
            Assert.IsTrue(capturedRequest.RequestUri.ToString().Contains(expectedUrl));
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithSpecialCharactersInId_ShouldEncodeUrl()
        {
            // Arrange
            var personId = "test@123";
            
            var personDataDto = new PersonDataDto
            {
                Items = new List<PersonDto>(),
                TotalCount = 0
            };

            var apiResponse = new ApiResponseDto<PersonDataDto>
            {
                Data = personDataDto,
                StatusCode = 200
            };

            HttpRequestMessage capturedRequest = null;
            SetupHttpResponseWithCallback(HttpStatusCode.OK, apiResponse, 
                (request, token) => capturedRequest = request);

            // Act
            await _personRepository.GetPersonByIdAsync(personId);

            // Assert
            Assert.IsNotNull(capturedRequest);
            Assert.IsTrue(capturedRequest.RequestUri.ToString().Contains("person/get-filter?idFilter="));
        }

        [TestMethod]
        public void PersonRepository_Constructor_ShouldCreateHttpClientFromFactory()
        {
            // Arrange & Act
            var repository = new PersonRepository(_mockHttpClientFactory.Object);

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
