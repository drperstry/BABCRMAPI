using System.ComponentModel.DataAnnotations;

namespace Bab.Jobs.Models
{
    public class WorkflowJobRequest
    {
        [Required]
        public string WorkflowId { get; set; }
       
        [Required]
        public string EntityId { get; set; }

        [Required]
        public string JobId { get; set; }

        [Required]
        public int RecurrenceFrequency { get; set; }

        [Required]
        public DateTime Time { get; set; }
        
    }
}
