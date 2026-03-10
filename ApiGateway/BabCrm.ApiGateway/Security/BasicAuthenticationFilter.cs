using BabCrm.Core;
using BabCrm.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BabCrm.ApiGateway.Security
{
    public class BasicAuthenticationFilter : IAuthorizationFilter
    {
        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var serviceProvider = context.HttpContext.RequestServices;
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                var validUserName = configuration.GetValue<string>("InternalService:Name").Decrypt();
                var validPassword = configuration.GetValue<string>("InternalService:Code").Decrypt();
                var encryptionKey = configuration.GetValue<string>("InternalService:InternalValue").Decrypt();

                if (context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute)))
                {
                    // Allow access to the action without authentication
                    return;
                }

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

                var username = credentials[0];
                var password = DecryptPassword(credentials[1], encryptionKey);

                if (!IsAuthorized(username, password, validUserName, validPassword))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                var claims = new[]
                {
                new Claim(ClaimTypes.Name, username)
            };

                var identity = new ClaimsIdentity(claims, "Basic");
                var principal = new ClaimsPrincipal(identity);

                context.HttpContext.User = principal;
            }
            catch (Exception ex)
            {
                context.Result = new BadRequestResult();
                Logger.Log(ex);
                return;
            }
        }

        private bool IsAuthorized(string username, string password, string validUserName, string validPassword)
        {
            return (username == validUserName && password == validPassword);
        }

        private string GetExpectedPassword(string validPassword)
        {
            string currentDate = DateTime.UtcNow.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            return validPassword + currentDate;
        }

        private string EncryptPassword(string password, string encryptionKey)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
                aes.IV = new byte[16] { 145, 249, 126, 218, 15, 17, 5, 94, 92, 84, 10, 34, 14, 86, 197, 132 }; // IV (Initialization Vector) should be random and unique for each encryption

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                using (var ms = new System.IO.MemoryStream())
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(passwordBytes, 0, passwordBytes.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private string DecryptPassword(string encryptedPassword, string encryptionKey)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
                aes.IV = new byte[16] { 145, 249, 126, 218, 15, 17, 5, 94, 92, 84, 10, 34, 14, 86, 197, 132 }; // IV should match the one used during encryption

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] encryptedPasswordBytes = Convert.FromBase64String(encryptedPassword);

                using (var ms = new System.IO.MemoryStream())
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                {
                    cs.Write(encryptedPasswordBytes, 0, encryptedPasswordBytes.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
    }
}
