using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using msusersgraphql.GraphQL.Types;
using msusersgraphql.Models.Dtos;
using msusersgraphql.Models.GraphQL;
using msusersgraphql.Services.Contact;
using msusersgraphql.Services.Employee;
using msusersgraphql.Services.Person;
using msusersgraphql.Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace msusersgraphql.Tests.Models.GraphQL
{
    [TestClass]
    public class UserQueryTests
    {
        private UserQuery _userQuery;
        private Mock<IUserService> _mockUserService;
        private Mock<IEmployeeService> _mockEmployeeService;
        private Mock<IPersonService> _mockPersonService;
        private Mock<IContactService> _mockContactService;
        private IServiceCollection _services;
        private IServiceProvider _serviceProvider;

        [TestInitialize]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _mockEmployeeService = new Mock<IEmployeeService>();
            _mockPersonService = new Mock<IPersonService>();
            _mockContactService = new Mock<IContactService>();

            _services = new ServiceCollection();
            _services.AddSingleton(_mockUserService.Object);
            _services.AddSingleton(_mockEmployeeService.Object);
            _services.AddSingleton(_mockPersonService.Object);
            _services.AddSingleton(_mockContactService.Object);
            
            _serviceProvider = _services.BuildServiceProvider();
            _userQuery = new UserQuery();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _serviceProvider?.Dispose();
        }

        #region Constructor Tests

        [TestMethod]
        public void UserQuery_Constructor_ShouldInitializeAllFields()
        {
            // Act
            var userQuery = new UserQuery();

            // Assert
            Assert.IsNotNull(userQuery);
            
            // Verificar que los campos están definidos
            var fields = userQuery.Fields.ToList();
            Assert.AreEqual(5, fields.Count);
            
            Assert.IsTrue(fields.Any(f => f.Name == "user"));
            Assert.IsTrue(fields.Any(f => f.Name == "users"));
            Assert.IsTrue(fields.Any(f => f.Name == "employee"));
            Assert.IsTrue(fields.Any(f => f.Name == "person"));
            Assert.IsTrue(fields.Any(f => f.Name == "contact"));
        }

        [TestMethod]
        public void UserQuery_UserField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var userField = _userQuery.Fields.FirstOrDefault(f => f.Name == "user");

            // Assert
            Assert.IsNotNull(userField);
            Assert.AreEqual("Get a user by ID", userField.Description);
            Assert.IsInstanceOfType(userField.Type, typeof(UserType));
            
            // Verificar argumentos
            var arguments = userField.Arguments?.ToList();
            Assert.IsNotNull(arguments);
            Assert.AreEqual(1, arguments.Count);
            Assert.AreEqual("id", arguments[0].Name);
            Assert.IsInstanceOfType(arguments[0].Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void UserQuery_UsersField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var usersField = _userQuery.Fields.FirstOrDefault(f => f.Name == "users");

            // Assert
            Assert.IsNotNull(usersField);
            Assert.AreEqual("Get a paginated list of users", usersField.Description);
            Assert.IsInstanceOfType(usersField.Type, typeof(UserListType));
            
            // Verificar argumentos
            var arguments = usersField.Arguments?.ToList();
            Assert.IsNotNull(arguments);
            Assert.AreEqual(2, arguments.Count);
            Assert.IsTrue(arguments.Any(a => a.Name == "pageNumber"));
            Assert.IsTrue(arguments.Any(a => a.Name == "pageSize"));
        }

        [TestMethod]
        public void UserQuery_EmployeeField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var employeeField = _userQuery.Fields.FirstOrDefault(f => f.Name == "employee");

            // Assert
            Assert.IsNotNull(employeeField);
            Assert.AreEqual("Get an employee by ID", employeeField.Description);
            Assert.IsInstanceOfType(employeeField.Type, typeof(EmployeeType));
            
            // Verificar argumentos
            var arguments = employeeField.Arguments?.ToList();
            Assert.IsNotNull(arguments);
            Assert.AreEqual(1, arguments.Count);
            Assert.AreEqual("id", arguments[0].Name);
        }

        [TestMethod]
        public void UserQuery_PersonField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var personField = _userQuery.Fields.FirstOrDefault(f => f.Name == "person");

            // Assert
            Assert.IsNotNull(personField);
            Assert.AreEqual("Get a person by ID", personField.Description);
            Assert.IsInstanceOfType(personField.Type, typeof(PersonType));
            
            // Verificar argumentos
            var arguments = personField.Arguments?.ToList();
            Assert.IsNotNull(arguments);
            Assert.AreEqual(1, arguments.Count);
            Assert.AreEqual("id", arguments[0].Name);
        }

        [TestMethod]
        public void UserQuery_ContactField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var contactField = _userQuery.Fields.FirstOrDefault(f => f.Name == "contact");

            // Assert
            Assert.IsNotNull(contactField);
            Assert.AreEqual("Get a contact by ID", contactField.Description);
            Assert.IsInstanceOfType(contactField.Type, typeof(ContactType));
            
            // Verificar argumentos
            var arguments = contactField.Arguments?.ToList();
            Assert.IsNotNull(arguments);
            Assert.AreEqual(1, arguments.Count);
            Assert.AreEqual("id", arguments[0].Name);
        }

        #endregion

        #region User Field Resolver Tests

        [TestMethod]
        public async Task UserField_ResolveAsync_WithValidId_ShouldReturnUser()
        {
            // Arrange
            var userId = "user123";
            var expectedUser = new UserDto
            {
                Id = userId,
                UserName = "testuser",
                State = "Active"
            };

            _mockUserService
                .Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync(expectedUser);

            var context = CreateResolveFieldContext("user", new Dictionary<string, object> { { "id", userId } });

            // Act
            var userField = _userQuery.Fields.FirstOrDefault(f => f.Name == "user");
            var result = await userField.Resolver.ResolveAsync(context) as UserDto;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUser.Id, result.Id);
            Assert.AreEqual(expectedUser.UserName, result.UserName);
            _mockUserService.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
        }

        [TestMethod]
        public async Task UserField_ResolveAsync_WithNullId_ShouldCallServiceWithNull()
        {
            // Arrange
            _mockUserService
                .Setup(x => x.GetUserByIdAsync(It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException("User ID cannot be empty"));

            var context = CreateResolveFieldContext("user", new Dictionary<string, object> { { "id", null } });

            // Act & Assert
            var userField = _userQuery.Fields.FirstOrDefault(f => f.Name == "user");
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => userField.Resolver.ResolveAsync(context));

            _mockUserService.Verify(x => x.GetUserByIdAsync(null), Times.Once);
        }

        #endregion

        #region Users Field Resolver Tests

        [TestMethod]
        public async Task UsersField_ResolveAsync_WithDefaultParameters_ShouldReturnUserList()
        {
            // Arrange
            var expectedUserList = new UserListDto
            {
                Data = new List<UserDto>
                {
                    new UserDto { Id = "1", UserName = "user1" },
                    new UserDto { Id = "2", UserName = "user2" }
                },
                TotalCount = 2,
                PageNumber = 1,
                PageSize = 10
            };

            _mockUserService
                .Setup(x => x.GetUsersAsync(1, 10))
                .ReturnsAsync(expectedUserList);

            var context = CreateResolveFieldContext("users", new Dictionary<string, object>());

            // Act
            var usersField = _userQuery.Fields.FirstOrDefault(f => f.Name == "users");
            var result = await usersField.Resolver.ResolveAsync(context) as UserListDto;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Data.Count);
            Assert.AreEqual(2, result.TotalCount);
            Assert.AreEqual(1, result.PageNumber);
            Assert.AreEqual(10, result.PageSize);
            _mockUserService.Verify(x => x.GetUsersAsync(1, 10), Times.Once);
        }

        [TestMethod]
        public async Task UsersField_ResolveAsync_WithCustomParameters_ShouldReturnUserList()
        {
            // Arrange
            var pageNumber = 2;
            var pageSize = 5;
            var expectedUserList = new UserListDto
            {
                Data = new List<UserDto> { new UserDto { Id = "3", UserName = "user3" } },
                TotalCount = 15,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            _mockUserService
                .Setup(x => x.GetUsersAsync(pageNumber, pageSize))
                .ReturnsAsync(expectedUserList);

            var context = CreateResolveFieldContext("users", new Dictionary<string, object> 
            { 
                { "pageNumber", pageNumber }, 
                { "pageSize", pageSize } 
            });

            // Act
            var usersField = _userQuery.Fields.FirstOrDefault(f => f.Name == "users");
            var result = await usersField.Resolver.ResolveAsync(context) as UserListDto;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Data.Count);
            Assert.AreEqual(pageNumber, result.PageNumber);
            Assert.AreEqual(pageSize, result.PageSize);
            _mockUserService.Verify(x => x.GetUsersAsync(pageNumber, pageSize), Times.Once);
        }

        #endregion

        #region Employee Field Resolver Tests

        [TestMethod]
        public async Task EmployeeField_ResolveAsync_WithValidId_ShouldReturnEmployee()
        {
            // Arrange
            var employeeId = "emp123";
            var expectedEmployee = new EmployeeDto
            {
                Id = employeeId,
                BussinesEmail = "employee@company.com",
                IdPosition = "developer"
            };

            _mockEmployeeService
                .Setup(x => x.GetEmployeeByIdAsync(employeeId))
                .ReturnsAsync(expectedEmployee);

            var context = CreateResolveFieldContext("employee", new Dictionary<string, object> { { "id", employeeId } });

            // Act
            var employeeField = _userQuery.Fields.FirstOrDefault(f => f.Name == "employee");
            var result = await employeeField.Resolver.ResolveAsync(context) as EmployeeDto;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedEmployee.Id, result.Id);
            Assert.AreEqual(expectedEmployee.BussinesEmail, result.BussinesEmail);
            _mockEmployeeService.Verify(x => x.GetEmployeeByIdAsync(employeeId), Times.Once);
        }

        #endregion

        #region Person Field Resolver Tests

        [TestMethod]
        public async Task PersonField_ResolveAsync_WithValidId_ShouldReturnPerson()
        {
            // Arrange
            var personId = "person123";
            var expectedPerson = new PersonDto
            {
                Id = personId,
                FirstName = "Juan",
                LastName = "Pérez",
                DocumentType = "CC",
                DocumentNumber = "12345678"
            };

            _mockPersonService
                .Setup(x => x.GetPersonByIdAsync(personId))
                .ReturnsAsync(expectedPerson);

            var context = CreateResolveFieldContext("person", new Dictionary<string, object> { { "id", personId } });

            // Act
            var personField = _userQuery.Fields.FirstOrDefault(f => f.Name == "person");
            var result = await personField.Resolver.ResolveAsync(context) as PersonDto;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPerson.Id, result.Id);
            Assert.AreEqual(expectedPerson.FirstName, result.FirstName);
            Assert.AreEqual(expectedPerson.LastName, result.LastName);
            _mockPersonService.Verify(x => x.GetPersonByIdAsync(personId), Times.Once);
        }

        #endregion

        #region Contact Field Resolver Tests

        [TestMethod]
        public async Task ContactField_ResolveAsync_WithValidId_ShouldReturnContact()
        {
            // Arrange
            var contactId = "contact123";
            var expectedContact = new ContactDto
            {
                Id = contactId,
                Email = "contact@example.com",
                Phone = "555-1234",
                Address = "123 Main St"
            };

            _mockContactService
                .Setup(x => x.GetContactByIdPersonAsync(contactId))
                .ReturnsAsync(expectedContact);

            var context = CreateResolveFieldContext("contact", new Dictionary<string, object> { { "id", contactId } });

            // Act
            var contactField = _userQuery.Fields.FirstOrDefault(f => f.Name == "contact");
            var result = await contactField.Resolver.ResolveAsync(context) as ContactDto;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedContact.Id, result.Id);
            Assert.AreEqual(expectedContact.Email, result.Email);
            Assert.AreEqual(expectedContact.Phone, result.Phone);
            _mockContactService.Verify(x => x.GetContactByIdPersonAsync(contactId), Times.Once);
        }

        #endregion

        #region Service Resolution Tests

        [TestMethod]
        public async Task AllFields_ShouldResolveServicesFromDependencyInjection()
        {
            // Arrange
            var contexts = new[]
            {
                CreateResolveFieldContext("user", new Dictionary<string, object> { { "id", "user1" } }),
                CreateResolveFieldContext("users", new Dictionary<string, object>()),
                CreateResolveFieldContext("employee", new Dictionary<string, object> { { "id", "emp1" } }),
                CreateResolveFieldContext("person", new Dictionary<string, object> { { "id", "person1" } }),
                CreateResolveFieldContext("contact", new Dictionary<string, object> { { "id", "contact1" } })
            };

            _mockUserService.Setup(x => x.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync(new UserDto());
            _mockUserService.Setup(x => x.GetUsersAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new UserListDto());
            _mockEmployeeService.Setup(x => x.GetEmployeeByIdAsync(It.IsAny<string>())).ReturnsAsync(new EmployeeDto());
            _mockPersonService.Setup(x => x.GetPersonByIdAsync(It.IsAny<string>())).ReturnsAsync(new PersonDto());
            _mockContactService.Setup(x => x.GetContactByIdPersonAsync(It.IsAny<string>())).ReturnsAsync(new ContactDto());

            // Act & Assert
            foreach (var context in contexts)
            {
                var field = _userQuery.Fields.FirstOrDefault(f => f.Name == context.FieldName);
                Assert.IsNotNull(field);
                
                var result = await field.Resolver.ResolveAsync(context);
                Assert.IsNotNull(result);
            }

            // Verificar que todos los servicios fueron llamados
            _mockUserService.Verify(x => x.GetUserByIdAsync(It.IsAny<string>()), Times.Once);
            _mockUserService.Verify(x => x.GetUsersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _mockEmployeeService.Verify(x => x.GetEmployeeByIdAsync(It.IsAny<string>()), Times.Once);
            _mockPersonService.Verify(x => x.GetPersonByIdAsync(It.IsAny<string>()), Times.Once);
            _mockContactService.Verify(x => x.GetContactByIdPersonAsync(It.IsAny<string>()), Times.Once);
        }

        #endregion

        #region Helper Methods

        private IResolveFieldContext CreateResolveFieldContext(string fieldName, Dictionary<string, object> arguments)
        {
            var context = new Mock<IResolveFieldContext>();
            context.Setup(x => x.RequestServices).Returns(_serviceProvider);
            context.Setup(x => x.FieldName).Returns(fieldName);
            
            // Configurar GetArgument para diferentes tipos
            context.Setup(x => x.GetArgument<string>("id"))
                   .Returns(arguments.ContainsKey("id") ? arguments["id"] as string : null);
            
            context.Setup(x => x.GetArgument<int?>("pageNumber"))
                   .Returns(arguments.ContainsKey("pageNumber") ? (int?)arguments["pageNumber"] : null);
            
            context.Setup(x => x.GetArgument<int?>("pageSize"))
                   .Returns(arguments.ContainsKey("pageSize") ? (int?)arguments["pageSize"] : null);

            return context.Object;
        }

        #endregion
    }
}
