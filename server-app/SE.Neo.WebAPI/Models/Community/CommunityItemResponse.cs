using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Models.CMS;
using SE.Neo.WebAPI.Models.Media;
using SE.Neo.WebAPI.Models.Role;
namespace SE.Neo.WebAPI.Models.Community
{
    public class CommunityItemResponse
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }
        public BlobResponse Image { get; set; }
        public CommunityItemType Type { get; set; }
        public string? JobTitle { get; set; }
        public int? MemberCount { get; set; }
        public IEnumerable<CategoryResponse>? Categories { get; set; }
        public bool IsFollowed { get; set; }

        public RoleResponse Role { get; set; }
    }
}
