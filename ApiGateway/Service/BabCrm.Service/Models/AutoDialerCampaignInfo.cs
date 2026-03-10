namespace BabCrm.Service.Models
{
    public class AutoDialerCampaignInfo
    {
        public string SkillGroup { get; set; }

        public string CampaignCode { get; set; }

        public List<AutoDialerLeadInfo> Leads { get; set; } = new List<AutoDialerLeadInfo>();
    }
}
