using PersonService.Validators.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.Tests.Validators.Person
{
    [TestClass]
    public class PersonCodeValidationAttributeTests
    {
        private PersonCodeValidationAttribute attribute;

        [TestInitialize]
        public void SetUp()
        {
            attribute = new PersonCodeValidationAttribute();
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenValueIsNull()
        {
            object value = null;

            var result = attribute.IsValid(value);

            Assert.IsFalse(result);
            Assert.AreEqual("Invalid personcode. Personcode must be 11 characters long.", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenValueIsEmptyString()
        {
            var value = string.Empty;

            var result = attribute.IsValid(value);

            Assert.IsFalse(result);
            Assert.AreEqual("Invalid personcode. Personcode must be 11 characters long.", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenValueIsWhitespace()
        {
            var value = "   ";

            var result = attribute.IsValid(value);

            Assert.IsFalse(result);
            Assert.AreEqual("Invalid personcode. Personcode must be 11 characters long.", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenValueIsTooShort()
        {
            var value = "123456789";

            var result = attribute.IsValid(value);

            Assert.IsFalse(result);
            Assert.AreEqual("Invalid personcode. Personcode must be 11 characters long.", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenValueIsTooLong()
        {
            var value = "123456789012";

            var result = attribute.IsValid(value);

            Assert.IsFalse(result);
            Assert.AreEqual("Invalid personcode. Personcode must be 11 characters long.", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnTrue_WhenValueIsExactly11CharactersLong()
        {
            var value = "12345678901";

            var result = attribute.IsValid(value);

            Assert.IsTrue(result);
            Assert.IsNull(attribute.ErrorMessage);
        }
    }
}
