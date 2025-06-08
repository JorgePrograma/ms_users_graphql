using Microsoft.VisualStudio.TestTools.UnitTesting;
using msusersgraphql.Models.Dtos;
using System.Collections.Generic;

namespace msusersgraphql.Tests.Models.Dtos
{
[TestClass]
public class EmployeeDtoTests
{
    [TestMethod]
    public void EmployeeDto_Constructor_ShouldInitializeWithNullValues()
    {
        // Arrange & Act
        var employeeDto = new EmployeeDto();

        // Assert
        Assert.IsNull(employeeDto.Id);
        Assert.IsNull(employeeDto.IdPerson);
        Assert.IsNull(employeeDto.IdUser);
        Assert.IsNull(employeeDto.BussinesEmail);
        Assert.IsNull(employeeDto.BussinesPhone);
        Assert.IsNull(employeeDto.IdPosition);
        Assert.IsNull(employeeDto.IdBranch);
    }

    [TestMethod]
    public void EmployeeDto_SetBasicProperties_ShouldStoreValuesCorrectly()
    {
        // Arrange
        var expectedId = "emp123";
        var expectedIdPerson = "person456";
        var expectedIdUser = "user789";

        // Act
        var employeeDto = new EmployeeDto
        {
            Id = expectedId,
            IdPerson = expectedIdPerson,
            IdUser = expectedIdUser
        };

        // Assert
        Assert.AreEqual(expectedId, employeeDto.Id);
        Assert.AreEqual(expectedIdPerson, employeeDto.IdPerson);
        Assert.AreEqual(expectedIdUser, employeeDto.IdUser);
    }

    [TestMethod]
    public void EmployeeDto_SetBusinessContactInfo_ShouldStoreValuesCorrectly()
    {
        // Arrange
        var expectedBusinessEmail = "employee@company.com";
        var expectedBusinessPhone = "555-0123";

        // Act
        var employeeDto = new EmployeeDto
        {
            BussinesEmail = expectedBusinessEmail,
            BussinesPhone = expectedBusinessPhone
        };

        // Assert
        Assert.AreEqual(expectedBusinessEmail, employeeDto.BussinesEmail);
        Assert.AreEqual(expectedBusinessPhone, employeeDto.BussinesPhone);
    }

    [TestMethod]
    public void EmployeeDto_SetPositionAndBranchIds_ShouldStoreValuesCorrectly()
    {
        // Arrange
        var expectedIdPosition = "pos123";
        var expectedIdBranch = "branch456";

        // Act
        var employeeDto = new EmployeeDto
        {
            IdPosition = expectedIdPosition,
            IdBranch = expectedIdBranch
        };

        // Assert
        Assert.AreEqual(expectedIdPosition, employeeDto.IdPosition);
        Assert.AreEqual(expectedIdBranch, employeeDto.IdBranch);
    }

    [TestMethod]
    public void EmployeeDto_SetAllProperties_ShouldStoreAllValuesCorrectly()
    {
        // Arrange & Act
        var employeeDto = new EmployeeDto
        {
            Id = "1",
            IdPerson = "2",
            IdUser = "3",
            BussinesEmail = "emp@company.com",
            BussinesPhone = "123-456-7890",
            IdPosition = "4",
            IdBranch = "5"
        };

        // Assert
        Assert.AreEqual("1", employeeDto.Id);
        Assert.AreEqual("2", employeeDto.IdPerson);
        Assert.AreEqual("3", employeeDto.IdUser);
        Assert.AreEqual("emp@company.com", employeeDto.BussinesEmail);
        Assert.AreEqual("123-456-7890", employeeDto.BussinesPhone);
        Assert.AreEqual("4", employeeDto.IdPosition);
        Assert.AreEqual("5", employeeDto.IdBranch);
    }
}

[TestClass]
public class EmployeeListDtoTests
{
    [TestMethod]
    public void EmployeeListDto_Constructor_ShouldInitializeDataCollectionAndPaginationProperties()
    {
        // Arrange & Act
        var employeeListDto = new EmployeeListDto();

        // Assert
        Assert.IsNotNull(employeeListDto.Data);
        Assert.AreEqual(0, employeeListDto.Data.Count);
        Assert.AreEqual(0, employeeListDto.TotalCount);
        Assert.AreEqual(0, employeeListDto.PageNumber);
        Assert.AreEqual(0, employeeListDto.PageSize);
        Assert.IsInstanceOfType(employeeListDto.Data, typeof(List<EmployeeDto>));
    }

    [TestMethod]
    public void EmployeeListDto_AddEmployees_ShouldAddEmployeesToDataCollection()
    {
        // Arrange
        var employeeListDto = new EmployeeListDto();
        var employees = new List<EmployeeDto>
        {
            new EmployeeDto { Id = "1", BussinesEmail = "emp1@company.com" },
            new EmployeeDto { Id = "2", BussinesEmail = "emp2@company.com" }
        };

        // Act
        employeeListDto.Data.AddRange(employees);

        // Assert
        Assert.AreEqual(2, employeeListDto.Data.Count);
        CollectionAssert.AreEqual(employees, employeeListDto.Data);
    }

    [TestMethod]
    public void EmployeeListDto_SetPaginationProperties_ShouldStoreValuesCorrectly()
    {
        // Arrange
        var employeeListDto = new EmployeeListDto();

        // Act
        employeeListDto.TotalCount = 50;
        employeeListDto.PageNumber = 2;
        employeeListDto.PageSize = 15;

        // Assert
        Assert.AreEqual(50, employeeListDto.TotalCount);
        Assert.AreEqual(2, employeeListDto.PageNumber);
        Assert.AreEqual(15, employeeListDto.PageSize);
    }
}

[TestClass]
public class EmployeeDataDtoTests
{
    [TestMethod]
    public void EmployeeDataDto_Constructor_ShouldInitializeItemsCollectionAndPaginationProperties()
    {
        // Arrange & Act
        var employeeDataDto = new EmployeeDataDto();

        // Assert
        Assert.IsNotNull(employeeDataDto.Items);
        Assert.AreEqual(0, employeeDataDto.Items.Count);
        Assert.AreEqual(0, employeeDataDto.TotalCount);
        Assert.AreEqual(0, employeeDataDto.PageNumber);
        Assert.AreEqual(0, employeeDataDto.PageSize);
        Assert.IsInstanceOfType(employeeDataDto.Items, typeof(List<EmployeeDto>));
    }

    [TestMethod]
    public void EmployeeDataDto_AddEmployees_ShouldAddEmployeesToItemsCollection()
    {
        // Arrange
        var employeeDataDto = new EmployeeDataDto();
        var employees = new List<EmployeeDto>
        {
            new EmployeeDto { Id = "1", IdPosition = "mgr" },
            new EmployeeDto { Id = "2", IdPosition = "dev" }
        };

        // Act
        employeeDataDto.Items.AddRange(employees);

        // Assert
        Assert.AreEqual(2, employeeDataDto.Items.Count);
        CollectionAssert.AreEqual(employees, employeeDataDto.Items);
    }

    [TestMethod]
    public void EmployeeDataDto_SetItemsCollection_ShouldReplaceItemsCollection()
    {
        // Arrange
        var employeeDataDto = new EmployeeDataDto();
        var newEmployees = new List<EmployeeDto>
        {
            new EmployeeDto { Id = "3", IdBranch = "branch1" }
        };

        // Act
        employeeDataDto.Items = newEmployees;

        // Assert
        Assert.AreEqual(newEmployees, employeeDataDto.Items);
        Assert.AreEqual(1, employeeDataDto.Items.Count);
        Assert.AreEqual("3", employeeDataDto.Items[0].Id);
    }
}

}
