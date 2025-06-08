using Microsoft.VisualStudio.TestTools.UnitTesting;
using msusersgraphql.Models.Dtos;
using System.Collections.Generic;

namespace msusersgraphql.Tests.Models.Dtos
{
    [TestClass]
    public class ApiResponseDtoTests
    {
        [TestMethod]
        public void ApiResponseDto_Constructor_ShouldInitializeErrorsCollection()
        {
            // Arrange & Act
            var response = new ApiResponseDto<string>();

            // Assert
            Assert.IsNotNull(response.Errors);
            Assert.AreEqual(0, response.Errors.Count);
            Assert.IsInstanceOfType(response.Errors, typeof(List<string>));
            Assert.AreEqual(0, response.StatusCode);
            Assert.IsNull(response.Data);
        }

        [TestMethod]
        public void ApiResponseDto_SetData_ShouldStoreDataCorrectly()
        {
            // Arrange
            var expectedData = "Test Data";
            var response = new ApiResponseDto<string>();

            // Act
            response.Data = expectedData;

            // Assert
            Assert.AreEqual(expectedData, response.Data);
        }

        [TestMethod]
        public void ApiResponseDto_SetDataWithComplexObject_ShouldStoreObjectCorrectly()
        {
            // Arrange
            var expectedData = new { Id = 1, Name = "Test Object" };
            var response = new ApiResponseDto<object>();

            // Act
            response.Data = expectedData;

            // Assert
            Assert.AreEqual(expectedData, response.Data);
            Assert.AreEqual(1, ((dynamic)response.Data).Id);
            Assert.AreEqual("Test Object", ((dynamic)response.Data).Name);
        }

        [TestMethod]
        public void ApiResponseDto_SetStatusCode_ShouldStoreStatusCodeCorrectly()
        {
            // Arrange
            var expectedStatusCode = 200;
            var response = new ApiResponseDto<string>();

            // Act
            response.StatusCode = expectedStatusCode;

            // Assert
            Assert.AreEqual(expectedStatusCode, response.StatusCode);
        }

        [TestMethod]
        public void ApiResponseDto_AddSingleError_ShouldAddErrorToCollection()
        {
            // Arrange
            var response = new ApiResponseDto<string>();
            var errorMessage = "Error occurred";

            // Act
            response.Errors.Add(errorMessage);

            // Assert
            Assert.AreEqual(1, response.Errors.Count);
            Assert.AreEqual(errorMessage, response.Errors[0]);
        }

        [TestMethod]
        public void ApiResponseDto_AddMultipleErrors_ShouldAddAllErrorsToCollection()
        {
            // Arrange
            var response = new ApiResponseDto<string>();
            var errors = new List<string> { "Error 1", "Error 2", "Error 3" };

            // Act
            response.Errors.AddRange(errors);

            // Assert
            Assert.AreEqual(3, response.Errors.Count);
            CollectionAssert.AreEqual(errors, response.Errors);
        }

        [TestMethod]
        public void ApiResponseDto_SetNullData_ShouldAllowNullData()
        {
            // Arrange
            var response = new ApiResponseDto<string>();

            // Act
            response.Data = null;

            // Assert
            Assert.IsNull(response.Data);
        }

        [TestMethod]
        public void ApiResponseDto_WithGenericType_ShouldWorkWithDifferentTypes()
        {
            // Arrange & Act
            var stringResponse = new ApiResponseDto<string>();
            var intResponse = new ApiResponseDto<int>();
            var boolResponse = new ApiResponseDto<bool>();

            // Assert
            Assert.IsInstanceOfType(stringResponse, typeof(ApiResponseDto<string>));
            Assert.IsInstanceOfType(intResponse, typeof(ApiResponseDto<int>));
            Assert.IsInstanceOfType(boolResponse, typeof(ApiResponseDto<bool>));
        }

        [TestMethod]
        public void ApiResponseDto_SetAllProperties_ShouldStoreAllValuesCorrectly()
        {
            // Arrange
            var expectedData = "Test Data";
            var expectedStatusCode = 201;
            var expectedErrors = new List<string> { "Warning 1", "Warning 2" };
            var response = new ApiResponseDto<string>();

            // Act
            response.Data = expectedData;
            response.StatusCode = expectedStatusCode;
            response.Errors.AddRange(expectedErrors);

            // Assert
            Assert.AreEqual(expectedData, response.Data);
            Assert.AreEqual(expectedStatusCode, response.StatusCode);
            CollectionAssert.AreEqual(expectedErrors, response.Errors);
        }
    }
}

