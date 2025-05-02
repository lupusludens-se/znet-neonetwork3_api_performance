using SE.Neo.WebAPI.Models.CMS;

namespace SE.Neo.WebAPI.Models.Initiative
{
    public class InitiativeExportResponse : InitiativeAdminResponse
    {
       
        public string RegionsString { get; set; }
        public string ChangedOn { get; set; }
    }

}