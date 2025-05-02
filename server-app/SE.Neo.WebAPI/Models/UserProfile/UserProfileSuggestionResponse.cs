using SE.Neo.WebAPI.Models.Shared;

namespace SE.Neo.WebAPI.Models.UserProfile
{
    public class UserProfileSuggestionResponse
    {
        public IEnumerable<SuggestionModel>? Suggestions { get; set; }
    }
}
