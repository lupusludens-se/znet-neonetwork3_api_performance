using SE.Neo.WebAPI.Models.Shared;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectCommunitySolarDetailsResponse : BaseCommentedProjectDetailsResponse
    {
        public float? MinimumAnnualMWh { get; set; }
        public int? TotalAnnualMWh { get; set; }
        public string UtilityTerritory { get; set; }
        public bool? ProjectAvailable { get; set; }
        public DateTime? ProjectAvailabilityApproximateDate { get; set; }
        public bool? IsInvestmentGradeCreditOfOfftakerRequired { get; set; }
        public IEnumerable<BaseIdNameResponse> ContractStructures { get; set; }
        public int? MinimumTermLength { get; set; }
        public IEnumerable<BaseIdNameResponse> ValuesProvided { get; set; }
    }
}
