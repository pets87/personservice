using PersonService.Dtos.Person;

namespace PersonService.Tests.Dtos.Person
{
    [TestClass]
    public class PersonChangesRequestDtoTests
    {
        [TestMethod]
        public void PersonChangesRequestDto_Properties_ShouldGetAndSetCorrectly()
        {
            var requestDto = new PersonChangesRequestDto();
            var expectedStartTime = new DateTime(2024, 8, 15, 9, 0, 0);
            var expectedEndTime = new DateTime(2024, 8, 15, 17, 0, 0);
            var expectedObservableParameters = new List<string> { "Param1", "Param2" };

            requestDto.StartTime = expectedStartTime;
            requestDto.EndTime = expectedEndTime;
            requestDto.ObservableParameters = expectedObservableParameters;

            Assert.AreEqual(expectedStartTime, requestDto.StartTime);
            Assert.AreEqual(expectedEndTime, requestDto.EndTime);
            CollectionAssert.AreEqual(expectedObservableParameters, requestDto.ObservableParameters);
        }

        [TestMethod]
        public void PersonChangesRequestDto_ObservableParameters_ShouldHandleNull()
        {
            var requestDto = new PersonChangesRequestDto();
            var expectedStartTime = new DateTime(2024, 8, 15, 9, 0, 0);
            var expectedEndTime = new DateTime(2024, 8, 15, 17, 0, 0);

            requestDto.StartTime = expectedStartTime;
            requestDto.EndTime = expectedEndTime;
            requestDto.ObservableParameters = null;

            Assert.AreEqual(expectedStartTime, requestDto.StartTime);
            Assert.AreEqual(expectedEndTime, requestDto.EndTime);
            Assert.IsNull(requestDto.ObservableParameters);
        }

        [TestMethod]
        public void PersonChangesRequestDto_ObservableParameters_ShouldHandleEmptyList()
        {
            var requestDto = new PersonChangesRequestDto();
            var expectedStartTime = new DateTime(2024, 8, 15, 9, 0, 0);
            var expectedEndTime = new DateTime(2024, 8, 15, 17, 0, 0);
            var expectedObservableParameters = new List<string>();

            requestDto.StartTime = expectedStartTime;
            requestDto.EndTime = expectedEndTime;
            requestDto.ObservableParameters = expectedObservableParameters;

            Assert.AreEqual(expectedStartTime, requestDto.StartTime);
            Assert.AreEqual(expectedEndTime, requestDto.EndTime);
            CollectionAssert.AreEqual(expectedObservableParameters, requestDto.ObservableParameters);
        }
    }
}
