using SE.Neo.WebAPI.Models.Activity;

namespace SE.Neo.WebAPI.Models.User
{
    public class UserPendingAddWithActivityRequest
    {
        public UserPendingAddRequest SignUpData { get; set; }
        public ActivityRequest ActivityData { get; set; }
    }
}
