using PersonService.Models;
using System.ComponentModel.DataAnnotations;

namespace PersonService.Tests.Models
{
    [TestClass]
    public class PersonChangeLogTests
    {
        [TestMethod]
        public void PersonChangeLog_RequiredProperties_ShouldBeValidated()
        {
            var changeLog = new PersonChangeLog
            {
                Id = Guid.Empty,
                PersonId = Guid.Empty,
                ChangeType = null,
            };

            var validationResults = ValidateModel(changeLog);

            Assert.IsTrue(validationResults.Exists(v => v.MemberNames.Contains(nameof(PersonChangeLog.ChangeType))));
        }

        [TestMethod]
        public void PersonChangeLog_ValidModel_ShouldPassValidation()
        {
            var changeLog = new PersonChangeLog
            {
                Id = Guid.NewGuid(),
                PersonId = Guid.NewGuid(),
                ChangeType = "INSERT",
                ChangeTime = DateTime.UtcNow,
                ChangedBy = "user123",
                OldValue = "old_value",
                NewValue = "new_value"
            };

            var validationResults = ValidateModel(changeLog);

            Assert.AreEqual(0, validationResults.Count);
        }

        [TestMethod]
        public void PersonChangeLog_Constructor_ShouldSetProperties()
        {
            var personId = Guid.NewGuid();
            var changeType = "UPDATE";

            var changeLog = new PersonChangeLog(personId, changeType);

            Assert.AreEqual(personId, changeLog.PersonId);
            Assert.AreEqual(changeType, changeLog.ChangeType);
            Assert.AreNotEqual(default(DateTime), changeLog.ChangeTime);
            Assert.IsTrue((DateTime.UtcNow - changeLog.ChangeTime).TotalSeconds < 1); // Ensure ChangeTime is close to now
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
