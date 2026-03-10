using System.ComponentModel.DataAnnotations;

namespace BabCrm.Service.ArchiveDataModels
{
    public class ArchiveDataRequest
    {
        [Required]
        public string TableName { get; set; }

        public CommonFilter Filter { get; set; }
    }
}
