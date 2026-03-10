namespace BabCrm.Service.Models
{
    public class CallbackRequestBaseModel : RmModel
    {
        public string Cif { get; set; }
        public string TimeInterval { get; set; }
        public DateTime CustomerChosenDate { get; set; }
        
    }
}
