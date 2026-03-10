namespace BabCrm.Service.Models
{
    public class EmployeeRMs : EmployeeBaseClass
    {
        public string Branch { get; set; }
        public AreaManager RegionalManagerDetails { get; set; }
        public AffluentAreaManager AffluentAreaManagerDetails { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
    }
}
