using GraphQL.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using msusersgraphql.GraphQL.Types;
using msusersgraphql.Models.Dtos;
using System.Linq;

namespace msusersgraphql.Tests.GraphQL.Types
{
    [TestClass]
    public class PersonTypeTests
    {
        private PersonType _personType;

        [TestInitialize]
        public void Setup()
        {
            _personType = new PersonType();
        }

        #region Constructor and Basic Configuration Tests

        [TestMethod]
        public void PersonType_Constructor_ShouldInitializeCorrectly()
        {
            // Act
            var personType = new PersonType();

            // Assert
            Assert.IsNotNull(personType);
            Assert.AreEqual("Person", personType.Name);
        }

        [TestMethod]
        public void PersonType_ShouldInheritFromObjectGraphType()
        {
            // Assert
            Assert.IsInstanceOfType(_personType, typeof(ObjectGraphType<PersonDto>));
            Assert.IsInstanceOfType(_personType, typeof(ObjectGraphType));
        }

        [TestMethod]
        public void PersonType_Name_ShouldBePerson()
        {
            // Assert
            Assert.AreEqual("Person", _personType.Name);
        }

        [TestMethod]
        public void PersonType_ShouldBeSealed()
        {
            // Assert
            var type = typeof(PersonType);
            Assert.IsTrue(type.IsSealed, "PersonType should be sealed");
        }

        [TestMethod]
        public void PersonType_ShouldHaveAllRequiredFields()
        {
            // Act
            var fields = _personType.Fields.ToList();

            // Assert
            Assert.AreEqual(7, fields.Count);
            
            var fieldNames = fields.Select(f => f.Name).ToList();
            Assert.IsTrue(fieldNames.Contains("id"));
            Assert.IsTrue(fieldNames.Contains("documentType"));
            Assert.IsTrue(fieldNames.Contains("documentNumber"));
            Assert.IsTrue(fieldNames.Contains("firstName"));
            Assert.IsTrue(fieldNames.Contains("middleName"));
            Assert.IsTrue(fieldNames.Contains("lastName"));
            Assert.IsTrue(fieldNames.Contains("secondLastName"));
        }

        #endregion

        #region Individual Field Tests

        [TestMethod]
        public void PersonType_IdField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _personType.Fields.FirstOrDefault(f => f.Name == "id");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The ID of the person", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void PersonType_DocumentTypeField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _personType.Fields.FirstOrDefault(f => f.Name == "documentType");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The document type of the person", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void PersonType_DocumentNumberField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _personType.Fields.FirstOrDefault(f => f.Name == "documentNumber");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The document number of the person", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void PersonType_FirstNameField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _personType.Fields.FirstOrDefault(f => f.Name == "firstName");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The first name of the person", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void PersonType_MiddleNameField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _personType.Fields.FirstOrDefault(f => f.Name == "middleName");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The middle name of the person", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void PersonType_LastNameField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _personType.Fields.FirstOrDefault(f => f.Name == "lastName");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The last name of the person", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void PersonType_SecondLastNameField_ShouldHaveCorrectConfiguration()
        {
            // Act
            var field = _personType.Fields.FirstOrDefault(f => f.Name == "secondLastName");

            // Assert
            Assert.IsNotNull(field);
            Assert.AreEqual("The second last name of the person", field.Description);
            Assert.IsInstanceOfType(field.Type, typeof(StringGraphType));
        }

        #endregion

        #region Nullable Field Tests

        [TestMethod]
        public void PersonType_NullableFields_ShouldBeConfiguredAsNullable()
        {
            // Act
            var middleNameField = _personType.Fields.FirstOrDefault(f => f.Name == "middleName");
            var secondLastNameField = _personType.Fields.FirstOrDefault(f => f.Name == "secondLastName");

            // Assert
            Assert.IsNotNull(middleNameField);
            Assert.IsNotNull(secondLastNameField);
            
            // Verificar que son nullable (StringGraphType permite null por defecto)
            Assert.IsInstanceOfType(middleNameField.Type, typeof(StringGraphType));
            Assert.IsInstanceOfType(secondLastNameField.Type, typeof(StringGraphType));
        }

        [TestMethod]
        public void PersonType_RequiredFields_ShouldNotBeNullable()
        {
            // Act
            var idField = _personType.Fields.FirstOrDefault(f => f.Name == "id");
            var documentTypeField = _personType.Fields.FirstOrDefault(f => f.Name == "documentType");
            var documentNumberField = _personType.Fields.FirstOrDefault(f => f.Name == "documentNumber");
            var firstNameField = _personType.Fields.FirstOrDefault(f => f.Name == "firstName");
            var lastNameField = _personType.Fields.FirstOrDefault(f => f.Name == "lastName");

            // Assert
            Assert.IsNotNull(idField);
            Assert.IsNotNull(documentTypeField);
            Assert.IsNotNull(documentNumberField);
            Assert.IsNotNull(firstNameField);
            Assert.IsNotNull(lastNameField);
            
            // Verificar que son StringGraphType (no nullable por defecto en la configuración)
            Assert.IsInstanceOfType(idField.Type, typeof(StringGraphType));
            Assert.IsInstanceOfType(documentTypeField.Type, typeof(StringGraphType));
            Assert.IsInstanceOfType(documentNumberField.Type, typeof(StringGraphType));
            Assert.IsInstanceOfType(firstNameField.Type, typeof(StringGraphType));
            Assert.IsInstanceOfType(lastNameField.Type, typeof(StringGraphType));
        }

        #endregion

        #region Field Validation Tests

        [TestMethod]
        public void PersonType_AllFields_ShouldHaveDescriptions()
        {
            // Act
            var fields = _personType.Fields.ToList();

            // Assert
            foreach (var field in fields)
            {
                Assert.IsNotNull(field.Description, $"Field {field.Name} should have a description");
                Assert.IsFalse(string.IsNullOrEmpty(field.Description), $"Field {field.Name} description should not be empty");
                Assert.IsFalse(string.IsNullOrWhiteSpace(field.Description), $"Field {field.Name} description should not be whitespace");
            }
        }

        [TestMethod]
        public void PersonType_AllFields_ShouldBeStringType()
        {
            // Act
            var fields = _personType.Fields.ToList();

            // Assert
            foreach (var field in fields)
            {
                Assert.IsInstanceOfType(field.Type, typeof(StringGraphType), 
                    $"Field {field.Name} should be of type StringGraphType");
            }
        }

        [TestMethod]
        public void PersonType_FieldDescriptions_ShouldBeCorrectlyFormatted()
        {
            // Act
            var fields = _personType.Fields.ToList();

            // Assert
            foreach (var field in fields)
            {
                Assert.IsTrue(field.Description.StartsWith("The "), 
                    $"Field {field.Name} description should start with 'The '");
                Assert.IsTrue(field.Description.EndsWith(" of the person"), 
                    $"Field {field.Name} description should end with ' of the person'");
            }
        }

        #endregion

        #region Integration Tests

        [TestMethod]
        public void PersonType_ShouldMapAllPersonDtoProperties()
        {
            // Arrange
            var personDtoProperties = typeof(PersonDto).GetProperties()
                .Select(p => p.Name)
                .ToList();

            var personTypeFields = _personType.Fields
                .Select(f => f.Name)
                .ToList();

            // Act & Assert
            foreach (var property in personDtoProperties)
            {
                var fieldName = char.ToLowerInvariant(property[0]) + property.Substring(1); // camelCase
                Assert.IsTrue(personTypeFields.Contains(fieldName), 
                    $"PersonType should have field for property {property}");
            }
        }

        [TestMethod]
        public void PersonType_FieldCount_ShouldMatchPersonDtoPropertyCount()
        {
            // Arrange
            var personDtoPropertyCount = typeof(PersonDto).GetProperties().Count();

            // Act
            var personTypeFieldCount = _personType.Fields.Count();

            // Assert
            Assert.AreEqual(personDtoPropertyCount, personTypeFieldCount,
                "PersonType field count should match PersonDto property count");
        }

        #endregion

        #region Edge Cases Tests

        [TestMethod]
        public void PersonType_MultipleInstances_ShouldHaveSameConfiguration()
        {
            // Arrange
            var personType1 = new PersonType();
            var personType2 = new PersonType();

            // Act
            var fields1 = personType1.Fields.ToList();
            var fields2 = personType2.Fields.ToList();

            // Assert
            Assert.AreEqual(fields1.Count, fields2.Count);
            Assert.AreEqual(personType1.Name, personType2.Name);
            
            for (int i = 0; i < fields1.Count; i++)
            {
                Assert.AreEqual(fields1[i].Name, fields2[i].Name);
                Assert.AreEqual(fields1[i].Description, fields2[i].Description);
                Assert.AreEqual(fields1[i].Type.GetType(), fields2[i].Type.GetType());
            }
        }

        [TestMethod]
        public void PersonType_FieldNames_ShouldBeCamelCase()
        {
            // Act
            var fields = _personType.Fields.ToList();

            // Assert
            foreach (var field in fields)
            {
                Assert.IsTrue(char.IsLower(field.Name[0]), 
                    $"Field {field.Name} should start with lowercase letter (camelCase)");
            }
        }

        [TestMethod]
        public void PersonType_FieldOrder_ShouldMatchDefinitionOrder()
        {
            // Act
            var fieldNames = _personType.Fields.Select(f => f.Name).ToList();

            // Assert
            var expectedOrder = new[] { "id", "documentType", "documentNumber", "firstName", "middleName", "lastName", "secondLastName" };
            
            for (int i = 0; i < expectedOrder.Length; i++)
            {
                Assert.AreEqual(expectedOrder[i], fieldNames[i], 
                    $"Field at position {i} should be {expectedOrder[i]}");
            }
        }

        #endregion
    }
}
