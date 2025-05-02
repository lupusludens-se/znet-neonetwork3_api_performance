using SE.Neo.Common.Models.Saved;
using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Models.Saved;
using SE.Neo.WebAPI.Models.User;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface ISavedContentApiService
    {
        Task<WrapperModel<SavedContentItemResponse>> GetSavedContentAsync(int userId, SavedContentFilter filter);

        Task AddProjectToSavedAsync(int userId, ProjectSavedRequest model);

        Task RemoveProjectFromSavedAsync(int userId, ProjectSavedRequest model);

        Task AddArticleToSavedAsync(int userId, ArticleSavedRequest model);

        Task RemoveArticleFromSavedAsync(int userId, ArticleSavedRequest model);

        Task<int?> AddForumToSavedAsync(UserModel user, ForumSavedRequest model);

        Task RemoveForumFromSavedAsync(int userId, ForumSavedRequest model);

        Task<UserSavedContentCountersResponse> GetCurrentUserSavedContentCountAsync(int userId, SavedContentFilter filter);
    }
}
