using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Saved;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Models.Saved;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Services
{
    public class SavedContentApiService : ISavedContentApiService
    {
        private readonly ILogger<SavedContentApiService> _logger;
        private readonly IMapper _mapper;
        private readonly ISavedContentService _savedContentService;
        private readonly IArticleService _articleService;
        private readonly IArticleApiService _articleApiService;

        public SavedContentApiService(ILogger<SavedContentApiService> logger,
            IMapper mapper,
            ISavedContentService savedService,
            IArticleService articleService,
            IArticleApiService articleApiService)
        {
            _logger = logger;
            _mapper = mapper;
            _savedContentService = savedService;
            _articleService = articleService;
            _articleApiService = articleApiService;
        }

        public async Task AddProjectToSavedAsync(int userId, ProjectSavedRequest model)
        {
            var projectSavedDTO = _mapper.Map<ProjectSavedDTO>(model);
            projectSavedDTO.UserId = userId;
            await _savedContentService.AddProjectToSavedAsync(projectSavedDTO);
        }

        public async Task AddArticleToSavedAsync(int userId, ArticleSavedRequest model)
        {
            var articleSavedDTO = _mapper.Map<ArticleSavedDTO>(model);
            articleSavedDTO.UserId = userId;
            await _savedContentService.AddArticleToSavedAsync(articleSavedDTO);
        }

        public async Task<int?> AddForumToSavedAsync(UserModel user, ForumSavedRequest model)
        {
            bool isAdminUser = user.RoleIds.Any(r => r.Equals((int)RoleType.Admin));
            var forumSavedDTO = _mapper.Map<ForumSavedDTO>(model);
            forumSavedDTO.UserId = user.Id;
            return await _savedContentService.AddForumToSavedAsync(forumSavedDTO, isAdminUser);
        }

        public async Task RemoveProjectFromSavedAsync(int userId, ProjectSavedRequest model)
        {
            var projectSavedDTO = _mapper.Map<ProjectSavedDTO>(model);
            projectSavedDTO.UserId = userId;
            await _savedContentService.RemoveProjectFromSavedAsync(projectSavedDTO);
        }

        public async Task RemoveArticleFromSavedAsync(int userId, ArticleSavedRequest model)
        {
            var articleSavedDTO = _mapper.Map<ArticleSavedDTO>(model);
            articleSavedDTO.UserId = userId;
            await _savedContentService.RemoveArticleFromSavedAsync(articleSavedDTO);
        }

        public async Task RemoveForumFromSavedAsync(int userId, ForumSavedRequest model)
        {
            var forumSavedDTO = _mapper.Map<ForumSavedDTO>(model);
            forumSavedDTO.UserId = userId;
            await _savedContentService.RemoveForumFromSavedAsync(forumSavedDTO);
        }

        public async Task<WrapperModel<SavedContentItemResponse>> GetSavedContentAsync(int userId, SavedContentFilter filter)
        {
            WrapperModel<SavedContentItemDTO> savedItemsResult = await _savedContentService.GetSavedContentAsync(userId, filter);

            return new WrapperModel<SavedContentItemResponse>()
            {
                Count = savedItemsResult.Count,
                DataList = savedItemsResult.DataList.Select(_mapper.Map<SavedContentItemResponse>)
            };
        }

        public async Task<UserSavedContentCountersResponse> GetCurrentUserSavedContentCountAsync(int userId, SavedContentFilter filter)
        {
            int articlesCount = await _savedContentService.GetSavedArticlesCountAsync(userId, filter);
            int projectsCount = await _savedContentService.GetSavedProjectsCountAsync(userId, filter);
            int forumsCount = await _savedContentService.GetSavedForumsCountAsync(userId, filter);

            return new UserSavedContentCountersResponse
            {
                ArticlesCount = articlesCount,
                ProjectsCount = projectsCount,
                ForumsCount = forumsCount
            };
        }
    }
}