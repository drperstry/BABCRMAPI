using System.ComponentModel.DataAnnotations;



namespace BabCrm.Service.Models
{
    public class TicketRequestModel : IValidatableObject
    {

        // required: FN LN  mobile  || optional: govid Email

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Mobile { get; set; }

        public string GovernmentId { get; set; }

        public string Email { get; set; }

        public string CIF { get; set; }

        [Required]
        public string ChannelCode { get; set; }

        [Required]
        public string ProductServiceId { get; set; }

        [Required]
        public string TypeId { get; set; }

        [Required]
        public string SubTypeId { get; set; }

        [Required]
        public string CategoryCode { get; set; }

        public string Description { get; set; }

        public string AccountNumber { get; set; }

        public string DepartmentId { get; set; }

        public string TransactionReference { get; set; }

        public double ClaimAmount { get; set; }

        public string CaseTitle { get; set; }

        public DateTime? TransactionDate { get; set; }

        public string TransactionCurrencyId { get; set; }

        public string ClaimReason { get; set; }

        public string POSTerminalId { get; set; }

        public string TransactionMerchantId { get; set; }

        public string TransactionAuthorizationCode { get; set; }

        public string TransactionCardType { get; set; }

        public DateTime? PreferredCallBackTime { get; set; }

        public string CallBackReason { get; set; }

        public string FinalReplyStatement { get; set; }

        public bool? IsEligibileForOfficialLetter { get; set; } //OfficialLetterEligibility -- need check with crm

        public int? ReplyMethod { get; set; }

        public int? ReceiveMethod { get; set; }

        public string CallerNumber { get; set; }

        public string CalledNumber { get; set; }

        public string TransactionType { get; set; }

        public string TransactionCode { get; set; }

        public bool IsFeesApplied { get; set; }

        public string CmsCardReplacementNumber { get; set; }

        public string PreferredSMSLanguage { get; set; }

        public UploadAttachmentRequest Attachment { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Check if CIF is null or empty
            if (string.IsNullOrWhiteSpace(CIF))
            {
                // Check if required fields are null or empty
                if (string.IsNullOrWhiteSpace(FirstName))
                {
                    yield return new ValidationResult("First name is required.", new[] { nameof(FirstName) });
                }

                if (string.IsNullOrWhiteSpace(LastName))
                {
                    yield return new ValidationResult("Last name is required.", new[] { nameof(LastName) });
                }

                if (string.IsNullOrWhiteSpace(Mobile))
                {
                    yield return new ValidationResult("Mobile is required.", new[] { nameof(Mobile) });
                }
            }
        }
    }
}
