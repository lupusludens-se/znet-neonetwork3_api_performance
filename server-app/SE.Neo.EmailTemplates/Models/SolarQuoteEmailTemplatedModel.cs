using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Shared;
using SE.Neo.EmailTemplates.Models.BaseModel;
namespace SE.Neo.EmailTemplates.Models
{
    public class SolarQuoteEmailTemplatedModel : BaseTemplatedEmailModel
    {
        public override string TemplateName => "SolarQuoteEmailTemplate";
        public IEnumerable<BaseIdNameDTO> Interests { get; set; }

        public IEnumerable<BaseIdNameDTO> ContractStructures { get; set; }

        public string AdditionalComments { get; set; }

        public string SiteAddress { get; set; }

        public long AnnualPower { get; set; }

        public bool BuildingOwned { get; set; }

        public bool RoofAvailable { get; set; }

        public long? RoofArea { get; set; }

        public AreaType? RoofAreaType { get; set; }

        public bool LandAvailable { get; set; }

        public long? LandArea { get; set; }

        public AreaType? LandAreaType { get; set; }

        public bool CarportAvailable { get; set; }

        public long? CarportArea { get; set; }

        public AreaType? CarportAreaType { get; set; }

        public string RequestedUserName { get; set; }

        public string CompanyName { get; set; }

        public bool PortfolioReview { get; set; }

        public string UserEmail { get; set; }

    }
}