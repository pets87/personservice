using PersonService.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.Tests.Exceptions
{
    [TestClass]
    public class EntityNotFoundExceptionTests
    {
        [TestMethod]
        public void Constructor_WithMessage_SetsMessageProperty()
        {
            var expectedMessage = "Entity not found";
            var exception = new EntityNotFoundException(expectedMessage);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [TestMethod]
        public void Constructor_WithMessageAndInnerException_SetsProperties()
        {
            var expectedMessage = "Entity not found";
            var innerException = new Exception("Inner exception message");
            var exception = new EntityNotFoundException(expectedMessage, innerException);
            Assert.AreEqual(expectedMessage, exception.Message);
            Assert.AreEqual(innerException, exception.InnerException);
        }

        [TestMethod]
        public void Constructor_WithInnerException_SetsInnerExceptionMessage()
        {
            var expectedMessage = "Entity not found";
            var innerExceptionMessage = "Inner exception message";
            var innerException = new Exception(innerExceptionMessage);
            var exception = new EntityNotFoundException(expectedMessage, innerException);

            Assert.AreEqual(expectedMessage, exception.Message);
            Assert.AreEqual(innerException, exception.InnerException);
            Assert.AreEqual(innerExceptionMessage, exception.InnerException?.Message);
        }
    }
}
