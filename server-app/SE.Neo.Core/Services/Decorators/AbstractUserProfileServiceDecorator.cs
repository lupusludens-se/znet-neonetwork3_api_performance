using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.UserProfile;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services.Decorators
{
    public abstract class AbstractUserProfileServiceDecorator : IUserProfileService
    {
        protected readonly IUserProfileService _userProfileService;

        public AbstractUserProfileServiceDecorator(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        public virtual async Task<int> CreateUpdateUserProfileAsync(int id, UserProfileDTO modelDTO, bool? isEditCurrentProfile=false)
        {
            return await _userProfileService.CreateUpdateUserProfileAsync(id, modelDTO, isEditCurrentProfile);
        }

        public virtual async Task CreateUserProfileInterestAsync(int userId, TaxonomyDTO modelDTO)
        {
            await _userProfileService.CreateUserProfileInterestAsync(userId, modelDTO);
        }

        public virtual async Task<UserProfileDTO?> GetUserProfileAsync(int id, int userId, string? expand)
        {
            return await _userProfileService.GetUserProfileAsync(id, userId, expand);
        }

        public virtual async Task<WrapperModel<UserProfileDTO>> GetUserProfilesAsync(BaseSearchFilterModel filter, int userId)
        {
            return await _userProfileService.GetUserProfilesAsync(filter, userId);
        }

        public virtual async Task<UserProfileSuggestionDTO> GetUserProfileSuggestionsAsync(int userId)
        {
            return await _userProfileService.GetUserProfileSuggestionsAsync(userId);
        }
        public async Task<int> SyncUserLoginCountAsync()
        {
            return await _userProfileService.SyncUserLoginCountAsync();
        }
    }
}
