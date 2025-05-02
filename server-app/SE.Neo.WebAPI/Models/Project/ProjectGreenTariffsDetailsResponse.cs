using SE.Neo.WebAPI.Models.Shared;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectGreenTariffsDetailsResponse : BaseCommentedProjectDetailsResponse
    {
        public string UtilityName { get; set; }
        public string ProgramWebsite { get; set; }
        public float? MinimumPurchaseVolume { get; set; }
        public int? TermLengthId { get; set; }
        public string? TermLengthName { get; set; }
        public IEnumerable<BaseIdNameResponse> ValuesProvided { get; set; }
    }
}
