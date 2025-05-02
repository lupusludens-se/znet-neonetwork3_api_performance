using SE.Neo.Common.Attributes;

namespace SE.Neo.Common.Models.Project
{
    public class ProjectGreenTariffsDetailsDTO : BaseCommentedProjectDetailsDTO
    {
        [PropertyComparation("Utility Name")]
        public string UtilityName { get; set; }

        [PropertyComparation("Program Website")]
        public string ProgramWebsite { get; set; }

        [PropertyComparation("Program Website")]
        public float? MinimumPurchaseVolume { get; set; }

        [PropertyComparation("Minimum Purchase Volume Available")]
        public int? TermLengthId { get; set; }

        public string? TermLengthName { get; set; }
    }
}
