namespace BabCrm.Service.Models
{
    public class CaseSurveyResponse
    {
        //public DateTime SurveySubmissionDate { get; set; }
        
        public string SurveyReferenceId { get; set; }

        public string SurveyResponseStatus { get; set; }
        
        public string SurveyComment { get; set; }

        public List<CaseSurveyResponseDetails> SurveyResponseDetails { get; set; }
    }
}
