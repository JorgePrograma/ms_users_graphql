using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using msusersgraphql.Models.Dtos;
using msusersgraphql.Repositories.Contact;
using msusersgraphql.Services.Contact;
using System;
using System.Threading.Tasks;

namespace msusersgraphql.Tests.Services.Contact
{
    [TestClass]
    public class ContactServiceTests
    {
        private Mock<IContactRepository> _mockContactRepository;
        private ContactService _contactService;

        [TestInitialize]
        public void Setup()
        {
            _mockContactRepository = new Mock<IContactRepository>();
            _contactService = new ContactService(_mockContactRepository.Object);
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WithValidId_ShouldReturnContact()
        {
            // Arrange
            var personId = "person123";
            var expectedContact = new ContactDto
            {
                Id = "contact456",
                IdPerson = personId,
                Email = "test@example.com",
                Phone = "555-1234",
                Address = "123 Main St",
                CellPhone = "300-123-4567",
                Country = "Colombia",
                Department = "Antioquia",
                Locality = "Medellín",
                Neighborhood = "El Poblado",
                City = "Medellín"
            };

            _mockContactRepository
                .Setup(x => x.GetContactByIdPersonAsync(personId))
                .ReturnsAsync(expectedContact);

            // Act
            var result = await _contactService.GetContactByIdPersonAsync(personId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedContact.Id, result.Id);
            Assert.AreEqual(expectedContact.IdPerson, result.IdPerson);
            Assert.AreEqual(expectedContact.Email, result.Email);
            Assert.AreEqual(expectedContact.Phone, result.Phone);
            Assert.AreEqual(expectedContact.Address, result.Address);
            Assert.AreEqual(expectedContact.CellPhone, result.CellPhone);
            Assert.AreEqual(expectedContact.Country, result.Country);
            Assert.AreEqual(expectedContact.Department, result.Department);
            Assert.AreEqual(expectedContact.Locality, result.Locality);
            Assert.AreEqual(expectedContact.Neighborhood, result.Neighborhood);
            Assert.AreEqual(expectedContact.City, result.City);

            // Verificar que se llamó al repositorio
            _mockContactRepository.Verify(x => x.GetContactByIdPersonAsync(personId), Times.Once);
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WithValidId_ShouldCallRepositoryOnce()
        {
            // Arrange
            var personId = "person456";
            var contact = new ContactDto { Id = "contact789", IdPerson = personId };

            _mockContactRepository
                .Setup(x => x.GetContactByIdPersonAsync(personId))
                .ReturnsAsync(contact);

            // Act
            await _contactService.GetContactByIdPersonAsync(personId);

            // Assert
            _mockContactRepository.Verify(x => x.GetContactByIdPersonAsync(personId), Times.Once);
            _mockContactRepository.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WhenRepositoryReturnsNull_ShouldReturnNull()
        {
            // Arrange
            var personId = "nonexistent";

            _mockContactRepository
                .Setup(x => x.GetContactByIdPersonAsync(personId))
                .ReturnsAsync((ContactDto)null);

            // Act
            var result = await _contactService.GetContactByIdPersonAsync(personId);

            // Assert
            Assert.IsNull(result);
            _mockContactRepository.Verify(x => x.GetContactByIdPersonAsync(personId), Times.Once);
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WithNullId_ShouldThrowArgumentException()
        {
            // Arrange
            string personId = null;

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _contactService.GetContactByIdPersonAsync(personId));

            Assert.AreEqual("Contact ID cannot be empty", exception.Message);
            _mockContactRepository.Verify(x => x.GetContactByIdPersonAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WithEmptyId_ShouldThrowArgumentException()
        {
            // Arrange
            var personId = "";

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _contactService.GetContactByIdPersonAsync(personId));

            Assert.AreEqual("Contact ID cannot be empty", exception.Message);
            _mockContactRepository.Verify(x => x.GetContactByIdPersonAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WithWhitespaceId_ShouldThrowArgumentException()
        {
            // Arrange
            var personId = "   ";

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _contactService.GetContactByIdPersonAsync(personId));

            Assert.AreEqual("Contact ID cannot be empty", exception.Message);
            _mockContactRepository.Verify(x => x.GetContactByIdPersonAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WithTabsAndSpacesId_ShouldThrowArgumentException()
        {
            // Arrange
            var personId = " \t \n ";

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => _contactService.GetContactByIdPersonAsync(personId));

            Assert.AreEqual("Contact ID cannot be empty", exception.Message);
            _mockContactRepository.Verify(x => x.GetContactByIdPersonAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            var personId = "error";
            var expectedException = new InvalidOperationException("Repository error");

            _mockContactRepository
                .Setup(x => x.GetContactByIdPersonAsync(personId))
                .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => _contactService.GetContactByIdPersonAsync(personId));

            Assert.AreEqual("Repository error", exception.Message);
            _mockContactRepository.Verify(x => x.GetContactByIdPersonAsync(personId), Times.Once);
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WithSpecialCharactersInId_ShouldPassToRepository()
        {
            // Arrange
            var personId = "person@123-456_789";
            var contact = new ContactDto { Id = "contact123", IdPerson = personId };

            _mockContactRepository
                .Setup(x => x.GetContactByIdPersonAsync(personId))
                .ReturnsAsync(contact);

            // Act
            var result = await _contactService.GetContactByIdPersonAsync(personId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(personId, result.IdPerson);
            _mockContactRepository.Verify(x => x.GetContactByIdPersonAsync(personId), Times.Once);
        }

        [TestMethod]
        public async Task GetContactByIdPersonAsync_WithVeryLongId_ShouldPassToRepository()
        {
            // Arrange
            var personId = new string('a', 1000); // ID muy largo
            var contact = new ContactDto { Id = "contact123", IdPerson = personId };

            _mockContactRepository
                .Setup(x => x.GetContactByIdPersonAsync(personId))
                .ReturnsAsync(contact);

            // Act
            var result = await _contactService.GetContactByIdPersonAsync(personId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(personId, result.IdPerson);
            _mockContactRepository.Verify(x => x.GetContactByIdPersonAsync(personId), Times.Once);
        }

        [TestMethod]
        public void ContactService_Constructor_ShouldAcceptRepository()
        {
            // Arrange
            var mockRepository = new Mock<IContactRepository>();

            // Act
            var service = new ContactService(mockRepository.Object);

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void ContactService_Constructor_WithNullRepository_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new ContactService(null));
        }
    }
}
