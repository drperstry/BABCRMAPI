using System.ComponentModel.DataAnnotations;

namespace BabCrm.Service.Models
{
    public class CustomerMessageRequest
    {
        [Required]
        public string CustomerId { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string CustomerMessageType { get; set; }

        public string CustomerMessageSubject { get; set; }
    }
}
