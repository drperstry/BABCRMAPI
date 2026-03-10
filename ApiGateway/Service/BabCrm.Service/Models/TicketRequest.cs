namespace BabCrm.Service.Models
{
    public class TicketRequest : TicketRequestModel
    {
        public string InternalProductId { get; set; }
        
        public string InternalTicketTypeId { get; set; }
        
        public string InternalTicketSubTypeId { get; set; }
    }
}
