using BabCrm.ObjectModel;

namespace BabCrm.Service.Models
{
    public class CustomerMessage
    {
        public string CustomerId { get; set; }
       
        public string Subject { get; set; }
        
        public string Body { get; set; }
        
        public LocalizedValue<string> CustomerMessageType { get; set; }

        public LocalizedValue<string> CustomerMessageSubject { get; set; }
    }
}
