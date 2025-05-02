namespace SE.Neo.Common.Models.Initiative
{
    public class InitiativeContentDTO
    {
        public bool IsNew { get; set; }
        public int InitiativeId { get; set; }
        public List<int> ArticleIds { get; set; }
        public List<int> CommunityUserIds { get; set; }
        public List<int> ProjectIds { get; set; }
        public List<int> ToolIds { get; set; }
        public List<int> DiscussionIds { get; set; }
    }
}
