using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;

namespace SE.Neo.Common.Models.UserProfile
{
    public class UserProfileDTO
    {
        public int UserId { get; set; }

        public string JobTitle { get; set; }

        public UserDTO? User { get; set; }

        public string LinkedInUrl { get; set; }

        public string About { get; set; }

        public int? StateId { get; set; }

        public StateDTO? State { get; set; }

        public bool IsFollowed { get; set; }

        public int FollowersCount { get; set; }

        public bool AcceptWelcomeSeriesEmail { get; set; }

        public int? ResponsibilityId { get; set; }

        public string ResponsibilityName { get; set; }

        public int? UserLoginCount { get; set; }

        public IEnumerable<CategoryDTO> Categories { get; set; }

        public IEnumerable<RegionDTO> Regions { get; set; }

        public IEnumerable<UserProfileUrlLinkDTO> UrlLinks { get; set; }
        public IEnumerable<UserFollowerDTO> Followers { get; set; }
        public IEnumerable<SkillsByCategoryDTO> SkillsByCategory { get; set; }
    }
}