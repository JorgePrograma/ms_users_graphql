using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using msusersgraphql.GraphQL.Types;
using msusersgraphql.Models.Dtos;
using msusersgraphql.Services.Employee;
using msusersgraphql.Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace msusersgraphql.Tests.GraphQL.Types
{
    [TestClass]
    public class UserTypeTests
    {
        private UserType _userType;
        private Mock<IEmployeeService> _mockEmployeeService;
        private IServiceProvider _serviceProvider;

        [TestInitialize]
        public void Setup()
        {
            _mockEmployeeService = new Mock<IEmployeeService>();

            var services = new ServiceCollection();
            services.AddSingleton(_mockEmployeeService.Object);
            _serviceProvider = services.BuildServiceProvider();

            _userType = new UserType();
        }

       /* [TestCleanup]
        public void Cleanup()
        {
            _serviceProvider?.Dispose();
        }
*/
        #region Constructor and Basic Configuration Tests

        [TestMethod]
        public void UserType_Constructor_ShouldInitializeCorrectly()
        {
            // Act
            var userType = new UserType();

            // Assert
            Assert.IsNotNull(userType);
            Assert.AreEqual("User", userType.Name);
        }

        [TestMethod]
        public void UserType_ShouldInheritFromObjectGraphType()
        {
            // Assert
            Assert.IsInstanceOfType(_userType, typeof(ObjectGraphType<UserDto>));
            Assert.IsInstanceOfType(_userType, typeof(ObjectGraphType));
        }

        [TestMethod]
        public void UserType_Name_ShouldBeUser()
        {
            // Assert
            Assert.AreEqual("User", _userType.Name);
        }

        [TestMethod]
        public void UserType_ShouldBeSealed()
        {
            // Assert
            var type = typeof(UserType);
            Assert.IsTrue(type.IsSealed, "UserType should be sealed");
        }

        [TestMethod]
        public void UserType_ShouldHaveAllRequiredFields()
        {
            // Act
            var fields = _userType.Fields.ToList();

            // Assert
            Assert.AreEqual(8, fields.Count); // 6 DTO fields + roles + employee

            var fieldNames = fields.Select(f => f.Name).ToList();
            Assert.IsTrue(fieldNames.Contains("id"));
            Assert.IsTrue(fieldNames.Contains("idUserIDCS"));
            Assert.IsTrue(fieldNames.Contains("avatarPath"));
            Assert.IsTrue(fieldNames.Contains("userName"));
            Assert.IsTrue(fieldNames.Contains("creationDate"));
            Assert.IsTrue(fieldNames.Contains("state"));
            Assert.IsTrue(fieldNames.Contains("roles"));
            Assert.IsTrue(fieldNames.Contains("employee"));
        }

        #endregion

        #region DTO Field Tests

        [TestMethod]
        public void UserType_IdField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _userType.Fields.FirstOrDefault(f => f.Name == "id");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The ID of the user", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void UserType_IdUserIDCSField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _userType.Fields.FirstOrDefault(f => f.Name == "idUserIDCS");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The IDCS ID of the user", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void UserType_AvatarPathField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _userType.Fields.FirstOrDefault(f => f.Name == "avatarPath");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The avatar path of the user", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void UserType_UserNameField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _userType.Fields.FirstOrDefault(f => f.Name == "userName");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The username of the user", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void UserType_CreationDateField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _userType.Fields.FirstOrDefault(f => f.Name == "creationDate");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The creation date of the user", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(DateTimeGraphType));
        }

        [TestMethod]
        public void UserType_StateField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _userType.Fields.FirstOrDefault(f => f.Name == "state");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The state of the user", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void UserType_RolesField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _userType.Fields.FirstOrDefault(f => f.Name == "roles");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The roles of the user", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(ListGraphType<UserRoleType>));
        }

        #endregion

        #region Employee Field Resolver Tests

        [TestMethod]
        public void UserType_EmployeeField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _userType.Fields.FirstOrDefault(f => f.Name == "employee");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("Employee data for this user", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(EmployeeType));
            Assert.IsNotNull(field.Resolver);
        }

        [TestMethod]
        public async Task UserType_EmployeeField_WithValidUserId_ShouldReturnEmployee()
        {
            // Arrange
            var user = new UserDto { Id = "user123" };
            var expectedEmployee = new EmployeeDto 
            { 
                Id = "emp123", 
                IdUser = "user123",
                BussinesEmail = "employee@company.com"
            };

            _mockEmployeeService
                .Setup(x => x.GetEmployeeByIdAsync("user123"))
                .ReturnsAsync(expectedEmployee);

            var context = CreateResolveFieldContext(user);

            // Act
            var employeeField = _userType.Fields.FirstOrDefault(f => f.Name == "employee");
            var result = await employeeField.Resolver.ResolveAsync(context) as EmployeeDto;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployee.Id, result.Id);
            Assert.AreEqual(expectedEmployee.IdUser, result.IdUser);
            _mockEmployeeService.Verify(x => x.GetEmployeeByIdAsync("user123"), Times.Once);
        }

        [TestMethod]
        public async Task UserType_EmployeeField_WhenServiceThrowsException_ShouldReturnNull()
        {
            // Arrange
            var user = new UserDto { Id = "user123" };
            
            _mockEmployeeService
                .Setup(x => x.GetEmployeeByIdAsync("user123"))
                .ThrowsAsync(new Exception("Service error"));

            var context = CreateResolveFieldContext(user);

            // Act
            var employeeField = _userType.Fields.FirstOrDefault(f => f.Name == "employee");
            var result = await employeeField.Resolver.ResolveAsync(context);

            // Assert
            Assert.IsNull(result);
            _mockEmployeeService.Verify(x => x.GetEmployeeByIdAsync("user123"), Times.Once);
        }

        [TestMethod]
        public async Task UserType_EmployeeField_WhenServiceReturnsNull_ShouldReturnNull()
        {
            // Arrange
            var user = new UserDto { Id = "user123" };
            
            _mockEmployeeService
                .Setup(x => x.GetEmployeeByIdAsync("user123"))
                .ReturnsAsync((EmployeeDto)null);

            var context = CreateResolveFieldContext(user);

            // Act
            var employeeField = _userType.Fields.FirstOrDefault(f => f.Name == "employee");
            var result = await employeeField.Resolver.ResolveAsync(context);

            // Assert
            Assert.IsNull(result);
            _mockEmployeeService.Verify(x => x.GetEmployeeByIdAsync("user123"), Times.Once);
        }

        #endregion

        #region Integration Tests

        [TestMethod]
        public void UserType_ShouldMapAllUserDtoProperties()
        {
            // Arrange
            var userDtoProperties = typeof(UserDto).GetProperties()
                .Where(p => p.Name != "Roles") // Roles se mapea por separado
                .Select(p => p.Name)
                .ToList();

            var userTypeFields = _userType.Fields
                .Where(f => f.Name != "roles" && f.Name != "employee") // Exclude special fields
                .Select(f => f.Name)
                .ToList();

            // Act & Assert
            foreach (var property in userDtoProperties)
            {
                var fieldName = char.ToLowerInvariant(property[0]) + property.Substring(1); // camelCase
                Assert.IsTrue(userTypeFields.Contains(fieldName), 
                    $"UserType should have field for property {property}");
            }
        }

        [TestMethod]
        public void UserType_AllDtoFields_ShouldHaveDescriptions()
        {
            // Act
            var dtoFields = _userType.Fields
                .Where(f => f.Name != "employee") // Exclude related field
                .ToList();

            // Assert
            foreach (var field in dtoFields)
            {
                Assert.IsNotNull(field.Description, $"Field {field.Name} should have a description");
                Assert.IsFalse(string.IsNullOrEmpty(field.Description), 
                    $"Field {field.Name} description should not be empty");
            }
        }

        #endregion

        #region Helper Methods

        private IResolveFieldContext CreateResolveFieldContext(UserDto user)
        {
            var context = new Mock<IResolveFieldContext>();
            context.Setup(x => x.RequestServices).Returns(_serviceProvider);
            context.Setup(x => x.Source).Returns(user);
            return context.Object;
        }

        #endregion
    }
}
