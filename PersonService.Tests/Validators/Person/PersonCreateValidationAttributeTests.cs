using PersonService.Dtos.Person;
using PersonService.Validators.Person;

namespace PersonService.Tests.Validators.Person
{
    [TestClass]
    public class PersonCreateValidationAttributeTests
    {
        private PersonCreateValidationAttribute attribute;

        [TestInitialize]
        public void SetUp()
        {
            attribute = new PersonCreateValidationAttribute();
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
        public void IsValid_ShouldReturnFalse_WhenIdIsSet()
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

            Assert.IsFalse(result);
            Assert.AreEqual("Cannot create new entity with existing Id. Use PUT instead", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenFirstNameIsEmpty()
        {
            var personDto = new PersonDto
            {
                Id = Guid.Empty,
                FirstName = string.Empty,
                LastName = "Doe",
                PersonalCode = "12345678901",
                DateOfBirth = new DateTime(1990, 1, 1)
            };

            var result = attribute.IsValid(personDto);

            Assert.IsFalse(result);
            Assert.AreEqual("FirstName cannot be empty", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenLastNameIsEmpty()
        {
            var personDto = new PersonDto
            {
                Id = Guid.Empty,
                FirstName = "John",
                LastName = string.Empty,
                PersonalCode = "12345678901",
                DateOfBirth = new DateTime(1990, 1, 1)
            };

            var result = attribute.IsValid(personDto);

            Assert.IsFalse(result);
            Assert.AreEqual("LastName cannot be empty", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenPersonalCodeIsEmpty()
        {
            var personDto = new PersonDto
            {
                Id = Guid.Empty,
                FirstName = "John",
                LastName = "Doe",
                PersonalCode = string.Empty,
                DateOfBirth = new DateTime(1990, 1, 1)
            };

            var result = attribute.IsValid(personDto);

            Assert.IsFalse(result);
            Assert.AreEqual("PersonalCode cannot be empty", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenDateOfBirthIsMinValue()
        {
            var personDto = new PersonDto
            {
                Id = Guid.Empty,
                FirstName = "John",
                LastName = "Doe",
                PersonalCode = "12345678901",
                DateOfBirth = DateTime.MinValue
            };

            var result = attribute.IsValid(personDto);

            Assert.IsFalse(result);
            Assert.AreEqual("DateOfBirth is invalid", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenDateOfBirthIsMaxValue()
        {
            var personDto = new PersonDto
            {
                Id = Guid.Empty,
                FirstName = "John",
                LastName = "Doe",
                PersonalCode = "12345678901",
                DateOfBirth = DateTime.MaxValue
            };

            var result = attribute.IsValid(personDto);

            Assert.IsFalse(result);
            Assert.AreEqual("DateOfBirth is invalid", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenDateOfBirthIsInTheFuture()
        {
            var personDto = new PersonDto
            {
                Id = Guid.Empty,
                FirstName = "John",
                LastName = "Doe",
                PersonalCode = "12345678901",
                DateOfBirth = DateTime.Now.AddYears(1) // Future date
            };

            var result = attribute.IsValid(personDto);

            Assert.IsFalse(result);
            Assert.AreEqual("DateOfBirth cannot be in the future", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenTimeOfDeathIsMinValue()
        {
            var personDto = new PersonDto
            {
                Id = Guid.Empty,
                FirstName = "John",
                LastName = "Doe",
                PersonalCode = "12345678901",
                DateOfBirth = new DateTime(1990, 1, 1),
                TimeOfDeath = DateTime.MinValue
            };

            var result = attribute.IsValid(personDto);

            Assert.IsFalse(result);
            Assert.AreEqual("TimeOfDeath is invalid", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenTimeOfDeathIsMaxValue()
        {
            var personDto = new PersonDto
            {
                Id = Guid.Empty,
                FirstName = "John",
                LastName = "Doe",
                PersonalCode = "12345678901",
                DateOfBirth = new DateTime(1990, 1, 1),
                TimeOfDeath = DateTime.MaxValue
            };

            var result = attribute.IsValid(personDto);

            Assert.IsFalse(result);
            Assert.AreEqual("TimeOfDeath is invalid", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenTimeOfDeathIsInTheFuture()
        {
            var personDto = new PersonDto
            {
                Id = Guid.Empty,
                FirstName = "John",
                LastName = "Doe",
                PersonalCode = "12345678901",
                DateOfBirth = new DateTime(1990, 1, 1),
                TimeOfDeath = DateTime.Now.AddYears(1) // Future date
            };

            var result = attribute.IsValid(personDto);

            Assert.IsFalse(result);
            Assert.AreEqual("TimeOfDeath cannot be in the future", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnTrue_WhenAllValuesAreValid()
        {
            var personDto = new PersonDto
            {
                Id = Guid.Empty,
                FirstName = "John",
                LastName = "Doe",
                PersonalCode = "12345678901",
                DateOfBirth = new DateTime(1990, 1, 1),
                TimeOfDeath = null
            };

            var result = attribute.IsValid(personDto);

            Assert.IsTrue(result);
            Assert.IsNull(attribute.ErrorMessage);
        }
    }
}
