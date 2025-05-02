using SE.Neo.WebAPI.Models.Country;

namespace SE.Neo.WebAPI.Models.User
{
    public class UserPendingItemResponse : UserPendingResponse
    {
        public string Email { get; set; }

        public int CountryId { get; set; }

        public CountryResponse Country { get; set; }

        public int HeardViaId { get; set; }

        public string HeardViaName { get; set; }

        public string AdminComments { get; set; }
        public string JoiningInterestDetails { get; set; }
    }
}
