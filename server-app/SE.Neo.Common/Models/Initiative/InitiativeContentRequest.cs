namespace SE.Neo.Common.Models.Initiative
{

    /// <summary>
    /// Class that contains request for saving different contents during Initiative creation
    /// </summary>
    public class InitiativeContentRequest
    {
        public bool IsNew { get; set; }
        public int InitiativeId { get; set; }
        public List<int>? ArticleIds { get; set; }
        public List<int>? CommunityUserIds { get; set; }
        public List<int>? ProjectIds { get; set; }
        public List<int>? ToolIds { get; set; }
        public List<int>? DiscussionIds { get; set; }

    }
}
