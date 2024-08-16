using PersonService.Dtos.Person;

namespace PersonService.Tests.Dtos.Person
{
    [TestClass]
    public class PersonChangedValueTests
    {
        [TestMethod]
        public void PersonChangedValue_Constructor_ShouldInitializePropertiesCorrectly()
        {
            var expectedField = "FirstName";
            var expectedOldValue = "John";
            var expectedNewValue = "Jonathan";

            var personChangedValue = new PersonChangedValue(expectedField, expectedOldValue, expectedNewValue);

            Assert.AreEqual(expectedField, personChangedValue.Field);
            Assert.AreEqual(expectedOldValue, personChangedValue.OldValue);
            Assert.AreEqual(expectedNewValue, personChangedValue.NewValue);
        }

        [TestMethod]
        public void PersonChangedValue_Field_ShouldGetAndSetCorrectly()
        {
            var personChangedValue = new PersonChangedValue("FieldName", "OldValue", "NewValue");
            var expectedField = "LastName";

            personChangedValue.Field = expectedField;

            Assert.AreEqual(expectedField, personChangedValue.Field);
        }

        [TestMethod]
        public void PersonChangedValue_OldValue_ShouldGetAndSetCorrectly()
        {
            var personChangedValue = new PersonChangedValue("FieldName", "OldValue", "NewValue");
            var expectedOldValue = "Smith";

            personChangedValue.OldValue = expectedOldValue;

            Assert.AreEqual(expectedOldValue, personChangedValue.OldValue);
        }

        [TestMethod]
        public void PersonChangedValue_NewValue_ShouldGetAndSetCorrectly()
        {
            var personChangedValue = new PersonChangedValue("FieldName", "OldValue", "NewValue");
            var expectedNewValue = "Doe";

            personChangedValue.NewValue = expectedNewValue;

            Assert.AreEqual(expectedNewValue, personChangedValue.NewValue);
        }
    }
}
