using BabCrm.ObjectModel;

namespace BabCrm.Service.Models
{
    public class SurveyQA
    {
        public string QuestionId { get; set; }
                      
        public LocalizedValue<string> QuestionName { get; set; }
                      
        public string QuestionWeight { get; set; }
                      
        public string SurveyTemplateId { get; set; }
                      
        public string QuestionOrder { get; set; }
                      
        public string QuestionCode { get; set; }

        public string AnswerId { get; set; }
                      
        public LocalizedValue<string> AnswerName { get; set; }
                     
        public string AnswerScore { get; set; }
                      
        public string AnswerOrder { get; set; }
                     
        public string AnswerCode { get; set; }
    }
}
