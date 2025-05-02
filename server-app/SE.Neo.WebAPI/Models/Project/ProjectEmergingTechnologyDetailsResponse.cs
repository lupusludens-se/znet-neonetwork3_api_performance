using SE.Neo.WebAPI.Models.Shared;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectEmergingTechnologyDetailsResponse : BaseCommentedProjectDetailsResponse
    {
        public float? MinimumAnnualValue { get; set; }
        public int? EnergyUnitId { get; set; }
        public IEnumerable<BaseIdNameResponse> ContractStructures { get; set; }
        public int? MinimumTermLength { get; set; }
        public IEnumerable<BaseIdNameResponse> ValuesProvided { get; set; }
    }
}
