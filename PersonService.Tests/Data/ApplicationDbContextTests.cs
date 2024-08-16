using Microsoft.EntityFrameworkCore;
using PersonService.Data;
using PersonService.Models;

namespace PersonService.Tests.Data
{
    [TestClass]
    public class ApplicationDbContextTests
    {
        private ApplicationDbContext? dbContext;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestApplicationDbContextDb")
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
        public void DbContext_ShouldCreateDatabaseWithPersonAndPersonChangeLogTables()
        {
            dbContext!.Database.EnsureCreated();

            Assert.IsFalse(dbContext.Persons.Any());
            Assert.IsFalse(dbContext.PersonChangeLogs.Any());
        }

        [TestMethod]
        public async Task PersonsDbSet_ShouldAllowCrudOperations()
        {
            var person = new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                PersonalCode = "12345678901",
                DateOfBirth = new DateTime(1990, 1, 1)
            };

            dbContext!.Persons.Add(person);
            await dbContext.SaveChangesAsync();

            var savedPerson = await dbContext.Persons.FindAsync(person.Id);
            Assert.IsNotNull(savedPerson);
            Assert.AreEqual(person.FirstName, savedPerson.FirstName);

            savedPerson.LastName = "UpdatedUser";
            dbContext.Persons.Update(savedPerson);
            await dbContext.SaveChangesAsync();

            var updatedPerson = await dbContext.Persons.FindAsync(person.Id);
            Assert.AreEqual("UpdatedUser", updatedPerson.LastName);

            dbContext.Persons.Remove(updatedPerson);
            await dbContext.SaveChangesAsync();

            var deletedPerson = await dbContext.Persons.FindAsync(person.Id);
            Assert.IsNull(deletedPerson);
        }

        [TestMethod]
        public async Task PersonChangeLogsDbSet_ShouldAllowCrudOperations()
        {
            // Arrange
            var changeLog = new PersonChangeLog
            {
                Id = Guid.NewGuid(),
                PersonId = Guid.NewGuid(),
                ChangeTime = DateTime.UtcNow,
                ChangeType = "Modified",
                OldValue = "OldValue",
                NewValue = "NewValue",
                ChangedBy = "TestUser"
            };

            
            dbContext!.PersonChangeLogs.Add(changeLog);
            await dbContext.SaveChangesAsync();

            var savedChangeLog = await dbContext.PersonChangeLogs.FindAsync(changeLog.Id);
            Assert.IsNotNull(savedChangeLog);
            Assert.AreEqual(changeLog.ChangeType, savedChangeLog.ChangeType);

            savedChangeLog.ChangeType = "Updated";
            dbContext.PersonChangeLogs.Update(savedChangeLog);
            await dbContext.SaveChangesAsync();

            var updatedChangeLog = await dbContext.PersonChangeLogs.FindAsync(changeLog.Id);
            Assert.AreEqual("Updated", updatedChangeLog.ChangeType);

            dbContext.PersonChangeLogs.Remove(updatedChangeLog);
            await dbContext.SaveChangesAsync();

            var deletedChangeLog = await dbContext.PersonChangeLogs.FindAsync(changeLog.Id);
            Assert.IsNull(deletedChangeLog);
        }
    }
}
