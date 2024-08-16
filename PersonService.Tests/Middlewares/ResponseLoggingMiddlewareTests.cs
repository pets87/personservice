using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using PersonService.Middlewares;

namespace PersonService.Tests.Middlewares
{
    [TestClass]
    public class ResponseLoggingMiddlewareTests
    {
        [TestMethod]
        public async Task InvokeAsync_LogsResponseDetails()
        {
            var loggerMock = new Mock<ILogger<ResponseLoggingMiddleware>>();
            var middleware = new ResponseLoggingMiddleware(async (innerHttpContext) =>
            {
                innerHttpContext.Response.StatusCode = 200;
                await innerHttpContext.Response.WriteAsync("Test Response Body");
            }, loggerMock.Object);

            var context = new DefaultHttpContext();
            var originalBodyStream = new MemoryStream();
            context.Response.Body = originalBodyStream;

            await middleware.InvokeAsync(context);

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Outgoing Response: 200")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Response Body: Test Response Body")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
