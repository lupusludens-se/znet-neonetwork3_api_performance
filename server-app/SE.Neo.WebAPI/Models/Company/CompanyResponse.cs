using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Media;
using SE.Neo.WebAPI.Models.CMS;
using SE.Neo.WebAPI.Models.Country;
using SE.Neo.WebAPI.Models.Project;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Models.UserProfile;

namespace SE.Neo.WebAPI.Models.Company
{
    public class CompanyResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public int TypeId { get; set; }

        public string TypeName { get; set; }

        public string ImageLogo { get; set; }

        public BlobDTO? Image { get; set; }

        public string CompanyUrl { get; set; }

        public string LinkedInUrl { get; set; }

        public string About { get; set; }

        public string MDMKey { get; set; }

        public int IndustryId { get; set; }

        public string IndustryName { get; set; }

        public int CountryId { get; set; }

        public CountryResponse Country { get; set; }

        public bool IsFollowed { get; set; }

        public int FollowersCount { get; set; }

        public IEnumerable<ProjectResponse> Projects { get; set; }

        public IEnumerable<UserResponse> Users { get; set; }

        public IEnumerable<UrlLinkModel> UrlLinks { get; set; }

        public IEnumerable<BaseIdNameResponse> OffsitePPAs { get; set; }

        public IEnumerable<CategoryResponse> Categories { get; set; }

        public IEnumerable<CompanyFollowerResponse> Followers { get; set; }
    }
}