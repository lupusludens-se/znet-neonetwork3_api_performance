using SE.Neo.Common.Models.Media;
using SE.Neo.Common.Models.User;
using SE.Neo.WebAPI.Models.CMS;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.State;
using SE.Neo.WebAPI.Models.User;

namespace SE.Neo.WebAPI.Models.UserProfile
{
    public class UserProfileResponse
    {
        public int UserId { get; set; }

        public UserResponse? User { get; set; }

        public string JobTitle { get; set; }

        public string LinkedInUrl { get; set; }

        public string About { get; set; }

        public int? StateId { get; set; }

        public StateResponse? State { get; set; }

        public int? ResponsibilityId { get; set; }

        public string? ResponsibilityName { get; set; }

        public bool IsFollowed { get; set; }

        public int FollowersCount { get; set; }

        public ICollection<CategoryResponse> Categories { get; set; }

        public ICollection<RegionResponse> Regions { get; set; }

        public ICollection<UrlLinkModel> UrlLinks { get; set; }

        public IEnumerable<UserFollowerResponse> Followers { get; set; }

        public ICollection<SkillsByCategoryModel>SkillsByCategory { get; set; }
    }
}