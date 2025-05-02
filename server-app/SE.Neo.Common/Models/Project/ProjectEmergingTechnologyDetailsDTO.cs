using SE.Neo.Common.Attributes;

namespace SE.Neo.Common.Models.Project
{
    public class ProjectEmergingTechnologyDetailsDTO : BaseCommentedProjectDetailsDTO
    {
        [PropertyComparation("Minimum Volume/Load Required")]
        public float? MinimumAnnualValue { get; set; }
        [PropertyComparation("Minimum Volume/Load Energy Unit Id")]
        public int? EnergyUnitId { get; set; }

        [PropertyComparation("Minimum Term Length Available")]
        public int? MinimumTermLength { get; set; }
    }
}
