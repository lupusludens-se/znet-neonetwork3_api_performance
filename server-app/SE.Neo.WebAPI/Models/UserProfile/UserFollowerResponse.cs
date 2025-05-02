using SE.Neo.Common.Models.Media;

namespace SE.Neo.WebAPI.Models.UserProfile
{
    public class UserFollowerResponse
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string JobTitle { get; set; }

        public string ImageName { get; set; }

        public string Company { get; set; }

        public BlobDTO? Image { get; set; }
        public bool isFollowed { get; set; }
    }
}