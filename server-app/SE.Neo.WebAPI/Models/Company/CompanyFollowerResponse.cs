using SE.Neo.Common.Models.User;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Models.UserProfile;

namespace SE.Neo.WebAPI.Models.Company
{
    public class CompanyFollowerResponse
    {
        public int Id { get; set; }

        public UserFollowerResponse Follower { get; set; }
    }
}
