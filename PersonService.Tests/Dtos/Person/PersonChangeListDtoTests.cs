using PersonService.Dtos.Person;

namespace PersonService.Tests.Dtos.Person
{
    [TestClass]
    public class PersonChangeListDtoTests
    {
        [TestMethod]
        public void PersonChangeListDto_Properties_ShouldGetAndSetCorrectly()
        {
            var personChangeListDto = new PersonChangeListDto();
            var expectedPersonCode = "11111111111";
            var expectedPersonChanges = new List<PersonChangeDto>
            {
                new PersonChangeDto
                {
                    ChangeType = "Update",
                    ChangedValues = new List<PersonChangedValue>
                    {
                        new PersonChangedValue("FirstName", "John", "Jonathan"),
                        new PersonChangedValue("LastName", "Doe", "Smith")
                    },
                    ChangedTime = new DateTime(2024, 8, 15, 10, 30, 0),
                    ChangedBy = "123456"
                }
            };

            personChangeListDto.PersonCode = expectedPersonCode;
            personChangeListDto.PersonChanges = expectedPersonChanges;

            Assert.AreEqual(expectedPersonCode, personChangeListDto.PersonCode);
            Assert.AreEqual(expectedPersonChanges.Count, personChangeListDto.PersonChanges.Count);

            for (int i = 0; i < expectedPersonChanges.Count; i++)
            {
                var expected = expectedPersonChanges[i];
                var actual = personChangeListDto.PersonChanges[i];
                Assert.AreEqual(expected.ChangeType, actual.ChangeType);
                CollectionAssert.AreEqual(expected.ChangedValues, actual.ChangedValues);
                Assert.AreEqual(expected.ChangedTime, actual.ChangedTime);
                Assert.AreEqual(expected.ChangedBy, actual.ChangedBy);
            }
        }
    }


   
}
