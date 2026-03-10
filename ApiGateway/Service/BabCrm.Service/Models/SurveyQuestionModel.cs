using BabCrm.ObjectModel;

namespace BabCrm.Service.Models
{

    public class SurveyQuestionModel : SurveyQuestion
    {
        public LocalizedValue<string> Name { get; set; }
    }
}
