using SE.Neo.WebAPI.Models.Shared;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectEfficiencyEquipmentMeasuresDetailsResponse : BaseCommentedProjectDetailsResponse
    {
        public IEnumerable<BaseIdNameResponse> ContractStructures { get; set; }
        public int? MinimumTermLength { get; set; }
        public bool? IsInvestmentGradeCreditOfOfftakerRequired { get; set; }
        public IEnumerable<BaseIdNameResponse> ValuesProvided { get; set; }
    }
}
