using SE.Neo.Common.Attributes;

namespace SE.Neo.Common.Models.Project
{
    public class ProjectEVChargingDetailsDTO : BaseCommentedProjectDetailsDTO
    {
        [PropertyComparation("Minimum Charging Stations Required")]
        public int? MinimumChargingStationsRequired { get; set; }

        [PropertyComparation("Minimum Term Length Available")]
        public int? MinimumTermLength { get; set; }
    }
}
