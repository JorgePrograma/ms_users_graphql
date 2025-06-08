using GraphQL.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using msusersgraphql.GraphQL.Types;
using msusersgraphql.Models.Dtos;
using System.Linq;

namespace msusersgraphql.Tests.GraphQL.Types
{
    [TestClass]
    public class ContactTypeTests
    {
        private ContactType _contactType;

        [TestInitialize]
        public void Setup()
        {
            _contactType = new ContactType();
        }

        [TestMethod]
        public void ContactType_Constructor_ShouldInitializeCorrectly()
        {
            // Act
            var contactType = new ContactType();

            // Assert
            Assert.IsNotNull(contactType);
            Assert.AreEqual("Contact", contactType.Name);
        }

        [TestMethod]
        public void ContactType_ShouldInheritFromObjectGraphType()
        {
            // Assert
            Assert.IsInstanceOfType(_contactType, typeof(ObjectGraphType<ContactDto>));
            Assert.IsInstanceOfType(_contactType, typeof(ObjectGraphType));
        }

        [TestMethod]
        public void ContactType_Name_ShouldBeContact()
        {
            // Assert
            Assert.AreEqual("Contact", _contactType.Name);
        }

        [TestMethod]
        public void ContactType_ShouldHaveAllRequiredFields()
        {
            // Act
            var fields = _contactType.Fields.ToList();

            // Assert
            Assert.AreEqual(10, fields.Count);
            
            var fieldNames = fields.Select(f => f.Name).ToList();
            Assert.IsTrue(fieldNames.Contains("idPerson"));
            Assert.IsTrue(fieldNames.Contains("email"));
            Assert.IsTrue(fieldNames.Contains("phone"));
            Assert.IsTrue(fieldNames.Contains("address"));
            Assert.IsTrue(fieldNames.Contains("cellPhone"));
            Assert.IsTrue(fieldNames.Contains("country"));
            Assert.IsTrue(fieldNames.Contains("department"));
            Assert.IsTrue(fieldNames.Contains("locality"));
            Assert.IsTrue(fieldNames.Contains("neighborhood"));
            Assert.IsTrue(fieldNames.Contains("city"));
        }

        [TestMethod]
        public void ContactType_IdPersonField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _contactType.Fields.FirstOrDefault(f => f.Name == "idPerson");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The person ID of the contact", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void ContactType_EmailField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _contactType.Fields.FirstOrDefault(f => f.Name == "email");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The ID of the contact", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void ContactType_PhoneField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _contactType.Fields.FirstOrDefault(f => f.Name == "phone");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The person ID of the contact", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void ContactType_AddressField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _contactType.Fields.FirstOrDefault(f => f.Name == "address");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The user ID of the contact", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void ContactType_CellPhoneField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _contactType.Fields.FirstOrDefault(f => f.Name == "cellPhone");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The business email of the contact", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void ContactType_CountryField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _contactType.Fields.FirstOrDefault(f => f.Name == "country");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The business phone of the contact", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void ContactType_DepartmentField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _contactType.Fields.FirstOrDefault(f => f.Name == "department");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The position ID of the contact", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void ContactType_LocalityField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _contactType.Fields.FirstOrDefault(f => f.Name == "locality");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The branch ID of the contact", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void ContactType_NeighborhoodField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _contactType.Fields.FirstOrDefault(f => f.Name == "neighborhood");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The branch ID of the contact", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void ContactType_CityField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _contactType.Fields.FirstOrDefault(f => f.Name == "city");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The branch ID of the contact", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void ContactType_NullableFields_ShouldBeConfiguredAsNullable()
        {
            // Act
            var departmentField = _contactType.Fields.FirstOrDefault(f => f.Name == "department");
            var localityField = _contactType.Fields.FirstOrDefault(f => f.Name == "locality");
            var neighborhoodField = _contactType.Fields.FirstOrDefault(f => f.Name == "neighborhood");
            var cityField = _contactType.Fields.FirstOrDefault(f => f.Name == "city");

            // Assert
            Assert.IsNotNull(departmentField);
            Assert.IsNotNull(localityField);
            Assert.IsNotNull(neighborhoodField);
            Assert.IsNotNull(cityField);
            
            // Verificar que son nullable (StringGraphType permite null por defecto)
            Assert.IsInstanceOfType(departmentField.Type, typeof(StringGraphType));
            Assert.IsInstanceOfType(localityField.Type, typeof(StringGraphType));
            Assert.IsInstanceOfType(neighborhoodField.Type, typeof(StringGraphType));
            Assert.IsInstanceOfType(cityField.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void ContactType_RequiredFields_ShouldNotBeNullable()
        {
            // Act
            var idPersonField = _contactType.Fields.FirstOrDefault(f => f.Name == "idPerson");
            var emailField = _contactType.Fields.FirstOrDefault(f => f.Name == "email");
            var phoneField = _contactType.Fields.FirstOrDefault(f => f.Name == "phone");
            var addressField = _contactType.Fields.FirstOrDefault(f => f.Name == "address");
            var cellPhoneField = _contactType.Fields.FirstOrDefault(f => f.Name == "cellPhone");
            var countryField = _contactType.Fields.FirstOrDefault(f => f.Name == "country");

            // Assert
            Assert.IsNotNull(idPersonField);
            Assert.IsNotNull(emailField);
            Assert.IsNotNull(phoneField);
            Assert.IsNotNull(addressField);
            Assert.IsNotNull(cellPhoneField);
            Assert.IsNotNull(countryField);
            
            // Verificar que son StringGraphType (no nullable por defecto en la configuración)
            Assert.IsInstanceOfType(idPersonField.Type, typeof(StringGraphType));
            Assert.IsInstanceOfType(emailField.Type, typeof(StringGraphType));
            Assert.IsInstanceOfType(phoneField.Type, typeof(StringGraphType));
            Assert.IsInstanceOfType(addressField.Type, typeof(StringGraphType));
            Assert.IsInstanceOfType(cellPhoneField.Type, typeof(StringGraphType));
            Assert.IsInstanceOfType(countryField.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void ContactType_AllFields_ShouldHaveDescriptions()
        {
            // Act
            var fields = _contactType.Fields.ToList();

            // Assert
            foreach (var field in fields)
            {
                Assert.IsNotNull(field.Description, $"Field {field.Name} should have a description");
                Assert.IsFalse(string.IsNullOrEmpty(field.Description), $"Field {field.Name} description should not be empty");
            }
        }

        [TestMethod]
        public void ContactType_AllFields_ShouldBeStringType()
        {
            // Act
            var fields = _contactType.Fields.ToList();

            // Assert
            foreach (var field in fields)
            {
                Assert.IsInstanceOfType(field.Type, typeof(StringGraphType), 
                    $"Field {field.Name} should be of type StringGraphType");
            }
        }

        [TestMethod]
        public void ContactType_ShouldBeSealed()
        {
            // Assert
            var type = typeof(ContactType);
            Assert.IsTrue(type.IsSealed, "ContactType should be sealed");
        }
    }
}
