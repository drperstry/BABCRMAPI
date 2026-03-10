using BabCrm.Crm.Configuration;
using BabCrm.Logging;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace BabCrm.Crm
{
    /// <summary>
    /// Provides services to connect to CRM through CRM Web API.
    /// </summary>
    public class CrmService
    {
        /// <summary>
        /// Service url.
        /// </summary>
        private readonly string serviceUrl;

        private readonly ICrmConfig crmConfig;

        /// <summary>
        /// Initializes a new instance of <see cref="CrmService"/> class.
        /// </summary>
        public CrmService(ICrmConfig crmConfig)
        {
            this.crmConfig = crmConfig;
            this.serviceUrl = crmConfig.ServiceUrl;
        }

        /// <summary>
        /// Generates the url that will be invoked.
        /// </summary>
        /// <param name="entityName">Plural name of targeted entity.</param>
        /// <param name="fetchXml">Query that will be executed.</param>
        /// <returns></returns>
        private string GenerateGetUrl(string entityName, string fetchXml)
        {
            if (string.IsNullOrEmpty(entityName))
            {
                throw new ArgumentNullException(nameof(entityName), " Value cannot be null or empty");
            }

            if (string.IsNullOrEmpty(fetchXml))
            {
                throw new ArgumentNullException(nameof(fetchXml), " Value cannot be null or empty");
            }

            //remove Enter and tabs
            fetchXml = fetchXml.Replace(Environment.NewLine, "").Replace("\t", "");

            return $"{serviceUrl}{entityName}?fetchXml={Uri.EscapeDataString(fetchXml)}";
        }

        /// <summary>
        /// Generates the url that will be invoked.
        /// </summary>
        /// <param name="entityName">Plural name of targeted entity.</param>
        /// <param name="fetchXml">Query that will be executed.</param>
        /// <returns></returns>
        private string GenerateUnBoundActionUrl(string actionName)
        {
            if (string.IsNullOrEmpty(actionName))
            {
                throw new ArgumentNullException(nameof(actionName), " Value cannot be null or empty");
            }

            return $"{serviceUrl}{actionName}";
        }

        /// <summary>
        /// Generates the url that will be invoked.
        /// </summary>
        /// <param name="entityName">Plural name of targeted entity.</param>
        /// <param name="fetchXml">Query that will be executed.</param>
        /// <returns></returns>
        private string GenerateBoundActionUrl(string entityName, string actionName, string id)
        {
            if (string.IsNullOrEmpty(entityName))
            {
                throw new ArgumentNullException(nameof(entityName), " Value cannot be null or empty");
            }
            if (string.IsNullOrEmpty(actionName))
            {
                throw new ArgumentNullException(nameof(actionName), " Value cannot be null or empty");
            }
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), " Value cannot be null or empty");
            }

            return $"{serviceUrl}{entityName}({id})/{actionName}";
        }

        /// <summary>
        /// Generates the URL of associate N:N relation.
        /// </summary>
        /// <param name="firstEntity">Plural first entity name.</param>
        /// <param name="firstId">ID of first entity.</param>
        /// <param name="relationTable">Relation entity name.</param>
        /// <returns>Targeted URL.</returns>
        private string GenerateAssociateUrl(string firstEntity, string firstId, string relationTable)
        {
            if (string.IsNullOrEmpty(firstEntity))
            {
                throw new ArgumentNullException(nameof(firstEntity), " Value cannot be null or empty");
            }

            return $"{serviceUrl}{firstEntity}({firstId})/{relationTable}/$ref";
        }

        /// <summary>
        /// Generates the URL of disassociate N:N relation.
        /// </summary>
        /// <param name="firstEntity">Plural first entity name.</param>
        /// <param name="firstId">ID of first entity.</param>
        /// <param name="relationTable">Relation entity name.</param>
        /// <param name="secondEntity">Plural second entity name.></param>
        /// <param name="secondId">ID of second entity.</param>
        /// <returns>Targeted URL.</returns>
        private string GenerateDisassociateUrl(string firstEntity, string firstId, string relationTable, string secondEntity, string secondId)
        {
            if (string.IsNullOrEmpty(firstEntity))
            {
                throw new ArgumentNullException(nameof(firstEntity), " Value cannot be null or empty");
            }

            return $"{serviceUrl}{firstEntity}({firstId})/{relationTable}/$ref?$id={serviceUrl}{secondEntity}({secondId})";
        }

        /// <summary>
        /// Generates the url of associate parameter.
        /// </summary>
        /// <param name="entity">Plural first entity name.</param>
        /// <param name="id">ID of first entity.</param>
        /// <returns>Targeted URL.</returns>
        private string GenerateAssociateParameterUrl(string entity, string id)
        {
            if (string.IsNullOrEmpty(entity))
            {
                throw new ArgumentNullException(nameof(entity), " Value cannot be null or empty");
            }

            return $"{serviceUrl}{entity}({id})";
        }

        /// <summary>
        /// Generates the POST url that will be invoked Add, Update and Delete.
        /// </summary>
        private string GeneratePostUrl(string entityName)
        {
            if (string.IsNullOrEmpty(entityName))
            {
                throw new ArgumentNullException(nameof(entityName), " Value cannot be null or empty");
            }

            return $"{serviceUrl}{entityName}";
        }

        private string GenerateExecuteWorkflowUrl(string workflowId)
        {
            string query = $"workflows({workflowId})/Microsoft.Dynamics.CRM.ExecuteWorkflow";

            if (string.IsNullOrEmpty(workflowId))
            {
                throw new ArgumentNullException(nameof(workflowId), " Value cannot be null or empty");
            }

            return $"{serviceUrl}{query}";
        }

        private string GenerateBatchUrl()
        {
            return $"{serviceUrl}$batch";
        }

        public async Task<JArray> GetOptionSet(string optionSetName)
        {
            var requestUri = $@"{serviceUrl}GlobalOptionSetDefinitions(Name='{optionSetName}')";

            try
            {
                using (var client = crmConfig.BuildClient())
                using (var response = await client.GetResponseAsync(requestUri))
                {
                    //Check the response status
                    if (response == null || !response.IsSuccessStatusCode || response.Content == null)
                    {
                        try
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();

                            Logger.Log($@"CrmService.Get {requestUri} Reponse {errorContent} ", "ERROR");
                        }
                        catch (Exception responseException)
                        {
                            Logger.Log(responseException);
                            throw;
                        }

                        return null;
                    }

                    var content = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(content)) return null;

                    var contentObject = JObject.Parse(content);

                    return contentObject?["Options"] == null ? null : JArray.Parse(contentObject["Options"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }
        }

        public async Task<JArray> GetEntitySpecificOptionSet(string entityName, string optionSetName)
        {
            var requestUri = $@"{serviceUrl}EntityDefinitions(LogicalName= '{entityName}')/Attributes/Microsoft.Dynamics.CRM.PicklistAttributeMetadata?$select=LogicalName&$expand=OptionSet($select=Options)&$filter=LogicalName eq '{optionSetName}'";

            try
            {
                using (var client = crmConfig.BuildClient())
                using (var response = await client.GetResponseAsync(requestUri))
                {
                    //Check the response status
                    if (response == null || !response.IsSuccessStatusCode || response.Content == null)
                    {
                        try
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();

                            Logger.Log($@"CrmService.Get {requestUri} Reponse {errorContent} ", "ERROR");
                        }
                        catch (Exception responseException)
                        {
                            Logger.Log(responseException);
                            throw;
                        }

                        return null;
                    }

                    var content = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(content)) return null;

                    var contentObject = JObject.Parse(content);

                    return contentObject?["value"][0]["OptionSet"]["Options"] == null ? null : JArray.Parse(contentObject["value"][0]["OptionSet"]["Options"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }
        }

        public async Task<JArray> GetStatusCodes(string entityName)
        {
            var requestUri = $@"{serviceUrl}EntityDefinitions(LogicalName='{entityName}')/Attributes/Microsoft.Dynamics.CRM.StatusAttributeMetadata?$select=LogicalName&$expand=OptionSet
";

            try
            {
                using (var client = crmConfig.BuildClient())
                using (var response = await client.GetResponseAsync(requestUri))
                {
                    //Check the response status
                    if (response == null || !response.IsSuccessStatusCode || response.Content == null)
                    {
                        try
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();

                            Logger.Log($@"CrmService.Get {requestUri} Reponse {errorContent} ", "ERROR");
                        }
                        catch (Exception responseException)
                        {
                            Logger.Log(responseException);
                            throw;
                        }

                        return null;
                    }

                    var content = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(content)) return null;

                    var contentObject = JObject.Parse(content);

                    return contentObject?["value"].First()["OptionSet"]["Options"] == null ? null : JArray.Parse(contentObject["value"].First()["OptionSet"]["Options"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }
        }

        /// <summary>
        /// Execute fetchxml query through CRM web api.
        /// </summary>
        /// <param name="entityName">Plural entity name.</param>
        /// <param name="fetchXml">Fetch xml as query.</param>
        /// <returns>Returns asynchronous job that returns the result of fetchxml as JArray.</returns>
        public async Task<JArray> Get(string entityName, string fetchXml)
        {
            var requestUri = GenerateGetUrl(entityName, fetchXml);


            try
            {
                //Call the url function
                using (var client = crmConfig.BuildClient())
                using (var response = await client.GetResponseAsync(requestUri))
                {
                    //Check the response status
                    if (response == null || !response.IsSuccessStatusCode || response.Content == null)
                    {
                        try
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();

                            Logger.Log($@"CrmService.Get {requestUri} Reponse {errorContent} {fetchXml}", "ERROR");
                        }
                        catch
                        {
                        }

                        return null;
                    }

                    var content = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(content)) return null;

                    var contentObject = JObject.Parse(content);

                    return contentObject?["value"] == null ? null : JArray.Parse(contentObject["value"].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }
        }

        public async Task<string> GetEntityObjectTypeCode(string EntityName)
        {
            var endPoint = $@"{serviceUrl}EntityDefinitions(LogicalName='{EntityName}')?$select=LogicalName,ObjectTypeCode";

            try
            {
                using (var client = crmConfig.BuildClient())
                using (var response = await client.GetResponseAsync(endPoint))
                {
                    //Check the response status
                    if (response == null || !response.IsSuccessStatusCode || response.Content == null)
                    {
                        try
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();

                            Logger.Log($@"CrmService.Get {serviceUrl}{endPoint} Reponse {errorContent} ", "ERROR");
                        }
                        catch (Exception responseException)
                        {
                            Logger.Log(responseException);
                            throw;
                        }

                        return null;
                    }

                    var content = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(content)) return null;

                    var contentObject = JObject.Parse(content);

                    return contentObject?["ObjectTypeCode"] == null ? null : contentObject["ObjectTypeCode"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }
        }

        /// <summary>
        /// Adds or updates record in CRM.
        /// </summary>
        public async Task ExecuteWorkflow(string workflowId, string entityId)
        {
            var requestUri = GenerateExecuteWorkflowUrl(workflowId);

            var data = new
            {
                EntityId = entityId
            };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

            using (var request = new HttpRequestMessage(new HttpMethod("POST"), requestUri)
            {
                Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
            })
            {
                using (var client = crmConfig.BuildClient())
                using (var response = await client.SendRequestAsync(request))
                {
                    if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NoContent)  //204
                    {
                        string responseJson = await response.Content.ReadAsStringAsync();

                        //var recordUri = response.Headers.GetValues("OData-EntityId").FirstOrDefault();

                        //var preRecordId = recordUri.LastIndexOf('(');

                        //return JObject.FromObject(new
                        //{
                        //    Id = recordUri.Substring(preRecordId + 1, recordUri.Length - preRecordId - 2)
                        //});
                    }
                    else
                    {
                        try
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();

                            Logger.Log($@"CrmService. {requestUri} Reponse {errorContent} {json}", "ERROR");
                        }
                        catch (Exception ex)
                        {
                            Logger.Log($@"CrmService. {requestUri} Reponse {ex.ToString()} {json}", "ERROR");
                        }

                        //return null;
                    }
                }
            }
        }


        /// <summary>
        /// Adds or updates record in CRM.
        /// </summary>
        public async Task<JObject> Save(string entityName, JObject json, string updateGuid = "")
        {
            var requestUri = GeneratePostUrl(entityName);

            var isUpdate = !string.IsNullOrWhiteSpace(updateGuid);

            if (isUpdate)
            {
                requestUri += $@"({updateGuid})";
            }

            using (var request = new HttpRequestMessage(new HttpMethod(isUpdate ? "PATCH" : "POST"), requestUri)
            {
                Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
            })
            {
                using (var client = crmConfig.BuildClient())
                using (var response = await client.SendRequestAsync(request))
                {
                    if (response.StatusCode == HttpStatusCode.NoContent)  //204
                    {
                        var recordUri = response.Headers.GetValues("OData-EntityId").FirstOrDefault();

                        var preRecordId = recordUri.LastIndexOf('(');

                        return JObject.FromObject(new
                        {
                            Id = recordUri.Substring(preRecordId + 1, recordUri.Length - preRecordId - 2)
                        });
                    }
                    else
                    {
                        var method = isUpdate ? "Patch" : "Post";

                        try
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();

                            Logger.Log($@"CrmService.{method} {requestUri} Reponse {errorContent} {json}", "ERROR");
                        }
                        catch (Exception ex)
                        {
                            Logger.Log($@"CrmService.{method} {requestUri} Reponse {ex.ToString()} {json}", "ERROR");
                        }

                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Adds record from N:N entity.
        /// </summary>
        /// <param name="firstEntity">Plural first entity name.</param>
        /// <param name="firstId">ID of first entity.</param>
        /// <param name="relationTable">Relation entity name.</param>
        /// <param name="secondEntity">Plural second entity name.></param>
        /// <param name="secondId">ID of second entity.</param>
        /// <returns>Returns asynchronous job that represents the execution result.</returns>
        public async Task<bool> Associate(string firstEntity, string firstId, string relationTable, string secondEntity, string secondId)
        {
            var requestUri = GenerateAssociateUrl(firstEntity, firstId, relationTable);

            var obj = new JObject
            {
                ["@odata.id"] = GenerateAssociateParameterUrl(secondEntity, secondId)
            };

            using (
                var request = new HttpRequestMessage(new HttpMethod("POST"), requestUri)
                {
                    Content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json")
                })
            {
                using (var client = crmConfig.BuildClient())
                using (var response = await client.SendRequestAsync(request))
                {
                    if (response.StatusCode == HttpStatusCode.NoContent)  //204
                    {
                        return true;
                    }
                    else
                    {
                        Logger.Log(await response.Content?.ReadAsStringAsync());

                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Removes record from N:N entity.
        /// </summary>
        /// <param name="firstEntity">Plural first entity name.</param>
        /// <param name="firstId">ID of first entity.</param>
        /// <param name="relationTable">Relation entity name.</param>
        /// <param name="secondEntity">Plural second entity name.></param>
        /// <param name="secondId">ID of second entity.</param>
        /// <returns>Returns asynchronous job that represents the execution result.</returns>
        public async Task<bool> Disassociate(string firstEntity, string firstId, string relationTable, string secondEntity, string secondId)
        {
            var requestUri = GenerateDisassociateUrl(firstEntity, firstId, relationTable, secondEntity, secondId);

            using (var client = crmConfig.BuildClient())
            using (var response = await client.SendDeleteRequestAsync(requestUri))
            {
                if (response.StatusCode == HttpStatusCode.NoContent)  //204
                {
                    return true;
                }
                else
                {
                    Logger.Log(await response.Content?.ReadAsStringAsync());

                    return false;
                }
            }
        }

        /// <summary>
        /// Delete record in CRM.
        /// </summary>
        /// <param name="entityName">Entity name.</param>
        /// <param name="recordGuid">Record guid ID.</param>
        /// <returns></returns>
        public async Task<bool> Delete(string entityName, string recordGuid)
        {
            var requestUri = GeneratePostUrl(entityName) + $@"({recordGuid})";

            using (var client = crmConfig.BuildClient())
            using (var response = await client.SendDeleteRequestAsync(requestUri))
            {
                if (response.StatusCode == HttpStatusCode.NoContent)  //204
                {
                    return true;
                }
                else
                {
                    Logger.Log(await response.Content?.ReadAsStringAsync());

                    return false;
                }
            }
        }

        public async Task<bool> RemoveReference(string entityName, Guid recordGuid, string referenceFieldName)
        {
            var requestUri = $@"{GeneratePostUrl(entityName)}({recordGuid})/{referenceFieldName}/$ref";

            using (var client = crmConfig.BuildClient())
            using (var response = await client.SendDeleteRequestAsync(requestUri))
            {
                if (response.StatusCode == HttpStatusCode.NoContent)  //204
                {
                    return true;
                }
                else
                {
                    Logger.Log(await response.Content?.ReadAsStringAsync());

                    return false;

                }
            }
        }

        public async Task<IEnumerable<BatchResponse>> ExecuteBatchRequest(BatchRequest batchRequest)
        {
            var requestUri = GenerateBatchUrl();

            using (var request = batchRequest.GetHttpRequest(requestUri))
            {
                using (var client = crmConfig.BuildClient())
                using (var response = await client.SendRequestAsync(request))
                {
                    if (response == null || !response.IsSuccessStatusCode)
                    {
                        Logger.Log($"Status Code: {response.StatusCode} - Response: {await response.Content?.ReadAsStringAsync()}");

                        return null;
                    }
                    else
                    {
                        if (response.Content.Headers.ContentType.MediaType.IsMultipartContentType())
                        {
                            List<BatchResponse> result = new List<BatchResponse>();

                            var boundary = MultipartRequestHelper.GetBoundary(response.Content.Headers.ContentType, 50);

                            using (var contentStream = await response.Content.ReadAsStreamAsync())
                            {
                                var reader = new MultipartReader(boundary, contentStream);
                                var section = await reader.ReadNextSectionAsync();
                                var responseIndex = 0;

                                while (section != null)
                                {
                                    using (var streamReader = new StreamReader(
                                        section.Body,
                                        Encoding.UTF8,
                                        detectEncodingFromByteOrderMarks: true,
                                        bufferSize: 1024,
                                        leaveOpen: true))
                                    {
                                        // The value length limit is enforced by MultipartBodyLengthLimit
                                        var value = await streamReader.ReadToEndAsync();

                                        if (String.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                                        {
                                            value = String.Empty;
                                        }

                                        var batchResponse = GetPartJson(value);
                                        if (batchResponse != null)
                                        {
                                            result.Add(batchResponse);
                                        }
                                        else
                                        {
                                            LogBatchError(batchRequest, value, responseIndex);
                                        }

                                        responseIndex++;
                                    }

                                    // Drains any remaining section body that has not been consumed and
                                    // reads the headers for the next section.
                                    section = await reader.ReadNextSectionAsync();
                                }

                                return result;
                            }
                        }

                        return null;
                    }
                }
            }
        }

        public async Task<IEnumerable<string>> ExecuteChangeSetBatchRequest(ChangeSetBatchRequest changesetBatchRequest)
        {
            var requestUri = GenerateBatchUrl();

            using (var request = changesetBatchRequest.GetHttpRequest(requestUri))
            {
                using (var client = crmConfig.BuildClient())
                using (var response = await client.SendRequestAsync(request))
                {
                    if (response == null || !response.IsSuccessStatusCode)
                    {
                        Logger.Log($"Status Code: {response.StatusCode} - Response: {await response.Content?.ReadAsStringAsync()}");

                        return null;
                    }
                    else
                    {
                        if (response.Content.Headers.ContentType.MediaType.IsMultipartContentType())
                        {
                            List<string> result = new List<string>();

                            var boundary = MultipartRequestHelper.GetBoundary(response.Content.Headers.ContentType, 50);

                            using (var contentStream = await response.Content.ReadAsStreamAsync())
                            {
                                var reader = new MultipartReader(boundary, contentStream);
                                var section = await reader.ReadNextSectionAsync();
                                var responseIndex = 0;

                                while (section != null)
                                {
                                    using (var streamReader = new StreamReader(
                                        section.Body,
                                        Encoding.UTF8,
                                        detectEncodingFromByteOrderMarks: true,
                                        bufferSize: 1024,
                                        leaveOpen: true))
                                    {
                                        // The value length limit is enforced by MultipartBodyLengthLimit
                                        var value = await streamReader.ReadToEndAsync();

                                        if (String.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                                        {
                                            value = String.Empty;
                                        }

                                        var id = GetPartItemId(value);
                                        if (!string.IsNullOrEmpty(id))
                                        {
                                            result.Add(id);
                                        }
                                        else
                                        {
                                            LogBatchError(changesetBatchRequest, value, responseIndex);
                                        }

                                        responseIndex++;
                                    }

                                    // Drains any remaining section body that has not been consumed and
                                    // reads the headers for the next section.
                                    section = await reader.ReadNextSectionAsync();
                                }

                                return result;
                            }
                        }

                        return null;
                    }
                }
            }
        }

        public async Task<JToken> UnBoundAction(JObject json, string actionName)
        {
            var requestUri = GenerateUnBoundActionUrl(actionName);

            using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
            })
            {
                //Call the url function
                using (var client = crmConfig.BuildClient())
                using (var response = await client.SendRequestAsync(request))
                {
                    //Check the response status
                    if (response == null || !response.IsSuccessStatusCode || response.Content == null)
                    {
                        try
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();

                            //Logger.Log($@"CrmService.Get {requestUri} Reponse {errorContent} {fetchXml}", "ERROR");
                        }
                        catch
                        {
                        }

                        return null;
                    }

                    var content = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(content)) return null;

                    var contentObject = JObject.Parse(content);

                    return contentObject?["value"] ?? contentObject;
                }
            }
        }

        public async Task<JToken> BoundAction(JObject json, string entityName, string id, string actionName)
        {
            var requestUri = GenerateBoundActionUrl(entityName, actionName, id);

            using (var request = new HttpRequestMessage(new HttpMethod("POST"), requestUri)
            {
                Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
            })
            {
                //Call the url function
                using (var client = crmConfig.BuildClient())
                using (var response = await client.SendRequestAsync(request))
                {
                    //Check the response status
                    if (response == null || !response.IsSuccessStatusCode || response.Content == null)
                    {
                        try
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();

                            //Logger.Log($@"CrmService.Get {requestUri} Reponse {errorContent} {fetchXml}", "ERROR");
                        }
                        catch
                        {
                        }

                        return null;
                    }

                    var content = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(content)) return null;

                    var contentObject = JObject.Parse(content);

                    return contentObject?["value"] ?? contentObject;
                }
            }
        }

        private void LogBatchError(BatchRequest batchRequest, string responsePart, int responseIndex)
        {
            Logger.Log($"CrmService.Batch: {batchRequest.GetBatchRequestItem(responseIndex)} {Environment.NewLine} Response: {responsePart} {Environment.NewLine}ReponseIndex: {responseIndex}");
        }

        private void LogBatchError(ChangeSetBatchRequest batchRequest, string responsePart, int responseIndex)
        {
            Logger.Log($"CrmService.Batch: {batchRequest.GetBatchRequestItem(responseIndex)} {Environment.NewLine} Response: {responsePart} {Environment.NewLine}ReponseIndex: {responseIndex}");
        }

        private string GetPartItemId(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;

            var headers = content.Split('\n');

            var odataHeader = headers?.SingleOrDefault(m => m.StartsWith("OData-EntityId", StringComparison.CurrentCultureIgnoreCase));

            if (string.IsNullOrEmpty(odataHeader)) return null;

            var regex = new System.Text.RegularExpressions.Regex(@".*\((.*)\).*");

            var itemGuid = regex.Replace(odataHeader, "$1");

            return itemGuid;
        }

        private BatchResponse GetPartJson(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;

            content = content.Split(new string[] { $"{Environment.NewLine}{Environment.NewLine}" }, StringSplitOptions.None)[1];

            var contentObject = JObject.Parse(content);

            if (contentObject?["@odata.context"] == null || contentObject?["value"] == null) return null;

            var regex = new System.Text.RegularExpressions.Regex(".*#(.*)\\(.*|.*#(.*)");

            return new BatchResponse()
            {
                EntityPluralName = regex.Replace(contentObject["@odata.context"].ToString(), "$1$2"),
                Data = contentObject.Value<JArray>("value")
            };
        }

        public async Task<string> WhoAmI()
        {
            var requestUri = serviceUrl + "WhoAmI";

            try
            {
                //Call the url function
                using (var client = crmConfig.BuildClient())
                using (var response = await client.GetResponseAsync(requestUri))
                {
                    //Check the response status
                    if (response == null || !response.IsSuccessStatusCode || response.Content == null)
                    {
                        try
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();

                            Logger.Log($@"CrmService.WhoAmI {requestUri} Reponse {errorContent}", "ERROR");
                        }
                        catch
                        {
                        }

                        return null;
                    }

                    var content = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(content)) return null;

                    var contentObject = JObject.Parse(content);

                    Guid userId = (Guid)contentObject["UserId"];

                    return userId == null ? null : userId.ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }
        }
    }
}
