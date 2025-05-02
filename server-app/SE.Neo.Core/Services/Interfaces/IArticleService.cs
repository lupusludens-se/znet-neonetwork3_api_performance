using SE.Neo.Common.Models;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface IArticleService
    {
        Task<WrapperModel<ArticleDTO>> GetArticlesAsync(BaseSearchFilterModel filter, int userId, List<int> roleIds);

        Task<ArticleDTO?> GetArticleAsync(int id, int userId, List<int> roleIds, string? expand = null);

        Task<string> GetUserArticlesParameterAsync(int userId, bool isDashboard);

        Task SyncTaxonomiesAsync(TaxonomyDTO modelDTO);

        Task SyncArticlesAsync(IEnumerable<ArticleDTO> modelDTO);

        Task CreateArticleTrendingAsync(int userId, int articleId);

        Task<WrapperModel<ArticleDTO>> GetArticleTrendingsAsync(PaginationModel filter, int userId, List<int> roleIds);

        bool IsArticleExist(int articleId);
        Task<List<ArticleNewAndNoteworthyDTO>> GetRecentArticles(int userId, int count, List<int> roleIds);
        Task<IEnumerable<ArticleNewAndNoteworthyDTO>> GetTrendingProjectForDashboard(int userId, int count, List<int> recentArticleIdsList, List<int> roleIds);
        /// <summary>
        /// Function to Get the any 2 public articles on the dashboard
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<List<ArticleNewAndNoteworthyDTO>> GetPublicArticles(int count);
        Task<IEnumerable<ArticleNewAndNoteworthyDTO>> GetPrivateArticles(int count, int Limit);
        Task<ArticleDTO?> GetPublicInitiativeArticleContent(string searchArticleByTitle);
    }
}