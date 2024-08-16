using PersonService.Dtos.Person;

namespace PersonService.Tests.Dtos.Person
{
    [TestClass]
    public class PersonChangesResponseDtoTests
    {
        [TestMethod]
        public void PersonChangesResponseDto_Changes_ShouldGetAndSetCorrectly()
        {
            var responseDto = new PersonChangesResponseDto();
            var expectedChanges = new List<PersonChangeListDto>
            {
                new PersonChangeListDto
                {
                    PersonCode = "38605043778",
                    PersonChanges = new List<PersonChangeDto>
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
                            ChangedBy = "Admin"
                        }
                    }
                },
                new PersonChangeListDto
                {
                    PersonCode = "46806164715",
                    PersonChanges = new List<PersonChangeDto>
                    {
                        new PersonChangeDto
                        {
                            ChangeType = "Delete",
                            ChangedValues = new List<PersonChangedValue>
                            {
                                new PersonChangedValue("FirstName", "John", "Jonas")
                            },
                            ChangedTime = new DateTime(2024, 8, 16, 11, 45, 0),
                            ChangedBy = "11111111111"
                        }
                    }
                }
            };

            responseDto.Changes = expectedChanges;

            Assert.AreEqual(expectedChanges.Count, responseDto.Changes.Count);

            for (int i = 0; i < expectedChanges.Count; i++)
            {
                var expected = expectedChanges[i];
                var actual = responseDto.Changes[i];

                Assert.AreEqual(expected.PersonCode, actual.PersonCode);
                Assert.AreEqual(expected.PersonChanges.Count, actual.PersonChanges.Count);

                for (int j = 0; j < expected.PersonChanges.Count; j++)
                {
                    var expectedChange = expected.PersonChanges[j];
                    var actualChange = actual.PersonChanges[j];

                    Assert.AreEqual(expectedChange.ChangeType, actualChange.ChangeType);
                    Assert.AreEqual(expectedChange.ChangedValues.Count, actualChange.ChangedValues.Count);

                    for (int k = 0; k < expectedChange.ChangedValues.Count; k++)
                    {
                        var expectedValue = expectedChange.ChangedValues[k];
                        var actualValue = actualChange.ChangedValues[k];

                        Assert.AreEqual(expectedValue.Field, actualValue.Field);
                        Assert.AreEqual(expectedValue.OldValue, actualValue.OldValue);
                        Assert.AreEqual(expectedValue.NewValue, actualValue.NewValue);
                    }

                    Assert.AreEqual(expectedChange.ChangedTime, actualChange.ChangedTime);
                    Assert.AreEqual(expectedChange.ChangedBy, actualChange.ChangedBy);
                }
            }
        }

        [TestMethod]
        public void PersonChangesResponseDto_Changes_ShouldHandleEmptyList()
        {
            var responseDto = new PersonChangesResponseDto();
            var expectedChanges = new List<PersonChangeListDto>();

            responseDto.Changes = expectedChanges;

            Assert.AreEqual(expectedChanges.Count, responseDto.Changes.Count);
        }

        [TestMethod]
        public void PersonChangesResponseDto_Changes_ShouldHandleNull()
        {
            var responseDto = new PersonChangesResponseDto();
            var expectedChanges = (List<PersonChangeListDto>)null;

            responseDto.Changes = expectedChanges;

            Assert.IsNull(responseDto.Changes);
        }
    }

  

}
