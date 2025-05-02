using SE.Neo.Common.Models.Initiative;

namespace SE.Neo.WebAPI.Models.Initiative
{
    public class InitiativeAndProgressDetailsResponse : BaseInitiative
    {
        public int CurrentStepId { get; set; }

        public IEnumerable<InitiativeStepResponse> Steps { get; set; }

        public InitiativeRecommendationCount? RecommendationsCount { get; set; }
    }
}
