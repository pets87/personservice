using PersonService.Models;
using System.ComponentModel.DataAnnotations;

namespace PersonService.Tests.Models
{
    [TestClass]
    public class PersonTests
    {
        [TestMethod]
        public void Person_RequiredProperties_ShouldBeValidated()
        {
            var person = new Person
            {
                Id = Guid.Empty,
                PersonalCode = null,
                FirstName = null,
                LastName = null,
                DateOfBirth = DateTime.MinValue,
                TimeOfDeath = null,
                UpdatedBy = "12345",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var validationResults = ValidateModel(person);

            Assert.AreEqual(3, validationResults.Count);
            Assert.IsTrue(validationResults.Exists(v => v.MemberNames.Contains(nameof(Person.PersonalCode))));
            Assert.IsTrue(validationResults.Exists(v => v.MemberNames.Contains(nameof(Person.FirstName))));
            Assert.IsTrue(validationResults.Exists(v => v.MemberNames.Contains(nameof(Person.LastName))));
        }

        [TestMethod]
        public void Person_ValidModel_ShouldPassValidation()
        {
            var person = new Person
            {
                Id = Guid.NewGuid(),
                PersonalCode = "1234567890",
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                TimeOfDeath = null
            };

            var validationResults = ValidateModel(person);

            Assert.AreEqual(0, validationResults.Count);
        }

        [TestMethod]
        public void Person_Constructor_ShouldSetProperties()
        {
            var personCode = "1234567890";
            var firstName = "John";
            var lastName = "Doe";

            var person = new Person(personCode, firstName, lastName);

            Assert.AreEqual(personCode, person.PersonalCode);
            Assert.AreEqual(firstName, person.FirstName);
            Assert.AreEqual(lastName, person.LastName);
        }

        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, serviceProvider: null, items: null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}
