namespace SE.Neo.Common.Models.Initiative
{
    public class InitiativeRecommendationCount
    {
        public int InitiativeId { get; set; }

        public int ArticlesCount { get; set; }

        public int ProjectsCount { get; set; }
        public int ToolsCount { get; set; }
        public int CommunityUsersCount { get; set; }
        public int MessagesUnreadCount { get; set; }
    }
}
