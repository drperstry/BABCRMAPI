using Azure.Core;
using BabCrm.ApiGateway;
using BabCrm.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BabCrm.ApiGateway
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CatchRequest
    {
        private readonly RequestDelegate _next;

        public CatchRequest(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;
            var messageId = context.Request.Headers["Message-ID"];
            var languageHeader = request.Headers["Accept-Language"];
            var authorization = request.Headers["Authorization"];
            var apiVersion = request.Headers["X-Api-Version"];
            string verb = request.Method;

            if (request.Method == HttpMethods.Post && request.ContentLength > 0)
            {
                request.EnableBuffering();
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];

                var x = await request.Body.ReadAsync(buffer, 0, buffer.Length);

                var requestContent = Encoding.UTF8.GetString(buffer);

                request.Body.Position = 0;  //rewinding the stream to 0

                var url = request.GetDisplayUrl();

                var queryString = request.QueryString.Value;

                dynamic requestObj = new ExpandoObject();

                requestObj.Url = url;

                var hasQueryString = request.QueryString.HasValue;

                if (hasQueryString)
                {
                    requestObj.QueryString = request.QueryString.Value;
                }

                requestObj.Body = requestContent;

                Logger.InfoLog("message was received", messageId, url,languageHeader,requestContent, verb,authorization,apiVersion);

            }else if (request.Method == HttpMethods.Get)
            {
                var url = request.GetDisplayUrl();


                string queryString = "";

                if (request.QueryString.HasValue)
                {
                    queryString = request.QueryString.Value; 
                }

                //Logger.RequestLog(null, "GET", url, languageHeader,queryString,requestId);
                Logger.InfoLog("message was received", messageId, url,languageHeader, null, verb, authorization, apiVersion);

            }
            else if(request.Method == HttpMethods.Delete)
            {
                request.EnableBuffering();
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];

                var x = await request.Body.ReadAsync(buffer, 0, buffer.Length);

                var requestContent = Encoding.UTF8.GetString(buffer);

                request.Body.Position = 0;  //rewinding the stream to 0

                var url = request.GetDisplayUrl();

                var queryString = request.QueryString.Value;

                dynamic requestObj = new ExpandoObject();

                requestObj.Url = url;

                var hasQueryString = request.QueryString.HasValue;

                if (hasQueryString)
                {
                    requestObj.QueryString = request.QueryString.Value;
                }

                requestObj.Body = requestContent;

                //Logger.RequestLog(requestObj, "DELETE", url, null, null, requestId);
                Logger.InfoLog("message was received", messageId, url,languageHeader, requestContent, verb, authorization, apiVersion);
            }
            await _next(context);
        }
    }
}
    

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CatchRequestExtensions
    {
        public static IApplicationBuilder UseCatchRequest(this IApplicationBuilder builder)
        {
        return builder.UseMiddleware<CatchRequest>();
        }
}

