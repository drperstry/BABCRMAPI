namespace BabCrm.Service.Models
{
    public class UploadAttachmentResponse
    {

        public bool Success { get; set; }

        public string ErrorMessage { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string AttachmentName { get; set; }
    }
}
