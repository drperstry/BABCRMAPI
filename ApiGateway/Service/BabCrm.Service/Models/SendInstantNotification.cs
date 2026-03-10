namespace BabCrm.Service.Models
{
    public class SendInstantNotification
    {
        public string CIF { get; set; }
        public string NotificationMethod { get; set; }
        public string EventType { get; set; }
        public string EventSubSType { get; set; }
        public string ParameterOneAr { get; set; }
        public string ParameterOneEn { get; set; }
        public string ParameterTwoAr { get; set; }
        public string ParameterTwoEn { get; set; }
        public string ParameterThreeAr { get; set; }
        public string ParameterThreeEn { get; set; }
        public string ParameterFourAr { get; set; }
        public string ParameterFourEn { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string TemplateCode { get; set; }
        public string PreferredLanguage { get; set; }
        public DateTime? ParameterThreeDateAr { get; set; }
        public DateTime? ParameterThreeDateEn { get; set; }
        public DateTime? ExpectedSendingTime { get; set; }
        public string FilePath { get; set; }
        public string SenderId { get; set; }
        public string OrganizationId { get; set; }
    }
}
