using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Models.UserProfile;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface IUserProfileApiService
    {
        Task<WrapperModel<UserProfileResponse>> GetUserProfilesAsync(BaseSearchFilterModel filter, int userId);

        Task<UserProfileResponse?> GetUserProfileAsync(int id, int userId, string? expand);

        Task<int> CreateUpdateUserProfileAsync(int id, UserProfileRequest userViewModel, string? ConsentIp = "", string? ConsentUserAgent = "", bool? isEditCurrentProfile=false);

        Task CreateUserProfileInterestAsync(int userId, UserProfileInterestRequest viewModel);

        Task<UserProfileSuggestionResponse> GetUserProfileSuggestionsAsync(int userId);

        Task<int> SyncUserLoginCountAsync();
    }
}