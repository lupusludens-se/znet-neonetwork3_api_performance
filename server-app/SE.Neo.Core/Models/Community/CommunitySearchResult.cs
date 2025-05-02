using SE.Neo.Common.Enums;

namespace SE.Neo.Core.Models.Community
{
    public class CommunitySearchResult
    {
        public int Id { get; set; }
        public string Sort { get; set; }
        public CommunityItemType Type { get; set; }
        public bool IsFollowed { get; set; }
        public int Order { get; set; }
        public int StartsOrContains { get; set; }
    }
}
