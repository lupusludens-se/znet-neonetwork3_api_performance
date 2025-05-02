using SE.Neo.Common.Models.Shared;
namespace SE.Neo.Common.Models.Tool
{
    public class SolarQuoteDTO
    {
        public int Id { get; set; }

        public bool PortfolioReview { get; set; }

        public int RequestedByUserId { get; set; }

        public string? SiteAddress { get; set; }

        public long? AnnualPower { get; set; }

        public bool? BuildingOwned { get; set; }

        public bool? RoofAvailable { get; set; }

        public long? RoofArea { get; set; }

        public int? RoofAreaType { get; set; }

        public bool? LandAvailable { get; set; }

        public long? LandArea { get; set; }

        public int? LandAreaType { get; set; }

        public bool? CarportAvailable { get; set; }

        public long? CarportArea { get; set; }

        public int? CarportAreaType { get; set; }

        public IEnumerable<BaseIdNameDTO>? ContractStructures { get; set; }

        public IEnumerable<BaseIdNameDTO>? Interests { get; set; }

        public string? AdditionalComments { get; set; }

    }
}
