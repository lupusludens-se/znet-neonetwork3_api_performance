using SE.Neo.Common.Attributes;

namespace SE.Neo.Common.Models.Project
{
    public class ProjectFuelCellsDetailsDTO : BaseCommentedProjectDetailsDTO
    {
        [PropertyComparation("Minimum Annual Site Load (MWh)")]
        public float? MinimumAnnualSiteKWh { get; set; }

        [PropertyComparation("Minimum Term Length Available")]
        public int? MinimumTermLength { get; set; }
    }
}
