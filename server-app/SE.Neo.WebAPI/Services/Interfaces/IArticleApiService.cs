using SE.Neo.Common.Models;
using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Models.Article;
using SE.Neo.WebAPI.Models.User;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface IArticleApiService
    {
        Task<WrapperModel<ArticleResponse>> GetArticlesAsync(BaseSearchFilterModel filter, UserModel currentUser);

        Task<ArticleResponse?> GetArticleAsync(int id, UserModel currentUser, string? expand);

        Task SyncContentsAsync();

        Task CreateArticleTrendingAsync(int userId, int articleId);

        Task<WrapperModel<ArticleResponse>> GetArticleTrendingsAsync(PaginationModel filter, UserModel currentUser);

        bool IsArticleExist(int articleId);
        Task<WrapperModel<NewAndNoteworthyArticlesResponse>> GetNewAndNoteworthyArticlesAsync(UserModel currentUser);
        Task<ArticleResponse?> GetPublicInitiativeArticleContent();
    }
}