using System.Collections.Generic;

namespace BabCrm.Crm
{
    using BabCrm.Crm.Configuration;
    using System;
    using System.Net.Http;
    using System.Text;

    public class BatchRequest : IDisposable
    {
        private readonly ICrmConfig crmConfig;
        private const string batchIdentifier = "123456";
        private readonly MultipartContent content;
        private readonly List<BatchRequestItem> requests;

        public string BatchIdentifier
        {
            get
            {
                return batchIdentifier;
            }
        }

        public BatchRequest(ICrmConfig crmConfig)
        {
            this.crmConfig = crmConfig ?? throw new System.ArgumentNullException(nameof(crmConfig));
            this.content = new MultipartContent("mixed", $"batch_{BatchIdentifier}");
            this.requests = new List<BatchRequestItem>();
        }

        public BatchRequest AddRequest(BatchRequestItem batchRequestItem)
        {
            var stringBuilder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(batchRequestItem?.EntityPluralName))
            {
                stringBuilder.Append($@"GET {crmConfig.ServiceUrl}{batchRequestItem.EntityPluralName}");

                if (!string.IsNullOrWhiteSpace(batchRequestItem.FetchXml))
                {
                    stringBuilder.Append($@"?fetchXml={System.Uri.EscapeUriString(GetFetchXml(batchRequestItem.FetchXml))}");
                }

                stringBuilder.AppendLine(" HTTP/1.1");
                stringBuilder.AppendLine();

                content.Add(new ApplicationHttpContent(stringBuilder.ToString()));
                requests.Add(batchRequestItem);
            }

            return this;
        }

        public BatchRequestItem GetBatchRequestItem(int index)
        {
            if (index >= 0 && index < requests.Count)
            {
                return requests[index];
            }

            return null;
        }

        private string GetFetchXml(string fetchXml)
        {
            fetchXml = fetchXml.Replace(Environment.NewLine, "").Replace("\t", "");

            return fetchXml;
        }

        public HttpRequestMessage GetHttpRequest(string requestUri)
        {
            return new HttpRequestMessage(new HttpMethod("POST"), requestUri)
            {
                Content = content
            };
        }

        public void Dispose()
        {
            content?.Dispose();
            requests.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
