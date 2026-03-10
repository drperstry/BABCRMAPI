using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BabCrm.Service.Models
{
    public class SurveyResponse
    {
        [Required]
        public string CustomerId { get; set; }

        [Required]
        public string CaseNumberId { get; set; }

        [Required]
        public string SurveyTemplateCode { get; set; }
        
        public string Comments { get; set; }

        public IEnumerable<SurveyResponseDetail> SurveyResponseDetail { get; set; }
    }
}
