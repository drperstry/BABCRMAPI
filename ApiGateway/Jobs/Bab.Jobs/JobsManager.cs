using Bab.Jobs.Models;
using BabCrm.Core;
using BabCrm.Jobs.Models;
using BabCrm.Logging;
using BabCrm.Service;
using BabCrm.Service.Models;
using Hangfire;
using Hangfire.Storage;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Bab.Jobs
{
    public class JobsManager
    {
        private readonly IJobsStore _jobsStore;
        private readonly ReportsManager _reportsManager;
        private IMWExternalServiceStore _mWExternalServiceStore;
        private readonly string _reportConfigurationId;


        public JobsManager(IJobsStore jobsStore, ReportsManager reportsManager, IConfiguration configuration, IMWExternalServiceStore mWExternalServiceStore)
        {
            this._jobsStore = jobsStore;
            _reportsManager = reportsManager;
            _mWExternalServiceStore = mWExternalServiceStore;
            _reportConfigurationId = configuration["ReportConfigurationId"];
        }

        public SubmissionResponse DeleteJob(string jobId, bool isRecurringJob)
        {
            bool isJobDeleted;

            if (isRecurringJob)
            {
                if (DoesJobExist(jobId))
                {
                    // Attempt to remove the recurring job
                    RecurringJob.RemoveIfExists(jobId);

                    if (DoesJobExist(jobId))
                    {
                        return SubmissionResponse.Error($"Failed to delete job with Id: {jobId}");
                    }

                    return SubmissionResponse.Ok();

                }

                return SubmissionResponse.Error("Job not found");
            }
            else
            {
                isJobDeleted = BackgroundJob.Delete(jobId); ;

                if (isJobDeleted)
                {
                    return SubmissionResponse.Ok();
                }

                return SubmissionResponse.Error($"Failed to delete job with Id: {jobId}");
            }
        }

        public string SaveDueDate(RequestData request)
        {
            var year = request.dueDate.Year;
            var month = request.dueDate.Month;
            var day = request.dueDate.Day;
            var hour = request.dueDate.Hour;
            var min = request.dueDate.Minute;
            var sec = request.dueDate.Second;
            var time = new DateTime(year, month, day, hour, min, sec);
            DateTime convertedDate = DateTime.SpecifyKind(time, DateTimeKind.Local);
            DateTime dt = convertedDate.ToLocalTime();
            var jobId = BackgroundJob.Schedule(() => Update(request), time);

            return jobId;
        }

        [AutomaticRetry(Attempts = 5, DelaysInSeconds = new int[] { 300 }, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task Update(RequestData request)
        {
            try
            {
                await _jobsStore.ExecuteTimerRequest(request);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<List<CaseEscalationBrief>> GetEscalationInfoByLevel(int escalationLevel)
        {
            var escalationInfos = await _jobsStore.GetEscalationInfo();

            if (escalationInfos.IsEmpty())
            {
                return null;
            }

            var escalationBriefs = new List<CaseEscalationBrief>();


            var escalationBriefList = escalationInfos
                .GroupBy(p => new { p.TeamId, p.TeamName });

            return null;
        }

        public async Task<List<(DateTime From, DateTime To)>> GetHolidays(Guid reportConfigurationId)
        {
            var holidays = await _jobsStore.GetHolidays(reportConfigurationId);

            if (holidays == null || !holidays.Any())
            {
                return null;
            }

            return holidays
                .Where(h => h.From != null && h.To != null)
                .Select(h => (h.From.Value.Date, h.To.Value.Date))
                .ToList();
        }

        public bool IsHolidayOrWeekend(List<(DateTime From, DateTime To)> holidays)
        {
            var today = DateTime.UtcNow.Date;

            // Check if today is Friday or Saturday
            if (today.DayOfWeek == DayOfWeek.Friday || today.DayOfWeek == DayOfWeek.Saturday)
                return true;

            // Check if today is within any holiday range
            if (holidays != null && holidays.Any(h => today >= h.From && today <= h.To))
                return true;

            return false;
        }

        [AutomaticRetry(Attempts = 200, DelaysInSeconds = new int[] { 300 }, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task<SubmissionResponse> CreateEscalationJob(string entityId)
        {
            try
            {
                Guid reportConfigurationId = Guid.Parse(entityId);

                var holidays = await GetHolidays(reportConfigurationId);


                if (IsHolidayOrWeekend(holidays))
                {
                    return SubmissionResponse.Ok("Holiday job was skipped.");
                }

                var escalationInfo = await GetEscalationInfo();

                if (escalationInfo.IsEmpty())
                {
                    return SubmissionResponse.Ok("No escalation found");
                }

                await FillEmailInfoV2(escalationInfo);

                //var errors = new List<string>();
                //var errorCount = 0;

                //foreach (var email in emailInfo)
                //{
                //    var response = await CreateEmailRecord(email);

                //    if (!response.Success)
                //    {
                //        errorCount++;
                //        errors.Add(response.ErrorCode);
                //    }

                //}

                //if (errors.Count > 0)
                //{
                //    return SubmissionResponse.Error(errors);
                //}

                return SubmissionResponse.Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [AutomaticRetry(Attempts = 200, DelaysInSeconds = new int[] { 300 }, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task ExecuteWorflow(string workflowId, string entityId)
        {
            await _jobsStore.ExecuteWorkflow(workflowId, entityId);
        }

        public async Task<SubmissionResponse> SendNotificationBySegment(string segmentId)
        {
            var segmentInfo = await _jobsStore.GetSegmentCampaignById(segmentId);

            if (segmentInfo == null)
            {
                return SubmissionResponse.Error("1000", "Invalid campaignId");
            }

            var contacts = await _jobsStore.GetContactsInfo(segmentInfo.SegmentCode);

            if (contacts == null || !contacts.Any())
            {
                return SubmissionResponse.Ok("No related contacts");
            }

            var notifications = new List<NotificationInfo>();

            foreach (var item in contacts)
            {
                var body = GenerateBody(segmentInfo, item);
                notifications.Add(
                    new NotificationInfo
                    {
                        Body = body,
                        ContactId = item.ContactId,
                        MobileNumber = item.MobilePhone,
                        Subject = segmentInfo.SubjectEn + "-" + segmentInfo.SubjectAr,
                        Language = segmentInfo.LanguageCode,
                        Runtime = segmentInfo.Runtime,
                        EventSubType = segmentInfo.EventSubType,
                        EventType = segmentInfo.EventType,
                        OrganizationId = segmentInfo.OrganizationId,
                        SenderId = segmentInfo.SenderId
                    });
            }

            DateTime runTime = DateTime.Now;

            if (segmentInfo.Runtime.HasValue)
            {
                runTime = segmentInfo.Runtime.Value;
            }

            var year = runTime.Year;
            var month = runTime.Month;
            var day = runTime.Day;
            var hour = runTime.Hour;
            var min = runTime.Minute;
            var sec = runTime.Second;
            var time = new DateTime(year, month, day, hour, min, sec);

            if (segmentInfo.ReplyMethodCode == "1")//SMS
            {
                var jobId = BackgroundJob.Schedule(() => SendSegmentSMS(notifications), time);

                return SubmissionResponse.Ok(new { jobId = jobId });
            }
            if (segmentInfo.ReplyMethodCode == "2")//email
            {
                var result = await _jobsStore.CreateEmailRecords(notifications);

                if (result == null)
                {
                    return SubmissionResponse.Error("5000", "Failed to create email records");
                }

                var jobId = BackgroundJob.Schedule(() => SendSegmentEmails(result), time);

                return SubmissionResponse.Ok(new { jobId = jobId });

            }

            return SubmissionResponse.Error("Invalid reply method");
        }

        [AutomaticRetry(Attempts = 10, DelaysInSeconds = new int[] { 300 }, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task SendSegmentEmails(IEnumerable<string> emailIds)
        {
            try
            {
                await _jobsStore.BulkUpdateEmailStatus(emailIds);
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(SendSegmentEmails), null);
                throw;
            }
        }

        [AutomaticRetry(Attempts = 10, DelaysInSeconds = new int[] { 300 }, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task SendSegmentSMS(IEnumerable<NotificationInfo> notifications)
        {
            try
            {
                foreach (var item in notifications)
                {
                    var notification = new SendInstantNotification
                    {
                        EventType = item.EventType,
                        EventSubSType = item.EventSubType,
                        SenderId = item.SenderId,
                        OrganizationId = item.OrganizationId,
                        MobileNumber = item.MobileNumber,
                        NotificationMethod = "1",
                        ParameterOneEn = item.Body.BodyEn,
                        ParameterOneAr = item.Body.BodyAr,
                        PreferredLanguage = item.Language
                    };

                    await _mWExternalServiceStore.CallSendInstantNotification(notification);
                }
            }
            catch (Exception ex)
            {
                Logger.ApiLog(null, ex, nameof(SendSegmentSMS), null);
                throw;
            }
        }




        private async Task<List<CaseEscalationBrief>> GetEscalationInfo()
        {
            try
            {
                var escalationInfos = await _jobsStore.GetEscalationInfo();

                if (escalationInfos.IsEmpty())
                {
                    return null;
                }

                var escalationBriefs = new List<CaseEscalationBrief>();

                // Escalation Level 1: Group by TeamId + TeamName
                var levelOneGroups = escalationInfos
                    .Where(e => e.EscalationLevel == 1)
                    .GroupBy(e => new { e.TeamId, e.TeamName });

                foreach (var group in levelOneGroups)
                {
                    foreach (var escalationGroup in group.GroupBy(e => e.EscalationLevel))
                    {
                        var escalationCases = escalationGroup.ToDictionary(info => info.CaseId, info => info.CaseNumber);
                        var firstEscalationInfo = escalationGroup.FirstOrDefault();

                        var escalationBrief = new CaseEscalationBrief
                        {
                            TeamId = group.Key.TeamId,
                            TeamName = group.Key.TeamName,
                            EscalationLevel = escalationGroup.Key,
                            Cases = escalationCases,
                            CCEmailId = firstEscalationInfo?.EscalationCCEmailId,
                            FirstEmailId = firstEscalationInfo?.EscalationOneFirstEmailId,
                            SecondEmailId = firstEscalationInfo?.EscalationOneSecondEmailId,
                            CaseCategoryCode = firstEscalationInfo?.CaseCategoryCode
                        };

                        escalationBriefs.Add(escalationBrief);
                    }
                }

                // Escalation Levels 2 : Group by FirstEmailId
                var levelTwoGroups = escalationInfos
                    .Where(e => e.EscalationLevel == 2)
                    .GroupBy(e =>
                        new
                        {
                            e.EscalationLevel,
                            FirstEmailId = e.EscalationTwoFirstEmailId
                        });

                foreach (var group in levelTwoGroups)
                {
                    var escalationCases = group.ToDictionary(info => info.CaseId, info => info.CaseNumber);
                    var firstEscalationInfo = group.FirstOrDefault();

                    var escalationBrief = new CaseEscalationBrief
                    {
                        TeamId = firstEscalationInfo?.TeamId,
                        TeamName = firstEscalationInfo?.TeamName,
                        EscalationLevel = group.Key.EscalationLevel,
                        Cases = escalationCases,
                        CCEmailId = firstEscalationInfo?.EscalationCCEmailId,
                        CaseCategoryCode = firstEscalationInfo?.CaseCategoryCode
                    };
                    escalationBrief.FirstEmailId = firstEscalationInfo?.EscalationTwoFirstEmailId;
                    escalationBrief.SecondEmailId = firstEscalationInfo?.EscalationTwoSecondEmailId;

                    escalationBriefs.Add(escalationBrief);
                }

                // Escalation Levels 3: Group by FirstEmailId
                var levelThreeGroups = escalationInfos
                    .Where(e => e.EscalationLevel == 3 && e.CaseCategoryCode.Equals("2"))
                    .GroupBy(e =>
                        new
                        {
                            e.EscalationLevel,
                            FirstEmailId = e.EscalationThreeFirstEmailId
                        });

                foreach (var group in levelThreeGroups)
                {
                    var escalationCases = group.ToDictionary(info => info.CaseId, info => info.CaseNumber);
                    var firstEscalationInfo = group.FirstOrDefault();

                    var escalationBrief = new CaseEscalationBrief
                    {
                        TeamId = firstEscalationInfo?.TeamId,
                        TeamName = firstEscalationInfo?.TeamName,
                        EscalationLevel = group.Key.EscalationLevel,
                        Cases = escalationCases,
                        CCEmailId = firstEscalationInfo?.EscalationCCEmailId,
                        CaseCategoryCode = firstEscalationInfo?.CaseCategoryCode
                    };
                    escalationBrief.FirstEmailId = firstEscalationInfo?.EscalationThreeFirstEmailId;
                    escalationBrief.SecondEmailId = firstEscalationInfo?.EscalationThreeSecondEmailId;

                    escalationBriefs.Add(escalationBrief);
                }

                return escalationBriefs;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private async Task<SubmissionResponse> CreateEmailRecord(Email emailInfo)
        {

            var emailId = await _jobsStore.CreateEmailRecord(emailInfo);

            if (!string.IsNullOrWhiteSpace(emailId))
            {
                emailInfo.Attachments.ObjectTypeCode = "email";
                emailInfo.Attachments.ActivityId = emailId;
                var result = await _jobsStore.AddAttachment(emailInfo.Attachments);

                if (!result)
                {
                    return SubmissionResponse.Error($"failed to add attachment for {emailInfo.Body}");
                }

                await _jobsStore.UpdateEmailStatus(emailId);

                return SubmissionResponse.Ok();

                //try
                //{
                //    await _internalIntegrationsStore.UpdateEmailStatus(emailId);
                //}
                //catch (Exception)
                //{
                //    return SubmissionResponse.Error("failed to send email");
                //}
            }


            return SubmissionResponse.Error($"Failed to create Email Record for {emailInfo.Body}");
        }

        private async Task<SubmissionResponse> CreateEmailRecord(ReportConfiguration reportConfig)
        {

            var emailId = await _jobsStore.CreateEmailRecord(reportConfig);

            if (!string.IsNullOrWhiteSpace(emailId))
            {
                reportConfig.Attachments.ObjectTypeCode = "email";
                reportConfig.Attachments.ActivityId = emailId;
                var result = await _jobsStore.AddAttachment(reportConfig.Attachments);

                if (!result)
                {
                    return SubmissionResponse.Error($"failed to add attachment for {reportConfig.MailSubject}");
                }

                await _jobsStore.UpdateEmailStatus(emailId);

                return SubmissionResponse.Ok();

            }


            return SubmissionResponse.Error($"Failed to create Email Record for {reportConfig.MailSubject}");
        }

        private async Task<IEnumerable<Email>> FillEmailInfo(List<CaseEscalationBrief> escalationBriefList)
        {
            escalationBriefList = escalationBriefList.Where(e => e.EscalationLevel > 0).ToList();

            var emailTasks = new List<Task<KeyValuePair<Email, string>>>();

            foreach (var item in escalationBriefList)
            {
                var caseIds = item.Cases.Keys.ToList();

                var cases = string.Join(", ", item.Cases.Values);

                // Get report for cases asynchronously
                var emailTask = GetEmailWithReportAsync(item, cases, caseIds);
                emailTasks.Add(emailTask);
            }

            // Await all email tasks to complete
            var emailResults = await Task.WhenAll(emailTasks);

            // Create a dictionary to map email object to its report content
            var emailReports = emailResults.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // Populate the email list using the dictionary
            var result = new List<Email>();
            foreach (var kvp in emailReports)
            {
                kvp.Key.Attachments = new ActivityAttachment
                {
                    ObjectTypeCode = "email",
                    FileName = $"{kvp.Key.TeamName}-Escalationlevel{kvp.Key.EscalationLevel}.pdf",
                    Body = kvp.Value
                };
                result.Add(kvp.Key);
            }

            return result;
        }

        private async Task<KeyValuePair<Email, string>> GetEmailWithReportAsync(CaseEscalationBrief item, string cases, List<string> caseIds)
        {
            // Get report for cases
            var report = await _reportsManager.GetReport(caseIds);

            var escalationEmailConfig = await _jobsStore.GetEmailBodies(_reportConfigurationId);

            var emailBody = escalationEmailConfig?.FirstOrDefault(x => x.EscalationLevel == item.EscalationLevel)?.Body;

            var emailInfo = new Email
            {
                FirstEmailId = item.FirstEmailId,
                SecondEmailId = item.SecondEmailId,
                CCEmailId = item.CCEmailId,
                Body = emailBody,
                Subject = item.EscalationLevel == 1 ? $"{item.TeamName} >> Escalation level : {item.EscalationLevel}" : $"Escalation level : {item.EscalationLevel}",
                TeamName = item.EscalationLevel == 1 ? item.TeamName : "",
                EscalationLevel = item.EscalationLevel
            };

            // Return email object along with the report content
            return new KeyValuePair<Email, string>(emailInfo, report);
        }


        private async Task<IEnumerable<Email>> FillEmailInfoV1(List<CaseEscalationBrief> escalationBriefList)
        {
            escalationBriefList = escalationBriefList.Where(e => e.EscalationLevel > 0).ToList();

            var result = new List<Email>();

            foreach (var item in escalationBriefList)
            {
                var caseIds = item.Cases.Keys.ToList();

                var cases = string.Join(", ", item.Cases.Values);

                //Get report for cases
                var report = await _reportsManager.GetReport(caseIds);

                var emailInfo = new Email
                {
                    FirstEmailId = item.FirstEmailId,
                    SecondEmailId = item.SecondEmailId,
                    CCEmailId = item.CCEmailId,
                    Body = cases,
                    Subject = $"{item.TeamName} >> Escalation level : {item.EscalationLevel}",
                    Attachments = new ActivityAttachment
                    {
                        ObjectTypeCode = "email",
                        FileName = $"{item.TeamName}-Escalationlevel{item.EscalationLevel}.pdf",
                        Body = report
                    }

                };
                result.Add(emailInfo);
            }
            return result;
        }

        private IEnumerable<RecurringJobDto> GetCurrentRecurringJob()
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                // Retrieve all recurring jobs
                var recurringJobs = connection.GetRecurringJobs();

                return recurringJobs;
            }
        }

        private bool DoesJobExist(string jobId)
        {
            var recurringJobs = GetCurrentRecurringJob();

            return recurringJobs.Any(job => job.Id == jobId);
        }

        public async Task<SubmissionResponse> SendEmailReport(string configurationGuid)
        {
            var reportConfig = await _jobsStore.GetEmailConfiguration(configurationGuid);

            var result = await _reportsManager.GetReport(reportConfig.ReportPath);

            reportConfig.Attachments = new ActivityAttachment
            {
                ObjectTypeCode = "email",
                FileName = $"{reportConfig.ReportName}.pdf",
                Body = result
            };

            var emailRecord = await CreateEmailRecord(reportConfig);

            if (!emailRecord.Success)
            {
                return SubmissionResponse.Error(emailRecord.ErrorCode);
            }

            return SubmissionResponse.Ok();

            //var result = await CreateEmailRecord(reportConfig);
        }

        public async Task FillEmailInfoV2(List<CaseEscalationBrief> escalationBriefList)
        {
            try
            {
                escalationBriefList = escalationBriefList.Where(e => e.EscalationLevel > 0).ToList();

                foreach (var item in escalationBriefList)
                {
                    var caseIds = item.Cases.Keys.ToList();
                    var cases = string.Join(", ", item.Cases.Values);

                    // Enqueue the job to process each item and create an email record
                    BackgroundJob.Enqueue(() => ProcessAndCreateEmailRecordAsync(item, cases, caseIds));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [AutomaticRetry(Attempts = 200, DelaysInSeconds = new int[] { 300 }, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task<SubmissionResponse> ProcessAndCreateEmailRecordAsync(CaseEscalationBrief item, string cases, List<string> caseIds)
        {
            // Asynchronously get the report for each item
            try
            {
                var emailResult = await GetEmailWithReportAsync(item, cases, caseIds);

                // Construct the email with report content as an attachment
                var email = emailResult.Key;
                email.Attachments = new ActivityAttachment
                {
                    ObjectTypeCode = "email",
                    FileName = email.EscalationLevel == 1 ? $"{email.TeamName}-Escalationlevel{email.EscalationLevel}.pdf" : $"Escalationlevel{email.EscalationLevel}.pdf",
                    Body = emailResult.Value
                };

                // Call CreateEmailRecord to create the email record in the system
                var response = await CreateEmailRecord(email);

                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Helper function to retrieve report and construct email info
        //private async Task<KeyValuePair<Email, string>> GetEmailWithReportAsync(CaseEscalationBrief item, string cases, List<string> caseIds)
        //{
        //    // Get report for cases
        //    var report = await _reportsManager.GetReport(caseIds);

        //    var emailBody = escalationEmailConfig?.FirstOrDefault(x => x.EscalationLevel == item.EscalationLevel)?.Body;

        //    var emailInfo = new Email
        //    {
        //        FirstEmailId = item.FirstEmailId,
        //        SecondEmailId = item.SecondEmailId,
        //        CCEmailId = item.CCEmailId,
        //        Body = emailBody,
        //        Subject = $"{item.TeamName} >> Escalation level : {item.EscalationLevel}",
        //        TeamName = item.TeamName,
        //        EscalationLevel = item.EscalationLevel
        //    };

        //    // Return email object along with the report content
        //    return new KeyValuePair<Email, string>(emailInfo, report);
        //}



        private async Task<IEnumerable<RMContactInfo>> GetContactsInfo(string segmentCode)
        {
            try
            {
                var contacts = await _jobsStore.GetContactsInfo(segmentCode, pageIndex: 1);

                if (contacts == null)
                    return Enumerable.Empty<RMContactInfo>();

                var fetchedcontacts = new List<RMContactInfo>();

                fetchedcontacts.AddRange(contacts);

                int pageIndex = 1;

                while (contacts.Count() == 5000)
                {
                    pageIndex++;

                    contacts = await _jobsStore.GetContactsInfo(segmentCode, pageIndex);

                    if (contacts == null)
                        break;

                    fetchedcontacts.AddRange(contacts);
                }

                return fetchedcontacts;
            }
            catch (Exception ex)
            {
                //return Enumerable.Empty<RMContactInfo>();
                Logger.ApiLog(new { SegmentCode = segmentCode }, ex, nameof(GetContactsInfo), null);
                throw;
            }
        }

        private string ReplacePlaceholders(string template, RMContactInfo contact, bool isArabic)
        {
            if (string.IsNullOrEmpty(template))
                return string.Empty;

            var sb = new StringBuilder(template);

            // these always use the same contact field
            sb.Replace("<Param1>", contact.Segment ?? string.Empty);
            sb.Replace("<Param4>", contact.RmMobileNumber ?? string.Empty);

            // language‐specific fields
            if (isArabic)
            {
                sb.Replace("<Param2>", contact.FullNameAr ?? string.Empty);
                sb.Replace("<Param3>", contact.RMFullNameAr ?? string.Empty);
            }
            else
            {
                sb.Replace("<Param2>", contact.FullNameEn ?? string.Empty);
                sb.Replace("<Param3>", contact.RMFullNameEn ?? string.Empty);
            }

            return sb.ToString();
        }

        private NotificationBody GenerateBody(SegmentCampaignInfo segmentInfo, RMContactInfo contact)
        {

            var bodyEn = ReplacePlaceholders(segmentInfo.BodyEn, contact, isArabic: false);
            var bodyAr = ReplacePlaceholders(segmentInfo.BodyAr, contact, isArabic: true);

            // if you only ever need one language, return bodyEn or bodyAr directly.
            // here we join them with a blank line, but skip any empty sections:
            var parts = new[] { bodyEn, bodyAr }
                .Where(s => !string.IsNullOrWhiteSpace(s));

            return new NotificationBody
            {
                BodyEn = bodyEn,
                BodyAr = bodyAr,
                Body = string.Join(Environment.NewLine + Environment.NewLine, parts)
            };
        }



        //public async Task<SegmentCampaignInfo> GetSegmentCampaignInfoById(string segmentCampaignId)
        //{
        //    return await _jobsStore.GetSegmentCampaignById(segmentCampaignId);
        //}
    }
}