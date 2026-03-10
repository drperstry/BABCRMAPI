namespace BabCrm.Service.Models
{
    public class CallbackRequest : CallbackRequestBaseModel
    {
        public string InternalRmId { get; set; }
        public string InternalUserId { get; set; }
    }
}
