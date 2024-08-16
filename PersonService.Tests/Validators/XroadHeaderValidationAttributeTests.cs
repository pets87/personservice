using PersonService.Dtos;
using PersonService.Validators;

namespace PersonService.Tests.Validators
{

    [TestClass]
    public class XroadHeaderValidationAttributeTests
    {
        private XroadHeaderValidationAttribute attribute;

        [TestInitialize]
        public void SetUp()
        {
            attribute = new XroadHeaderValidationAttribute();
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenValueIsNull()
        {
            object value = null;

            var result = attribute.IsValid(value);

            Assert.IsFalse(result);
            Assert.AreEqual("No X-Road Headers found", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenXRoadClientIsMissing()
        {
            var xroadHeaders = new XroadHeaders
            {
                XRoadClient = null,
                XRoadService = "some/service/path",
                XRoadSecurityServer = "some/server/path"
            };

            var result = attribute.IsValid(xroadHeaders);

            Assert.IsFalse(result);
            Assert.AreEqual("No X-Road-Client header is mandatory", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenXRoadClientIsInvalid()
        {
            var xroadHeaders = new XroadHeaders
            {
                XRoadClient = "invalid-client-format",
                XRoadService = "some/service/path",
                XRoadSecurityServer = "some/server/path"
            };

            var result = attribute.IsValid(xroadHeaders);

            Assert.IsFalse(result);
            Assert.AreEqual("X-Road-Client must be in form of [X-Road instance]/[member class]/[member code]/[subsystem code]", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenXRoadServiceIsInvalid()
        {
            var xroadHeaders = new XroadHeaders
            {
                XRoadClient = "xroad-instance/member-class/member-code/subsystem-code",
                XRoadService = "invalid-service-format",
                XRoadSecurityServer = "some/server/path"
            };

            var result = attribute.IsValid(xroadHeaders);

            Assert.IsFalse(result);
            Assert.AreEqual("X-Road-Service must be in form of [X-Road instance]/[member class]/[member code]/[subsystem code]/[service code]", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenXRoadSecurityServerIsInvalid()
        {
            var xroadHeaders = new XroadHeaders
            {
                XRoadClient = "xroad-instance/member-class/member-code/subsystem-code",
                XRoadService = "xroad-instance/member-class/member-code/subsystem-code/service-code",
                XRoadSecurityServer = "invalid-security-server-format"
            };

            var result = attribute.IsValid(xroadHeaders);

            Assert.IsFalse(result);
            Assert.AreEqual("X-Road-Security-Server must be in form of [X-Road instance]/[member class]/[member code]/[server code]", attribute.ErrorMessage);
        }

        [TestMethod]
        public void IsValid_ShouldReturnTrue_WhenAllHeadersAreValid()
        {
            var xroadHeaders = new XroadHeaders
            {
                XRoadClient = "xroad-instance/member-class/member-code/subsystem-code",
                XRoadService = "xroad-instance/member-class/member-code/subsystem-code/service-code",
                XRoadSecurityServer = "xroad-instance/member-class/member-code/server-code"
            };

            var result = attribute.IsValid(xroadHeaders);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValid_ShouldReturnTrue_WhenOptionalHeadersAreMissing()
        {
            var xroadHeaders = new XroadHeaders
            {
                XRoadClient = "xroad-instance/member-class/member-code/subsystem-code"
               
            };

            var result = attribute.IsValid(xroadHeaders);

            Assert.IsTrue(result);
        }
    }
}
