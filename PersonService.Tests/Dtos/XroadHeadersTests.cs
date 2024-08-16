using PersonService.Dtos;

namespace PersonService.Tests.Dtos
{
    [TestClass]
    public class XroadHeadersTests
    {
        [TestMethod]
        public void XroadHeaders_AllProperties_ShouldGetAndSetCorrectly()
        {
            var headers = new XroadHeaders();
            var expectedXRoadClient = "TestClient";
            var expectedXRoadService = "TestService";
            var expectedXRoadUserId = "TestUserId";
            var expectedXRoadId = "TestId";
            var expectedXRoadRequestHash = "TestRequestHash";
            var expectedXRoadRequestId = "TestRequestId";
            var expectedXRoadSecurityServer = "TestSecurityServer";
            var expectedXRoadRepresentedParty = "TestRepresentedParty";
            var expectedXRoadIssue = "TestIssue";

            headers.XRoadClient = expectedXRoadClient;
            headers.XRoadService = expectedXRoadService;
            headers.XRoadUserId = expectedXRoadUserId;
            headers.XRoadId = expectedXRoadId;
            headers.XRoadRequestHash = expectedXRoadRequestHash;
            headers.XRoadRequestId = expectedXRoadRequestId;
            headers.XRoadSecurityServer = expectedXRoadSecurityServer;
            headers.XRoadRepresentedParty = expectedXRoadRepresentedParty;
            headers.XRoadIssue = expectedXRoadIssue;

            Assert.AreEqual(expectedXRoadClient, headers.XRoadClient);
            Assert.AreEqual(expectedXRoadService, headers.XRoadService);
            Assert.AreEqual(expectedXRoadUserId, headers.XRoadUserId);
            Assert.AreEqual(expectedXRoadId, headers.XRoadId);
            Assert.AreEqual(expectedXRoadRequestHash, headers.XRoadRequestHash);
            Assert.AreEqual(expectedXRoadRequestId, headers.XRoadRequestId);
            Assert.AreEqual(expectedXRoadSecurityServer, headers.XRoadSecurityServer);
            Assert.AreEqual(expectedXRoadRepresentedParty, headers.XRoadRepresentedParty);
            Assert.AreEqual(expectedXRoadIssue, headers.XRoadIssue);
        }
    }
}
