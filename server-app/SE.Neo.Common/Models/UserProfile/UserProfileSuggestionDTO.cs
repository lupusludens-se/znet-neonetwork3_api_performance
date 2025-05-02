using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Common.Models.UserProfile
{
    public class UserProfileSuggestionDTO
    {
        public IEnumerable<SuggestionDTO> Suggestions { get; set; }
    }
}