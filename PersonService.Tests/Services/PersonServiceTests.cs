using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PersonService.Data;
using PersonService.Dtos.Person;
using PersonService.Exceptions;
using PersonService.Models;

namespace PersonService.Tests.Services
{

    [TestClass]
    public class PersonServiceTests
    {
        private ApplicationDbContext context;
        private IMapper mapper;
        private PersonService.Services.Impl.PersonService personService;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestPersonServiceDb")
                .Options;

            context = new ApplicationDbContext(options);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Person, PersonDto>();
                cfg.CreateMap<PersonDto, Person>();
            });

            mapper = config.CreateMapper();

            personService = new PersonService.Services.Impl.PersonService(context, mapper);
        }
        [TestCleanup]
        public void TearDown()
        {
            context?.Database.EnsureDeleted();
            context?.Dispose();
        }

        [TestMethod]
        public async Task GetPerson_ShouldReturnPersonDto_WhenPersonExists()
        {
            var personCode = "12345678901";
            var person = new Person { PersonalCode = personCode, FirstName = "John", LastName = "Doe" };
            await context.Persons.AddAsync(person);
            await context.SaveChangesAsync();

            var result = await personService.GetPerson(personCode);

            Assert.AreEqual(personCode, result.PersonalCode);
        }

        [TestMethod]
        public async Task GetPerson_ShouldThrowException_WhenPersonNotExists()
        {
            var personCode = "00000000000";

            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () => { await personService.GetPerson(personCode); });           
        }

        [TestMethod]
        public async Task CreatePerson_ShouldReturnPersonDto_WhenPersonIsCreated()
        {
            var personDto = new PersonDto { PersonalCode = "12345678901", FirstName= "John", LastName = "Doe" };

            var result = await personService.CreatePerson(personDto);

            Assert.AreEqual(personDto.PersonalCode, result.PersonalCode);
            Assert.IsNotNull(result.Id); // Check if the Id is set
        }

        [TestMethod]
        public async Task UpdatePerson_ShouldReturnPersonDto_WhenPersonIsUpdated()
        {
            var personId = Guid.NewGuid();
            var person = new Person
            {
                Id = personId,
                PersonalCode = "12345678901",
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1980, 1, 1)
            };
            await context.Persons.AddAsync(person);
            await context.SaveChangesAsync();

            var personDto = new PersonDto
            {
                Id = personId,
                PersonalCode = "12345678901",
                FirstName = "Jane",
                LastName = "Doe",
                DateOfBirth = new DateTime(1980, 1, 1)
            };

            var result = await personService.UpdatePerson(personDto);

            Assert.AreEqual(personDto.FirstName, result.FirstName);
            Assert.AreEqual(personDto.LastName, result.LastName);
        }
        [TestMethod]
        public async Task UpdatePerson_ShouldThrowException_WhenPersonNotExists()
        {
            var personDto = new PersonDto
            {
                Id = Guid.NewGuid(),
                PersonalCode = "12345678901",
                FirstName = "Jane",
                LastName = "Doe",
                DateOfBirth = new DateTime(1980, 1, 1)
            };

            try
            {
                await personService.UpdatePerson(personDto);
            }
            catch (EntityNotFoundException e) 
            {
                Assert.AreEqual($"Entity Not found with Id: {personDto.Id}", e.Message);
            }
        }


        [TestMethod]
        public async Task DeletePerson_ShouldReturnTrue_WhenPersonIsDeleted()
        {
            var personId = Guid.NewGuid();
            var person = new Person
            {
                Id = personId,
                PersonalCode = "12345678901",                
                FirstName = "John",
                LastName = "Doe"
            };
            await context.Persons.AddAsync(person);
            await context.SaveChangesAsync();

            var personDto = new PersonDto
            {
                Id = personId
            };

            var result = await personService.DeletePerson(personDto);

            Assert.IsTrue(result);
            Assert.IsFalse(await context.Persons.AnyAsync(p => p.Id == personId));
        }

        [TestMethod]
        public async Task DeletePerson_ShouldThrowException_WhenPersonNotExists()
        {
            var personDto = new PersonDto
            {
                Id = Guid.NewGuid(),
                PersonalCode = "12345678901",
                FirstName = "Jane",
                LastName = "Doe",
                DateOfBirth = new DateTime(1980, 1, 1)
            };

            try
            {
                await personService.DeletePerson(personDto);
            }
            catch (EntityNotFoundException e)
            {
                Assert.AreEqual($"Entity Not found with Id: {personDto.Id}", e.Message);
            }
        }

        [TestMethod]
        public async Task GetPersonChanges_ShouldReturnPersonChangesResponseDto_WhenChangesExist()
        {
            var personId = Guid.NewGuid();
            var personCode = "12345678901";
            var person = new Person
            {
                Id = personId,
                PersonalCode = personCode,
                FirstName = "John",
                LastName = "Doe"
            };
            await context.Persons.AddAsync(person);
            await context.SaveChangesAsync();

            var personChangeLog = new PersonChangeLog
            {
                PersonId = personId,
                ChangeType = "Update",
                OldValue = "{\"PersonalCode\":\"12345678900\"}",
                NewValue = "{\"PersonalCode\":\"12345678901\"}",
                ChangeTime = DateTime.UtcNow,
                ChangedBy = "11111111111"
            };
            await context.PersonChangeLogs.AddAsync(personChangeLog);
            await context.SaveChangesAsync();

            var request = new PersonChangesRequestDto
            {
                StartTime = DateTime.UtcNow.AddMinutes(-10),
                EndTime = DateTime.UtcNow.AddMinutes(10),
                ObservableParameters = new List<string> { "PersonalCode" }
            };

            var result = await personService.GetPersonChanges(request);

            Assert.AreEqual(1, result.Changes.Count);
            Assert.AreEqual(personCode, result.Changes[0].PersonCode);
            Assert.AreEqual(1, result.Changes[0].PersonChanges.Count);
            Assert.AreEqual("Update", result.Changes[0].PersonChanges[0].ChangeType);
            Assert.AreEqual("12345678900", result.Changes[0].PersonChanges[0].ChangedValues[0].OldValue);
            Assert.AreEqual("12345678901", result.Changes[0].PersonChanges[0].ChangedValues[0].NewValue);
        }
    }
}
