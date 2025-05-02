using SE.Neo.WebAPI.Models.Shared;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectCarbonOffsetsDetailsResponse : BaseCommentedProjectDetailsResponse
    {
        public float? MinimumPurchaseVolume { get; set; }

        public IEnumerable<BaseIdNameResponse> StripLengths { get; set; }

        public IEnumerable<BaseIdNameResponse> ValuesProvided { get; set; }
    }
}
