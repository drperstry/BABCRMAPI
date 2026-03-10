namespace BabCrm.Crm
{
    using BabCrm.Crm.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;

    public class ChangeSetBatchRequest
    {
        private readonly ICrmConfig crmConfig;
        private const string batchIdentifier = "123456";
        private readonly MultipartContent content;
        private readonly List<ChangeSetBatchRequestItem> requests;
        private readonly List<DeleteBatchRequestItem> deleteRequests;

        public string BatchIdentifier
        {
            get
            {
                return batchIdentifier;
            }
        }

        public ChangeSetBatchRequest(ICrmConfig crmConfig)
        {
            this.crmConfig = crmConfig ?? throw new System.ArgumentNullException(nameof(crmConfig));
            this.content = new MultipartContent("mixed", $"changeset_{BatchIdentifier}");
            this.requests = new List<ChangeSetBatchRequestItem>();
            this.deleteRequests = new List<DeleteBatchRequestItem>();
        }

        public ChangeSetBatchRequest AddRequest(ChangeSetBatchRequestItem changesetRequestItem)
        {
            var stringBuilder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(changesetRequestItem?.EntityPluralName) || !changesetRequestItem.PostObject.IsNullOrEmpty())
            {
                stringBuilder.Append($@"POST {crmConfig.ServiceUrl}{changesetRequestItem.EntityPluralName}");

                stringBuilder.AppendLine(" HTTP/1.1");
                stringBuilder.AppendLine("Content-Type:application/json;type=entry");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine(changesetRequestItem.PostObject.ToString());

                content.Add(new ApplicationHttpContent(stringBuilder.ToString()));
                requests.Add(changesetRequestItem);
            }

            return this;
        }

        public ChangeSetBatchRequest AddRequest(ChangeSetBatchRequestItem changesetRequestItem, string id)
        {
            var stringBuilder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(changesetRequestItem?.EntityPluralName) || !changesetRequestItem.PostObject.IsNullOrEmpty())
            {
                stringBuilder.Append($@"PATCH {crmConfig.ServiceUrl}{changesetRequestItem.EntityPluralName}({id})");

                stringBuilder.AppendLine(" HTTP/1.1");
                stringBuilder.AppendLine("Content-Type:application/json;type=entry");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine(changesetRequestItem.PostObject.ToString());

                content.Add(new ApplicationHttpContent(stringBuilder.ToString()));
                requests.Add(changesetRequestItem);
            }

            return this;
        }

        public ChangeSetBatchRequest AddDeleteRequest(DeleteBatchRequestItem deleteBatchRequestItem)
        {
            var stringBuilder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(deleteBatchRequestItem?.EntityPluralName) || !string.IsNullOrWhiteSpace(deleteBatchRequestItem?.RequestId))
            {
                stringBuilder.Append($@"DELETE {crmConfig.ServiceUrl}{deleteBatchRequestItem.EntityPluralName}({deleteBatchRequestItem.RequestId})");

                stringBuilder.AppendLine(" HTTP/1.1");
                stringBuilder.AppendLine("Content-Type:application/json;type=entry");
                stringBuilder.AppendLine();

                content.Add(new ApplicationHttpContent(stringBuilder.ToString()));
                deleteRequests.Add(deleteBatchRequestItem);
            }

            return this;
        }

        public ChangeSetBatchRequestItem GetBatchRequestItem(int index)
        {
            if (index >= 0 && index < requests.Count)
            {
                return requests[index];
            }

            return null;
        }

        public HttpRequestMessage GetHttpRequest(string requestUri)
        {
            return new HttpRequestMessage(new HttpMethod("POST"), requestUri)
            {
                Content = content,
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
