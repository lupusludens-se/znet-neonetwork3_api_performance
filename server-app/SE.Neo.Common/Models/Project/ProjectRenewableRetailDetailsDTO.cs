using SE.Neo.Common.Attributes;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Common.Models.Project
{
    public class ProjectRenewableRetailDetailsDTO : BaseCommentedProjectDetailsDTO
    {
        [PropertyComparation("Minimum Annual Site")]
        public float? MinimumAnnualSiteKWh { get; set; }

        [PropertyComparation("Minimum Term Length Available")]
        public float? MinimumTermLength { get; set; }

        [PropertyComparation("Purchase Options")]
        public IEnumerable<BaseIdNameDTO> PurchaseOptions { get; set; }
    }
}
