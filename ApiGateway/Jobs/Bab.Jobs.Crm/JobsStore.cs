using Bab.Jobs.Models;
using BabCrm.Core;
using BabCrm.Crm;
using BabCrm.Crm.Configuration;
using BabCrm.Jobs.Models;
using BabCrm.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Bab.Jobs.Crm
{
    public class JobsStore : IJobsStore
    {
        private readonly CrmService _crmService;
        private readonly ICrmConfig _crmConfig;
        private readonly string rmEmailSenderId;

        private string escalationUser;

        public JobsStore(CrmService crmService, ICrmConfig crmConfig)
        {
            this._crmService = crmService;
            this._crmConfig = crmConfig;
            IConfiguration configuration = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .Build();

            var reportSection = configuration.GetSection("Report");

            rmEmailSenderId = configuration["RmEmailSenderId"];

            escalationUser = reportSection["EscalationUser"];
        }

        public async Task ExecuteTimerRequest(RequestData request)
        {
            try
            {
                var obj = new JObject
                {
                    ["statuscode"] = 961110002,
                };

                await _crmService.Save(request.logicalName, obj, request.Id.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Holiday>> GetHolidays(Guid reportConfigurationId)
        {
            var fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='ntw_reportholidayschedule'>
                                <attribute name='ntw_reportholidayscheduleid'/>
                                <attribute name='ntw_name'/>
                                <attribute name='createdon'/>
                                <attribute name='ntw_to'/>
                                <attribute name='ntw_from'/>
                                <order attribute='ntw_name' descending='false'/>
                                <filter type='and'>
                                  <condition attribute='statecode' operator='eq' value='0'/>
                                </filter>
                                <link-entity name='ntw_reportconfiguration' from='ntw_reportconfigurationid' to='ntw_relatedreportconfigurationid' link-type='inner' alias='ac'>
                                  <filter type='and'>
                                    <condition attribute='ntw_reportconfigurationid' operator='eq' value='{reportConfigurationId}'/>
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";

            var response = await _crmService.Get(Constants.ReportHolidaySchedule, fetchXml);

            if (response == null || !response.Any())
            {
                return null;
            }

            return response.Select(item => new Holiday
            {
                From = item.GetValue<DateTime>("ntw_from").ToLocalTime(),
                To = item.GetValue<DateTime>("ntw_to").ToLocalTime(),

            });
        }



        public async Task<IEnumerable<EscalationInfo>> GetEscalationInfo()
        {
            var query = $@"<fetch>
                              <entity name='incident'>
                                <attribute name='ntw_currentescalationlevel' />
                                <attribute name='escalatedon' />
                                <attribute name='ntw_nextescalationon' />
                                <attribute name='ticketnumber' />
                                <attribute name='incidentid' />
                                <order attribute='createdon' descending='false' />
                                <filter>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                  <condition attribute='ntw_currentescalationlevel' operator='ne' value='0' />
                                  <condition attribute='ntw_currentescalationlevel' operator='not-null' />
                                </filter>
                                <link-entity name='ntw_requestcategory' from='ntw_requestcategoryid' to='ntw_categoryid' visible='false' link-type='outer' alias='a_a67c40c5e3c3ed119d856045bd6aa06d'>
                                  <attribute name='ntw_code' alias='categoryCode'/>
                                </link-entity>
                                <link-entity name='team' from='teamid' to='ntw_escalationteam'>
                                  <attribute name='teamid' alias='respteamid' />
                                  <attribute name='name' alias='respteamname' />
                                  <link-entity name='ntw_escalationmatrix' from='ntw_teamnameid' to='teamid'>
                                    <attribute name='ntw_escalationmatrixid' alias='escalationmatrixid' />
                                    <attribute name='ntw_escalation5firstemailid' alias='escalation5firstemailid' />
                                    <attribute name='ntw_escalation4firstemailid' alias='escalation4firstemailid' />
                                    <attribute name='ntw_escalation4secondemailid' alias='escalation4secondemailid' />
                                    <attribute name='ntw_escalation5secondemailid' alias='escalation5secondemailid' />
                                    <attribute name='ntw_escalation1secondemailid' alias='escalation1secondemailid' />
                                    <attribute name='ntw_escalation2firstemailid' alias='escalation2firstemailid' />
                                    <attribute name='ntw_escalationccemailid' alias='escalationccemailid' />
                                    <attribute name='ntw_escalation1firstemailid' alias='escalation1firstemailid' />
                                    <attribute name='ntw_escalation2secondemailid' alias='escalation2secondemailid' />
                                    <attribute name='ntw_escalation3firstemailid' alias='escalation3firstemailid' />
                                    <attribute name='ntw_escalation3secondemailid' alias='escalation3secondemailid' />
                                  </link-entity>
                                </link-entity>
                              </entity>
                            </fetch>";

            var escalationInfo = await _crmService.Get(Constants.IncidentEntityName, query);

            return FillEscalationInfo(escalationInfo);
        }

        public async Task<IEnumerable<string>> CreateEmailRecords(List<NotificationInfo> notifications)
        {
            var createdIds = new List<string>();

            // Split into batches of 500 to stay well under the 1000-limit
            foreach (var batch in notifications.Chunk(500))
            {
                var changeSet = new ChangeSetBatchRequest(_crmConfig);

                foreach (var item in batch)
                {
                    var email = new JObject
                    {
                        ["subject"] = item.Subject,
                        ["description"] = item.Language == "A" ? item.Body.BodyAr : item.Body.BodyEn
                    };

                    //participationtypemask = 1 for sender , 2 for reciever
                    var parties = new JArray
                    {
                        new JObject
                        {
                            ["participationtypemask"] = 1,
                            ["partyid_systemuser@odata.bind"] = $"/systemusers({rmEmailSenderId})"
                        },
                        new JObject
                        {
                            ["participationtypemask"] = 2,
                            ["partyid_contact@odata.bind"] = $"/contacts({item.ContactId})"
                        }
                    };

                    email["email_activity_parties"] = parties;

                    changeSet.AddRequest(new ChangeSetBatchRequestItem
                    {
                        PostObject = email,
                        EntityPluralName = "emails"  // make sure this matches your CRM
                    });
                }

                try
                {
                    var responses = await _crmService.ExecuteChangeSetBatchRequest(changeSet);
                    if (responses != null)
                    {
                        createdIds.AddRange(responses);
                    }
                }
                catch (Exception ex)
                {
                    // Log failure for this chunk, but continue with other chunks
                    Logger.Log(ex, $"Failed to create email batch of {batch.Count()} items");
                }
            }

            return createdIds.Any() ? createdIds : null;
        }

        public async Task<string> CreateEmailRecord(Email emailInfo)
        {
            // Attachment pending info

            var obj = new JObject
            {
                ["subject"] = emailInfo.Subject,
                ["description"] = emailInfo.Body,
            };

            var parties = new JArray();

            //participationtypemask = 1 for sender , 2 for reciever
            var sender = new JObject
            {
                ["participationtypemask"] = 1,
                [$"partyid_systemuser@odata.bind"] = $"/{Constants.SystemUserEntityName}({escalationUser.Decrypt()})"
            };
            var recipient1 = new JObject
            {
                ["participationtypemask"] = 2,
                [$"partyid_ntw_emailreceiver@odata.bind"] = $"/{Constants.EmailReceiverEntityName}({emailInfo.FirstEmailId})"
            };
            var recipient2 = new JObject
            {
                ["participationtypemask"] = 2,
                [$"partyid_ntw_emailreceiver@odata.bind"] = $"/{Constants.EmailReceiverEntityName}({emailInfo.SecondEmailId})"
            };

            //var ccRecipients = new JArray();
            //ccRecipients.Add(

            var ccRecipients = new JObject
            {
                ["participationtypemask"] = 3, // Participationtypemask = 3 for CC recipient
                [$"partyid_ntw_emailreceiver@odata.bind"] = $"/{Constants.EmailReceiverEntityName}({emailInfo.CCEmailId})"
            };

            parties.Add(sender);
            parties.Add(recipient1);
            if (!string.IsNullOrWhiteSpace(emailInfo.SecondEmailId))
            {
                parties.Add(recipient2);
            }
            if (!string.IsNullOrWhiteSpace(emailInfo.CCEmailId))
            {
                parties.Add(ccRecipients);
            }

            obj["email_activity_parties"] = parties;
            //obj["Cc"] = ccRecipients;


            var result = await _crmService.Save(Constants.EmailEntityName, obj);
            var emailId = result != null && !result["Id"].IsNullOrEmpty() ? result["Id"].ToString() : null;

            return emailId;
        }

        public async Task<string> CreateEmailRecord(ReportConfiguration reportConfig)
        {
            // Attachment pending info

            var obj = new JObject
            {
                ["subject"] = reportConfig.MailSubject,
                ["description"] = reportConfig.MailDescription,
            };

            var parties = new JArray();

            //participationtypemask = 1 for sender , 2 for reciever
            var sender = new JObject
            {
                ["participationtypemask"] = 1,
                [$"partyid_systemuser@odata.bind"] = $"/{Constants.SystemUserEntityName}({reportConfig.EmailSender})"
            };
            var recipient1 = new JObject
            {
                ["participationtypemask"] = 2,
                [$"partyid_ntw_emailreceiver@odata.bind"] = $"/{Constants.EmailReceiverEntityName}({reportConfig.EmailRecipient})"
            };

            //var ccRecipients = new JArray();
            //ccRecipients.Add(

            if (reportConfig.CcMailGroup != null)
            {
                var ccRecipients = new JObject
                {
                    ["participationtypemask"] = 3, // Participationtypemask = 3 for CC recipient
                    [$"partyid_ntw_emailreceiver@odata.bind"] = $"/{Constants.EmailReceiverEntityName}({reportConfig.CcMailGroup})"
                };

                parties.Add(ccRecipients);

            }

            parties.Add(sender);
            parties.Add(recipient1);

            obj["email_activity_parties"] = parties;
            //obj["Cc"] = ccRecipients;


            var result = await _crmService.Save(Constants.EmailEntityName, obj);
            var emailId = result != null && !result["Id"].IsNullOrEmpty() ? result["Id"].ToString() : null;

            return emailId;
        }

        public async Task UpdateEmailStatus(string emailId)
        {

            var obj = new JObject
            {
                ["ntw_sendemail"] = true,
            };

            await _crmService.Save(Constants.EmailEntityName, obj, emailId);

        }

        public async Task<IEnumerable<string>> BulkUpdateEmailStatus(IEnumerable<string> emailIds)
        {
            var updatedIds = new List<string>();

            foreach (var batch in emailIds.Chunk(500))
            {
                var batchRequest = new ChangeSetBatchRequest(_crmConfig);

                foreach (var id in batch)
                {
                    var obj = new JObject
                    {
                        ["ntw_sendemail"] = true,
                    };

                    var batchRequestItem = new ChangeSetBatchRequestItem
                    {
                        PostObject = obj,
                        EntityPluralName = Constants.EmailEntityName
                    };

                    batchRequest.AddRequest(batchRequestItem, id);

                }

                try
                {
                    var responses = await _crmService.ExecuteChangeSetBatchRequest(batchRequest);
                    if (responses != null)
                    {
                        updatedIds.AddRange(responses);
                    }
                }
                catch (Exception ex)
                {
                    // Log failure for this chunk, but continue with other chunks
                    Logger.Log(ex, $"Failed to create email batch of {batch.Count()} items");
                }
            }


            return updatedIds.Any() ? updatedIds : null;
        }

        public async Task<bool> AddAttachment(ActivityAttachment attachment)
        {
            var obj = new JObject
            {
                ["objectid_activitypointer@odata.bind"] = $"/{Constants.ActivityPointerEntityName}({attachment.ActivityId})",
                ["objecttypecode"] = attachment.ObjectTypeCode,
                ["filename"] = attachment.FileName,
                ["body"] = attachment.Body,
            };

            var result = await _crmService.Save(Constants.ActivityMIMEAttachmentEntity, obj);

            return result != null;
        }

        public async Task ExecuteWorkflow(string workflowId, string entityId)
        {
            await _crmService.ExecuteWorkflow(workflowId, entityId);
        }

        private IEnumerable<EscalationInfo> FillEscalationInfo(JArray data)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var escalationsInfo = new List<EscalationInfo>();

            foreach (var item in data)
            {
                var escalationInfo = new EscalationInfo
                {
                    CaseId = item.GetValue<string>("incidentid"),
                    CaseNumber = item.GetValue<string>("ticketnumber"),
                    EscalationLevel = item.GetValue<int>("ntw_currentescalationlevel"),
                    EscalatedOn = item.GetValue<string>("escalatedon"),
                    NextEscalationOn = item.GetValue<string>("ntw_nextescalationon"),
                    TeamId = item.GetValue<string>("respteamid"),
                    TeamName = item.GetValue<string>("respteamname"),
                    EscalationMatrixId = item.GetValue<string>("escalationmatrixid"),
                    EscalationOneFirstEmailId = item.GetValue<string>("escalation1firstemailid"),
                    EscalationOneSecondEmailId = item.GetValue<string>("escalation1secondemailid"),
                    EscalationTwoFirstEmailId = item.GetValue<string>("escalation2firstemailid"),
                    EscalationTwoSecondEmailId = item.GetValue<string>("escalation2secondemailid"),
                    EscalationThreeFirstEmailId = item.GetValue<string>("escalation3firstemailid"),
                    EscalationThreeSecondEmailId = item.GetValue<string>("escalation3secondemailid"),
                    EscalationFourFirstEmailId = item.GetValue<string>("escalation4firstemailid"),
                    EscalationFourSecondEmailId = item.GetValue<string>("escalation4secondemailid"),
                    EscalationFiveFirstEmailId = item.GetValue<string>("escalation5firstemailid"),
                    EscalationFiveSecondEmailId = item.GetValue<string>("escalation5secondemailid"),
                    EscalationCCEmailId = item.GetValue<string>("escalationccemailid"),
                    CaseCategoryCode = item.GetValue<string>("categoryCode")

                };
                escalationsInfo.Add(escalationInfo);
            }
            return escalationsInfo;
        }

        public async Task<ReportConfiguration> GetEmailConfiguration(string configurationGuid)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='ntw_reportconfiguration'>
                                <attribute name='ntw_reportconfigurationid'/>
                                <attribute name='ntw_name'/>
                                <attribute name='createdon'/>
                                <attribute name='ntw_reportserviceurlid' alias='reportserviceurl'/>
                                <attribute name='ntw_reportpath'/>
                                <attribute name='ntw_reportname'/>
                                <attribute name='ntw_mailsubject'/>
                                <attribute name='ntw_maildescription'/>
                                <attribute name='ntw_jobid'/>
                                <attribute name='ntw_executeonset'/>
                                <attribute name='ntw_emailsenderid' alias='emailsender'/>
                                <attribute name='ntw_emailrecipientid' alias='emailrecipient'/>
                                <attribute name='emailaddress'/>
                                <attribute name='ntw_ccmailgroupid' alias='ccmailgroup'/>
                                <order attribute='ntw_name' descending='false'/>
                                <filter type='and'>
                                <condition attribute='ntw_reportconfigurationid' operator='eq' value='{configurationGuid}'/>
                                </filter>
                                <link-entity name='ntws_servicelocation' from='ntws_servicelocationid' to='ntw_reportserviceurlid' visible='false' link-type='outer'>
                                <attribute name='ntws_serviceurl' alias='serviceurl'/>
                                </link-entity>
                                </entity>
                          </fetch>";

            var emailConfig = await _crmService.Get(Constants.ReportConfigurationEntityName, query);

            return FillEmailConfigurationInfo(emailConfig);
        }

        private ReportConfiguration FillEmailConfigurationInfo(JArray data)
        {
            if (data == null || !data.Any())
            {
                return null;
            }

            var reportConfiguration = data.FirstOrDefault();

            var obj = new ReportConfiguration
            {
                ReportName = reportConfiguration.GetValue<string>("ntw_reportname"),
                ReportServiceUrl = reportConfiguration.GetValue<string>("serviceurl"),
                ReportPath = reportConfiguration.GetValue<string>("ntw_reportpath"),
                EmailSender = reportConfiguration.GetValue<string>("emailsender"),
                EmailRecipient = reportConfiguration.GetValue<string>("emailrecipient"),
                CcMailGroup = reportConfiguration.GetValue<string>("ccmailgroup"),
                MailSubject = reportConfiguration.GetValue<string>("ntw_mailsubject"),
                MailDescription = reportConfiguration.GetValue<string>("ntw_maildescription")
            };

            return obj;
        }

        public async Task<IEnumerable<RMContactInfo>> GetContactsInfo(string segmentCode, int pageIndex = 1)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='5000' page='{pageIndex}'>
                                <entity name='contact'>
                                    <attribute name='contactid'/>
                                    <attribute name='lastname'/>
                                    <attribute name='firstname'/>
                                    <attribute name='mobilephone'/>
                                    <attribute name='ntw_fullnamearabic'/>
                                    <order attribute='lastname' descending='false'/>
                                    <link-entity name='ntw_customersegment' from='ntw_customersegmentid' to='ntw_segmentid' link-type='inner' alias='ag'>
                                        <attribute name='ntw_name' alias='segmentname'/>
                                        <filter type='and'>
                                            <condition attribute='ntw_code' operator='eq' value='{segmentCode}'/>
                                        </filter>
                                    </link-entity>
                                    <link-entity name='ntx_employee' from='ntx_employeeid' to='ntx_relationshipmanagerid' visible='false' link-type='outer'>
                                        <attribute name='ntx_fullname' alias='rmfullnameen'/>
                                    </link-entity>
                                </entity>
                            </fetch>";

            var result = await _crmService.Get(Constants.Contact, query);

            return FillContactInfos(result);
        }

        public async Task<SegmentCampaignInfo> GetSegmentCampaignById(string segmentCampaignId)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='ntw_segmentcampaign'>
                                <attribute name='ntw_runtime' />
                                <filter type='and'>
                                    <condition attribute='ntw_segmentcampaignid' operator='eq' value='{segmentCampaignId}' />
                                </filter>
                                <link-entity name='ntw_customersegment' from='ntw_customersegmentid' to='ntw_segmentid' visible='false' link-type='outer' alias='a_segment'>
                                    <attribute name='ntw_code' alias='customersegmentcode' />
                                </link-entity>
                                <link-entity name='ntw_notificationtemplate' from='ntw_notificationtemplateid' to='ntw_notificationtemplateid' visible='false' link-type='outer' alias='a_template'>
                                    <attribute name='ntw_bodyen' alias='bodyen' />
                                    <attribute name='ntw_replymethodset' alias='replymethodcode' />
                                    <attribute name='new_body' alias='bodyar' />
                                    <attribute name='ntw_type' alias='eventtype' />
                                    <attribute name='ntw_subtype' alias='eventsubtype' />
                                    <attribute name='ntw_organizationid' alias='orgid' />
                                    <attribute name='ntw_senderid' alias='sendr' />
                                </link-entity>
                                <link-entity name='ntw_language' from='ntw_languageid' to='ntw_languageid' visible='false' link-type='outer' alias='a_68d6'>
                                    <attribute name='ntw_code' alias='languagecode' />
                                </link-entity>
                            </entity>
                        </fetch>";

            var data = await _crmService.Get(Constants.SegmentCampaign, query);

            var item = data?.FirstOrDefault();
            if (item == null) return null;

            return new SegmentCampaignInfo
            {
                Runtime = item.GetValue<DateTime>("ntw_runtime"),
                SegmentCode = item.GetValue<string>("customersegmentcode"),
                BodyEn = item.GetValue<string>("bodyen"),
                BodyAr = item.GetValue<string>("bodyar"),
                ReplyMethodCode = item.GetValue<string>("replymethodcode"),
                LanguageCode = item.GetValue<string>("languagecode"),
                EventType = item.GetValue<string>("eventtype"),
                EventSubType = item.GetValue<string>("eventsubtype"),
                OrganizationId = item.GetValue<string>("orgid"),
                SenderId = item.GetValue<string>("sendr"),
            };
        }

        public async Task<IEnumerable<EscalationEmailConfig>> GetEmailBodies(string reportConfigurationId)
        {
            var query = $@"<fetch distinct='false' mapping='logical' output-format='xml-platform' version='1.0'>
                             <entity name='ntw_reportconfiguration'>
                               <attribute name='ntw_reportconfigurationid'/>
                               <attribute name='ntw_name'/>
                               <attribute name='ntw_thirdescalationbody'/>
                               <attribute name='ntw_secondescalationbody'/>
                               <attribute name='ntw_firstescalationbody'/>
                               <order descending='false' attribute='ntw_name'/>
                               <filter type='and'>
                                 <condition attribute='ntw_reportconfigurationid'  operator='eq' value='{reportConfigurationId}' />
                               </filter>
                             </entity>
                            </fetch>";

            var data = await _crmService.Get(Constants.ReportConfigurationEntityName, query);

            var item = data?.FirstOrDefault();
            if (item == null) return null;

            var emailBodyList = new List<EscalationEmailConfig>
            {
                new EscalationEmailConfig { EscalationLevel = 1, Body = item.GetValue<string>("ntw_firstescalationbody") },
                new EscalationEmailConfig { EscalationLevel = 2, Body = item.GetValue<string>("ntw_secondescalationbody") },
                new EscalationEmailConfig { EscalationLevel = 3, Body = item.GetValue<string>("ntw_thirdescalationbody") }
            };

            return emailBodyList;
        }

        private IEnumerable<RMContactInfo> FillContactInfos(JArray data)
        {
            if (data == null || !data.Any())
                return null;

            var contacts = new List<RMContactInfo>();

            foreach (var item in data)
            {
                var contact = new RMContactInfo
                {
                    ContactId = item.GetValue<string>("contactid"),
                    FullNameEn = $"{item.GetValue<string>("firstname")} {item.GetValue<string>("lastname")}",
                    FullNameAr = item.GetValue<string>("ntw_fullnamearabic"),
                    MobilePhone = item.GetValue<string>("mobilephone"),
                    Segment = item.GetValue<string>("segmentname"),
                    RMFullNameEn = item.GetValue<string>("rmfullnameen"),
                };
                contacts.Add(contact);
            }

            return contacts;
        }


    }
}