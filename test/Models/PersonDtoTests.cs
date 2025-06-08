using Microsoft.VisualStudio.TestTools.UnitTesting;
using msusersgraphql.Models.Dtos;
using System.Collections.Generic;

namespace msusersgraphql.Tests.Models.Dtos
{
[TestClass]
public class PersonDtoTests
{
    [TestMethod]
    public void PersonDto_Constructor_ShouldInitializeWithNullValues()
    {
        // Arrange & Act
        var personDto = new PersonDto();

        // Assert
        Assert.IsNull(personDto.Id);
        Assert.IsNull(personDto.DocumentType);
        Assert.IsNull(personDto.DocumentNumber);
        Assert.IsNull(personDto.FirstName);
        Assert.IsNull(personDto.MiddleName);
        Assert.IsNull(personDto.LastName);
        Assert.IsNull(personDto.SecondLastName);
    }

    [TestMethod]
    public void PersonDto_SetBasicProperties_ShouldStoreValuesCorrectly()
    {
        // Arrange
        var expectedId = "person123";
        var expectedDocumentType = "CC";
        var expectedDocumentNumber = "12345678";

        // Act
        var personDto = new PersonDto
        {
            Id = expectedId,
            DocumentType = expectedDocumentType,
            DocumentNumber = expectedDocumentNumber
        };

        // Assert
        Assert.AreEqual(expectedId, personDto.Id);
        Assert.AreEqual(expectedDocumentType, personDto.DocumentType);
        Assert.AreEqual(expectedDocumentNumber, personDto.DocumentNumber);
    }

    [TestMethod]
    public void PersonDto_SetNameProperties_ShouldStoreValuesCorrectly()
    {
        // Arrange
        var expectedFirstName = "Juan";
        var expectedMiddleName = "Carlos";
        var expectedLastName = "Pérez";
        var expectedSecondLastName = "García";

        // Act
        var personDto = new PersonDto
        {
            FirstName = expectedFirstName,
            MiddleName = expectedMiddleName,
            LastName = expectedLastName,
            SecondLastName = expectedSecondLastName
        };

        // Assert
        Assert.AreEqual(expectedFirstName, personDto.FirstName);
        Assert.AreEqual(expectedMiddleName, personDto.MiddleName);
        Assert.AreEqual(expectedLastName, personDto.LastName);
        Assert.AreEqual(expectedSecondLastName, personDto.SecondLastName);
    }

    [TestMethod]
    public void PersonDto_SetAllProperties_ShouldStoreAllValuesCorrectly()
    {
        // Arrange & Act
        var personDto = new PersonDto
        {
            Id = "1",
            DocumentType = "CC",
            DocumentNumber = "87654321",
            FirstName = "María",
            MiddleName = "Elena",
            LastName = "Rodríguez",
            SecondLastName = "López"
        };

        // Assert
        Assert.AreEqual("1", personDto.Id);
        Assert.AreEqual("CC", personDto.DocumentType);
        Assert.AreEqual("87654321", personDto.DocumentNumber);
        Assert.AreEqual("María", personDto.FirstName);
        Assert.AreEqual("Elena", personDto.MiddleName);
        Assert.AreEqual("Rodríguez", personDto.LastName);
        Assert.AreEqual("López", personDto.SecondLastName);
    }

    [TestMethod]
    public void PersonDto_SetOnlyRequiredProperties_ShouldAllowPartialData()
    {
        // Arrange & Act
        var personDto = new PersonDto
        {
            Id = "2",
            DocumentType = "TI",
            DocumentNumber = "98765432",
            FirstName = "Ana",
            LastName = "Martínez"
        };

        // Assert
        Assert.AreEqual("2", personDto.Id);
        Assert.AreEqual("TI", personDto.DocumentType);
        Assert.AreEqual("98765432", personDto.DocumentNumber);
        Assert.AreEqual("Ana", personDto.FirstName);
        Assert.AreEqual("Martínez", personDto.LastName);
        Assert.IsNull(personDto.MiddleName);
        Assert.IsNull(personDto.SecondLastName);
    }
}

[TestClass]
public class PersonListDtoTests
{
    [TestMethod]
    public void PersonListDto_Constructor_ShouldInitializeDataCollectionAndPaginationProperties()
    {
        // Arrange & Act
        var personListDto = new PersonListDto();

        // Assert
        Assert.IsNotNull(personListDto.Data);
        Assert.AreEqual(0, personListDto.Data.Count);
        Assert.AreEqual(0, personListDto.TotalCount);
        Assert.AreEqual(0, personListDto.PageNumber);
        Assert.AreEqual(0, personListDto.PageSize);
        Assert.IsInstanceOfType(personListDto.Data, typeof(List<PersonDto>));
    }

    [TestMethod]
    public void PersonListDto_AddPersons_ShouldAddPersonsToDataCollection()
    {
        // Arrange
        var personListDto = new PersonListDto();
        var persons = new List<PersonDto>
        {
            new PersonDto { Id = "1", FirstName = "Pedro", LastName = "González" },
            new PersonDto { Id = "2", FirstName = "Laura", LastName = "Fernández" }
        };

        // Act
        personListDto.Data.AddRange(persons);

        // Assert
        Assert.AreEqual(2, personListDto.Data.Count);
        CollectionAssert.AreEqual(persons, personListDto.Data);
    }

    [TestMethod]
    public void PersonListDto_SetPaginationProperties_ShouldStoreValuesCorrectly()
    {
        // Arrange
        var personListDto = new PersonListDto();

        // Act
        personListDto.TotalCount = 75;
        personListDto.PageNumber = 3;
        personListDto.PageSize = 25;

        // Assert
        Assert.AreEqual(75, personListDto.TotalCount);
        Assert.AreEqual(3, personListDto.PageNumber);
        Assert.AreEqual(25, personListDto.PageSize);
    }
}

[TestClass]
public class PersonDataDtoTests
{
    [TestMethod]
    public void PersonDataDto_Constructor_ShouldInitializeItemsCollectionAndPaginationProperties()
    {
        // Arrange & Act
        var personDataDto = new PersonDataDto();

        // Assert
        Assert.IsNotNull(personDataDto.Items);
        Assert.AreEqual(0, personDataDto.Items.Count);
        Assert.AreEqual(0, personDataDto.TotalCount);
        Assert.AreEqual(0, personDataDto.PageNumber);
        Assert.AreEqual(0, personDataDto.PageSize);
        Assert.IsInstanceOfType(personDataDto.Items, typeof(List<PersonDto>));
    }

    [TestMethod]
    public void PersonDataDto_AddPersons_ShouldAddPersonsToItemsCollection()
    {
        // Arrange
        var personDataDto = new PersonDataDto();
        var persons = new List<PersonDto>
        {
            new PersonDto { Id = "1", DocumentType = "CC", DocumentNumber = "11111111" },
            new PersonDto { Id = "2", DocumentType = "CE", DocumentNumber = "22222222" }
        };

        // Act
        personDataDto.Items.AddRange(persons);

        // Assert
        Assert.AreEqual(2, personDataDto.Items.Count);
        CollectionAssert.AreEqual(persons, personDataDto.Items);
    }

    [TestMethod]
    public void PersonDataDto_SetItemsCollection_ShouldReplaceItemsCollection()
    {
        // Arrange
        var personDataDto = new PersonDataDto();
        var newPersons = new List<PersonDto>
        {
            new PersonDto { Id = "3", FirstName = "Carlos", LastName = "Ruiz" }
        };

        // Act
        personDataDto.Items = newPersons;

        // Assert
        Assert.AreEqual(newPersons, personDataDto.Items);
        Assert.AreEqual(1, personDataDto.Items.Count);
        Assert.AreEqual("3", personDataDto.Items[0].Id);
    }

    [TestMethod]
    public void PersonDataDto_SetCompletePersonData_ShouldStoreAllDataCorrectly()
    {
        // Arrange
        var personDataDto = new PersonDataDto();
        var persons = new List<PersonDto>
        {
            new PersonDto
            {
                Id = "1",
                DocumentType = "CC",
                DocumentNumber = "12345678",
                FirstName = "Roberto",
                MiddleName = "Antonio",
                LastName = "Silva",
                SecondLastName = "Mendoza"
            }
        };

        // Act
        personDataDto.Items = persons;
        personDataDto.TotalCount = 1;
        personDataDto.PageNumber = 1;
        personDataDto.PageSize = 10;

        // Assert
        Assert.AreEqual(1, personDataDto.Items.Count);
        Assert.AreEqual("1", personDataDto.Items[0].Id);
        Assert.AreEqual("CC", personDataDto.Items[0].DocumentType);
        Assert.AreEqual("12345678", personDataDto.Items[0].DocumentNumber);
        Assert.AreEqual("Roberto", personDataDto.Items[0].FirstName);
        Assert.AreEqual("Antonio", personDataDto.Items[0].MiddleName);
        Assert.AreEqual("Silva", personDataDto.Items[0].LastName);
        Assert.AreEqual("Mendoza", personDataDto.Items[0].SecondLastName);
        Assert.AreEqual(1, personDataDto.TotalCount);
        Assert.AreEqual(1, personDataDto.PageNumber);
        Assert.AreEqual(10, personDataDto.PageSize);
    }
}

}
