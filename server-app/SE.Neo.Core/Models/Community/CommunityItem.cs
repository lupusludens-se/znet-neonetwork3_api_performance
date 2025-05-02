using SE.Neo.Common.Enums;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Entities.CMS;
namespace SE.Neo.Core.Models.Community
{
    public class CommunityItem
    {
        public int Id { get; set; }
        public CommunityItemType Type { get; set; }
        public bool? IsFollowed { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public Blob Image { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
        public string? JobTitle { get; set; }
        public int? MemberCount { get; set; }

        public IEnumerable<UserRole>? Roles { get; set; }
    }
}
