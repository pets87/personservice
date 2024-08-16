using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Moq;
using PersonService.Data.Interceptors;
using PersonService.Data;
using PersonService.Models;

namespace PersonService.Tests.Data.Interceptors
{

    [TestClass]
    public class PersonInterceptorTests
    {
        private PersonInterceptor? interceptor;
        private Mock<IHttpContextAccessor>? mockHttpContextAccessor;
        private Mock<HttpContext>? mockHttpContext;
        private ApplicationDbContext? dbContext;

        [TestInitialize]
        public void SetUp()
        {
            mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContext = new Mock<HttpContext>();

            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestPersonInterceptorDb")
                .Options;

            dbContext = new ApplicationDbContext(dbContextOptions);
            interceptor = new PersonInterceptor(mockHttpContextAccessor.Object);
        }

        [TestCleanup]
        public void TearDown()
        {
            dbContext?.Database.EnsureDeleted();
            dbContext?.Dispose();
        }

        [TestMethod]
        public void UpdateBaseEntityFields_ShouldUpdateFields_WhenModified()
        {
            var person = new Person { Id = Guid.NewGuid(), UpdatedBy = null, FirstName = "John", LastName = "Doe", PersonalCode = "11111111111" };

            dbContext.Persons.Add(person);
            dbContext.SaveChanges();

            person.FirstName = "Updated Name";
            var entry = dbContext.Entry(person);
            entry.State = EntityState.Modified;
            var entries = dbContext.ChangeTracker.Entries<BaseEntity>().ToList();

            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);
            mockHttpContext.Setup(x => x.Request.Headers).Returns(new HeaderDictionary { { "X-Road-UserId", "TestUser" } });

            interceptor.UpdateBaseEntityFields(entries, "TestUser");

            Assert.AreEqual("TestUser", entry.Property(x => x.UpdatedBy).CurrentValue);
            Assert.IsTrue((DateTime)entry.Property(x => x.UpdatedAt).CurrentValue > DateTime.MinValue);
        }

        [TestMethod]
        public void UpdateBaseEntityFields_ShouldNotUpdate_WhenUserIsNullOrEmpty()
        {
            var person = new Person { Id = Guid.NewGuid(), UpdatedBy = "OriginalUser", FirstName = "John", LastName = "Doe", PersonalCode = "11111111111" };

            dbContext.Persons.Add(person);
            dbContext.SaveChanges();
            person.FirstName = "Updated Name";
            var entry = dbContext.Entry(person);
            entry.State = EntityState.Modified;
            var entries = dbContext.ChangeTracker.Entries<BaseEntity>().ToList();

            interceptor.UpdateBaseEntityFields(entries, null);

            Assert.AreEqual("OriginalUser", entry.Property(x => x.UpdatedBy).CurrentValue);
        }

        [TestMethod]
        public void AddPersonsChangeLog_ShouldAddLogEntry_WhenPersonIsModified()
        {
            var person = new Person { Id = Guid.NewGuid(), UpdatedBy = "TestUser", FirstName = "John", LastName = "Doe", PersonalCode = "11111111111" };

            dbContext.Persons.Add(person);
            dbContext.SaveChanges();
            person.FirstName = "Updated Name";
            var entry = dbContext.Entry(person);
            entry.State = EntityState.Modified;

            DbContextEventData dbContextEventData = new DbContextEventData(null, null, dbContext);
            interceptor.AddPersonsChangeLog(dbContextEventData);
            dbContext.SaveChanges();

            var changeLog = dbContext.PersonChangeLogs.FirstOrDefault();
            Assert.IsNotNull(changeLog);
            Assert.AreEqual(person.Id, changeLog.PersonId);
            Assert.AreEqual("Modified", changeLog.ChangeType);
            Assert.AreEqual("TestUser", changeLog.ChangedBy);
        }

        [TestMethod]
        public void AddPersonsChangeLog_ShouldNotAddLogEntry_WhenNoChangesDetected()
        {
            var person = new Person { Id = Guid.NewGuid(), FirstName = "Original Name", UpdatedBy = "TestUser", LastName = "Doe", PersonalCode = "11111111111" };

            dbContext.Persons.Add(person);
            dbContext.SaveChanges();

            var entry = dbContext.Entry(person);
            entry.State = EntityState.Unchanged;

            DbContextEventData dbContextEventData = new DbContextEventData(null, null, dbContext);
            interceptor.AddPersonsChangeLog(dbContextEventData);
            dbContext.SaveChanges();

            Assert.IsFalse(dbContext.PersonChangeLogs.Any(x => x.PersonId == person.Id));
        }
    }
}
