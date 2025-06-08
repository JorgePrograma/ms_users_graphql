using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using msusersgraphql.Extensions;
using msusersgraphql.Models.GraphQL;
using msusersgraphql.Repositories.Contact;
using msusersgraphql.Repositories.Employee;
using msusersgraphql.Repositories.Person;
using msusersgraphql.Repositories.User;
using msusersgraphql.Services.Contact;
using msusersgraphql.Services.Employee;
using msusersgraphql.Services.Person;
using msusersgraphql.Services.User;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace msusersgraphql.Tests.Extensions
{
    [TestClass]
    public class ServiceExtensionsTests
    {
        private IServiceCollection _services;
        private StringWriter _consoleOutput;
        private TextWriter _originalConsoleOut;

        [TestInitialize]
        public void Setup()
        {
            _services = new ServiceCollection();
            
            // Capturar salida de Console para verificar logs
            _consoleOutput = new StringWriter();
            _originalConsoleOut = Console.Out;
            Console.SetOut(_consoleOutput);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Console.SetOut(_originalConsoleOut);
            _consoleOutput?.Dispose();
        }

        [TestMethod]
        public void ConfigureServices_ShouldRegisterAllRepositories()
        {
            // Act
            _services.ConfigureServices();
            var serviceProvider = _services.BuildServiceProvider();

            // Assert - Verificar que todos los repositorios están registrados
            Assert.IsNotNull(serviceProvider.GetService<IUserRepository>());
            Assert.IsNotNull(serviceProvider.GetService<IEmployeeRepository>());
            Assert.IsNotNull(serviceProvider.GetService<IPersonRepository>());
            Assert.IsNotNull(serviceProvider.GetService<IContactRepository>());
        }

        [TestMethod]
        public void ConfigureServices_ShouldRegisterAllServices()
        {
            // Act
            _services.ConfigureServices();
            var serviceProvider = _services.BuildServiceProvider();

            // Assert - Verificar que todos los servicios están registrados
            Assert.IsNotNull(serviceProvider.GetService<IUserService>());
            Assert.IsNotNull(serviceProvider.GetService<IEmployeeService>());
            Assert.IsNotNull(serviceProvider.GetService<IPersonService>());
            Assert.IsNotNull(serviceProvider.GetService<IContactService>());
        }

        [TestMethod]
        public void ConfigureServices_ShouldRegisterGraphQLComponents()
        {
            // Act
            _services.ConfigureServices();
            var serviceProvider = _services.BuildServiceProvider();

            // Assert - Verificar que los componentes GraphQL están registrados
            Assert.IsNotNull(serviceProvider.GetService<UserSchema>());
            Assert.IsNotNull(serviceProvider.GetService<UserQuery>());
        }

        [TestMethod]
        public void ConfigureServices_ShouldRegisterHttpClientFactory()
        {
            // Act
            _services.ConfigureServices();
            var serviceProvider = _services.BuildServiceProvider();

            // Assert - Verificar que IHttpClientFactory está registrado
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            Assert.IsNotNull(httpClientFactory);

            // Verificar que se puede crear el cliente "UsersAPI"
            var httpClient = httpClientFactory.CreateClient("UsersAPI");
            Assert.IsNotNull(httpClient);
        }

        [TestMethod]
        public void ConfigureServices_HttpClient_ShouldHaveCorrectBaseAddress()
        {
            // Act
            _services.ConfigureServices();
            var serviceProvider = _services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient("UsersAPI");

            // Assert
            var expectedBaseAddress = "https://ib3m6t7bp7sjmglwvvpg3xrmzu.apigateway.sa-bogota-1.oci.customer-oci.com/api/v1/";
            Assert.AreEqual(expectedBaseAddress, httpClient.BaseAddress.ToString());
        }

        [TestMethod]
        public void ConfigureServices_HttpClient_ShouldHaveAcceptHeader()
        {
            // Act
            _services.ConfigureServices();
            var serviceProvider = _services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient("UsersAPI");

            // Assert
            Assert.IsTrue(httpClient.DefaultRequestHeaders.Accept.Any(h => h.MediaType == "application/json"));
        }

        [TestMethod]
        public void ConfigureServices_HttpClient_ShouldHaveBearerToken()
        {
            // Act
            _services.ConfigureServices();
            var serviceProvider = _services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient("UsersAPI");

            // Assert
            Assert.IsNotNull(httpClient.DefaultRequestHeaders.Authorization);
            Assert.AreEqual("Bearer", httpClient.DefaultRequestHeaders.Authorization.Scheme);
            Assert.IsNotNull(httpClient.DefaultRequestHeaders.Authorization.Parameter);
            Assert.IsTrue(httpClient.DefaultRequestHeaders.Authorization.Parameter.StartsWith("eyJ"));
        }

        [TestMethod]
        public void ConfigureServices_ShouldLogTokenConfiguration()
        {
            // Act
            _services.ConfigureServices();
            var serviceProvider = _services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            httpClientFactory.CreateClient("UsersAPI");

            // Assert
            var consoleOutput = _consoleOutput.ToString();
            Assert.IsTrue(consoleOutput.Contains("Token configurado correctamente"));
        }

        [TestMethod]
        public void ConfigureServices_RepositoriesLifetime_ShouldBeScoped()
        {
            // Act
            _services.ConfigureServices();

            // Assert
            var userRepositoryDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IUserRepository));
            var employeeRepositoryDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IEmployeeRepository));
            var personRepositoryDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IPersonRepository));
            var contactRepositoryDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IContactRepository));

            Assert.IsNotNull(userRepositoryDescriptor);
            Assert.AreEqual(ServiceLifetime.Scoped, userRepositoryDescriptor.Lifetime);
            
            Assert.IsNotNull(employeeRepositoryDescriptor);
            Assert.AreEqual(ServiceLifetime.Scoped, employeeRepositoryDescriptor.Lifetime);
            
            Assert.IsNotNull(personRepositoryDescriptor);
            Assert.AreEqual(ServiceLifetime.Scoped, personRepositoryDescriptor.Lifetime);
            
            Assert.IsNotNull(contactRepositoryDescriptor);
            Assert.AreEqual(ServiceLifetime.Scoped, contactRepositoryDescriptor.Lifetime);
        }

        [TestMethod]
        public void ConfigureServices_ServicesLifetime_ShouldBeScoped()
        {
            // Act
            _services.ConfigureServices();

            // Assert
            var userServiceDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IUserService));
            var employeeServiceDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IEmployeeService));
            var personServiceDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IPersonService));
            var contactServiceDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IContactService));

            Assert.IsNotNull(userServiceDescriptor);
            Assert.AreEqual(ServiceLifetime.Scoped, userServiceDescriptor.Lifetime);
            
            Assert.IsNotNull(employeeServiceDescriptor);
            Assert.AreEqual(ServiceLifetime.Scoped, employeeServiceDescriptor.Lifetime);
            
            Assert.IsNotNull(personServiceDescriptor);
            Assert.AreEqual(ServiceLifetime.Scoped, personServiceDescriptor.Lifetime);
            
            Assert.IsNotNull(contactServiceDescriptor);
            Assert.AreEqual(ServiceLifetime.Scoped, contactServiceDescriptor.Lifetime);
        }

        [TestMethod]
        public void ConfigureServices_GraphQLLifetime_ShouldBeSingleton()
        {
            // Act
            _services.ConfigureServices();

            // Assert
            var userSchemaDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(UserSchema));
            var userQueryDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(UserQuery));

            Assert.IsNotNull(userSchemaDescriptor);
            Assert.AreEqual(ServiceLifetime.Singleton, userSchemaDescriptor.Lifetime);
            
            Assert.IsNotNull(userQueryDescriptor);
            Assert.AreEqual(ServiceLifetime.Singleton, userQueryDescriptor.Lifetime);
        }

        [TestMethod]
        public void ConfigureServices_ShouldRegisterCorrectImplementations()
        {
            // Act
            _services.ConfigureServices();

            // Assert
            var userRepositoryDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IUserRepository));
            var userServiceDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IUserService));

            Assert.AreEqual(typeof(UserRepository), userRepositoryDescriptor.ImplementationType);
            Assert.AreEqual(typeof(UserService), userServiceDescriptor.ImplementationType);
        }

        [TestMethod]
        public void ConfigureServices_MultipleCallsToSameService_ShouldReturnSameInstanceForSingleton()
        {
            // Act
            _services.ConfigureServices();
            var serviceProvider = _services.BuildServiceProvider();

            // Assert - Verificar que los singletons retornan la misma instancia
            var userSchema1 = serviceProvider.GetService<UserSchema>();
            var userSchema2 = serviceProvider.GetService<UserSchema>();
            
            Assert.AreSame(userSchema1, userSchema2);

            var userQuery1 = serviceProvider.GetService<UserQuery>();
            var userQuery2 = serviceProvider.GetService<UserQuery>();
            
            Assert.AreSame(userQuery1, userQuery2);
        }

        [TestMethod]
        public void ConfigureServices_MultipleCallsToSameService_ShouldReturnDifferentInstancesForScoped()
        {
            // Act
            _services.ConfigureServices();
            var serviceProvider = _services.BuildServiceProvider();

            // Assert - Verificar que los scoped retornan diferentes instancias en diferentes scopes
            using (var scope1 = serviceProvider.CreateScope())
            using (var scope2 = serviceProvider.CreateScope())
            {
                var userService1 = scope1.ServiceProvider.GetService<IUserService>();
                var userService2 = scope2.ServiceProvider.GetService<IUserService>();
                
                Assert.AreNotSame(userService1, userService2);
            }
        }

        [TestMethod]
        public void ConfigureServices_HttpClientConfiguration_ShouldBeReusable()
        {
            // Act
            _services.ConfigureServices();
            var serviceProvider = _services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

            // Assert - Verificar que se pueden crear múltiples clientes con la misma configuración
            var httpClient1 = httpClientFactory.CreateClient("UsersAPI");
            var httpClient2 = httpClientFactory.CreateClient("UsersAPI");

            Assert.AreNotSame(httpClient1, httpClient2); // Diferentes instancias
            Assert.AreEqual(httpClient1.BaseAddress, httpClient2.BaseAddress); // Misma configuración
            Assert.AreEqual(httpClient1.DefaultRequestHeaders.Authorization.Parameter, 
                           httpClient2.DefaultRequestHeaders.Authorization.Parameter);
        }

        [TestMethod]
        public void ConfigureServices_WithNullServiceCollection_ShouldThrowArgumentNullException()
        {
            // Arrange
            IServiceCollection nullServices = null;

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => nullServices.ConfigureServices());
        }
    }
}
