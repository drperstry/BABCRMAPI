using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace BabCrm.ApiGateway.Security
{
    public class BasicAuthenticationAttribute : Attribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authHeader = context.HttpContext.Request.Headers[HeaderNames.Authorization];
            
            if (authHeader.Count == 0)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!AuthenticationHeaderValue.TryParse(authHeader, out var headerValue) || headerValue.Scheme != "Basic")
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var decodedHeader = Encoding.UTF8.GetString(Convert.FromBase64String(headerValue.Parameter));
            var credentials = decodedHeader.Split(':');

            if (credentials.Length != 2)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!IsAuthorized(credentials[0], credentials[1]))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, credentials[0])
        };
            var identity = new ClaimsIdentity(claims, "Basic");
            var principal = new ClaimsPrincipal(identity);

            context.HttpContext.User = principal;
        }

        private bool IsAuthorized(string username, string password)
        {
            return (username == "BAB2023" && password == "yxFm7p8p+6j4ucyJ33r8cq8tej4EqUwn");
        }
    }
}
