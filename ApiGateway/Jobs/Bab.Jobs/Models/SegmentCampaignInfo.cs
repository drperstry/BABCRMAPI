namespace BabCrm.Jobs.Models
{
    public class SegmentCampaignInfo
    {
        public DateTime? Runtime { get; set; }
        public string SegmentCode { get; set; }
        public string BodyEn { get; set; }
        public string BodyAr { get; set; }

        public string SubjectEn { get; set; }

        public string SubjectAr { get; set; }

        public string ReplyMethodCode { get; set; }

        public string LanguageCode { get; set; }

        public string EventType { get; set; }

        public string EventSubType { get; set; }

        public string OrganizationId { get; set; }

        public string SenderId { get; set; }
    }

    public class NotificationInfo
    {
        public string ContactId { get; set; }

        public string MobileNumber { get; set; }

        public NotificationBody Body { get; set; }

        public string Subject { get; set; }

        public string Language { get; internal set; }

        public DateTime? Runtime { get; set; }

        public string EventType { get; set; }

        public string EventSubType { get; set; }

        public string OrganizationId { get; set; }

        public string SenderId { get; set; }
    }

    public class NotificationBody
    {
        public string Body { get; set; }

        public string BodyEn { get; set; }

        public string BodyAr { get; set; }
    }
}
