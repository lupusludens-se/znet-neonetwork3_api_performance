using SE.Neo.Common.Models.Saved;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface ISavedContentService
    {
        Task<WrapperModel<SavedContentItemDTO>> GetSavedContentAsync(int userId, SavedContentFilter filter);

        Task AddProjectToSavedAsync(ProjectSavedDTO model);

        Task RemoveProjectFromSavedAsync(ProjectSavedDTO model);

        Task AddArticleToSavedAsync(ArticleSavedDTO model);

        Task RemoveArticleFromSavedAsync(ArticleSavedDTO model);

        Task RemoveArticleFromAllSavedAsync(ArticleSavedDTO model);

        Task<int?> AddForumToSavedAsync(ForumSavedDTO model, bool isAdminUser = false);

        Task RemoveForumFromSavedAsync(ForumSavedDTO model);

        Task<int> GetSavedArticlesCountAsync(int userId, SavedContentFilter filter);

        Task<int> GetSavedProjectsCountAsync(int userId, SavedContentFilter filter);

        Task<int> GetSavedForumsCountAsync(int userId, SavedContentFilter filter);
    }
}