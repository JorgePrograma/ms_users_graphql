using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using msusersgraphql.Models.Dtos;
using msusersgraphql.Repositories.Person;
using msusersgraphql.Services.Person;
using System;
using System.Threading.Tasks;

namespace msusersgraphql.Tests.Services.Person
{
    [TestClass]
    public class PersonServiceTests
    {
        private Mock<IPersonRepository> _mockPersonRepository;
        private PersonService _personService;

        [TestInitialize]
        public void Setup()
        {
            _mockPersonRepository = new Mock<IPersonRepository>();
            _personService = new PersonService(_mockPersonRepository.Object);
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithValidId_ShouldReturnPerson()
        {
            // Arrange
            var personId = "person123";
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

            _mockPersonRepository
                .Setup(x => x.GetPersonByIdAsync(personId))
                .ReturnsAsync(expectedPerson);

            // Act
            var result = await _personService.GetPersonByIdAsync(personId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPerson.Id, result.Id);
            Assert.AreEqual(expectedPerson.DocumentType, result.DocumentType);
            Assert.AreEqual(expectedPerson.DocumentNumber, result.DocumentNumber);
            Assert.AreEqual(expectedPerson.FirstName, result.FirstName);
            Assert.AreEqual(expectedPerson.MiddleName, result.MiddleName);
            Assert.AreEqual(expectedPerson.LastName, result.LastName);
            Assert.AreEqual(expectedPerson.SecondLastName, result.SecondLastName);

            // Verificar que se llamó al repositorio
            _mockPersonRepository.Verify(x => x.GetPersonByIdAsync(personId), Times.Once);
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithValidId_ShouldCallRepositoryOnce()
        {
            // Arrange
            var personId = "person456";
            var person = new PersonDto { Id = personId, FirstName = "Test" };

            _mockPersonRepository
                .Setup(x => x.GetPersonByIdAsync(personId))
                .ReturnsAsync(person);

            // Act
            await _personService.GetPersonByIdAsync(personId);

            // Assert
            _mockPersonRepository.Verify(x => x.GetPersonByIdAsync(personId), Times.Once);
            _mockPersonRepository.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WhenRepositoryReturnsNull_ShouldReturnNull()
        {
            // Arrange
            var personId = "nonexistent";

            _mockPersonRepository
                .Setup(x => x.GetPersonByIdAsync(personId))
                .ReturnsAsync((PersonDto)null);

            // Act
            var result = await _personService.GetPersonByIdAsync(personId);

            // Assert
            Assert.IsNull(result);
            _mockPersonRepository.Verify(x => x.GetPersonByIdAsync(personId), Times.Once);
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithNullId_ShouldThrowArgumentException()
        {
            // Arrange
            string personId = null;

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _personService.GetPersonByIdAsync(personId));

            Assert.AreEqual("Person ID cannot be empty", exception.Message);
            _mockPersonRepository.Verify(x => x.GetPersonByIdAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithEmptyId_ShouldThrowArgumentException()
        {
            // Arrange
            var personId = "";

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _personService.GetPersonByIdAsync(personId));

            Assert.AreEqual("Person ID cannot be empty", exception.Message);
            _mockPersonRepository.Verify(x => x.GetPersonByIdAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithWhitespaceId_ShouldThrowArgumentException()
        {
            // Arrange
            var personId = "   ";

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _personService.GetPersonByIdAsync(personId));

            Assert.AreEqual("Person ID cannot be empty", exception.Message);
            _mockPersonRepository.Verify(x => x.GetPersonByIdAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithTabsAndSpacesId_ShouldThrowArgumentException()
        {
            // Arrange
            var personId = " \t \n ";

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _personService.GetPersonByIdAsync(personId));

            Assert.AreEqual("Person ID cannot be empty", exception.Message);
            _mockPersonRepository.Verify(x => x.GetPersonByIdAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            var personId = "error";
            var expectedException = new InvalidOperationException("Repository error");

            _mockPersonRepository
                .Setup(x => x.GetPersonByIdAsync(personId))
                .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => _personService.GetPersonByIdAsync(personId));

            Assert.AreEqual("Repository error", exception.Message);
            _mockPersonRepository.Verify(x => x.GetPersonByIdAsync(personId), Times.Once);
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithSpecialCharactersInId_ShouldPassToRepository()
        {
            // Arrange
            var personId = "person@123-456_789";
            var person = new PersonDto { Id = personId, FirstName = "Test" };

            _mockPersonRepository
                .Setup(x => x.GetPersonByIdAsync(personId))
                .ReturnsAsync(person);

            // Act
            var result = await _personService.GetPersonByIdAsync(personId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(personId, result.Id);
            _mockPersonRepository.Verify(x => x.GetPersonByIdAsync(personId), Times.Once);
        }

        [TestMethod]
        public async Task GetPersonByIdAsync_WithVeryLongId_ShouldPassToRepository()
        {
            // Arrange
            var personId = new string('a', 1000); // ID muy largo
            var person = new PersonDto { Id = personId, FirstName = "Test" };

            _mockPersonRepository
                .Setup(x => x.GetPersonByIdAsync(personId))
                .ReturnsAsync(person);

            // Act
            var result = await _personService.GetPersonByIdAsync(personId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(personId, result.Id);
            _mockPersonRepository.Verify(x => x.GetPersonByIdAsync(personId), Times.Once);
        }

        [TestMethod]
        public void PersonService_Constructor_ShouldAcceptRepository()
        {
            // Arrange
            var mockRepository = new Mock<IPersonRepository>();

            // Act
            var service = new PersonService(mockRepository.Object);

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void PersonService_Constructor_WithNullRepository_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new PersonService(null));
        }
    }
}
