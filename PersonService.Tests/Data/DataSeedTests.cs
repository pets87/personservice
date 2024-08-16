using Microsoft.EntityFrameworkCore;
using PersonService.Data;
using PersonService.Models;

namespace PersonService.Tests.Data
{
    [TestClass]
    public class DataSeedTests
    {
        private ApplicationDbContext? dbContext;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDataSeedDb")
                .Options;

            dbContext = new ApplicationDbContext(options);
        }

        [TestCleanup]
        public void TearDown()
        {
            dbContext?.Database.EnsureDeleted();
            dbContext?.Dispose();
        }

        [TestMethod]
        public void SeedPersons_ShouldSeedDatabase_WhenNoPersonsExist()
        {
            DataSeed.SeedPersons(dbContext);

            var persons = dbContext.Persons.ToList();
            Assert.AreEqual(3, persons.Count);
            Assert.IsTrue(persons.Any(p => p.FirstName == "Edgar" && p.LastName == "Savisaar"));
            Assert.IsTrue(persons.Any(p => p.FirstName == "Roberto" && p.LastName == "Dinamite"));
            Assert.IsTrue(persons.Any(p => p.FirstName == "Marek" && p.LastName == "Plura"));
        }

        [TestMethod]
        public void SeedPersons_ShouldNotAddPersons_WhenPersonsAlreadyExist()
        {
            var existingPerson = new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Existing",
                LastName = "Person",
                PersonalCode = "12345678901",
                DateOfBirth = new DateTime(1990, 1, 1)
            };
            dbContext.Persons.Add(existingPerson);
            dbContext.SaveChanges();

            DataSeed.SeedPersons(dbContext);

            var persons = dbContext.Persons.ToList();
            Assert.AreEqual(1, persons.Count);
            Assert.IsTrue(persons.Any(p => p.FirstName == "Existing" && p.LastName == "Person"));
        }

        [TestMethod]
        public void SeedPersons_ShouldNotAddDuplicates_WhenCalledMultipleTimes()
        {
            DataSeed.SeedPersons(dbContext);
            DataSeed.SeedPersons(dbContext);

            var persons = dbContext.Persons.ToList();
            Assert.AreEqual(3, persons.Count);
        }
    }
}
