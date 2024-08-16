using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace PersonService.Dtos
{
    public class XroadHeaders
    {
        [FromHeader(Name = "X-Road-Client")]
        public string? XRoadClient { get; set; }

        [FromHeader(Name = "X-Road-Service")]
        public string? XRoadService { get; set; }

        [FromHeader(Name = "X-Road-UserId")]
        public string? XRoadUserId { get; set; }

        [FromHeader(Name = "X-Road-Id")]
        public string? XRoadId { get; set; }

        [FromHeader(Name = "X-Road-Request-Hash")]
        public string? XRoadRequestHash { get; set; }

        [FromHeader(Name = "X-Road-Request-Id")]
        public string? XRoadRequestId { get; set; }

        [FromHeader(Name = "X-Road-Security-Server")]
        public string? XRoadSecurityServer { get; set; }

        [FromHeader(Name = "X-Road-Represented-Party")]
        public string? XRoadRepresentedParty { get; set; }

        [FromHeader(Name = "X-Road-Issue")]
        public string? XRoadIssue { get; set; }
    }

}
