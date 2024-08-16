using System.Text;

namespace PersonService.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RequestLoggingMiddleware> logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //Log XRoad request

            var sbLog = new StringBuilder();
            var headers = context.Request.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}");
            var headersString = string.Join(";", headers);
            sbLog.AppendLine($"Incoming Request: {context.Request.Method} {context.Request.Path}\nHeaders:\n{headersString}");          

            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;
            if (!string.IsNullOrWhiteSpace(body))
            {
                sbLog.AppendLine($"Request Body: {body}");
            }
            logger.LogInformation(sbLog.ToString());

            await next(context);
        }
    }
}
