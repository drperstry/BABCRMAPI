using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Bab.Attachments.Models
{
    public class Attachment
    {
        [Required]
        public string Base64Data { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string FolderPath { get; set; }

        [Required]
        public string RecordId { get; set; }
    }
}
