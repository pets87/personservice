using System.Text;

namespace PersonService.Middlewares
{
    public class ResponseLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ResponseLoggingMiddleware> logger;

        public ResponseLoggingMiddleware(RequestDelegate next, ILogger<ResponseLoggingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //Log XRoad response

            var sbLog = new StringBuilder();
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await next(context);

                var headers = context.Response.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}");
                var headersString = string.Join(";", headers);

                sbLog.AppendLine($"Outgoing Response: {context.Response.StatusCode}\nHeaders:\n{headersString}");

                responseBody.Seek(0, SeekOrigin.Begin);
                var text = await new StreamReader(responseBody).ReadToEndAsync();
                responseBody.Seek(0, SeekOrigin.Begin);

                if (!string.IsNullOrWhiteSpace(text))
                {
                    sbLog.AppendLine($"Response Body: {text}");
                }

                logger.LogInformation(sbLog.ToString());
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }
}
