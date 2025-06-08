using Microsoft.VisualStudio.TestTools.UnitTesting;
using msusersgraphql.Models.Dtos;
using System.Collections.Generic;

namespace msusersgraphql.Tests.Models.Dtos
{
    [TestClass]
    public class UserDataDtoTests
    {
        [TestMethod]
        public void UserDataDto_Constructor_ShouldInitializeItemsCollection()
        {
            // Arrange & Act
            var userDataDto = new UserDataDto();

            // Assert
            Assert.IsNotNull(userDataDto.Items);
            Assert.AreEqual(0, userDataDto.Items.Count);
            Assert.AreEqual(0, userDataDto.TotalCount);
            Assert.AreEqual(0, userDataDto.PageNumber);
            Assert.AreEqual(0, userDataDto.PageSize);
            Assert.IsInstanceOfType(userDataDto.Items, typeof(List<UserDto>));
        }

        [TestMethod]
        public void UserDataDto_SetTotalCount_ShouldStoreTotalCountCorrectly()
        {
            // Arrange
            var userDataDto = new UserDataDto();
            var expectedTotalCount = 150;

            // Act
            userDataDto.TotalCount = expectedTotalCount;

            // Assert
            Assert.AreEqual(expectedTotalCount, userDataDto.TotalCount);
        }

        [TestMethod]
        public void UserDataDto_SetPageNumber_ShouldStorePageNumberCorrectly()
        {
            // Arrange
            var userDataDto = new UserDataDto();
            var expectedPageNumber = 3;

            // Act
            userDataDto.PageNumber = expectedPageNumber;

            // Assert
            Assert.AreEqual(expectedPageNumber, userDataDto.PageNumber);
        }

        [TestMethod]
        public void UserDataDto_SetPageSize_ShouldStorePageSizeCorrectly()
        {
            // Arrange
            var userDataDto = new UserDataDto();
            var expectedPageSize = 25;

            // Act
            userDataDto.PageSize = expectedPageSize;

            // Assert
            Assert.AreEqual(expectedPageSize, userDataDto.PageSize);
        }

        [TestMethod]
        public void UserDataDto_AddUsers_ShouldAddUsersToItemsCollection()
        {
            // Arrange
            var userDataDto = new UserDataDto();
            var users = new List<UserDto>
        {
            new UserDto { Id = "1", UserName = "User1", State = "Active" },
            new UserDto { Id = "2", UserName = "User2", State = "Inactive" }
        };

            // Act
            userDataDto.Items.AddRange(users);

            // Assert
            Assert.AreEqual(2, userDataDto.Items.Count);
            CollectionAssert.AreEqual(users, userDataDto.Items);
        }

        [TestMethod]
        public void UserDataDto_SetItemsCollection_ShouldReplaceItemsCollection()
        {
            // Arrange
            var userDataDto = new UserDataDto();
            var newUsers = new List<UserDto>
        {
            new UserDto { Id = "3", UserName = "User3" }
        };

            // Act
            userDataDto.Items = newUsers;

            // Assert
            Assert.AreEqual(newUsers, userDataDto.Items);
            Assert.AreEqual(1, userDataDto.Items.Count);
            Assert.AreEqual("3", userDataDto.Items[0].Id);
        }

        [TestMethod]
        public void UserDataDto_SetAllPaginationProperties_ShouldStoreAllValuesCorrectly()
        {
            // Arrange
            var userDataDto = new UserDataDto();
            var expectedTotalCount = 100;
            var expectedPageNumber = 2;
            var expectedPageSize = 10;

            // Act
            userDataDto.TotalCount = expectedTotalCount;
            userDataDto.PageNumber = expectedPageNumber;
            userDataDto.PageSize = expectedPageSize;

            // Assert
            Assert.AreEqual(expectedTotalCount, userDataDto.TotalCount);
            Assert.AreEqual(expectedPageNumber, userDataDto.PageNumber);
            Assert.AreEqual(expectedPageSize, userDataDto.PageSize);
        }

        [TestMethod]
        public void UserDataDto_EmptyItems_ShouldRemainEmptyCollection()
        {
            // Arrange & Act
            var userDataDto = new UserDataDto();

            // Assert
            Assert.AreEqual(0, userDataDto.Items.Count);
            Assert.IsNotNull(userDataDto.Items);
        }

        [TestMethod]
        public void UserDataDto_SetCompleteUserData_ShouldStoreAllDataCorrectly()
        {
            // Arrange
            var userDataDto = new UserDataDto();
            var users = new List<UserDto>
        {
            new UserDto
            {
                Id = "1",
                UserName = "TestUser",
                State = "Active",
                CreationDate = DateTime.Now,
                Roles = new List<UserRoleDto>
                {
                    new UserRoleDto { Id = "1", Name = "Admin" }
                }
            }
        };

            // Act
            userDataDto.Items = users;
            userDataDto.TotalCount = 50;
            userDataDto.PageNumber = 1;
            userDataDto.PageSize = 20;

            // Assert
            Assert.AreEqual(1, userDataDto.Items.Count);
            Assert.AreEqual("1", userDataDto.Items[0].Id);
            Assert.AreEqual("TestUser", userDataDto.Items[0].UserName);
            Assert.AreEqual("Active", userDataDto.Items[0].State);
            Assert.AreEqual(1, userDataDto.Items[0].Roles.Count);
            Assert.AreEqual(50, userDataDto.TotalCount);
            Assert.AreEqual(1, userDataDto.PageNumber);
            Assert.AreEqual(20, userDataDto.PageSize);
        }
    }
}