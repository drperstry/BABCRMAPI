using System.ComponentModel.DataAnnotations;

namespace BabCrm.Service.Models
{
    public class UploadAttachmentRequest

    {
        [Required]
        public string Base64Data { get; set; }

        [Required]
        public string FileName { get; set; }

        // public string FolderPath { get; set; }
    }
}
