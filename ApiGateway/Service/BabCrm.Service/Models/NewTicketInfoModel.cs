namespace BabCrm.Service.Models
{
    public class NewTicketInfoModel
    {
        public string CaseId { get; set; }

        public string CaseNumber { get; set; }

        public UploadAttachmentResponse AttachmentInfo { get; set; } = null;
    }
}
