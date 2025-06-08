using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using msusersgraphql.Models.Dtos;
using msusersgraphql.Repositories.User;
using msusersgraphql.Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace msusersgraphql.Tests.Services.User
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private UserService _userService;

        [TestInitialize]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        #region GetUserByIdAsync Tests

        [TestMethod]
        public async Task GetUserByIdAsync_WithValidId_ShouldReturnUser()
        {
            // Arrange
            var userId = "user123";
            var expectedUser = new UserDto
            {
                Id = userId,
                IdUserIDCS = "idcs456",
                UserName = "testuser",
                State = "Active",
                CreationDate = DateTime.UtcNow,
                AvatarPath = "/avatars/user123.jpg",
                Roles = new List<UserRoleDto>
                {
                    new UserRoleDto { Id = "1", Name = "Admin" },
                    new UserRoleDto { Id = "2", Name = "User" }
                }
            };

            _mockUserRepository
                .Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUser.Id, result.Id);
            Assert.AreEqual(expectedUser.IdUserIDCS, result.IdUserIDCS);
            Assert.AreEqual(expectedUser.UserName, result.UserName);
            Assert.AreEqual(expectedUser.State, result.State);
            Assert.AreEqual(expectedUser.CreationDate, result.CreationDate);
            Assert.AreEqual(expectedUser.AvatarPath, result.AvatarPath);
            Assert.AreEqual(2, result.Roles.Count);
            Assert.AreEqual("Admin", result.Roles.First().Name);

            // Verificar que se llamó al repositorio
            _mockUserRepository.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
        }

        [TestMethod]
        public async Task GetUserByIdAsync_WithValidId_ShouldCallRepositoryOnce()
        {
            // Arrange
            var userId = "user456";
            var user = new UserDto { Id = userId, UserName = "testuser" };

            _mockUserRepository
                .Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            await _userService.GetUserByIdAsync(userId);

            // Assert
            _mockUserRepository.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
            _mockUserRepository.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task GetUserByIdAsync_WhenRepositoryReturnsNull_ShouldReturnNull()
        {
            // Arrange
            var userId = "nonexistent";

            _mockUserRepository
                .Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync((UserDto)null);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.IsNull(result);
            _mockUserRepository.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
        }

        [TestMethod]
        public async Task GetUserByIdAsync_WithNullId_ShouldThrowArgumentException()
        {
            // Arrange
            string userId = null;

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _userService.GetUserByIdAsync(userId));

            Assert.AreEqual("User ID cannot be empty", exception.Message);
            _mockUserRepository.Verify(x => x.GetUserByIdAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetUserByIdAsync_WithEmptyId_ShouldThrowArgumentException()
        {
            // Arrange
            var userId = "";

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _userService.GetUserByIdAsync(userId));

            Assert.AreEqual("User ID cannot be empty", exception.Message);
            _mockUserRepository.Verify(x => x.GetUserByIdAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetUserByIdAsync_WithWhitespaceId_ShouldThrowArgumentException()
        {
            // Arrange
            var userId = "   ";

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _userService.GetUserByIdAsync(userId));

            Assert.AreEqual("User ID cannot be empty", exception.Message);
            _mockUserRepository.Verify(x => x.GetUserByIdAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetUserByIdAsync_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            var userId = "error";
            var expectedException = new InvalidOperationException("Repository error");

            _mockUserRepository
                .Setup(x => x.GetUserByIdAsync(userId))
                .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => _userService.GetUserByIdAsync(userId));

            Assert.AreEqual("Repository error", exception.Message);
            _mockUserRepository.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
        }

        #endregion

        #region GetUsersAsync Tests

        [TestMethod]
        public async Task GetUsersAsync_WithDefaultParameters_ShouldReturnUserList()
        {
            // Arrange
            var users = new List<UserDto>
            {
                new UserDto { Id = "1", UserName = "user1", State = "Active" },
                new UserDto { Id = "2", UserName = "user2", State = "Inactive" }
            };

            var expectedUserList = new UserListDto
            {
                Data = users,
                TotalCount = 2,
                PageNumber = 1,
                PageSize = 10
            };

            _mockUserRepository
                .Setup(x => x.GetUsersAsync(1, 10))
                .ReturnsAsync(expectedUserList);

            // Act
            var result = await _userService.GetUsersAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Data.Count);
            Assert.AreEqual(2, result.TotalCount);
            Assert.AreEqual(1, result.PageNumber);
            Assert.AreEqual(10, result.PageSize);
            Assert.AreEqual("user1", result.Data.First().UserName);

            _mockUserRepository.Verify(x => x.GetUsersAsync(1, 10), Times.Once);
        }

        [TestMethod]
        public async Task GetUsersAsync_WithCustomParameters_ShouldReturnUserList()
        {
            // Arrange
            var pageNumber = 2;
            var pageSize = 5;
            var users = new List<UserDto>
            {
                new UserDto { Id = "3", UserName = "user3" }
            };

            var expectedUserList = new UserListDto
            {
                Data = users,
                TotalCount = 15,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            _mockUserRepository
                .Setup(x => x.GetUsersAsync(pageNumber, pageSize))
                .ReturnsAsync(expectedUserList);

            // Act
            var result = await _userService.GetUsersAsync(pageNumber, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Data.Count);
            Assert.AreEqual(15, result.TotalCount);
            Assert.AreEqual(pageNumber, result.PageNumber);
            Assert.AreEqual(pageSize, result.PageSize);

            _mockUserRepository.Verify(x => x.GetUsersAsync(pageNumber, pageSize), Times.Once);
        }

        [TestMethod]
        public async Task GetUsersAsync_WithValidParameters_ShouldCallRepositoryOnce()
        {
            // Arrange
            var pageNumber = 3;
            var pageSize = 15;
            var userList = new UserListDto { Data = new List<UserDto>(), TotalCount = 0 };

            _mockUserRepository
                .Setup(x => x.GetUsersAsync(pageNumber, pageSize))
                .ReturnsAsync(userList);

            // Act
            await _userService.GetUsersAsync(pageNumber, pageSize);

            // Assert
            _mockUserRepository.Verify(x => x.GetUsersAsync(pageNumber, pageSize), Times.Once);
            _mockUserRepository.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task GetUsersAsync_WithZeroPageNumber_ShouldThrowArgumentException()
        {
            // Arrange
            var pageNumber = 0;
            var pageSize = 10;

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _userService.GetUsersAsync(pageNumber, pageSize));

            Assert.AreEqual("Page number and page size must be greater than 0", exception.Message);
            _mockUserRepository.Verify(x => x.GetUsersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task GetUsersAsync_WithNegativePageNumber_ShouldThrowArgumentException()
        {
            // Arrange
            var pageNumber = -1;
            var pageSize = 10;

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _userService.GetUsersAsync(pageNumber, pageSize));

            Assert.AreEqual("Page number and page size must be greater than 0", exception.Message);
            _mockUserRepository.Verify(x => x.GetUsersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task GetUsersAsync_WithZeroPageSize_ShouldThrowArgumentException()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 0;

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _userService.GetUsersAsync(pageNumber, pageSize));

            Assert.AreEqual("Page number and page size must be greater than 0", exception.Message);
            _mockUserRepository.Verify(x => x.GetUsersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task GetUsersAsync_WithNegativePageSize_ShouldThrowArgumentException()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = -5;

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _userService.GetUsersAsync(pageNumber, pageSize));

            Assert.AreEqual("Page number and page size must be greater than 0", exception.Message);
            _mockUserRepository.Verify(x => x.GetUsersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task GetUsersAsync_WithBothParametersZero_ShouldThrowArgumentException()
        {
            // Arrange
            var pageNumber = 0;
            var pageSize = 0;

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _userService.GetUsersAsync(pageNumber, pageSize));

            Assert.AreEqual("Page number and page size must be greater than 0", exception.Message);
            _mockUserRepository.Verify(x => x.GetUsersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task GetUsersAsync_WithBothParametersNegative_ShouldThrowArgumentException()
        {
            // Arrange
            var pageNumber = -2;
            var pageSize = -3;

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _userService.GetUsersAsync(pageNumber, pageSize));

            Assert.AreEqual("Page number and page size must be greater than 0", exception.Message);
            _mockUserRepository.Verify(x => x.GetUsersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task GetUsersAsync_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var expectedException = new InvalidOperationException("Repository error");

            _mockUserRepository
                .Setup(x => x.GetUsersAsync(pageNumber, pageSize))
                .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => _userService.GetUsersAsync(pageNumber, pageSize));

            Assert.AreEqual("Repository error", exception.Message);
            _mockUserRepository.Verify(x => x.GetUsersAsync(pageNumber, pageSize), Times.Once);
        }

        [TestMethod]
        public async Task GetUsersAsync_WhenRepositoryReturnsNull_ShouldReturnNull()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;

            _mockUserRepository
                .Setup(x => x.GetUsersAsync(pageNumber, pageSize))
                .ReturnsAsync((UserListDto)null);

            // Act
            var result = await _userService.GetUsersAsync(pageNumber, pageSize);

            // Assert
            Assert.IsNull(result);
            _mockUserRepository.Verify(x => x.GetUsersAsync(pageNumber, pageSize), Times.Once);
        }

        #endregion

        #region Constructor Tests

        [TestMethod]
        public void UserService_Constructor_ShouldAcceptRepository()
        {
            // Arrange
            var mockRepository = new Mock<IUserRepository>();

            // Act
            var service = new UserService(mockRepository.Object);

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void UserService_Constructor_WithNullRepository_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new UserService(null));
        }

        #endregion
    }
}
