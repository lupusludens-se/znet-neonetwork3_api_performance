using SE.Neo.Common.Enums;

namespace SE.Neo.Core.Models.Community
{
    public class CommunityCreateQueryModel
    {
        public int UserId { get; set; }
        public string? Search { get; set; }
        public string? Filter { get; set; }
        public CommunityItemType? Type { get; set; }
        public bool OnlyFollowed { get; set; }
    }
}
