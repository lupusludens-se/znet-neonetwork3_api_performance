using SE.Neo.Common.Models.User;

namespace SE.Neo.Common.Models.Company
{
    public class CompanyFollowerDTO
    {
        public int Id { get; set; }

        public int FollowerId { get; set; }

        public UserDTO Follower { get; set; }

        public int CompanyId { get; set; }
    }
}
