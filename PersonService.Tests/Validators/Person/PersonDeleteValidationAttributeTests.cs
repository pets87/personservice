using PersonService.Dtos.Person;
using PersonService.Validators.Person;

namespace PersonService.Tests.Validators.Person
{
    [TestClass]
    public class PersonDeleteValidationAttributeTests
    {
        private PersonDeleteValidationAttribute attribute;

        [TestInitialize]
        public void SetUp()
        {
            attribute = new PersonDeleteValidationAttribute();
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenValueIsNull()
        {
            object value = null;

            var result = attribute.IsValid(value);

            Assert.IsFalse(result);
            Assert.AreEqual("Input cannot be null.", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenIdIsNotSet()
        {
            var personDto = new PersonDto
            {
                Id = null,
                FirstName = "John",
                LastName = "Doe",
                PersonalCode = "12345678901",
                DateOfBirth = new DateTime(1990, 1, 1)
            };

            var result = attribute.IsValid(personDto);

            Assert.IsFalse(result);
            Assert.AreEqual("Cannot delete entity without Id", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenIdIsEmptyGuid()
        {
            var personDto = new PersonDto
            {
                Id = Guid.Empty,
                FirstName = "John",
                LastName = "Doe",
                PersonalCode = "12345678901",
                DateOfBirth = new DateTime(1990, 1, 1)
            };

            var result = attribute.IsValid(personDto);

            Assert.IsFalse(result);
            Assert.AreEqual("Cannot delete entity without Id", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnTrue_WhenIdIsValid()
        {
            var personDto = new PersonDto
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                PersonalCode = "12345678901",
                DateOfBirth = new DateTime(1990, 1, 1)
            };

            var result = attribute.IsValid(personDto);

            Assert.IsTrue(result);
            Assert.IsNull(attribute.ErrorMessage);
        }
    }
}
