using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using PersonService.Middlewares;
using System.Text;

namespace PersonService.Tests.Middlewares
{
    [TestClass]
    public class RequestLoggingMiddlewareTests
    {
        [TestMethod]
        public async Task InvokeAsync_LogsGetRequestDetails()
        {
            var loggerMock = new Mock<ILogger<RequestLoggingMiddleware>>();
            var middleware = new RequestLoggingMiddleware(async (innerHttpContext) => { await Task.CompletedTask; }, loggerMock.Object);

            var context = new DefaultHttpContext();
            context.Request.Method = "GET";
            context.Request.Path = "/api/person";

            await middleware.InvokeAsync(context);

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Incoming Request: GET")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }


        [TestMethod]
        public async Task InvokeAsync_LogsPostRequestDetails()
        {
            var loggerMock = new Mock<ILogger<RequestLoggingMiddleware>>();
            var middleware = new RequestLoggingMiddleware(async (innerHttpContext) => { await Task.CompletedTask; }, loggerMock.Object);

            var context = new DefaultHttpContext();
            context.Request.Method = "POST";
            context.Request.Path = "/api/person";
            context.Request.Headers["Content-Type"] = "application/json";

            var requestBody = """
                                {
                    "id": "8e7e7cbe-035a-4bd2-9df1-5f82604fa9aa",
                    "personalCode": "38605043778",
                    "firstName": "Edgarr",
                    "lastName": "Savisaar",
                    "dateOfBirth": "1986-05-04T12:23:21Z",
                    "timeOfDeath": null
                }
                """;
            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
            context.Request.ContentLength = requestBody.Length;
            context.Request.EnableBuffering();

            await middleware.InvokeAsync(context);

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("38605043778")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
