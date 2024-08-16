using PersonService.Utilities;
using System.Text.Json;

namespace PersonService.Tests.Utilities
{
    [TestClass]
    public class PersonUtilsTests
    {
        [TestMethod]
        public void GetChangedValues_Insert_ShouldReturnCorrectChanges()
        {
            var oldValue = (string?)null;
            var newValue = JsonSerializer.Serialize(new Dictionary<string, object>
            {
                { "FirstName", "John" },
                { "LastName", "Doe" }
            });
            var observableParameters = new List<string> { "FirstName" };

            var result = PersonUtils.GetChangedValues(oldValue, newValue, observableParameters);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("FirstName", result[0].Field);
            Assert.AreEqual(null, result[0].OldValue);
            Assert.AreEqual("John", result[0].NewValue);
        }

        [TestMethod]
        public void GetChangedValues_Update_ShouldReturnCorrectChanges()
        {
            var oldValue = JsonSerializer.Serialize(new Dictionary<string, object>
            {
                { "FirstName", "John" },
                { "LastName", "Doe" }
            });
            var newValue = JsonSerializer.Serialize(new Dictionary<string, object>
            {
                { "FirstName", "Jonathan" },
                { "LastName", "Doe" }
            });
            var observableParameters = new List<string> { "FirstName" };

            var result = PersonUtils.GetChangedValues(oldValue, newValue, observableParameters);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("FirstName", result[0].Field);
            Assert.AreEqual("John", result[0].OldValue);
            Assert.AreEqual("Jonathan", result[0].NewValue);
        }

        [TestMethod]
        public void GetChangedValues_Delete_ShouldReturnCorrectChanges()
        {
            var oldValue = JsonSerializer.Serialize(new Dictionary<string, object>
            {
                { "FirstName", "John" },
                { "LastName", "Doe" }
            });
            var newValue = (string?)null;
            var observableParameters = new List<string> { "FirstName" };

            var result = PersonUtils.GetChangedValues(oldValue, newValue, observableParameters);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("FirstName", result[0].Field);
            Assert.AreEqual("John", result[0].OldValue);
            Assert.AreEqual(null, result[0].NewValue);
        }

        [TestMethod]
        public void GetChangedValues_Insert_WithNoObservableParameters_ShouldReturnAllChanges()
        {
            var oldValue = (string?)null;
            var newValue = JsonSerializer.Serialize(new Dictionary<string, object>
            {
                { "FirstName", "John" },
                { "LastName", "Doe" }
            });
            var observableParameters = (List<string>?)null;

            var result = PersonUtils.GetChangedValues(oldValue, newValue, observableParameters);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("FirstName", result[0].Field);
            Assert.AreEqual(null, result[0].OldValue);
            Assert.AreEqual("John", result[0].NewValue);
            Assert.AreEqual("LastName", result[1].Field);
            Assert.AreEqual(null, result[1].OldValue);
            Assert.AreEqual("Doe", result[1].NewValue);
        }

        [TestMethod]
        public void GetChangedValues_Update_WithNoObservableParameters_ShouldReturnAllChanges()
        {
            var oldValue = JsonSerializer.Serialize(new Dictionary<string, object>
            {
                { "FirstName", "John" },
                { "LastName", "Doe" }
            });
            var newValue = JsonSerializer.Serialize(new Dictionary<string, object>
            {
                { "FirstName", "Jonathan" },
                { "LastName", "Smith" }
            });
            var observableParameters = (List<string>?)null;

            var result = PersonUtils.GetChangedValues(oldValue, newValue, observableParameters);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("FirstName", result[0].Field);
            Assert.AreEqual("John", result[0].OldValue);
            Assert.AreEqual("Jonathan", result[0].NewValue);
            Assert.AreEqual("LastName", result[1].Field);
            Assert.AreEqual("Doe", result[1].OldValue);
            Assert.AreEqual("Smith", result[1].NewValue);
        }

        [TestMethod]
        public void GetChangedValues_Delete_WithNoObservableParameters_ShouldReturnAllChanges()
        {
            var oldValue = JsonSerializer.Serialize(new Dictionary<string, object>
            {
                { "FirstName", "John" },
                { "LastName", "Doe" }
            });
            var newValue = (string?)null;
            var observableParameters = (List<string>?)null;

            var result = PersonUtils.GetChangedValues(oldValue, newValue, observableParameters);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("FirstName", result[0].Field);
            Assert.AreEqual("John", result[0].OldValue);
            Assert.AreEqual(null, result[0].NewValue);
            Assert.AreEqual("LastName", result[1].Field);
            Assert.AreEqual("Doe", result[1].OldValue);
            Assert.AreEqual(null, result[1].NewValue);
        }

        [TestMethod]
        public void GetChangedValues_ShouldHandleEmptyJson()
        {
            var oldValue = JsonSerializer.Serialize(new Dictionary<string, object>());
            var newValue = JsonSerializer.Serialize(new Dictionary<string, object>());
            var observableParameters = new List<string> { "FirstName" };

            var result = PersonUtils.GetChangedValues(oldValue, newValue, observableParameters);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetChangedValues_ShouldHandleInvalidJson()
        {
            var oldValue = "{ \"FirstName\": \"John\" }";
            var newValue = "{ \"FirstName\": \"Jonathan\" ";
            var observableParameters = new List<string> { "FirstName" };

            var result = PersonUtils.GetChangedValues(oldValue, newValue, observableParameters);

            Assert.IsNull(result);
        }
    }
}
