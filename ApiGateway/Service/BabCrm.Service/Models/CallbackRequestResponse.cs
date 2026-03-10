namespace BabCrm.Service.Models
{
    public class CallbackRequestResponse
    {
        public string RmEmail { get; set; }
        public string RmName { get; set; }
        public string RmPhoneNumber { get; set; }
        public DateTime CustomerRequestedDate { get; set; }
        public string TimeSlot { get; set; }
        public string RequestStatus { get; set; }
        public string CallStatus { get; set; }
        public DateTime? RmCloseDateTime { get; set; }
        public string TimeSlotCode { get; set; }
    }
}
