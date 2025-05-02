using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Common.Models.Initiative
{
    /// <summary>
    /// Initiative Recommendation Request; For Learn, community, files, projects
    /// </summary>
    public class InitiativeRecommendationRequest : ExpandOrderModel
    {
        /// <summary>
        /// Whether Create or Edit Form
        /// </summary>
        public bool IsCreate { get; set; }

        /// <summary>
        /// Attached content id for applying pagination and auto selection
        /// </summary>
        public int? AttachedContentId { get; set; }
    }
}
