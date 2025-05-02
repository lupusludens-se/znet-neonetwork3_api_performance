using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Saved;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Entities.CMS;
using SE.Neo.Core.Models.Saved;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services
{
    public class SavedContentService : ISavedContentService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<SavedContentService> _logger;
        private readonly IMapper _mapper;
        protected readonly IDistributedCache _cache;

        public SavedContentService(ApplicationContext context, ILogger<SavedContentService> logger, IMapper mapper, IDistributedCache cache)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<WrapperModel<SavedContentItemDTO>> GetSavedContentAsync(int userId, SavedContentFilter filter)
        {
            var query = CreateGetQuery(userId, filter.Type, filter.Search);
            query = query.OrderByDescending(x => x.CreatedOn);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await query.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<SavedContentItemDTO> { Count = count, DataList = new List<SavedContentItemDTO>() };
                }
            }

            if (filter.Skip.HasValue)
            {
                query = query.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                query = query.Take(filter.Take.Value);
            }

            var savedContentItems = await query.ToListAsync();
            if (!savedContentItems.Any())
            {
                return new WrapperModel<SavedContentItemDTO> { Count = count, DataList = new List<SavedContentItemDTO>() };
            }

            var projectsIds = savedContentItems.Where(x => x.Type == SavedContentType.Project).Select(x => x.Id);
            List<Project> projects = new List<Project>();
            if (projectsIds.Any())
            {
                projects.AddRange(await _context.Projects
                    .Include(x => x.Regions).ThenInclude(x => x.Region)
                    .Include(x => x.Technologies).ThenInclude(x => x.Technology)
                    .Include(x => x.Category)
                    .Where(x => projectsIds.Contains(x.Id))
                    .ToListAsync());
            }

            var forumsIds = savedContentItems.Where(x => x.Type == SavedContentType.Forum).Select(x => x.Id);
            List<Discussion> forums = new List<Discussion>();
            if (forumsIds.Any())
            {
                forums.AddRange(await _context.Discussions
                    .Include(p => p.Messages)
                    .Include(p => p.DiscussionCategories).ThenInclude(dc => dc.Category)
                    .Include(p => p.DiscussionRegions).ThenInclude(dc => dc.Region)
                    .Where(x => forumsIds.Contains(x.Id))
                    .ToListAsync());
            }

            var articlesIds = savedContentItems.Where(x => x.Type == SavedContentType.Article).Select(x => x.Id);
            List<Article> articles = new List<Article>();
            if (articlesIds.Any())
            {
                articles.AddRange(await _context.Articles
                    .Include(p => p.ArticleCategories).ThenInclude(dc => dc.Category)
                    .Include(p => p.ArticleRegions).ThenInclude(dc => dc.Region)
                    .Include(p => p.ArticleSolutions).ThenInclude(dc => dc.Solution)
                    .Include(p => p.ArticleTechnologies).ThenInclude(dc => dc.Technology)
                    .Where(x => articlesIds.Contains(x.Id))
                    .ToListAsync());
            }

            var savedContentItemDTOs = new List<SavedContentItemDTO>();
            foreach (var item in savedContentItems)
            {
                switch (item.Type)
                {
                    case SavedContentType.Article:
                        var article = articles.FirstOrDefault(x => x.Id == item.Id);
                        savedContentItemDTOs.Add(_mapper.Map<SavedContentItemDTO>(article));
                        break;

                    case SavedContentType.Project:
                        var project = projects.FirstOrDefault(x => x.Id == item.Id);
                        savedContentItemDTOs.Add(_mapper.Map<SavedContentItemDTO>(project));
                        break;

                    case SavedContentType.Forum:
                        var forum = forums.FirstOrDefault(x => x.Id == item.Id);
                        savedContentItemDTOs.Add(_mapper.Map<SavedContentItemDTO>(forum));
                        break;

                    default:
                        break;
                }
            }

            return new WrapperModel<SavedContentItemDTO> { Count = count, DataList = savedContentItemDTOs };
        }

        public async Task AddProjectToSavedAsync(ProjectSavedDTO model)
        {
            var savedProject = await _context.SavedProjects.SingleOrDefaultAsync(sp => sp.UserId == model.UserId && sp.ProjectId == model.ProjectId);
            if (savedProject == null)
            {
                await _context.SavedProjects.AddAsync(_mapper.Map<ProjectSaved>(model));

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new CustomException(string.Format(CoreErrorMessages.SavedContentAlreadySaved, "Project"));
            }
        }

        public async Task AddArticleToSavedAsync(ArticleSavedDTO model)
        {
            var savedArticle = await _context.SavedArticles.SingleOrDefaultAsync(st => st.UserId == model.UserId && st.ArticleId == model.ArticleId);
            if (savedArticle == null)
            {
                await _context.SavedArticles.AddAsync(_mapper.Map<ArticleSaved>(model));

                await _context.SaveChangesAsync();

                _cache.Remove(CoreCacheKeys.ArticleSavedContext + model.UserId);
            }
            else
            {
                throw new CustomException(string.Format(CoreErrorMessages.SavedContentAlreadySaved, "Article"));
            }
        }

        public async Task<int?> AddForumToSavedAsync(ForumSavedDTO model, bool isAdminUser = false)
        {
            var forum = await _context.Discussions.Include(ud => ud.DiscussionUsers).AsNoTracking().SingleOrDefaultAsync(d => d.Id == model.ForumId && !d.IsDeleted);
            if (forum == null)
            {
                _logger.LogWarning("User {UserId} can not add forum {ForumId} to saved. Forum not found, private or deleted.", model.UserId, model.ForumId);
                return null;
            }

            if (!isAdminUser)
            {
                if (forum.Type == DiscussionType.PrivateChat || (forum.Type == DiscussionType.PrivateForum &&
                    !forum.DiscussionUsers.Any(du => du.UserId == model.UserId)))
                {
                    _logger.LogWarning("User {UserId} can not add forum {ForumId} to saved. Forum not found, private or deleted.", model.UserId, model.ForumId);
                    return null;
                }
            }

            var savedForum = await _context.SavedForums.SingleOrDefaultAsync(st => st.UserId == model.UserId && st.DiscussionId == model.ForumId);
            if (savedForum == null)
            {
                await _context.SavedForums.AddAsync(_mapper.Map<DiscussionSaved>(model));

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new CustomException(string.Format(CoreErrorMessages.SavedContentAlreadySaved, "Forum"));
                return null;
            }
            return model.ForumId;
        }

        public async Task RemoveProjectFromSavedAsync(ProjectSavedDTO model)
        {
            var savedProject = await _context.SavedProjects.SingleOrDefaultAsync(sp => sp.UserId == model.UserId && sp.ProjectId == model.ProjectId);
            if (savedProject != null)
            {
                _context.SavedProjects.Remove(savedProject);

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new CustomException(string.Format(CoreErrorMessages.SavedContentAlreadyDeleted, "Project"));
            }
        }

        public async Task RemoveArticleFromSavedAsync(ArticleSavedDTO model)
        {
            var savedArticle = await _context.SavedArticles.SingleOrDefaultAsync(st => st.UserId == model.UserId && st.ArticleId == model.ArticleId);
            if (savedArticle != null)
            {
                _context.SavedArticles.Remove(savedArticle);

                await _context.SaveChangesAsync();

                _cache.Remove(CoreCacheKeys.ArticleSavedContext + model.UserId);
            }
            else
            {
                throw new CustomException(string.Format(CoreErrorMessages.SavedContentAlreadyDeleted, "Article"));
            }
        }

        public async Task RemoveForumFromSavedAsync(ForumSavedDTO model)
        {
            var savedForum = await _context.SavedForums.SingleOrDefaultAsync(st => st.UserId == model.UserId && st.DiscussionId == model.ForumId);
            if (savedForum != null)
            {
                _context.SavedForums.Remove(savedForum);

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new CustomException(string.Format(CoreErrorMessages.SavedContentAlreadyDeleted, "Forum"));
            }
        }

        public async Task RemoveArticleFromAllSavedAsync(ArticleSavedDTO model)
        {
            _context.SavedArticles.RemoveRange(
                _context.SavedArticles.Where(s => s.ArticleId == model.ArticleId));

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetSavedArticlesCountAsync(int userId, SavedContentFilter filter)
        {
            var query = CreateGetQuery(userId, SavedContentType.Article, filter.Search);

            return await query.CountAsync();
        }

        public async Task<int> GetSavedProjectsCountAsync(int userId, SavedContentFilter filter)
        {
            var query = CreateGetQuery(userId, SavedContentType.Project, filter.Search);

            return await query.CountAsync();
        }

        public async Task<int> GetSavedForumsCountAsync(int userId, SavedContentFilter filter)
        {
            var query = CreateGetQuery(userId, SavedContentType.Forum, filter.Search);

            return await query.CountAsync();
        }

        private IQueryable<SavedContentItem> CreateGetQuery(int userId, SavedContentType? type, string? search)
        {
            var savedProjectsQuery = _context.SavedProjects.AsNoTracking()
                .Where(sp => sp.UserId == userId && sp.Project.StatusId == Enums.ProjectStatus.Active
                    && (!string.IsNullOrEmpty(search) ? sp.Project.Title.ToLower().Contains(search.ToLower()) : true))
                .Select(x => new SavedContentItem()
                {
                    Id = x.ProjectId,
                    CreatedOn = x.CreatedOn,
                    Type = SavedContentType.Project,
                });
            var savedArticlesQuery = _context.SavedArticles.AsNoTracking()
                .Where(sp => sp.UserId == userId && !sp.Article.IsDeleted
                    && (!string.IsNullOrEmpty(search) ? sp.Article.Title.ToLower().Contains(search.ToLower()) : true))
                .Select(x => new SavedContentItem
                {
                    Id = x.ArticleId,
                    CreatedOn = x.CreatedOn,
                    Type = SavedContentType.Article,
                });
            var savedForumsQuery = _context.SavedForums.AsNoTracking()
                .Where(sp => sp.UserId == userId && !sp.Discussion.IsDeleted && (sp.Discussion.Type == DiscussionType.PublicForum || sp.Discussion.Type == DiscussionType.PrivateForum)
                    && (!string.IsNullOrEmpty(search) ? sp.Discussion.Subject.ToLower().Contains(search.ToLower()) : true))
                .Select(x => new SavedContentItem
                {
                    Id = x.DiscussionId,
                    CreatedOn = x.CreatedOn,
                    Type = SavedContentType.Forum,
                });

            if (!type.HasValue)
            {
                return savedProjectsQuery
                    .Union(savedArticlesQuery)
                    .Union(savedForumsQuery);
            }
            else
            {
                return type switch
                {
                    SavedContentType.Project => savedProjectsQuery,
                    SavedContentType.Article => savedArticlesQuery,
                    SavedContentType.Forum => savedForumsQuery,
                    _ => throw new NotSupportedException()
                };
            }
        }
    }
}