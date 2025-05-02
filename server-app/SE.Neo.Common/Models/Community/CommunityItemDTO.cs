using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Media;
using SE.Neo.Common.Models.Shared;
namespace SE.Neo.Common.Models.Community
{
    public class CommunityItemDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }
        public BlobDTO Image { get; set; }
        public CommunityItemType Type { get; set; }
        public int? MemberCount { get; set; }
        public string? JobTitle { get; set; }
        public IEnumerable<CategoryDTO>? Categories { get; set; }

        public bool IsFollowed { get; set; }

        public IEnumerable<RoleDTO>? Roles { get; set; }

    }
}
