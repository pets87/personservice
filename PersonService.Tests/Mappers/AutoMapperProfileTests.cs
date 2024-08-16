using AutoMapper;
using PersonService.Dtos.Person;
using PersonService.Mappers;
using PersonService.Models;

namespace PersonService.Tests.Mappers
{
    [TestClass]
    public class AutoMapperProfileTests
    {
        private readonly IMapper _mapper;

        public AutoMapperProfileTests()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<AutoMapperProfile>(); });

            _mapper = config.CreateMapper();
        }

        [TestMethod]
        public void AutoMapperProfile_MapsPersonDtoToPerson()
        {
            var personDto = new PersonDto
            {
                Id = Guid.NewGuid(),
                FirstName = "Edgar",
                LastName = "Savisaar",
                PersonalCode = "38605043778",
                DateOfBirth = new DateTime(1986, 05, 04, 12, 23, 21, DateTimeKind.Utc)
            };

            var person = _mapper.Map<Person>(personDto);

            Assert.AreEqual(personDto.Id, person.Id);
            Assert.AreEqual(personDto.FirstName, person.FirstName);
            Assert.AreEqual(personDto.LastName, person.LastName);
            Assert.AreEqual(personDto.DateOfBirth, person.DateOfBirth);
            Assert.AreEqual(personDto.TimeOfDeath, person.TimeOfDeath);
            Assert.AreEqual(personDto.PersonalCode, person.PersonalCode);
        }

        [TestMethod]
        public void AutoMapperProfile_MapsPersonToPersonDto()
        {
            var person = new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Roberto",
                LastName = "Dinamite",
                PersonalCode = "46806164715",
                DateOfBirth = new DateTime(1968, 06, 16, 10, 12, 52, DateTimeKind.Utc),
                TimeOfDeath = new DateTime(2023, 1, 1, 10, 0, 0)
            };

            var personDto = _mapper.Map<PersonDto>(person);

            Assert.AreEqual(person.Id, personDto.Id);
            Assert.AreEqual(person.FirstName, personDto.FirstName);
            Assert.AreEqual(person.LastName, personDto.LastName);
            Assert.AreEqual(person.DateOfBirth, personDto.DateOfBirth);
            Assert.AreEqual(person.TimeOfDeath, personDto.TimeOfDeath);
            Assert.AreEqual(person.PersonalCode, personDto.PersonalCode);
        }
    }
}
