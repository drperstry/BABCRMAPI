namespace BabCrm.Service.Models
{
    public class SurveyAnswer
    {
        public string Id { get; set; }

        public string Score { get; set; }

        public string QuestionId { get; set; }

        public string Order { get; set; }
        
        public string Code { get; set; }

        public bool IsOther { get; set; }
    }
}
