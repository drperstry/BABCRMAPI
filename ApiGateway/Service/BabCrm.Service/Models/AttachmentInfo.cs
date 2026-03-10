namespace BabCrm.Service.Models
{
    public class AttachmentInfo
    {
        public string FileName { get; set; }
       
        public string AttachmentId { get; set; }

        public string RecordId { get; set; }

        public string FilePath { get; set; }
        
        public string TitleEn { get; set; }
        
        public string TitleAr { get; set; }
    }

    public enum RecordType
    {
        INCIDENT
    }
}
