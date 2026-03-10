namespace BabCrm.Service.Models
{
    public class TicketFilter
    {
        public string NationalId { get; set; }

        public string MobileNumber { get; set; }

        public string CaseNumber { get; set; }

        public string StatusCode { get; set; }

        public bool? IsOpen { get; set; }  

        public string TypeCode { get; set; }

        public string SubTypeCode { get; set; }

        public string CategoryCode { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? IsInternal { get; set; } = false;
    }
}
