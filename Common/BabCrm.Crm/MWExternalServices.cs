using BabCrm.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BabCrm.Crm
{
    public class MWExternalServices
    {
        private IHttpClientFactory _client;

        IConfiguration _configuration;

        private readonly string baseAddress = "";

        public MWExternalServices(IHttpClientFactory client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;

            baseAddress = _configuration["MWApiBaseAddress"];
        }

        /// <summary>
        /// this is used for the send instant notification service which based on MW team it does not return a response
        /// so the response of MQRC_NO_MSG_AVAILABLE is not considered an error like other services
        /// </summary>
        /// <param name="objectToSend"></param>
        /// <param name="servicePath"></param>
        /// <returns></returns>
        public async Task Post(object objectToSend, string servicePath)
        {
            var requestUri = GenerateUri(servicePath);
            try
            {
                var json = JsonSerializer.Serialize(objectToSend);

                var dataToSend = new StringContent(json, Encoding.UTF8, "application/json");

                var username = "BAB2023";

                var password = GeneratePassword("yxFm7p8p+6j4ucyJ33r8cq8tej4EqUwn");

                var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

                var client = _client.CreateClient("MWClient");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                using (var response = await client.PostAsync(requestUri, dataToSend))
                {
                    if (response == null || !response.IsSuccessStatusCode || response.Content == null)
                    {
                        var errorContent = response != null && response.Content != null
                            ? await response.Content.ReadAsStringAsync()
                            : "No response content";

                        if (!String.Equals(errorContent, "MQRC_NO_MSG_AVAILABLE"))
                        {
                            Logger.Log($@"MWExternalService.Post {requestUri}, Request:{json}, Reponse {errorContent}:{response?.StatusCode}", "ERROR");

                            throw new Exception(errorContent);
                        }

                    }

                    var content = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private string GenerateUri(string servicePath)
        {
            return $"{baseAddress}/{servicePath}";
        }

        private string GeneratePassword(string value)
        {
            // this was added to ensure that the date generated follows gregorian date and not hijri
            CultureInfo culture = new CultureInfo("en-US");

            var currentDate = DateTime.UtcNow.ToString("yyyyMMdd",culture);

            value += currentDate;

            var encryptedValue = Encrypt(value);

            return encryptedValue;
        }

        private string Encrypt(string value)
        {
            using (Aes aes = Aes.Create())
            {
                //aes.Key =null; 
                //aes.IV = null; // IV (Initialization Vector) should be random and unique for each encryption

                aes.Key = Encoding.UTF8.GetBytes("#8X$1(d*0&W_2@^+");
                aes.IV = new byte[16] { 145, 249, 126, 218, 15, 17, 5, 94, 92, 84, 10, 34, 14, 86, 197, 132 }; // IV (Initialization Vector) should be random and unique for each encryption



                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                byte[] passwordBytes = Encoding.UTF8.GetBytes(value);

                using (var ms = new System.IO.MemoryStream())
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(passwordBytes, 0, passwordBytes.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

    }
}
