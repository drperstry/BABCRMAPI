using BabCrm.ObjectModel;

namespace BabCrm.Service.Models
{
    public class SurveyAnswerModel : SurveyAnswer
    {
        public LocalizedValue<string> Name { get; set; }
    }
}
