using Bab.Jobs.Models;
using BabCrm.Jobs.Models;

namespace Bab.Jobs
{
    public interface IJobsStore
    {
        Task ExecuteTimerRequest(RequestData request);
        Task<IEnumerable<Holiday>> GetHolidays(Guid reportConfigurationId);

        Task<IEnumerable<EscalationInfo>> GetEscalationInfo();

        Task<IEnumerable<string>> CreateEmailRecords(List<NotificationInfo> notifications);

        Task<IEnumerable<string>> BulkUpdateEmailStatus(IEnumerable<string> emailIds);

        Task<string> CreateEmailRecord(Email emailInfo);

        Task<string> CreateEmailRecord(ReportConfiguration reportConfig);

        Task<bool> AddAttachment(ActivityAttachment attachment);

        Task ExecuteWorkflow(string workflowId, string entityId);

        Task UpdateEmailStatus(string emailId);

        Task<ReportConfiguration> GetEmailConfiguration(string configurationGuid);

        Task<SegmentCampaignInfo> GetSegmentCampaignById(string segmentCampaignId);

        Task<IEnumerable<EscalationEmailConfig>> GetEmailBodies(string reportConfigurationId);

        Task<IEnumerable<RMContactInfo>> GetContactsInfo(string segmentCode, int pageIndex = 1);
    }
}
