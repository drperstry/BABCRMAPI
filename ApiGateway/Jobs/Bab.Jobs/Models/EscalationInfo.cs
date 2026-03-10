namespace Bab.Jobs.Models
{
    public class EscalationInfo
    {
        public int EscalationLevel { get; set; }

        public string EscalatedOn { get; set; }

        public string NextEscalationOn { get; set; }

        public string CaseNumber { get; set; }

        public string CaseId { get; set; }

        public string TeamId { get; set; }

        public string TeamName { get; set; }

        public string EscalationMatrixId { get; set; }

        public string EscalationCCEmailId { get; set; }

        public string EscalationOneFirstEmailId { get; set; }

        public string EscalationOneSecondEmailId { get; set; }

        public string EscalationTwoFirstEmailId { get; set; }

        public string EscalationTwoSecondEmailId { get; set; }

        public string EscalationThreeFirstEmailId { get; set; }

        public string EscalationThreeSecondEmailId { get; set; }

        public string EscalationFourFirstEmailId { get; set; }

        public string EscalationFourSecondEmailId { get; set; }

        public string EscalationFiveFirstEmailId { get; set; }

        public string EscalationFiveSecondEmailId { get; set; }
        public string CaseCategoryCode { get; set; }
    }

    public class EscalationInfoByEscalationLevel
    {
        public int EscalationLevel { get; set; }
        public List<EscalationInfo> EscalationInfos { get; set; }
    }

    public class ResponsibleTeam
    {
        public string TeamId { get; set; }

        public List<EscalationInfoByEscalationLevel> EscalationInfoByEscalationLevels { get; set; }
    }

    public class CaseEscalationBrief
    {
        public string TeamId { get; set; }

        public string TeamName { get; set; }

        public int EscalationLevel { get; set; }

        public Dictionary<string, string> Cases { get; set; }

        public string FirstEmailId { get; set; }

        public string SecondEmailId { get; set; }

        public string CCEmailId { get; set; }

        public string CaseCategoryCode { get; set; }


    }

    public class Email
    {
        public string FirstEmailId { get; set; }

        public string SecondEmailId { get; set; }

        public string CCEmailId { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public ActivityAttachment Attachments { get; set; }

        public string TeamName { get; set; }

        public int EscalationLevel { get; set; }
    }

    public class ActivityAttachment
    {
        public string ActivityId { get; set; }

        public string ObjectTypeCode { get; set; }

        public string Body { get; set; }

        public string FileName { get; set; }
    }
}
