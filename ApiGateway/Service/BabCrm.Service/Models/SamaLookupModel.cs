namespace BabCrm.Service.Models
{
    public class SamaLookupModel: BaseLookupModel
    {
        public string RelatedRecordId { get; set; }

        public BaseLookupModel SamaValues { get; set; }
        public string RelatedRecordCode { get; set; }
    }
}
