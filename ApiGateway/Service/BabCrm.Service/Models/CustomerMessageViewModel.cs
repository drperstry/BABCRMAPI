namespace BabCrm.Service.Models
{
    public class CustomerMessageViewModel
    {
        public string ReferenceId { get; set; }

        public string CustomerMessageId { get; set; }

        public string CustomerFullName { get; set; }

        public string ReplyMessage { get; set; }

        public string CustomerId { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string CustomerMessageType { get; set; }

        public string CustomerMessageSubject { get; set; }
        
        public string StatusCode { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Status { get; internal set; }
    }


}
