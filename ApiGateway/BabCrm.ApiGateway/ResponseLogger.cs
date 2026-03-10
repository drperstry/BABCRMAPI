using Azure.Core;
using BabCrm.Logging;

namespace BabCrm.ApiGateway
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ResponseLogger
    {
        private readonly RequestDelegate _next;

        public ResponseLogger(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var messageId = httpContext.Request.Headers["Message-ID"];
            Stream originalBody = httpContext.Response.Body;
            var languageHeader = httpContext.Request.Headers["Accept-Language"];

            try
            {
                using (var memStream = new MemoryStream())
                {
                    httpContext.Response.Body = memStream;

                    await _next(httpContext);

                    memStream.Position = 0;
                    string responseBody = await new StreamReader(memStream).ReadToEndAsync();
                    Console.WriteLine(responseBody);
                    Logger.InfoLog("message replied", messageId, null, languageHeader, responseBody,null,null);
                    memStream.Position = 0;
                    await memStream.CopyToAsync(originalBody);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                httpContext.Response.Body = originalBody;
            }

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ResponseLoggerExtensions
    {
        public static IApplicationBuilder UseResponseLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseLogger>();
        }
    }
}
