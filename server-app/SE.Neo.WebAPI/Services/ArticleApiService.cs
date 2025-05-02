using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using SE.Neo.Common;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Configs;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;
using SE.Neo.WebAPI.Constants;
using SE.Neo.WebAPI.Models.Article;
using SE.Neo.WebAPI.Models.CMS;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;
using System.Text.Json;
using SE.Neo.Infrastructure.Configs;
namespace SE.Neo.WebAPI.Services
{
    public class ArticleApiService : IArticleApiService
    {
        private readonly IArticleService _articleService;
        private readonly IWordPressContentService _wordPressService;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly ISavedContentService _savedContentService;
        private readonly MemoryCacheTimeStamp _memoryCacheTimeStamp;
        private const string FilterForYou = "foryou";
        private readonly ILogger<ArticleApiService> _logger;
        private readonly PublicPlatformConfiguration _publicPlatformConfiguration;
        public ArticleApiService(
            IWordPressContentService wordPressService,
            IArticleService articleService,
            IMapper mapper,
            IDistributedCache cache,
            ISavedContentService savedContentService,
            IOptions<MemoryCacheTimeStamp> memoryCacheTimeStamp,
            ILogger<ArticleApiService> logger,
            IOptions<PublicPlatformConfiguration> publicPlatformConfiguration)
        {
            _wordPressService = wordPressService;
            _articleService = articleService;
            _mapper = mapper;
            _cache = cache;
            _savedContentService = savedContentService;
            _memoryCacheTimeStamp = memoryCacheTimeStamp.Value;
            _logger = logger;
            _publicPlatformConfiguration = publicPlatformConfiguration.Value;
        }

        public async Task<WrapperModel<ArticleResponse>> GetArticlesAsync(BaseSearchFilterModel filter, UserModel currentUser)
        {
            int userId = currentUser?.Id ?? 0;
            WrapperModel<ArticleDTO> articles = await _articleService.GetArticlesAsync(filter, userId, currentUser?.RoleIds);
            var wrapper = new WrapperModel<ArticleResponse>
            {
                Count = articles.Count,
                DataList = articles.DataList.Select(_mapper.Map<ArticleResponse>)
            };

            return wrapper;
        }

        public async Task<ArticleResponse?> GetArticleAsync(int id, UserModel currentUser, string? expand)
        {
            var modelDTO = await _articleService.GetArticleAsync(id, currentUser?.Id ?? 0, currentUser?.RoleIds ?? new List<int>(), expand);
            return _mapper.Map<ArticleResponse>(modelDTO);
        }

        #region DataSync

        public async Task SyncContentsAsync()
        {
            var cacheTimeStamp = await _cache.GetRecordAsync<int>(CacheKeys.CMSSyncDate);

            _ = int.TryParse(await _wordPressService.GetTaxonomyUpdateDate(), out int cacheCMSTimeStamp);

            // Sync taxonomies if there was changes on WP side.
            if (cacheCMSTimeStamp > cacheTimeStamp || cacheCMSTimeStamp == 0)
            {
                _cache.Remove(CoreCacheKeys.RegionContext);
                _cache.Remove(CoreCacheKeys.SolutionContext);
                _cache.Remove(CoreCacheKeys.CategoryContext);
                _cache.Remove(CoreCacheKeys.TechnologyContext);

                await SyncTaxonomiesAsync();

                await _cache.SetRecordAsync(CacheKeys.CMSSyncDate, cacheCMSTimeStamp, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_memoryCacheTimeStamp.Long)
                });
            }
            // Sync articles.
            await SyncArticlesAsync();
        }

        private async Task SyncTaxonomiesAsync()
        {
            try
            {
                string sortAndFilterParameter = "per_page=100&page=1&order=asc&orderby=id";

                string serviseResult = await _wordPressService.GetContentFromCMS("categories?" + sortAndFilterParameter);

                _logger.LogInformation("wordpress categories in json format" + serviseResult);

                List<CategoryResponse> jsonCategories =
                        JsonSerializer.Deserialize<List<CategoryResponse>>(serviseResult);

                serviseResult = await _wordPressService.GetContentFromCMS("solutions?" + sortAndFilterParameter);

                _logger.LogInformation("wordpress solutions in json format" + serviseResult);

                List<SolutionResponse> jsonSolutions =
                    JsonSerializer.Deserialize<List<SolutionResponse>>(serviseResult);

                serviseResult = await _wordPressService.GetContentFromCMS("technologies?" + sortAndFilterParameter);

                _logger.LogInformation("wordpress technologies in json format" + serviseResult);

                List<TechnologyResponse> jsonTechnologies =
                    JsonSerializer.Deserialize<List<TechnologyResponse>>(serviseResult);

                serviseResult = await _wordPressService.GetContentFromCMS("contenttags?" + sortAndFilterParameter);

                List<ContentTagResponse> jsonContentTags =
                    JsonSerializer.Deserialize<List<ContentTagResponse>>(serviseResult);

                TaxonomyRequest taxonomyRequest =
                    new()
                    {
                        categories = jsonCategories,
                        solutions = jsonSolutions,
                        technologies = jsonTechnologies,
                        contenttags = jsonContentTags
                    };

                // Get Region Count from CMS
                serviseResult = await _wordPressService.GetContentFromCMS("taxonomies");

                TaxonomyTypeModel jsonRegionType =
                    JsonSerializer.Deserialize<TaxonomyTypeModel>(serviseResult);


                List<RegionResponse> jsonRegions = new List<RegionResponse>();
                // Check range of available regions to sync
                int regionPagesCount = (int)Math.Ceiling((decimal)jsonRegionType.Regions.Count / 100);

                for (int i = 1; i <= regionPagesCount; i++)
                {
                    serviseResult = await _wordPressService.GetContentFromCMS("regions?per_page=100&page=" + i + "&order=asc&orderby=id");
                    jsonRegions.AddRange(JsonSerializer.Deserialize<List<RegionResponse>>(serviseResult));
                }
                taxonomyRequest.regions = jsonRegions;

                TaxonomyDTO modelDTO = _mapper.Map<TaxonomyDTO>(taxonomyRequest);
                await _articleService.SyncTaxonomiesAsync(modelDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while Data Sync in Article API Service while syncing Taxonomies. Error :{ex.InnerException} ");
                throw;
            }
        }

        private async Task SyncArticlesAsync()
        {
            try
            {
                string environment = Environment.GetEnvironmentVariable("APP_ENV") ?? "tst";

                // TODO: Rewrite Api Articles to Decorator
                var countUrl = "posts?get_count=1&status=publish";
                if (environment.GetValueFromDescription<EnvironmentEnum>() == EnvironmentEnum.Dev ||
                    environment.GetValueFromDescription<EnvironmentEnum>() == EnvironmentEnum.PreProd)
                {
                    countUrl += ",draft";
                }
                int articleCount = int.Parse(await _wordPressService.GetContentFromCMS(countUrl));

                // Check range of available articles to sync
                int articlePagesCount = (int)Math.Ceiling((decimal)articleCount / 100);

                List<ArticleCMSDTO> jsonArticles = new List<ArticleCMSDTO>();

                for (int i = 1; i <= articlePagesCount; i++)
                {
                    var postUrl = "posts?per_page=100&page=" + i + "&order=asc&orderby=id&status=publish";
                    if (environment.GetValueFromDescription<EnvironmentEnum>() == EnvironmentEnum.Dev ||
                        environment.GetValueFromDescription<EnvironmentEnum>() == EnvironmentEnum.PreProd)
                    {
                        postUrl += ",draft";
                    }
                    string serviseResult = await _wordPressService.GetContentFromCMS(postUrl);
                    jsonArticles.AddRange(JsonSerializer.Deserialize<List<ArticleCMSDTO>>(serviseResult));
                }

                await _articleService.SyncArticlesAsync(jsonArticles.Select(_mapper.Map<ArticleDTO>));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while Data Sync in Article API Service. Error :{ex.InnerException} ");
                throw;
            }
        }

        #endregion DataSync

        #region Trending

        public async Task CreateArticleTrendingAsync(int userId, int articleId)
        {
            bool exist = _articleService.IsArticleExist(articleId);
            if (!exist)
                await SyncContentsAsync();

            await _articleService.CreateArticleTrendingAsync(userId, articleId);
        }

        public async Task<WrapperModel<ArticleResponse>> GetArticleTrendingsAsync(PaginationModel filter, UserModel currentUser)
        {
            int userId = currentUser != null ? currentUser.Id : 0;
            WrapperModel<ArticleDTO> articles = await _articleService.GetArticleTrendingsAsync(filter, userId, currentUser?.RoleIds);
            var wrapper = new WrapperModel<ArticleResponse>
            {
                Count = articles.Count,
                DataList = articles.DataList.Select(_mapper.Map<ArticleResponse>)
            };
            return wrapper;
        }

        #endregion Trending

        #region NewAndNoteworthy
        public async Task<WrapperModel<NewAndNoteworthyArticlesResponse>> GetNewAndNoteworthyArticlesAsync(UserModel currentUser)
        {
            int newArticleCountInDashboard = 3, trendingArticleCountInDashboard = 3;
            int trendingCount = 0;
            List<ArticleNewAndNoteworthyDTO> trendingArticleResultDTO = new List<ArticleNewAndNoteworthyDTO>();
            List<NewAndNoteworthyArticlesResponse> result = new List<NewAndNoteworthyArticlesResponse>();
            if (currentUser != null)
            {
                List<ArticleNewAndNoteworthyDTO> topThreeRecentArticles = await _articleService.GetRecentArticles(currentUser.Id, newArticleCountInDashboard, currentUser.RoleIds);
                List<int> topNewAndNoteworthyArticlesId = topThreeRecentArticles.Select(t => t.Id).ToList();

                List<ArticleNewAndNoteworthyDTO> trendingArticles = _articleService.GetTrendingProjectForDashboard(currentUser.Id, trendingArticleCountInDashboard, topNewAndNoteworthyArticlesId, currentUser.RoleIds).Result.ToList();

                if (trendingArticles != null && (trendingArticles.Count >= trendingArticleCountInDashboard))
                {
                    trendingArticleResultDTO = trendingArticles.Take(3).ToList();
                }
                else
                {
                    if (trendingArticles != null && trendingArticles.Count > 0)
                    {
                        trendingArticleResultDTO.AddRange(trendingArticles);
                        trendingCount = trendingArticles.Count;
                        topNewAndNoteworthyArticlesId.AddRange(trendingArticles.Select(t => t.Id).ToList());
                    }
                    int remainingArticles = trendingArticleCountInDashboard - trendingCount;
                    BaseSearchFilterModel baseSearchFilterModel = new BaseSearchFilterModel()
                    {
                        FilterBy = FilterForYou
                    };
                    WrapperModel<ArticleDTO> articles = await _articleService.GetArticlesAsync(baseSearchFilterModel, currentUser.Id, currentUser.RoleIds);
                    IList<ArticleDTO> forYouAllArticleDTO = articles.DataList.Where(t => !topNewAndNoteworthyArticlesId.Contains(t.Id)).Take(remainingArticles).ToList();
                    List<ArticleNewAndNoteworthyDTO> forYouArticleDTO = forYouAllArticleDTO.Select(_mapper.Map<ArticleNewAndNoteworthyDTO>).ToList();

                    trendingArticleResultDTO.AddRange(forYouArticleDTO);
                }
                result.AddRange(GetNewAndNoteworthyArticleResponse(topThreeRecentArticles));
                result.AddRange(GetNewAndNoteworthyArticleResponse(trendingArticleResultDTO));
            }
            else
            {
                List<ArticleNewAndNoteworthyDTO> publicArticles = await _articleService.GetPublicArticles(ZnConstants.TotalPublicArticlesInDashboard);

                List<ArticleNewAndNoteworthyDTO> privateArticles = (await _articleService.GetPrivateArticles(ZnConstants.TotalPublicArticlesInDashboard, ZnConstants.PrivateArticlesLimit)).ToList();

                result.AddRange(GetNewAndNoteworthyArticleResponse(publicArticles));
                result.AddRange(GetNewAndNoteworthyArticleResponse(privateArticles));

            }
            var wrapper = new WrapperModel<NewAndNoteworthyArticlesResponse>
            {
                Count = result.Count,
                DataList = result
            };
            return wrapper;
        }

        public List<NewAndNoteworthyArticlesResponse> GetNewAndNoteworthyArticleResponse(List<ArticleNewAndNoteworthyDTO> articleDTOs)
        {
            List<NewAndNoteworthyArticlesResponse> result = articleDTOs.Select(_mapper.Map<NewAndNoteworthyArticlesResponse>).ToList();
            return result;
        }
        #endregion NewAndNoteworthy
        public bool IsArticleExist(int articleId)
        {
            return _articleService.IsArticleExist(articleId);
        }
        public async Task<ArticleResponse?> GetPublicInitiativeArticleContent()
        {
            var modelDTO = await _articleService.GetPublicInitiativeArticleContent(_publicPlatformConfiguration.DIGInitiativeSearchText);
            return _mapper.Map<ArticleResponse>(modelDTO);
        }
    }
}