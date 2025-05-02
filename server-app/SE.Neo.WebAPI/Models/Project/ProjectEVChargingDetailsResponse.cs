using SE.Neo.WebAPI.Models.Shared;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectEVChargingDetailsResponse : BaseCommentedProjectDetailsResponse
    {
        public int? MinimumChargingStationsRequired { get; set; }
        public IEnumerable<BaseIdNameResponse> ContractStructures { get; set; }
        public int? MinimumTermLength { get; set; }
        public IEnumerable<BaseIdNameResponse> ValuesProvided { get; set; }
    }
}
