using Microsoft.VisualStudio.TestTools.UnitTesting;
using msusersgraphql.Models.Dtos;
using System.Collections.Generic;

namespace msusersgraphql.Tests.Models.Dtos
{
    [TestClass]
    public class ContactDtoTests
    {
        [TestMethod]
        public void ContactDto_Constructor_ShouldInitializeWithNullValues()
        {
            // Arrange & Act
            var contactDto = new ContactDto();

            // Assert
            Assert.IsNull(contactDto.Id);
            Assert.IsNull(contactDto.IdPerson);
            Assert.IsNull(contactDto.Email);
            Assert.IsNull(contactDto.Phone);
            Assert.IsNull(contactDto.Address);
            Assert.IsNull(contactDto.CellPhone);
            Assert.IsNull(contactDto.Country);
            Assert.IsNull(contactDto.Department);
            Assert.IsNull(contactDto.Locality);
            Assert.IsNull(contactDto.Neighborhood);
            Assert.IsNull(contactDto.City);
        }

        [TestMethod]
        public void ContactDto_SetBasicProperties_ShouldStoreValuesCorrectly()
        {
            // Arrange
            var expectedId = "123";
            var expectedIdPerson = "456";
            var expectedEmail = "test@example.com";
            var expectedPhone = "555-1234";

            // Act
            var contactDto = new ContactDto
            {
                Id = expectedId,
                IdPerson = expectedIdPerson,
                Email = expectedEmail,
                Phone = expectedPhone
            };

            // Assert
            Assert.AreEqual(expectedId, contactDto.Id);
            Assert.AreEqual(expectedIdPerson, contactDto.IdPerson);
            Assert.AreEqual(expectedEmail, contactDto.Email);
            Assert.AreEqual(expectedPhone, contactDto.Phone);
        }

        [TestMethod]
        public void ContactDto_SetAddressProperties_ShouldStoreValuesCorrectly()
        {
            // Arrange
            var expectedAddress = "123 Main St";
            var expectedCountry = "Colombia";
            var expectedDepartment = "Antioquia";
            var expectedCity = "Medellín";
            var expectedLocality = "El Poblado";
            var expectedNeighborhood = "Manila";

            // Act
            var contactDto = new ContactDto
            {
                Address = expectedAddress,
                Country = expectedCountry,
                Department = expectedDepartment,
                City = expectedCity,
                Locality = expectedLocality,
                Neighborhood = expectedNeighborhood
            };

            // Assert
            Assert.AreEqual(expectedAddress, contactDto.Address);
            Assert.AreEqual(expectedCountry, contactDto.Country);
            Assert.AreEqual(expectedDepartment, contactDto.Department);
            Assert.AreEqual(expectedCity, contactDto.City);
            Assert.AreEqual(expectedLocality, contactDto.Locality);
            Assert.AreEqual(expectedNeighborhood, contactDto.Neighborhood);
        }

        [TestMethod]
        public void ContactDto_SetCellPhone_ShouldStoreCellPhoneCorrectly()
        {
            // Arrange
            var expectedCellPhone = "300-123-4567";
            var contactDto = new ContactDto();

            // Act
            contactDto.CellPhone = expectedCellPhone;

            // Assert
            Assert.AreEqual(expectedCellPhone, contactDto.CellPhone);
        }

        [TestMethod]
        public void ContactDto_SetAllProperties_ShouldStoreAllValuesCorrectly()
        {
            // Arrange
            var contactDto = new ContactDto();

            // Act
            contactDto.Id = "1";
            contactDto.IdPerson = "2";
            contactDto.Email = "contact@test.com";
            contactDto.Phone = "123-456-7890";
            contactDto.Address = "Calle 123 #45-67";
            contactDto.CellPhone = "321-654-9870";
            contactDto.Country = "Colombia";
            contactDto.Department = "Cundinamarca";
            contactDto.Locality = "Bogotá";
            contactDto.Neighborhood = "Chapinero";
            contactDto.City = "Bogotá D.C.";

            // Assert
            Assert.AreEqual("1", contactDto.Id);
            Assert.AreEqual("2", contactDto.IdPerson);
            Assert.AreEqual("contact@test.com", contactDto.Email);
            Assert.AreEqual("123-456-7890", contactDto.Phone);
            Assert.AreEqual("Calle 123 #45-67", contactDto.Address);
            Assert.AreEqual("321-654-9870", contactDto.CellPhone);
            Assert.AreEqual("Colombia", contactDto.Country);
            Assert.AreEqual("Cundinamarca", contactDto.Department);
            Assert.AreEqual("Bogotá", contactDto.Locality);
            Assert.AreEqual("Chapinero", contactDto.Neighborhood);
            Assert.AreEqual("Bogotá D.C.", contactDto.City);
        }
    }

    [TestClass]
    public class ContactListDtoTests
    {
        [TestMethod]
        public void ContactListDto_Constructor_ShouldInitializeDataCollectionAndPaginationProperties()
        {
            // Arrange & Act
            var contactListDto = new ContactListDto();

            // Assert
            Assert.IsNotNull(contactListDto.Data);
            Assert.AreEqual(0, contactListDto.Data.Count);
            Assert.AreEqual(0, contactListDto.TotalCount);
            Assert.AreEqual(0, contactListDto.PageNumber);
            Assert.AreEqual(0, contactListDto.PageSize);
            Assert.IsInstanceOfType(contactListDto.Data, typeof(List<ContactDto>));
        }

        [TestMethod]
        public void ContactListDto_AddContacts_ShouldAddContactsToDataCollection()
        {
            // Arrange
            var contactListDto = new ContactListDto();
            var contacts = new List<ContactDto>
            {
                new ContactDto { Id = "1", Email = "contact1@test.com" },
                new ContactDto { Id = "2", Email = "contact2@test.com" }
            };

            // Act
            contactListDto.Data.AddRange(contacts);

            // Assert
            Assert.AreEqual(2, contactListDto.Data.Count);
            CollectionAssert.AreEqual(contacts, contactListDto.Data);
        }

        [TestMethod]
        public void ContactListDto_SetPaginationProperties_ShouldStoreValuesCorrectly()
        {
            // Arrange
            var contactListDto = new ContactListDto();
            var expectedTotalCount = 100;
            var expectedPageNumber = 3;
            var expectedPageSize = 20;

            // Act
            contactListDto.TotalCount = expectedTotalCount;
            contactListDto.PageNumber = expectedPageNumber;
            contactListDto.PageSize = expectedPageSize;

            // Assert
            Assert.AreEqual(expectedTotalCount, contactListDto.TotalCount);
            Assert.AreEqual(expectedPageNumber, contactListDto.PageNumber);
            Assert.AreEqual(expectedPageSize, contactListDto.PageSize);
        }

        [TestMethod]
        public void ContactListDto_SetDataCollection_ShouldReplaceDataCollection()
        {
            // Arrange
            var contactListDto = new ContactListDto();
            var newContacts = new List<ContactDto>
            {
                new ContactDto { Id = "3", Email = "new@test.com" }
            };

            // Act
            contactListDto.Data = newContacts;

            // Assert
            Assert.AreEqual(newContacts, contactListDto.Data);
            Assert.AreEqual(1, contactListDto.Data.Count);
            Assert.AreEqual("3", contactListDto.Data[0].Id);
        }
    }
}
