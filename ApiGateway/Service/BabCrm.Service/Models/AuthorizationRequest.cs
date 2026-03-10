namespace BabCrm.Service.Models
{
    public class AuthorizationRequest
    {
        public string Cif { get; set; }
        public string ApplicationNo { get; set; }
        public string RequestType { get; set; }
        public string ContactId { get; set; }
    }
}
