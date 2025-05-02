using SE.Neo.Common.Attributes;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Common.Models.Project
{
    public class ProjectCarbonOffsetsDetailsDTO : BaseCommentedProjectDetailsDTO
    {
        [PropertyComparation("Minimum Purchase Volume Available")]
        public float? MinimumPurchaseVolume { get; set; }

        [PropertyComparation("Strip Lengths Available")]
        public IEnumerable<BaseIdNameDTO> StripLengths { get; set; }
    }
}
