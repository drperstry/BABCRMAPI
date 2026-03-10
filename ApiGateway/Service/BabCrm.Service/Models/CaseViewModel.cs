namespace BabCrm.Service.Models
{
    public class CaseViewModel
    {
        public string CustomerId { get; set; }

        public string LegalId { get; set; }

        public string CaseNumber { get; set; }

        public string CaseTitle { get; set; }

        public string Status { get; set; }

        public string Channel { get; set; }

        public string Product { get; set; }

        public string CaseType { get; set; }

        public string CaseSubType { get; set; }

        public string CaseCategory { get; set; }

        public string Description { get; set; }

        public string CustomerName { get; set; }

        public string CustomerMobile { get; set; }

        public string AccountNumber { get; set; }

        public string Department { get; set; }

        public string TransactionReference { get; set; }

        public double ClaimAmount { get; set; }

        public DateTime TransactionDate { get; set; }

        public string TransactionCurrency { get; set; }

        public string ClaimReason { get; set; }

        public string POSTerminalId { get; set; }

        public string TransactionMerchantId { get; set; }

        public string TransactionAuthorizationCode { get; set; }

        public string TransactionCardType { get; set; }

        public string PreferredSMSLanguage { get; set; }

        public DateTime PreferredCallBackTime { get; set; }

        public string CallBackReason { get; set; }

        public string FinalReplyStatement { get; set; }

        public string IsEligibleForOfficialLetter { get; set; }

        public string CreatedBy { get; set; } // check with Ali

        public string ReplyMethod { get; set; }

        public DateTime OpenedDate { get; set; }

        public DateTime ClosedDate { get; set; }

        public int CaseStatus { get; set; }

        public List<AttachmentInfo> Attachments { get; set; } = new List<AttachmentInfo>();

        public string CaseId { get; set; }

        public List<CaseSurveyResponse> CaseSurveyResponses { get; set; }

        public List<CaseSurveyTemplate> CaseSurveyTemplates { get; set; }

        public string Cif { get; internal set; }

        public bool IsFromArchive { get; set; } = false;

        public string? AttachementPath { get; set; }

        public bool? AttachementSharePointUrl { get; set; }
        public string AttachmentPath { get; internal set; }
    }

    public class CaseSurveyTemplate
    {
        public string SurveyTemplateId { get; set; }

        public string SurveyTemplateName { get; set; }

        public string SurveyTemplateCode { get; set; }

        public List<SurveyTemplateQuestion> SurveyTemplateQuestions { get; set; }
    }

    public class SurveyTemplateQuestion
    {
        public string SurveyTemplateQuestionId { get; set; }

        public string SurveyTemplateQuestionName { get; set; }

        public string SurveyTemplateQuestionCode { get; set; }

        public string SurveyTemplateQuestionWeight { get; set; }

        public int SurveyTemplateQuestionOrder { get; set; }

        public List<SurveyTemplateAnswer> SurveyTemplateAnswers { get; set; }
    }

    public class SurveyTemplateAnswer
    {
        public string SurveyTemplateAnswerId { get; set; }

        public string SurveyTemplateAnswerName { get; set; }

        public int SurveyTemplateAnswerScore { get; set; }

        public int SurveyTemplateAnswerOrder { get; set; }

        public string SurveyTemplateAnswerCode { get; set; }
        public bool IsOther { get; internal set; }
    }
}
