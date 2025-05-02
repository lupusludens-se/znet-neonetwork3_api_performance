namespace SE.Neo.Common.Models.Initiative
{
    public class InitiativeRecommendationCountRequest
    {
        public List<int> InitiativeIds { get; set; }

        public Enums.InitiativeModules InitiativeContentType { get; set; }
    }
}
