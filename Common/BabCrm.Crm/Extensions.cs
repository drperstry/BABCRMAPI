using BabCrm.Core;
using BabCrm.Crm.Configuration;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;

namespace BabCrm.Crm
{
    public static class Extensions
    {
        /// <summary>
        /// Checks if Token is null or does not have value.
        /// </summary>
        /// <param name="token">JToken to check.</param>
        /// <returns>Return indicator that indicate if JToken is null or empty.</returns>
        public static bool IsNullOrEmpty(this JToken token)
        {
            return (token == null) ||
               (token.Type == JTokenType.Array && !token.HasValues) ||
               (token.Type == JTokenType.Object && !token.HasValues) ||
               (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
               (token.Type == JTokenType.Null);
        }

        internal static HttpClient BuildClient(this ICrmConfig crmConfig)
        {
            if (!crmConfig.IsIfd)
            {
                return BuildClientAd(crmConfig);
            }
            else
            {
                return BuildClientAdfs(crmConfig);
            }
        }

        internal static HttpClient BuildClientAdfs(ICrmConfig crmConfig)
        {
            using (HttpClient client = new HttpClient())
            {
                var returnClient = new HttpClient();
                string accessToken;

                var dict = new Dictionary<string, string>();
                dict.Add("client_id", crmConfig.ClientExternal);
                dict.Add("client_secret", crmConfig.ClientInternal);
                dict.Add("resource", crmConfig.ServiceUrl);
                dict.Add("username", crmConfig.Name.Decrypt());
                dict.Add("password", crmConfig.Code.Decrypt());
                dict.Add("grant_type", "password");

                var req = new HttpRequestMessage(HttpMethod.Post, crmConfig.AdfsUrl) { Content = new FormUrlEncodedContent(dict) };
                var res = client.SendAsync(req).Result;

                if (res.IsSuccessStatusCode)
                {
                    // Get the response content and parse it.  
                    JObject body = JObject.Parse(res.Content.ReadAsStringAsync().Result);
                    accessToken = (string)body["access_token"];
                    var authHeader = new AuthenticationHeaderValue("Bearer", accessToken);

                    returnClient.DefaultRequestHeaders.Authorization = authHeader;
                }

                returnClient.BaseAddress = new Uri(crmConfig.ServiceUrl);
                return returnClient;
            }
        }

        internal static HttpClient BuildClientAd(ICrmConfig crmConfig)
        {
            var handler = new HttpClientHandler();

            if (!string.IsNullOrWhiteSpace(crmConfig.Name))
            {
                handler.PreAuthenticate = true;
                handler.UseDefaultCredentials = false;

                if (!string.IsNullOrWhiteSpace(crmConfig.Domain))
                {
                    handler.Credentials = new NetworkCredential(crmConfig.Name.Decrypt(), crmConfig.Code.Decrypt(), crmConfig.Domain);
                }
                else
                {
                    handler.Credentials = new NetworkCredential(crmConfig.Name.Decrypt(), crmConfig.Code.Decrypt());
                }
            }

            return new HttpClient(handler)
            {
                BaseAddress = new Uri(crmConfig.ServiceUrl)
            };
        }

        public static async Task<HttpResponseMessage> GetResponseAsync(this HttpClient httpClient, string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            return await httpClient.SendRequestAsync(requestMessage);
        }

        public static async Task<HttpResponseMessage> SendRequestAsync(this HttpClient httpClient, HttpRequestMessage requestMessage)
        {
            return await httpClient.SendAsync(requestMessage).ConfigureAwait(false);
        }

        public static async Task<HttpResponseMessage> SendDeleteRequestAsync(this HttpClient httpClient, string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url);

            return await httpClient.SendRequestAsync(requestMessage);
        }

        public static T GetValue<T>(this JToken json, string fieldname, T defaultValue = default(T))
        {
            if (json[fieldname] == null || (json[fieldname] as JValue)?.Value == null)
            {
                return defaultValue;
            }

            try
            {
                var token = json[fieldname].Value<T>();
                return token == null ? defaultValue : token;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static string GetDateAsStringForCRM(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
    }
}
