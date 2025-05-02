using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.UserProfile;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task<WrapperModel<UserProfileDTO>> GetUserProfilesAsync(BaseSearchFilterModel filter, int userId);

        Task<UserProfileDTO?> GetUserProfileAsync(int id, int userId, string? expand);

        Task<int> CreateUpdateUserProfileAsync(int id, UserProfileDTO modelDTO, bool? isEditCuurentProfile = false);

        Task CreateUserProfileInterestAsync(int userId, TaxonomyDTO modelDTO);

        Task<UserProfileSuggestionDTO> GetUserProfileSuggestionsAsync(int userId);

        Task<int> SyncUserLoginCountAsync();
    }
}