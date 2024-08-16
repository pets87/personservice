using PersonService.Dtos;
using System.ComponentModel.DataAnnotations;

namespace PersonService.Validators
{
    public class XroadHeaderValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var xroadHeaders = value as XroadHeaders;
            if (xroadHeaders is null) 
            {
                ErrorMessage = "No X-Road Headers found";
                return false;
            }

            /*
              https://www.x-tee.ee/docs/live/xroad/pr-rest_x-road_message_protocol_for_rest.html
              4.3 - There is only one mandatory HTTP header in the protocol that needs to be set by the client. Otherwise the use of headers in X-Road REST service calls is OPTIONAL
             */
            
            if (string.IsNullOrWhiteSpace(xroadHeaders.XRoadClient)) 
            {
                ErrorMessage = "No X-Road-Client header is mandatory";
                return false;
            }
            var splittedXroadClient = xroadHeaders.XRoadClient.Split('/');
            if (splittedXroadClient.Length < 4) 
            {
                ErrorMessage = "X-Road-Client must be in form of [X-Road instance]/[member class]/[member code]/[subsystem code]";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(xroadHeaders.XRoadService)) 
            {
                var splittedXroadService = xroadHeaders.XRoadService.Split("/");
                if (splittedXroadService.Length < 5) 
                {
                    ErrorMessage = "X-Road-Service must be in form of [X-Road instance]/[member class]/[member code]/[subsystem code]/[service code]";
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(xroadHeaders.XRoadSecurityServer))
            {
                var splittedXRoadSecurityServer = xroadHeaders.XRoadSecurityServer.Split("/");
                if (splittedXRoadSecurityServer.Length < 4)
                {
                    ErrorMessage = "X-Road-Security-Server must be in form of [X-Road instance]/[member class]/[member code]/[server code]";
                    return false;
                }
            }

            return true;
        }
    }
}
