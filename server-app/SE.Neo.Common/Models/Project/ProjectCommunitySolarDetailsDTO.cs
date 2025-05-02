using SE.Neo.Common.Attributes;

namespace SE.Neo.Common.Models.Project
{
    public class ProjectCommunitySolarDetailsDTO : BaseCommentedProjectDetailsDTO
    {
        [PropertyComparation("Minimum Annual kWh Purchase")]
        public float? MinimumAnnualMWh { get; set; }

        [PropertyComparation("Total Annual kWh Available")]
        public int? TotalAnnualMWh { get; set; }

        [PropertyComparation("Utility Territory")]
        public string UtilityTerritory { get; set; }

        public bool? ProjectAvailable { get; set; }

        [PropertyComparation("Approximate Date of Project Availability")]
        public DateTime? ProjectAvailabilityApproximateDate { get; set; }

        [PropertyComparation("Requires Investment Grade Credit of Offtaker")]
        public bool? IsInvestmentGradeCreditOfOfftakerRequired { get; set; }

        [PropertyComparation("Minimum Term Length Available")]
        public int? MinimumTermLength { get; set; }
    }
}
