using PersonService.Dtos.Person;

namespace PersonService.Tests.Dtos.Person
{
    [TestClass]
    public class PersonChangeDtoTests
    {
        [TestMethod]
        public void PersonChangeDto_AllProperties_ShouldGetAndSetCorrectly()
        {
            var personChangeDto = new PersonChangeDto();
            var expectedChangeType = "Update";
            var expectedChangedValues = new List<PersonChangedValue>
            {
                new PersonChangedValue ( "FirstName", "John", "Jonathan" ),
                new PersonChangedValue ( "FirstName", "Jonas", "Jonathan" ),
            };
            var expectedChangedTime = DateTime.Now;
            var expectedChangedBy = "123456";

            personChangeDto.ChangeType = expectedChangeType;
            personChangeDto.ChangedValues = expectedChangedValues;
            personChangeDto.ChangedTime = expectedChangedTime;
            personChangeDto.ChangedBy = expectedChangedBy;

            Assert.AreEqual(expectedChangeType, personChangeDto.ChangeType);
            CollectionAssert.AreEqual(expectedChangedValues, personChangeDto.ChangedValues);
            Assert.AreEqual(expectedChangedTime, personChangeDto.ChangedTime);
            Assert.AreEqual(expectedChangedBy, personChangeDto.ChangedBy);
        }
    }

    
}
