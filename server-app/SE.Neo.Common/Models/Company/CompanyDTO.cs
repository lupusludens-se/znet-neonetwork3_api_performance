using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Media;
using SE.Neo.Common.Models.Project;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;

namespace SE.Neo.Common.Models.Company
{
    public class CompanyDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public int TypeId { get; set; }

        public string TypeName { get; set; }

        public string? ImageLogo { get; set; }

        public BlobDTO? Image { get; set; }

        public string CompanyUrl { get; set; }

        public string LinkedInUrl { get; set; }

        public string About { get; set; }

        public int CountryId { get; set; }

        public CountryDTO Country { get; set; }

        public string MDMKey { get; set; }

        public int IndustryId { get; set; }

        public string IndustryName { get; set; }

        public bool IsFollowed { get; set; }

        public int FollowersCount { get; set; }

        public IEnumerable<ProjectDTO> Projects { get; set; }

        public IEnumerable<UserDTO> Users { get; set; }

        public IEnumerable<CompanyUrlLinkDTO> UrlLinks { get; set; }

        public IEnumerable<CategoryDTO> Categories { get; set; }

        public IEnumerable<BaseIdNameDTO> OffsitePPAs { get; set; }

        public IEnumerable<CompanyFollowerDTO> Followers { get; set; }
    }
}