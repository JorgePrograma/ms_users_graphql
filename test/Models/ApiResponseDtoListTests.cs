using Microsoft.VisualStudio.TestTools.UnitTesting;
using msusersgraphql.Models.Dtos;
using System.Collections.Generic;

namespace msusersgraphql.Tests.Models.Dtos
{
    [TestClass]
    public class ApiResponseDtoListTests
    {
        [TestMethod]
        public void ApiResponseDtoList_Constructor_ShouldInitializeCollections()
        {
            // Arrange & Act
            var response = new ApiResponseDtoList<string>();

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNotNull(response.Errors);
            Assert.AreEqual(0, response.Data.Count);
            Assert.AreEqual(0, response.Errors.Count);
            Assert.AreEqual(0, response.StatusCode);
            Assert.IsInstanceOfType(response.Data, typeof(List<string>));
            Assert.IsInstanceOfType(response.Errors, typeof(List<string>));
        }

        [TestMethod]
        public void ApiResponseDtoList_AddDataItems_ShouldAddItemsToDataCollection()
        {
            // Arrange
            var response = new ApiResponseDtoList<string>();
            var items = new List<string> { "Item1", "Item2", "Item3" };

            // Act
            response.Data.AddRange(items);

            // Assert
            Assert.AreEqual(3, response.Data.Count);
            CollectionAssert.AreEqual(items, response.Data);
        }

        [TestMethod]
        public void ApiResponseDtoList_SetDataCollection_ShouldReplaceDataCollection()
        {
            // Arrange
            var response = new ApiResponseDtoList<string>();
            var newData = new List<string> { "New Item1", "New Item2" };

            // Act
            response.Data = newData;

            // Assert
            Assert.AreEqual(newData, response.Data);
            Assert.AreEqual(2, response.Data.Count);
        }

        [TestMethod]
        public void ApiResponseDtoList_AddErrors_ShouldAddErrorsToCollection()
        {
            // Arrange
            var response = new ApiResponseDtoList<string>();
            var errors = new List<string> { "Error 1", "Error 2" };

            // Act
            response.Errors.AddRange(errors);

            // Assert
            Assert.AreEqual(2, response.Errors.Count);
            CollectionAssert.AreEqual(errors, response.Errors);
        }

        [TestMethod]
        public void ApiResponseDtoList_SetStatusCode_ShouldStoreStatusCodeCorrectly()
        {
            // Arrange
            var response = new ApiResponseDtoList<string>();
            var expectedStatusCode = 404;

            // Act
            response.StatusCode = expectedStatusCode;

            // Assert
            Assert.AreEqual(expectedStatusCode, response.StatusCode);
        }

        [TestMethod]
        public void ApiResponseDtoList_WithComplexObjects_ShouldWorkCorrectly()
        {
            // Arrange
            var response = new ApiResponseDtoList<UserDto>();
            var users = new List<UserDto>
        {
            new UserDto { Id = "1", UserName = "User1" },
            new UserDto { Id = "2", UserName = "User2" }
        };

            // Act
            response.Data.AddRange(users);

            // Assert
            Assert.AreEqual(2, response.Data.Count);
            Assert.AreEqual("1", response.Data[0].Id);
            Assert.AreEqual("User1", response.Data[0].UserName);
            Assert.AreEqual("2", response.Data[1].Id);
            Assert.AreEqual("User2", response.Data[1].UserName);
        }

        [TestMethod]
        public void ApiResponseDtoList_EmptyCollections_ShouldRemainEmpty()
        {
            // Arrange & Act
            var response = new ApiResponseDtoList<int>();

            // Assert
            Assert.AreEqual(0, response.Data.Count);
            Assert.AreEqual(0, response.Errors.Count);
        }

        [TestMethod]
        public void ApiResponseDtoList_SetAllProperties_ShouldStoreAllValuesCorrectly()
        {
            // Arrange
            var response = new ApiResponseDtoList<string>();
            var expectedData = new List<string> { "Item1", "Item2" };
            var expectedErrors = new List<string> { "Error1" };
            var expectedStatusCode = 500;

            // Act
            response.Data = expectedData;
            response.Errors.AddRange(expectedErrors);
            response.StatusCode = expectedStatusCode;

            // Assert
            Assert.AreEqual(expectedData, response.Data);
            CollectionAssert.AreEqual(expectedErrors, response.Errors);
            Assert.AreEqual(expectedStatusCode, response.StatusCode);
        }
    }

}