using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using msusersgraphql.Models.Dtos;
using msusersgraphql.Repositories.Employee;
using msusersgraphql.Services.Employee;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace msusersgraphql.Tests.Services.Employee
{
    [TestClass]
    public class EmployeeServiceTests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private EmployeeService _employeeService;

        [TestInitialize]
        public void Setup()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _employeeService = new EmployeeService(_mockEmployeeRepository.Object);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithValidId_ShouldReturnEmployee()
        {
            // Arrange
            var employeeId = "emp123";
            var expectedEmployee = new EmployeeDto
            {
                Id = employeeId,
                IdPerson = "person456",
                IdUser = "user789",
                BussinesEmail = "employee@company.com",
                BussinesPhone = "555-0123",
                IdPosition = "pos101",
                IdBranch = "branch202"
            };

            _mockEmployeeRepository
                .Setup(x => x.GetEmployeeByIdAsync(employeeId))
                .ReturnsAsync(expectedEmployee);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(employeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployee.Id, result.Id);
            Assert.AreEqual(expectedEmployee.IdPerson, result.IdPerson);
            Assert.AreEqual(expectedEmployee.IdUser, result.IdUser);
            Assert.AreEqual(expectedEmployee.BussinesEmail, result.BussinesEmail);
            Assert.AreEqual(expectedEmployee.BussinesPhone, result.BussinesPhone);
            Assert.AreEqual(expectedEmployee.IdPosition, result.IdPosition);
            Assert.AreEqual(expectedEmployee.IdBranch, result.IdBranch);

            // Verificar que se llamó al repositorio
            _mockEmployeeRepository.Verify(x => x.GetEmployeeByIdAsync(employeeId), Times.Once);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithValidId_ShouldCallRepositoryOnce()
        {
            // Arrange
            var employeeId = "emp456";
            var employee = new EmployeeDto { Id = employeeId };

            _mockEmployeeRepository
                .Setup(x => x.GetEmployeeByIdAsync(employeeId))
                .ReturnsAsync(employee);

            // Act
            await _employeeService.GetEmployeeByIdAsync(employeeId);

            // Assert
            _mockEmployeeRepository.Verify(x => x.GetEmployeeByIdAsync(employeeId), Times.Once);
            _mockEmployeeRepository.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WhenRepositoryReturnsNull_ShouldReturnNull()
        {
            // Arrange
            var employeeId = "nonexistent";

            _mockEmployeeRepository
                .Setup(x => x.GetEmployeeByIdAsync(employeeId))
                .ReturnsAsync((EmployeeDto)null);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(employeeId);

            // Assert
            Assert.IsNull(result);
            _mockEmployeeRepository.Verify(x => x.GetEmployeeByIdAsync(employeeId), Times.Once);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithNullId_ShouldThrowArgumentException()
        {
            // Arrange
            string employeeId = null;

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _employeeService.GetEmployeeByIdAsync(employeeId));

            Assert.AreEqual("Employee ID cannot be empty", exception.Message);
            _mockEmployeeRepository.Verify(x => x.GetEmployeeByIdAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithEmptyId_ShouldThrowArgumentException()
        {
            // Arrange
            var employeeId = "";

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _employeeService.GetEmployeeByIdAsync(employeeId));

            Assert.AreEqual("Employee ID cannot be empty", exception.Message);
            _mockEmployeeRepository.Verify(x => x.GetEmployeeByIdAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithWhitespaceId_ShouldThrowArgumentException()
        {
            // Arrange
            var employeeId = "   ";

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _employeeService.GetEmployeeByIdAsync(employeeId));

            Assert.AreEqual("Employee ID cannot be empty", exception.Message);
            _mockEmployeeRepository.Verify(x => x.GetEmployeeByIdAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithTabsAndSpacesId_ShouldThrowArgumentException()
        {
            // Arrange
            var employeeId = " \t \n ";

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _employeeService.GetEmployeeByIdAsync(employeeId));

            Assert.AreEqual("Employee ID cannot be empty", exception.Message);
            _mockEmployeeRepository.Verify(x => x.GetEmployeeByIdAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            var employeeId = "error";
            var expectedException = new InvalidOperationException("Repository error");

            _mockEmployeeRepository
                .Setup(x => x.GetEmployeeByIdAsync(employeeId))
                .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => _employeeService.GetEmployeeByIdAsync(employeeId));

            Assert.AreEqual("Repository error", exception.Message);
            _mockEmployeeRepository.Verify(x => x.GetEmployeeByIdAsync(employeeId), Times.Once);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithSpecialCharactersInId_ShouldPassToRepository()
        {
            // Arrange
            var employeeId = "emp@123-456_789";
            var employee = new EmployeeDto { Id = employeeId };

            _mockEmployeeRepository
                .Setup(x => x.GetEmployeeByIdAsync(employeeId))
                .ReturnsAsync(employee);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(employeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employeeId, result.Id);
            _mockEmployeeRepository.Verify(x => x.GetEmployeeByIdAsync(employeeId), Times.Once);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_WithVeryLongId_ShouldPassToRepository()
        {
            // Arrange
            var employeeId = new string('a', 1000); // ID muy largo
            var employee = new EmployeeDto { Id = employeeId };

            _mockEmployeeRepository
                .Setup(x => x.GetEmployeeByIdAsync(employeeId))
                .ReturnsAsync(employee);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(employeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employeeId, result.Id);
            _mockEmployeeRepository.Verify(x => x.GetEmployeeByIdAsync(employeeId), Times.Once);
        }

        [TestMethod]
        public void EmployeeService_Constructor_ShouldAcceptRepository()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();

            // Act
            var service = new EmployeeService(mockRepository.Object);

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void EmployeeService_Constructor_WithNullRepository_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new EmployeeService(null));
        }
    }
}
