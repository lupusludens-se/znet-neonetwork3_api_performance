using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Configs;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Entities.CMS;
using SE.Neo.Core.Services.Interfaces;
using System.Text;

namespace SE.Neo.Core.Services
{
    public partial class ArticleService : BaseService, IArticleService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ArticleService> _logger;
        protected readonly IDistributedCache _cache;
        private readonly MemoryCacheTimeStamp _memoryCacheTimeStamp;
        private readonly ICommonService _commonService;

        public ArticleService(ApplicationContext context, IMapper mapper,
            IOptions<MemoryCacheTimeStamp> memoryCacheTimeStamp, IDistributedCache cache, ICommonService commonService, ILogger<ArticleService> logger) : base(cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
            _memoryCacheTimeStamp = memoryCacheTimeStamp.Value;
            _commonService = commonService;
            _logger = logger;
        }

        public async Task<WrapperModel<ArticleDTO>> GetArticlesAsync(BaseSearchFilterModel filter, int userId, List<int> roleIds)
        {
            var articlesQueryable = ExpandSortArticles(_context.Articles.Where(s => !s.IsDeleted).AsNoTracking(), filter.Expand, filter.OrderBy);
            bool isAdmin = false;

            articlesQueryable = FilterSearchArticles(articlesQueryable, filter.Search, filter.FilterBy, userId);
            if (roleIds?.Any() == true)
            {
                isAdmin = roleIds.Contains((int)Common.Enums.RoleType.Admin);
                articlesQueryable = articlesQueryable.Where(s => s.ArticleRoles.Any(r => roleIds.Contains(r.RoleId)) || isAdmin);
            }
            else if (userId == 0 && !string.IsNullOrEmpty(filter.Search))
            {
                articlesQueryable = articlesQueryable.OrderByDescending(x => x.IsPublicArticle).ThenBy(x => x.Title);
            }
            else
            {
                articlesQueryable = articlesQueryable.OrderByDescending(a => a.ModifiedOn);
            }

            if (filter.Random.HasValue)
            {
                articlesQueryable = articlesQueryable.OrderBy(_ => Guid.NewGuid()).Take(filter.Random.Value);
            }

            int count = 0;
            IEnumerable<Article> articles = Enumerable.Empty<Article>();

            //Previous code
            if (string.IsNullOrEmpty(filter.Search) && articlesQueryable != null)
            {
                if (filter.IncludeCount)
                {
                    count = await articlesQueryable.CountAsync();
                    if (count == 0)
                    {
                        return new WrapperModel<ArticleDTO> { Count = count, DataList = new List<ArticleDTO>() };
                    }
                }

                if (filter.Skip.HasValue)
                {
                    articlesQueryable = articlesQueryable.Skip(filter.Skip.Value);
                }

                if (filter.Take.HasValue)
                {
                    articlesQueryable = articlesQueryable.Take(filter.Take.Value);
                }
                articles = !filter.Random.HasValue ? await articlesQueryable.AsSplitQuery().ToListAsync() : await articlesQueryable.ToListAsync();

            }
            //Code fix for NNG-492
            else
            {
                articles = !filter.Random.HasValue ? await articlesQueryable.AsSplitQuery().ToListAsync() : await articlesQueryable.ToListAsync();

                if (userId == 0)
                {
                    articles = articles.Where(x => x.Title.ToLowerInvariant().Contains(filter.Search.ToLowerInvariant())
                    || HTMLExtensions.RemoveAllHTML(x.Content).ToLowerInvariant().Contains(filter.Search.ToLowerInvariant()) ||
                    x.ArticleTechnologies.Any(s => s.Technology.Name.ToLower().Contains(filter.Search.ToLower())) ||
                    x.ArticleSolutions.Any(s => s.Solution.Name.ToLower().Contains(filter.Search.ToLower())) ||
                    x.ArticleCategories.Any(s => s.Category.Name.ToLower().Contains(filter.Search.ToLower())) ||
                    x.ArticleContentTags.Any(s => s.ContentTag.Name.ToLower().Contains(filter.Search.ToLower())));
                }
                else
                {
                    articles = articles.Where(x => x.Title.ToLowerInvariant().Contains(filter.Search.ToLowerInvariant())
                    || HTMLExtensions.RemoveAllHTML(x.Content).ToLowerInvariant().Contains(filter.Search.ToLowerInvariant()));
                }

                if (filter.IncludeCount)
                {
                    count = articles.Count();
                    if (count == 0)
                    {
                        return new WrapperModel<ArticleDTO> { Count = count, DataList = new List<ArticleDTO>() };
                    }
                }

                if (filter.Skip.HasValue)
                {
                    articles = articles.Skip(filter.Skip.Value);
                }

                if (filter.Take.HasValue)
                {
                    articles = articles.Take(filter.Take.Value);
                }
            }

            List<ArticleDTO> articleDTOs = articles.Select(_mapper.Map<ArticleDTO>).ToList();

            foreach (var article in articleDTOs)
            {
                if (userId > 0)
                {
                    article.IsSaved = await IsArticleSavedAsync(article.Id, userId);
                }
                else
                {
                    article.Content = String.Empty;
                }
            }

            return new WrapperModel<ArticleDTO> { Count = count, DataList = articleDTOs };
        }

        public async Task<ArticleDTO?> GetArticleAsync(int id, int userId, List<int> roleIds, string? expand = null)
        {
            var articlesQueryable = ExpandSortArticles(_context.Articles.AsNoTracking().AsSplitQuery(), expand);

            bool isAdmin = roleIds.Contains((int)Common.Enums.RoleType.Admin);

            var article = await articlesQueryable.FirstOrDefaultAsync(userId != 0 ? (p => p.Id == id && !p.IsDeleted
                && (p.ArticleRoles.Any(r => roleIds.Contains(r.RoleId)) || isAdmin)) :
                (p => p.Id == id && !p.IsDeleted && p.IsPublicArticle)
                );

            ArticleDTO? articleDTO = _mapper.Map<ArticleDTO>(article);

            if (articleDTO != null && userId > 0)
                articleDTO.IsSaved = await IsArticleSavedAsync(article.Id, userId);

            return articleDTO;
        }

        public async Task<string> GetUserArticlesParameterAsync(int userId, bool isDashboard)
        {
            StringBuilder result = new StringBuilder();

            var contentLists = await _context.UserProfileCategories.AsNoTracking()
                .Where(s => s.UserProfile.UserId.Equals(userId)).Select(s => "categories[]=" + s.CategoryId + "&").ToListAsync();
            if (contentLists.Any())
                result.Append(string.Join("", contentLists));

            if (isDashboard)
            {
                List<int> userProfileRegions = await _commonService.GetRegionListForUserProfile(userId, true, false, true);
                contentLists = userProfileRegions.Select(s => "regions[]=" + s + "&").ToList();

                if (contentLists.Any())
                    result.Append(string.Join("", contentLists));
            }

            if (result.Length == 0)
                return string.Empty;

            return result.ToString().Remove(result.Length - 1);
        }

        #region Data Sync

        public async Task SyncTaxonomiesAsync(TaxonomyDTO taxonomies)
        {
            if (taxonomies.categories.Any())
                await HandleTaxonomiesAsync(taxonomies.categories, _context.Categories, "dbo.CMS_Category");

            if (taxonomies.solutions.Any())
                await HandleTaxonomiesAsync(taxonomies.solutions, _context.Solutions, "dbo.CMS_Solution");

            if (taxonomies.technologies.Any())
                await HandleTaxonomiesAsync(taxonomies.technologies, _context.Technologies, "dbo.CMS_Technology");

            if (taxonomies.regions.Any())
                await HandleRegionTaxonomiesAsync(taxonomies.regions, _context.Regions, "dbo.CMS_Region");

            if (taxonomies.contenttags.Any())
                await HandleTaxonomiesAsync(taxonomies.contenttags, _context.ContentTags, "dbo.CMS_Content_Tag");
        }

        public async Task SyncArticlesAsync(IEnumerable<ArticleDTO> modelDTOs)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var roleList = await _cache.GetRecordAsync<List<Role>>(CoreCacheKeys.RoleContext);
                if (roleList is null)
                {
                    roleList = await _context.Roles.ToListAsync();
                    await _cache.SetRecordAsync(CoreCacheKeys.RoleContext, roleList, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_memoryCacheTimeStamp.Long)
                    });
                }
                try
                {
                    List<int> articleIdsOnlyPublicFieldChanged = new List<int>();
                    // List of Articles that are in DB
                    IEnumerable<Article> entities = await _context.Articles
                        .Where(s => modelDTOs.Select(es => es.Id).Contains(s.Id))
                        .ToListAsync();

                    // Undelete articles in DB
                    entities.Where(s => s.IsDeleted).ToList().ForEach(e => e.IsDeleted = false);

                    // List of articles to insert
                    var articlesDTOsToInsert = modelDTOs
                        .Where(s => !entities.Where(w => !w.IsDeleted).Select(es => es.Id).Contains(s.Id));

                    IEnumerable<Article> articlesToInsert = articlesDTOsToInsert.Select(_mapper.Map<Article>);

                    if (articlesToInsert.Any())
                    {
                        _context.Articles.AddRange(articlesToInsert);
                        articlesDTOsToInsert.ToList().ForEach(async articlesDTOToInsert =>
                        {
                            await HandleTaxonomiesForArticleAsync(articlesDTOToInsert, roleList);
                        });
                    }
                    IList<int> modifiedArticleIds = new List<int>();

                    //Modified Article Ids
                    var articlesToUpdate = (from dbArticle in entities
                                            join wpArticle in modelDTOs on dbArticle.Id equals wpArticle.Id
                                            where dbArticle.Modified < wpArticle.Modified
                                            select dbArticle.Id).ToList();
                    IEnumerable<Article> modifiedArticleEntities = await _context.Articles
                        .Where(s => articlesToUpdate.Contains(s.Id))
                        .Include(s => s.ArticleRoles).ThenInclude(s => s.Role)
                        .Include(s => s.ArticleCategories)
                        .Include(s => s.ArticleRegions)
                        .Include(s => s.ArticleSolutions)
                        .Include(s => s.ArticleTechnologies)
                        .Include(s => s.ArticleContentTags)
                        .ToListAsync();

                    // Update articles info from WP model
                    modelDTOs.ToList().ForEach(async modelDTO =>
                    {

                        Article? item = null;
                        if (modifiedArticleEntities != null && modifiedArticleEntities.Count() > 0)
                        {
                            if (articlesToUpdate.Contains(modelDTO.Id))
                            {
                                var entityDB = modifiedArticleEntities.FirstOrDefault(x => x.Id == modelDTO.Id);
                                var postFromDB = new
                                {
                                    title = entityDB.Title,
                                    description = entityDB.Content,
                                    pdfUrl = entityDB.PdfUrl,
                                    imageUrl = entityDB.ImageUrl,
                                    videoUrl = entityDB.VideoUrl,
                                    typeId = entityDB.TypeId,
                                    regionsId = entityDB.ArticleRegions?.Select(x => x.RegionId)?.OrderBy(y => y)?.ToList() ?? new List<int>(),
                                    categories = entityDB.ArticleCategories?.Select(x => x.CategoryId)?.OrderBy(y => y)?.ToList() ?? new List<int>(),
                                    rolesId = entityDB.ArticleRoles?.Select(x => x.Role?.CMSRoleId)?.OrderBy(y => y)?.ToList(),
                                    solutions = entityDB.ArticleSolutions?.Select(x => x.SolutionId)?.OrderBy(y => y)?.ToList() ?? new List<int>(),
                                    technologies = entityDB.ArticleTechnologies?.Select(x => x.TechnologyId)?.OrderBy(y => y)?.ToList() ?? new List<int>(),
                                    contentTags = entityDB.ArticleContentTags?.Select(x => x.ContentTagId)?.OrderBy(y => y)?.ToList() ?? new List<int>()
                                };

                                var postFromDBString = JsonConvert.SerializeObject(postFromDB);

                                var postFromWP = new
                                {
                                    title = modelDTO.Title,
                                    description = modelDTO.Content,
                                    pdfUrl = modelDTO.PdfUrl,
                                    imageUrl = modelDTO.ImageUrl,
                                    videoUrl = modelDTO.VideoUrl,
                                    typeId = modelDTO.TypeId,
                                    regionsId = modelDTO.Regions?.Select(x => x.Id)?.OrderBy(y => y)?.ToList() ?? new List<int>(),
                                    categories = modelDTO.Categories?.Select(x => x.Id)?.OrderBy(y => y)?.ToList() ?? new List<int>(),
                                    rolesId = modelDTO.Roles?.Select(x => x.CMSRoleId)?.OrderBy(y => y)?.ToList(),
                                    solutions = modelDTO.Solutions?.Select(x => x.Id)?.OrderBy(y => y)?.ToList() ?? new List<int>(),
                                    technologies = modelDTO.Technologies?.Select(x => x.Id)?.OrderBy(y => y)?.ToList() ?? new List<int>(),
                                    contentTags = modelDTO.ContentTags?.Select(x => x.Id)?.OrderBy(y => y)?.ToList() ?? new List<int>()
                                };

                                var postFromWPString = JsonConvert.SerializeObject(postFromWP);
                                _logger.LogInformation($"String from Database  : {postFromDBString} ");
                                _logger.LogInformation($"String from WordPress : {postFromWPString}");

                                if (postFromDBString == postFromWPString && entityDB.IsPublicArticle != modelDTO.IsPublicArticle)
                                {
                                    item = modifiedArticleEntities.Where(i => i.Id.Equals(modelDTO.Id)).FirstOrDefault();
                                    articleIdsOnlyPublicFieldChanged.Add(item.Id);
                                }

                            }

                        }

                        if (item == null)
                        {
                            item = entities.Where(i =>
                                   i.Id.Equals(modelDTO.Id) && i.Modified < modelDTO.Modified).FirstOrDefault();

                            if (item != null)
                            {
                                _mapper.Map(modelDTO, item);
                                await HandleTaxonomiesForArticleAsync(modelDTO, roleList);
                            }

                        }
                    });

                    // Delete missing in WP model articles from DB
                    (await _context.Articles.Where(s => !modelDTOs.Select(es => es.Id).Contains(s.Id))
                        .ToListAsync()).ForEach(e => e.IsDeleted = true);

                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.CMS_Article ON;");
                    await _context.SaveChangesAsync();
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.CMS_Article OFF;");

                    transaction.Commit();

                    if (articleIdsOnlyPublicFieldChanged.Count > 0)
                    {
                        var idsParameter = string.Join(",", articleIdsOnlyPublicFieldChanged);
                        int updatedRowsCount = await _context.Database.ExecuteSqlRawAsync("[dbo].[sp_UpdateArticlePublicField] @IDList ",
                                            new SqlParameter("@IDList", idsParameter));
                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError($"Error occured while Data Sync in Article Service while syncing Articles. Error :{ex.InnerException} ");
                    throw;
                }
            }
        }
        #endregion Data Sync

        #region Trending

        public async Task CreateArticleTrendingAsync(int userId, int articleId)
        {
            if (_context.Articles.SingleOrDefault(b => b.Id == articleId) == null)
                throw new CustomException($"{CoreErrorMessages.ErrorOnSaving} Article does not exist.");

            try
            {
                var model = new ArticleView { UserId = userId, ArticleId = articleId };
                _context.ArticleViews.Add(model);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
            }
        }

        public async Task<WrapperModel<ArticleDTO>> GetArticleTrendingsAsync(PaginationModel filter, int userId, List<int> roleIds)
        {
            bool isAdmin = false;

            List<int> articleIds = new List<int>();

            if (roleIds?.Any() == true)
            {
                isAdmin = roleIds.Contains((int)Common.Enums.RoleType.Admin);
                articleIds = _context.ArticleViews
             .Where(t => t.CreatedOn.Value >= DateTime.UtcNow.AddDays(-7) && !t.Article.IsDeleted
                 && (t.Article.ArticleRoles.Any(r => roleIds.Contains(r.RoleId)) || isAdmin)).GroupBy(p => p.ArticleId)
                 .Select(g => new { id = g.Key, count = g.Count() }).OrderByDescending(o => o.count).Select(s => s.id).ToList();
            }
            else
            {
                articleIds = _context.ArticleViews
            .Where(t => t.CreatedOn.Value >= DateTime.UtcNow.AddDays(-7) && !t.Article.IsDeleted).GroupBy(p => p.ArticleId)
                .Select(g => new { id = g.Key, count = g.Count() }).OrderByDescending(o => o.count).Select(s => s.id).ToList();
            }

            IQueryable<Article> articlesQueryable = _context.Articles.Where(s => articleIds.Contains(s.Id)).AsNoTracking();

            articlesQueryable = articlesQueryable.AsNoTracking().Include(p => p.ArticleCategories).ThenInclude(dc => dc.Category)
                    .Include(p => p.ArticleRegions).ThenInclude(dc => dc.Region)
                    .Include(p => p.ArticleSolutions).ThenInclude(dc => dc.Solution)
                    .Include(p => p.ArticleTechnologies).ThenInclude(dc => dc.Technology)
                    .Include(p => p.ArticleContentTags).ThenInclude(dc => dc.ContentTag);
            List<Article> articles = new List<Article>();
            try
            {
                articles = (await articlesQueryable.AsSplitQuery().ToListAsync()).OrderBy(x => articleIds.IndexOf(x.Id)).ToList();
            }
            catch (Exception ex)
            {
                articles = (await articlesQueryable.ToListAsync()).OrderBy(x => articleIds.IndexOf(x.Id)).ToList();
                _logger.LogError($"Error while loading trending for the userid: {userId}. Error: {ex.Message}");
                _logger.LogError($"Error while loading trending for the userid: {userId}. Error Inner Exception Message: {ex.InnerException?.Message}");
            }

            int count = 0;
            if (filter.IncludeCount)
            {
                count = articles.Count;
                if (count == 0)
                {
                    return new WrapperModel<ArticleDTO> { Count = count, DataList = new List<ArticleDTO>() };
                }
            }
            if (filter.Skip.HasValue)
            {
                articles = articles.Skip(filter.Skip.Value).ToList();
            }

            if (filter.Take.HasValue)
            {
                articles = articles.Take(filter.Take.Value).ToList();
            }
            List<ArticleDTO> articleDTOs = articles.Select(_mapper.Map<ArticleDTO>).ToList();
            foreach (var article in articleDTOs)
            {
                if (userId > 0)
                {
                    article.IsSaved = await IsArticleSavedAsync(article.Id, userId);
                }
                else
                {
                    article.Content = String.Empty;
                }
            }
            return new WrapperModel<ArticleDTO> { Count = count, DataList = articleDTOs };
        }

        #endregion Trending

        #region NewAndNoteworthy
        public async Task<List<ArticleNewAndNoteworthyDTO>> GetRecentArticles(int userId, int count, List<int> roleIds)
        {
            bool isAdmin = roleIds.Contains((int)Common.Enums.RoleType.Admin);
            IQueryable<int> userProfileRegions = _context.UserProfileRegions
                  .Where(upr => upr.UserProfile.UserId == userId)
                  .Select(x => x.RegionId);

            IQueryable<int> userProfileCategories = _context.UserProfileCategories
                .Where(upc => upc.UserProfile.UserId == userId)
                .Select(x => x.CategoryId);

            var articles = await _context.Articles.Where(a => !a.IsDeleted &&
                                          (a.ArticleRoles.Any(r => roleIds.Contains(r.RoleId)) || isAdmin) &&
                                          (a.ArticleRegions.Any(dr => userProfileRegions.Contains(dr.RegionId)) ||
                                          a.ArticleCategories.Any(dr => userProfileCategories.Contains(dr.CategoryId))))
                                          .OrderByDescending(a => a.CreatedOn)
                                          .ThenByDescending(f => f.ArticleRegions.Any(dr => userProfileRegions.Contains(dr.RegionId))
                                                            && f.ArticleCategories.Any(dr => userProfileCategories.Contains(dr.CategoryId)))
                        .ThenByDescending(f => f.ArticleRegions.Any(dr => userProfileRegions.Contains(dr.RegionId)))
                        .ThenByDescending(f => f.ArticleCategories.Any(dr => userProfileCategories.Contains(dr.CategoryId))).Take(count).ToListAsync();

            List<ArticleNewAndNoteworthyDTO> articleDTOs = articles.Select(_mapper.Map<ArticleNewAndNoteworthyDTO>).ToList();

            return articleDTOs;
        }

        /// <summary>
        /// Get Articles based on the most clicks in the last 7 days where either User Region or Category should match. If count is same 
        /// then priorty should be given in the mentioned 1. Both Region and Category should match 2. Region Match 3. Category Match
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="count"></param>
        /// <param name="newArticleIdsList"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ArticleNewAndNoteworthyDTO>> GetTrendingProjectForDashboard(int userId, int count, List<int> newArticleIdsList, List<int> roleIds)
        {
            bool isAdmin = roleIds.Contains((int)Common.Enums.RoleType.Admin);
            IQueryable<int> userProfileRegions = _context.UserProfileRegions
                  .Where(upr => upr.UserProfile.UserId == userId)
                  .Select(x => x.RegionId);

            IQueryable<int> userProfileCategories = _context.UserProfileCategories
                .Where(upc => upc.UserProfile.UserId == userId)
                .Select(x => x.CategoryId);

            List<int> articleIds = await _context.ArticleViews.Where(t => t.CreatedOn.Value >= DateTime.UtcNow.AddDays(-7) &&
                                                                     !newArticleIdsList.Contains(t.Article.Id) &&
                                                                     (t.Article.ArticleRoles.Any(r => roleIds.Contains(r.RoleId)) || isAdmin) &&
                                                                     !t.Article.IsDeleted &&
                                                                     (t.Article.ArticleRegions.Any(dr => userProfileRegions.Contains(dr.RegionId)) ||
                                                                      t.Article.ArticleCategories.Any(dr => userProfileCategories.Contains(dr.CategoryId))
                                                                     )).GroupBy(p => p.ArticleId)
                                                                .Select(g => new { id = g.Key, count = g.Count() }).OrderByDescending(o => o.count)
                                                               .ThenBy(m => (userProfileRegions.Join(_context.ArticleRegions, reg => reg, ar => ar.RegionId, (reg, ar) => ar.ArticleId == m.id).Any() &&
                                                                userProfileCategories.Join(_context.ArticleCategories, cat => cat, ac => ac.CategoryId, (cat, ac) => ac.ArticleId == m.id).Any() ? 1 :
                                                                userProfileRegions.Join(_context.ArticleRegions, reg => reg, ar => ar.RegionId, (reg, ar) => ar.ArticleId == m.id).Any() ? 2 :
                                                                userProfileCategories.Join(_context.ArticleCategories, cat => cat, ac => ac.CategoryId, (cat, ac) => ac.ArticleId == m.id).Any() ? 3 : -1))
                                                                .Select(s => s.id).Take(count).ToListAsync();


            IEnumerable<ArticleNewAndNoteworthyDTO> articles = (await _context.Articles.Where(s => articleIds.Contains(s.Id)).Select(t => new ArticleNewAndNoteworthyDTO
            {
                Id = t.Id,
                Title = t.Title
            }).ToListAsync()).OrderBy(pId => articleIds.IndexOf(pId.Id));

            return articles;

        }
        #region Public Dashboard
        public async Task<List<ArticleNewAndNoteworthyDTO>> GetPublicArticles(int count)
        {
            var articles = await _context.Articles.Where(a => !a.IsDeleted && a.IsPublicArticle).ToListAsync();
            var shuffledPublicArticles = ShuffleList(articles).Take(count).ToList();
            List<ArticleNewAndNoteworthyDTO> articleDTOs = shuffledPublicArticles.Select(_mapper.Map<ArticleNewAndNoteworthyDTO>).ToList();
            return articleDTOs;
        }
        public async Task<IEnumerable<ArticleNewAndNoteworthyDTO>> GetPrivateArticles(int count, int Limit)
        {
            List<int> articleIds = await _context.ArticleViews.Where(t =>
                                                                     !t.Article.IsDeleted && !t.Article.IsPublicArticle).GroupBy(p => p.ArticleId).
                                                                OrderByDescending(o => o.Count()).Select(g => new { id = g.Key, count = g.Count() })
                                                                                                                   .Select(s => s.id).Take(Limit).ToListAsync();
            var shuffledPrivateArticles = ShuffleList(articleIds).Take(count).ToList();
            IEnumerable<ArticleNewAndNoteworthyDTO> articles = (await _context.Articles.Where(s => shuffledPrivateArticles.Contains(s.Id)).Select(t => new ArticleNewAndNoteworthyDTO
            {
                Id = t.Id,
                Title = t.Title,
                IsPublicArticle = t.IsPublicArticle
            }).ToListAsync()).OrderBy(pId => articleIds.IndexOf(pId.Id));

            return articles;
        }
        List<T> ShuffleList<T>(List<T> list)
        {
            Random _random = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
        #endregion
        #endregion NewAndNoteworthy

        #region Validation

        public bool IsArticleExist(int articleId)
        {
            return _context.Articles.AsNoTracking().Any(m => m.Id == articleId && !m.IsDeleted);
        }

        #endregion Validation
        public async Task<ArticleDTO?> GetPublicInitiativeArticleContent(string searchArticleByTitle)
        {
            var article = await _context.Articles.Where(a => !a.IsDeleted &&  a.IsPublicArticle && a.Title.ToLower() == searchArticleByTitle.ToLower())
                            .OrderByDescending(x => x.CreatedByUserId).FirstOrDefaultAsync();
            ArticleDTO? articleDTO = _mapper.Map<ArticleDTO>(article);
            return articleDTO;

        }
    }
}
