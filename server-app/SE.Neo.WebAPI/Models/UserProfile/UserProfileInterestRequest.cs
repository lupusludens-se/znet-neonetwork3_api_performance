using SE.Neo.WebAPI.Models.CMS;

namespace SE.Neo.WebAPI.Models.UserProfile
{
    public class UserProfileInterestRequest
    {
        public IEnumerable<CategoryRequest>? Categories { get; set; }

        public IEnumerable<RegionRequest>? Regions { get; set; }
    }
}
