using BabCrm.ObjectModel;

namespace BabCrm.Service.Models
{
    public class CustomerMessageDetailsModel
    {
        public string Status { get; set; }

        public string ReadByUserId { get; set; }

        public string ReadBy { get; set; }

        public DateTime ReadDate { get; set; }

        public string RepliedBy { get; set; }

        public string RepliedByUserId { get; set; }

        public DateTime ReplyDate { get; set; }

        public string ConvertedToCaseBy { get; set; }

        public string ConvertedToCaseByUserId { get; set; }

        public DateTime ConvertedToCaseDate { get; set; }

        public string ReferenceId { get; set; }

        public string CustomerMessageId { get; set; }

        public string CustomerFullName { get; set; }

        public string ReplyMessage { get; set; }

        public string CustomerId { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string CustomerMessageType { get; set; }

        public string CustomerMessageSubject { get; set; }

        public DateTime CreatedOn { get; set; }
    }


}
