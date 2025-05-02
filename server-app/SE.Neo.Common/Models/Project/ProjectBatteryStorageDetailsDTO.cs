using SE.Neo.Common.Attributes;

namespace SE.Neo.Common.Models.Project
{
    public class ProjectBatteryStorageDetailsDTO : BaseCommentedProjectDetailsDTO
    {
        [PropertyComparation("Minimum Annual Peak (kW)")]
        public float? MinimumAnnualPeakKW { get; set; }

        [PropertyComparation("Minimum Term Length Available")]
        public int? MinimumTermLength { get; set; }
    }
}
