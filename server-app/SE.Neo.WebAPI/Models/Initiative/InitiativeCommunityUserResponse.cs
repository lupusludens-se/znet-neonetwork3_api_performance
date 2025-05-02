using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Media;
using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Models.CMS;
using SE.Neo.WebAPI.Models.Company;
using SE.Neo.WebAPI.Models.Media;
using SE.Neo.WebAPI.Models.User;

namespace SE.Neo.WebAPI.Models.Initiative
{
    public class InitiativeCommunityUserResponse
    {
        public int Id { get; set; }
        public int InitiativeId { get; set; }
        public int TypeId { get; set; }

        public string JobTitle { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CompanyName { get; set; }

        public string? ImageName { get; set; }

        public BlobResponse? Image { get; set; }

        public IEnumerable<RoleDTO> Roles { get; set; }
        public int TagsTotalCount { get; set; }
        public IEnumerable<CategoryResponse> Categories { get; set; }
        public bool? IsNew { get; set; } = false;
    }
}
