using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using msusersgraphql.Models.GraphQL;
using System;

namespace msusersgraphql.Tests.Models.GraphQL
{
    [TestClass]
    public class UserSchemaTests
    {
        private Mock<IServiceProvider> _mockServiceProvider;
        private Mock<UserQuery> _mockUserQuery;

        [TestInitialize]
        public void Setup()
        {
            _mockServiceProvider = new Mock<IServiceProvider>();
            _mockUserQuery = new Mock<UserQuery>();
        }

        [TestMethod]
        public void UserSchema_Constructor_ShouldInitializeWithValidServiceProvider()
        {
            // Arrange
            _mockServiceProvider
                .Setup(x => x.GetRequiredService<UserQuery>())
                .Returns(_mockUserQuery.Object);

            // Act
            var schema = new UserSchema(_mockServiceProvider.Object);

            // Assert
            Assert.IsNotNull(schema);
            Assert.IsNotNull(schema.Query);
            Assert.AreEqual(_mockUserQuery.Object, schema.Query);
            Assert.AreEqual("Users Management GraphQL API schema", schema.Description);
        }

        [TestMethod]
        public void UserSchema_Constructor_ShouldCallGetRequiredServiceForUserQuery()
        {
            // Arrange
            _mockServiceProvider
                .Setup(x => x.GetRequiredService<UserQuery>())
                .Returns(_mockUserQuery.Object);

            // Act
            var schema = new UserSchema(_mockServiceProvider.Object);

            // Assert
            _mockServiceProvider.Verify(x => x.GetRequiredService<UserQuery>(), Times.Once);
        }

        [TestMethod]
        public void UserSchema_Constructor_WithNullServiceProvider_ShouldThrowArgumentNullException()
        {
            // Arrange
            IServiceProvider nullProvider = null;

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new UserSchema(nullProvider));
        }

        [TestMethod]
        public void UserSchema_Constructor_WhenUserQueryNotRegistered_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _mockServiceProvider
                .Setup(x => x.GetRequiredService<UserQuery>())
                .Throws(new InvalidOperationException("Service not registered"));

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => new UserSchema(_mockServiceProvider.Object));
        }

        [TestMethod]
        public void UserSchema_Description_ShouldBeSetCorrectly()
        {
            // Arrange
            _mockServiceProvider
                .Setup(x => x.GetRequiredService<UserQuery>())
                .Returns(_mockUserQuery.Object);

            // Act
            var schema = new UserSchema(_mockServiceProvider.Object);

            // Assert
            Assert.AreEqual("Users Management GraphQL API schema", schema.Description);
        }

        [TestMethod]
        public void UserSchema_Query_ShouldBeInstanceOfUserQuery()
        {
            // Arrange
            _mockServiceProvider
                .Setup(x => x.GetRequiredService<UserQuery>())
                .Returns(_mockUserQuery.Object);

            // Act
            var schema = new UserSchema(_mockServiceProvider.Object);

            // Assert
            Assert.IsInstanceOfType(schema.Query, typeof(UserQuery));
        }

        [TestMethod]
        public void UserSchema_ShouldInheritFromSchema()
        {
            // Arrange
            _mockServiceProvider
                .Setup(x => x.GetRequiredService<UserQuery>())
                .Returns(_mockUserQuery.Object);

            // Act
            var schema = new UserSchema(_mockServiceProvider.Object);

            // Assert
            Assert.IsInstanceOfType(schema, typeof(Schema));
        }

        [TestMethod]
        public void UserSchema_Mutation_ShouldBeNull()
        {
            // Arrange
            _mockServiceProvider
                .Setup(x => x.GetRequiredService<UserQuery>())
                .Returns(_mockUserQuery.Object);

            // Act
            var schema = new UserSchema(_mockServiceProvider.Object);

            // Assert
            Assert.IsNull(schema.Mutation);
        }

        [TestMethod]
        public void UserSchema_Subscription_ShouldBeNull()
        {
            // Arrange
            _mockServiceProvider
                .Setup(x => x.GetRequiredService<UserQuery>())
                .Returns(_mockUserQuery.Object);

            // Act
            var schema = new UserSchema(_mockServiceProvider.Object);

            // Assert
            Assert.IsNull(schema.Subscription);
        }
    }
}
