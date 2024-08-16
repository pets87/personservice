using PersonService.Dtos.Person;

namespace PersonService.Tests.Dtos.Person
{
    [TestClass]
    public class PersonDtoTests
    {
        [TestMethod]
        public void PersonDto_Properties_ShouldGetAndSetCorrectly()
        {
            var personDto = new PersonDto();
            var expectedId = Guid.NewGuid();
            var expectedPersonalCode = "37412180181";
            var expectedFirstName = "John";
            var expectedLastName = "Doe";
            var expectedDateOfBirth = new DateTime(1980, 1, 1);
            var expectedTimeOfDeath = new DateTime(2024, 8, 15);

            personDto.Id = expectedId;
            personDto.PersonalCode = expectedPersonalCode;
            personDto.FirstName = expectedFirstName;
            personDto.LastName = expectedLastName;
            personDto.DateOfBirth = expectedDateOfBirth;
            personDto.TimeOfDeath = expectedTimeOfDeath;

            Assert.AreEqual(expectedId, personDto.Id);
            Assert.AreEqual(expectedPersonalCode, personDto.PersonalCode);
            Assert.AreEqual(expectedFirstName, personDto.FirstName);
            Assert.AreEqual(expectedLastName, personDto.LastName);
            Assert.AreEqual(expectedDateOfBirth, personDto.DateOfBirth);
            Assert.AreEqual(expectedTimeOfDeath, personDto.TimeOfDeath);
        }

        [TestMethod]
        public void PersonDto_Id_ShouldHandleNull()
        {
            var personDto = new PersonDto
            {
                Id = null,
                PersonalCode = "46806164715",
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1980, 1, 1),
                TimeOfDeath = null
            };

            Assert.IsNull(personDto.Id);
        }

        [TestMethod]
        public void PersonDto_TimeOfDeath_ShouldHandleNull()
        {
            var personDto = new PersonDto
            {
                Id = Guid.NewGuid(),
                PersonalCode = "38605043778",
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1980, 1, 1),
                TimeOfDeath = null
            };

            Assert.IsNull(personDto.TimeOfDeath);
        }
    }
}
