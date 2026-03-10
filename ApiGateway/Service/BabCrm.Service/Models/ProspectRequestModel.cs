namespace BabCrm.Service.Models
{
    public class ProspectRequestModel
    {
        public string IdNumber { get; set; }
      
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string EmployerName { get; set; }
        
        public int? Age { get; set; }
        
        public string City { get; set; }
        
        //option set
        public int?  MonthlySalary{ get; set; }
        
        public string NearestBranch { get; set; }
        
        public string MobilePhone { get; set; }
        
        public string Email { get; set; }

        //option set
        public int? PreferedTimeToContactYou { get; set; }
        
        public int? LegalIDType { get; set; } //ntw_legalidtypeset
        
        public DateTime? BirthDate { get; set; } //ntw_birthdate

        public string Topic { get; set; } //subject

        public int? Salary { get; set; } //ntw_monthlysalaryset

        public int? Division { get; set; } //ntw_divisionset

        public string PreferredProduct { get; set; } //ntw_PreferredProductID lookup => product

        public string Nationality { get; set; } //ntw_NationalityID lookup => ntw_country

        public string Channel { get; set; } //ntw_TeleSalesChannelID => ntw_telesaleschannel

        public string ExternalChannel { get; set; }

        public string RealEstateCity { get; set; } //address1_city

        public string RealEstateDistrict { get; set; } //address1_line2

        public double? Amount { get; set; } //budgetamount

        public string Employer { get; set; }//ntw_employername
    }
}
