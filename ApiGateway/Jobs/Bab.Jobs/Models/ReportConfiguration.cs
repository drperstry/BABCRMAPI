namespace Bab.Jobs.Models
{
    public class ReportConfiguration
    {
        public string ReportName { get; set; }
        public string ReportServiceUrl { get; set; }
        public string ReportPath { get; set; }
        public string EmailSender { get; set; }
        public string EmailRecipient { get; set; }
        public string CcMailGroup { get; set; }
        public string MailSubject { get; set; }
        public string MailDescription { get; set; }
        public ActivityAttachment Attachments { get; set; }
    }
}
