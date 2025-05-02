using SE.Neo.WebAPI.Models.Shared;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectBatteryStorageDetailsResponse : BaseCommentedProjectDetailsResponse
    {
        public float? MinimumAnnualPeakKW { get; set; }
        public IEnumerable<BaseIdNameResponse> ContractStructures { get; set; }
        public int? MinimumTermLength { get; set; }
        public IEnumerable<BaseIdNameResponse> ValuesProvided { get; set; }
    }
}
