namespace BabCrm.Jobs.Models
{
    public class RMContactInfo
    {
        public string ContactId { get; set; }

        public string FullNameEn { get; set; }

        public string FullNameAr { get; set; }

        public string MobilePhone { get; set; }

        //Dont exist on crm
        public string RmMobileNumber { get; set; }

        public string Segment { get; set; }

        public string RMFullNameEn { get; set; }

        //Dont exist on crm
        public string RMFullNameAr { get; set; }
    }
}
