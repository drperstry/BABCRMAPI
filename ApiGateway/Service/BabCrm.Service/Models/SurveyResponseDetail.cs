using BabCrm.Core;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BabCrm.Service.Models
{
    public class SurveyResponseDetail : IValidatableObject
    {
        [Required]
        public string QuestionCode { get; set;}
       

        public string AnswerCode { get; set;}

        public string OtherAnswer { get; set;}

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AnswerCode.IsEmpty())
            {
                // Check if required fields are null or empty
                if (OtherAnswer.IsEmpty())
                {
                    yield return new ValidationResult("Answer is required, either choose from options or fill the other answer", new[] { nameof(OtherAnswer) });
                }
            }
        }
    }
}
