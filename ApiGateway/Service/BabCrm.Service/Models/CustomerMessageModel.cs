using BabCrm.ObjectModel;

namespace BabCrm.Service.Models
{
    public class CustomerMessageModel : CustomerMessage
    {
        public string ReferenceId { get; set; }

        public string CustomerMessageId { get; set; }    
        
        public string CustomerFullName { get; set; }

        public string ReplyMessage { get; set; }

        public string Status { get; set; }

        public LocalizedValue<string> StatusName { get; set; }

        public DateTime CreatedOn { get; set; }

    }


}
