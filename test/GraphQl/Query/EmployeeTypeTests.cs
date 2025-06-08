using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using msusersgraphql.GraphQL.Types;
using msusersgraphql.Models.Dtos;
using msusersgraphql.Services.Contact;
using msusersgraphql.Services.Person;
using msusersgraphql.Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace msusersgraphql.Tests.GraphQL.Types
{
    [TestClass]
    public class EmployeeTypeTests
    {
        private EmployeeType _employeeType;
        private Mock<IPersonService> _mockPersonService;
        private Mock<IContactService> _mockContactService;
        private IServiceProvider _serviceProvider;

        [TestInitialize]
        public void Setup()
        {
            _mockPersonService = new Mock<IPersonService>();
            _mockContactService = new Mock<IContactService>();

            var services = new ServiceCollection();
            services.AddSingleton(_mockPersonService.Object);
            services.AddSingleton(_mockContactService.Object);
            _serviceProvider = services.BuildServiceProvider();

            _employeeType = new EmployeeType();
        }

       /* [TestCleanup]
        public void Cleanup()
        {
            _serviceProvider?.Dispose();
        }
*/
        #region Constructor and Basic Configuration Tests

        [TestMethod]
        public void EmployeeType_Constructor_ShouldInitializeCorrectly()
        {
            // Act
            var employeeType = new EmployeeType();

            // Assert
            Assert.IsNotNull(employeeType);
            Assert.AreEqual("Employee", employeeType.Name);
        }

        [TestMethod]
        public void EmployeeType_ShouldInheritFromObjectGraphType()
        {
            // Assert
            Assert.IsInstanceOfType(_employeeType, typeof(ObjectGraphType<EmployeeDto>));
            Assert.IsInstanceOfType(_employeeType, typeof(ObjectGraphType));
        }

        [TestMethod]
        public void EmployeeType_Name_ShouldBeEmployee()
        {
            // Assert
            Assert.AreEqual("Employee", _employeeType.Name);
        }

        [TestMethod]
        public void EmployeeType_ShouldBeSealed()
        {
            // Assert
            var type = typeof(EmployeeType);
            Assert.IsTrue(type.IsSealed, "EmployeeType should be sealed");
        }

        [TestMethod]
        public void EmployeeType_ShouldHaveAllRequiredFields()
        {
            // Act
            var fields = _employeeType.Fields.ToList();

            // Assert
            Assert.AreEqual(9, fields.Count); // 7 DTO fields + 2 related fields

            var fieldNames = fields.Select(f => f.Name).ToList();
            Assert.IsTrue(fieldNames.Contains("id"));
            Assert.IsTrue(fieldNames.Contains("idPerson"));
            Assert.IsTrue(fieldNames.Contains("idUser"));
            Assert.IsTrue(fieldNames.Contains("bussinesEmail"));
            Assert.IsTrue(fieldNames.Contains("bussinesPhone"));
            Assert.IsTrue(fieldNames.Contains("idPosition"));
            Assert.IsTrue(fieldNames.Contains("idBranch"));
            Assert.IsTrue(fieldNames.Contains("person"));
            Assert.IsTrue(fieldNames.Contains("contact"));
        }

        #endregion

        #region DTO Field Tests

        [TestMethod]
        public void EmployeeType_IdField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _employeeType.Fields.FirstOrDefault(f => f.Name == "id");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The ID of the employee", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void EmployeeType_IdPersonField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _employeeType.Fields.FirstOrDefault(f => f.Name == "idPerson");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The person ID of the employee", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void EmployeeType_IdUserField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _employeeType.Fields.FirstOrDefault(f => f.Name == "idUser");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The user ID of the employee", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void EmployeeType_BussinesEmailField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _employeeType.Fields.FirstOrDefault(f => f.Name == "bussinesEmail");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The business email of the employee", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void EmployeeType_BussinesPhoneField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _employeeType.Fields.FirstOrDefault(f => f.Name == "bussinesPhone");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The business phone of the employee", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void EmployeeType_IdPositionField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _employeeType.Fields.FirstOrDefault(f => f.Name == "idPosition");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The position ID of the employee", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void EmployeeType_IdBranchField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _employeeType.Fields.FirstOrDefault(f => f.Name == "idBranch");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The branch ID of the employee", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void EmployeeType_NullableFields_ShouldBeConfiguredAsNullable()
        {
            // Act
            var idPositionField = _employeeType.Fields.FirstOrDefault(f => f.Name == "idPosition");
            var idBranchField = _employeeType.Fields.FirstOrDefault(f => f.Name == "idBranch");

            // Assert
            Assert.IsNotNull(idPositionField);
            Assert.IsNotNull(idBranchField);
            Assert.IsInstanceOfType(idPositionField.Type, typeof(StringGraphType));
            Assert.IsInstanceOfType(idBranchField.Type, typeof(StringGraphType));
        }

        #endregion

        #region Related Field Tests

        [TestMethod]
        public void EmployeeType_PersonField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _employeeType.Fields.FirstOrDefault(f => f.Name == "person");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("Person data for this employee", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(PersonType));
            Assert.IsNotNull(field.Resolver);
        }

        [TestMethod]
        public void EmployeeType_ContactField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _employeeType.Fields.FirstOrDefault(f => f.Name == "contact");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("Person data for this employee", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(ContactType));
            Assert.IsNotNull(field.Resolver);
        }

        #endregion

        #region Person Field Resolver Tests

        [TestMethod]
        public async Task EmployeeType_PersonField_WithValidIdPerson_ShouldReturnPerson()
        {
            // Arrange
            var employee = new EmployeeDto { IdPerson = "person123" };
            var expectedPerson = new PersonDto 
            { 
                Id = "person123", 
                FirstName = "Juan", 
                LastName = "Pérez" 
            };

            _mockPersonService
                .Setup(x => x.GetPersonByIdAsync("person123"))
                .ReturnsAsync(expectedPerson);

            var context = CreateResolveFieldContext(employee);

            // Act
            var personField = _employeeType.Fields.FirstOrDefault(f => f.Name == "person");
            var result = await personField.Resolver.ResolveAsync(context) as PersonDto;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPerson.Id, result.Id);
            Assert.AreEqual(expectedPerson.FirstName, result.FirstName);
            _mockPersonService.Verify(x => x.GetPersonByIdAsync("person123"), Times.Once);
        }

        [TestMethod]
        public async Task EmployeeType_PersonField_WithNullIdPerson_ShouldReturnNull()
        {
            // Arrange
            var employee = new EmployeeDto { IdPerson = null };
            var context = CreateResolveFieldContext(employee);

            // Act
            var personField = _employeeType.Fields.FirstOrDefault(f => f.Name == "person");
            var result = await personField.Resolver.ResolveAsync(context);

            // Assert
            Assert.IsNull(result);
            _mockPersonService.Verify(x => x.GetPersonByIdAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task EmployeeType_PersonField_WhenServiceThrowsException_ShouldReturnNull()
        {
            // Arrange
            var employee = new EmployeeDto { IdPerson = "person123" };
            
            _mockPersonService
                .Setup(x => x.GetPersonByIdAsync("person123"))
                .ThrowsAsync(new Exception("Service error"));

            var context = CreateResolveFieldContext(employee);

            // Act
            var personField = _employeeType.Fields.FirstOrDefault(f => f.Name == "person");
            var result = await personField.Resolver.ResolveAsync(context);

            // Assert
            Assert.IsNull(result);
            _mockPersonService.Verify(x => x.GetPersonByIdAsync("person123"), Times.Once);
        }

        [TestMethod]
        public async Task EmployeeType_PersonField_WhenServiceReturnsNull_ShouldReturnNull()
        {
            // Arrange
            var employee = new EmployeeDto { IdPerson = "person123" };
            
            _mockPersonService
                .Setup(x => x.GetPersonByIdAsync("person123"))
                .ReturnsAsync((PersonDto)null);

            var context = CreateResolveFieldContext(employee);

            // Act
            var personField = _employeeType.Fields.FirstOrDefault(f => f.Name == "person");
            var result = await personField.Resolver.ResolveAsync(context);

            // Assert
            Assert.IsNull(result);
            _mockPersonService.Verify(x => x.GetPersonByIdAsync("person123"), Times.Once);
        }

        #endregion

        #region Contact Field Resolver Tests

        [TestMethod]
        public async Task EmployeeType_ContactField_WithValidIdPerson_ShouldReturnContact()
        {
            // Arrange
            var employee = new EmployeeDto { IdPerson = "person456" };
            var expectedContact = new ContactDto 
            { 
                Id = "contact123", 
                IdPerson = "person456",
                Email = "employee@company.com" 
            };

            _mockContactService
                .Setup(x => x.GetContactByIdPersonAsync("person456"))
                .ReturnsAsync(expectedContact);

            var context = CreateResolveFieldContext(employee);

            // Act
            var contactField = _employeeType.Fields.FirstOrDefault(f => f.Name == "contact");
            var result = await contactField.Resolver.ResolveAsync(context) as ContactDto;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedContact.Id, result.Id);
            Assert.AreEqual(expectedContact.Email, result.Email);
            _mockContactService.Verify(x => x.GetContactByIdPersonAsync("person456"), Times.Once);
        }

        [TestMethod]
        public async Task EmployeeType_ContactField_WithNullIdPerson_ShouldReturnNull()
        {
            // Arrange
            var employee = new EmployeeDto { IdPerson = null };
            var context = CreateResolveFieldContext(employee);

            // Act
            var contactField = _employeeType.Fields.FirstOrDefault(f => f.Name == "contact");
            var result = await contactField.Resolver.ResolveAsync(context);

            // Assert
            Assert.IsNull(result);
            _mockContactService.Verify(x => x.GetContactByIdPersonAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task EmployeeType_ContactField_WhenServiceThrowsException_ShouldReturnNull()
        {
            // Arrange
            var employee = new EmployeeDto { IdPerson = "person456" };
            
            _mockContactService
                .Setup(x => x.GetContactByIdPersonAsync("person456"))
                .ThrowsAsync(new Exception("Service error"));

            var context = CreateResolveFieldContext(employee);

            // Act
            var contactField = _employeeType.Fields.FirstOrDefault(f => f.Name == "contact");
            var result = await contactField.Resolver.ResolveAsync(context);

            // Assert
            Assert.IsNull(result);
            _mockContactService.Verify(x => x.GetContactByIdPersonAsync("person456"), Times.Once);
        }

        #endregion

        #region Service Resolution Tests

        [TestMethod]
        public async Task EmployeeType_RelatedFields_ShouldResolveServicesFromDependencyInjection()
        {
            // Arrange
            var employee = new EmployeeDto { IdPerson = "person789" };
            
            _mockPersonService
                .Setup(x => x.GetPersonByIdAsync("person789"))
                .ReturnsAsync(new PersonDto { Id = "person789" });
            
            _mockContactService
                .Setup(x => x.GetContactByIdPersonAsync("person789"))
                .ReturnsAsync(new ContactDto { Id = "contact789" });

            var context = CreateResolveFieldContext(employee);

            // Act
            var personField = _employeeType.Fields.FirstOrDefault(f => f.Name == "person");
            var contactField = _employeeType.Fields.FirstOrDefault(f => f.Name == "contact");

            var personResult = await personField.Resolver.ResolveAsync(context);
            var contactResult = await contactField.Resolver.ResolveAsync(context);

            // Assert
            Assert.IsNotNull(personResult);
            Assert.IsNotNull(contactResult);
            _mockPersonService.Verify(x => x.GetPersonByIdAsync("person789"), Times.Once);
            _mockContactService.Verify(x => x.GetContactByIdPersonAsync("person789"), Times.Once);
        }

        #endregion

        #region Integration Tests

        [TestMethod]
        public void EmployeeType_ShouldMapAllEmployeeDtoProperties()
        {
            // Arrange
            var employeeDtoProperties = typeof(EmployeeDto).GetProperties()
                .Select(p => p.Name)
                .ToList();

            var employeeTypeFields = _employeeType.Fields
                .Where(f => f.Name != "person" && f.Name != "contact") // Exclude related fields
                .Select(f => f.Name)
                .ToList();

            // Act & Assert
            foreach (var property in employeeDtoProperties)
            {
                var fieldName = char.ToLowerInvariant(property[0]) + property.Substring(1); // camelCase
                Assert.IsTrue(employeeTypeFields.Contains(fieldName), 
                    $"EmployeeType should have field for property {property}");
            }
        }

        [TestMethod]
        public void EmployeeType_AllDtoFields_ShouldHaveDescriptions()
        {
            // Act
            var dtoFields = _employeeType.Fields
                .Where(f => f.Name != "person" && f.Name != "contact")
                .ToList();

            // Assert
            foreach (var field in dtoFields)
            {
                Assert.IsNotNull(field.Description, $"Field {field.Name} should have a description");
                Assert.IsFalse(string.IsNullOrEmpty(field.Description), 
                    $"Field {field.Name} description should not be empty");
            }
        }

        [TestMethod]
        public void EmployeeType_AllDtoFields_ShouldBeStringType()
        {
            // Act
            var dtoFields = _employeeType.Fields
                .Where(f => f.Name != "person" && f.Name != "contact")
                .ToList();

            // Assert
            foreach (var field in dtoFields)
            {
                Assert.IsInstanceOfType(field.Type, typeof(StringGraphType), 
                    $"Field {field.Name} should be of type StringGraphType");
            }
        }

        #endregion

        #region Edge Cases Tests

        [TestMethod]
        public void EmployeeType_MultipleInstances_ShouldHaveSameConfiguration()
        {
            // Arrange
            var employeeType1 = new EmployeeType();
            var employeeType2 = new EmployeeType();

            // Act
            var fields1 = employeeType1.Fields.ToList();
            var fields2 = employeeType2.Fields.ToList();

            // Assert
            Assert.AreEqual(fields1.Count, fields2.Count);
            Assert.AreEqual(employeeType1.Name, employeeType2.Name);
            
            for (int i = 0; i < fields1.Count; i++)
            {
                Assert.AreEqual(fields1[i].Name, fields2[i].Name);
                Assert.AreEqual(fields1[i].Description, fields2[i].Description);
                Assert.AreEqual(fields1[i].Type.GetType(), fields2[i].Type.GetType());
            }
        }

        [TestMethod]
        public void EmployeeType_FieldNames_ShouldBeCamelCase()
        {
            // Act
            var fields = _employeeType.Fields.ToList();

            // Assert
            foreach (var field in fields)
            {
                Assert.IsTrue(char.IsLower(field.Name[0]), 
                    $"Field {field.Name} should start with lowercase letter (camelCase)");
            }
        }

        [TestMethod]
        public void EmployeeType_ContactFieldDescription_ShouldBeUpdated()
        {
            // Este test documenta que la descripción del campo contact parece incorrecta
            // Act
            var contactField = _employeeType.Fields.FirstOrDefault(f => f.Name == "contact");

            // Assert
            Assert.AreEqual("Person data for this employee", contactField.Description);
            // Nota: Debería ser "Contact data for this employee"
        }

        #endregion

        #region Helper Methods

        private IResolveFieldContext CreateResolveFieldContext(EmployeeDto employee)
        {
            var context = new Mock<IResolveFieldContext>();
            context.Setup(x => x.RequestServices).Returns(_serviceProvider);
            context.Setup(x => x.Source).Returns(employee);
            return context.Object;
        }

        #endregion
    }
}
