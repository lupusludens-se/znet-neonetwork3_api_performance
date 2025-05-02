using SE.Neo.Common.Attributes;

namespace SE.Neo.Common.Models.Project
{
    public class ProjectEfficiencyAuditsAndConsultingDetailsDTO : BaseCommentedProjectDetailsDTO
    {
        [PropertyComparation("Minimum Term Length Available")]
        public int? MinimumTermLength { get; set; }

        [PropertyComparation("Requires Investment Grade Credit of Offtaker")]
        public bool? IsInvestmentGradeCreditOfOfftakerRequired { get; set; }
    }
}
