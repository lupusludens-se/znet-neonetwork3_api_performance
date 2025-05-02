using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SE.Neo.Common.Models.Saved;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Constants;
using SE.Neo.WebAPI.Models.Saved;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Controllers
{
    [Authorize, Active]
    [ApiController]
    [Route("api/saved-content")]
    public class SavedContentController : ControllerBase
    {
        private readonly ISavedContentApiService _savedContentApiService;
        private readonly ILogger<SavedContentController> _logger;
        private readonly IArticleApiService _articleApiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SavedContentController"/> class.
        /// </summary>
        /// <param name="savedContentApiService">The service for handling saved content operations.</param>
        /// <param name="logger">The logger instance for logging operations.</param>
        /// <param name="articleApiService">The service for handling article operations.</param>
        public SavedContentController(
            ISavedContentApiService savedContentApiService,
            ILogger<SavedContentController> logger,
            IArticleApiService articleApiService
            )
        {
            _savedContentApiService = savedContentApiService;
            _logger = logger;
            _articleApiService = articleApiService;
        }

        /// <summary>
        /// Get list of the saved content by filters.
        /// </summary>
        /// <param name="filter">Defines filter parameters.</param>
        /// <returns>List of the saved content.</returns>
        [HttpGet]
        public async Task<IActionResult> GetSavedItemsAsync([FromQuery] SavedContentFilter filter)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            var items = await _savedContentApiService.GetSavedContentAsync(currentUser.Id, filter);

            return Ok(items);
        }

        /// <summary>
        /// Add project to saved.
        /// </summary>
        /// <param name="model">Project definition.</param>
        /// <returns>Ok result.</returns>
        [HttpPost("projects")]
        public async Task<IActionResult> AddProjectToSavedAsync(ProjectSavedRequest model)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            await _savedContentApiService.AddProjectToSavedAsync(currentUser.Id, model);

            return Ok(new ProjectSavedResponse() { ProjectId = model.ProjectId });
        }

        /// <summary>
        /// Remove project from saved.
        /// </summary>
        /// <param name="model">Project definition.</param>
        /// <returns>Ok result.</returns>
        [HttpDelete("projects/{projectId}")]
        public async Task<IActionResult> RemoveProjectFromSavedAsync([FromRoute] ProjectSavedRequest model)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            await _savedContentApiService.RemoveProjectFromSavedAsync(currentUser.Id, model);

            return Ok(new ProjectSavedResponse() { ProjectId = model.ProjectId });
        }

        /// <summary>
        /// Add article to saved.
        /// </summary>
        /// <param name="model">Article definition.</param>
        /// <returns>Ok result.</returns>
        [HttpPost("articles")]
        public async Task<IActionResult> AddArticleToSavedAsync(ArticleSavedRequest model)
        {
            bool exist = _articleApiService.IsArticleExist(model.ArticleId);
            if (!exist)
            {
                await _articleApiService.SyncContentsAsync();
                exist = _articleApiService.IsArticleExist(model.ArticleId);
                if (!exist)
                {
                    ModelState.AddModelError(nameof(model.ArticleId), ErrorMessages.CategoriesNotEmpty);
                    return UnprocessableEntity(new ValidationResponse(ModelState));
                }
            }

            var currentUser = (UserModel)HttpContext.Items["User"]!;

            await _savedContentApiService.AddArticleToSavedAsync(currentUser.Id, model);

            return Ok(new ArticleSavedResponse() { ArticleId = model.ArticleId });
        }

        /// <summary>
        /// Remove article from saved.
        /// </summary>
        /// <param name="model">Article definition.</param>
        /// <returns>Ok result.</returns>
        [HttpDelete("articles/{articleId}")]
        public async Task<IActionResult> RemoveArticleFromSavedAsync([FromRoute] ArticleSavedRequest model)
        {
            bool exist = _articleApiService.IsArticleExist(model.ArticleId);
            if (!exist)
            {
                ModelState.AddModelError(nameof(model.ArticleId), ErrorMessages.CategoriesNotEmpty);
                return UnprocessableEntity(new ValidationResponse(ModelState));
            }
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            await _savedContentApiService.RemoveArticleFromSavedAsync(currentUser.Id, model);

            return Ok(new ArticleSavedResponse() { ArticleId = model.ArticleId });
        }

        /// <summary>
        /// Add forum to saved.
        /// </summary>
        /// <param name="model">Forum definition.</param>
        /// <returns>Ok result.</returns>
        [HttpPost("forums")]
        public async Task<IActionResult> AddForumToSavedAsync(ForumSavedRequest model)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            int? forumId = await _savedContentApiService.AddForumToSavedAsync(currentUser, model);

            return forumId != null ? Ok(new ForumSavedResponse() { ForumId = forumId.Value }) : StatusCode(403);
        }

        /// <summary>
        /// Remove forum from saved.
        /// </summary>
        /// <param name="model">Forum definition.</param>
        /// <returns>Ok result.</returns>
        [HttpDelete("forums/{forumId}")]
        public async Task<IActionResult> RemoveForumFromSavedAsync([FromRoute] ForumSavedRequest model)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            await _savedContentApiService.RemoveForumFromSavedAsync(currentUser.Id, model);

            return Ok(new ForumSavedResponse() { ForumId = model.ForumId });
        }

        /// <summary>
        /// Get number of each type of the saved content.
        /// </summary>
        /// <param name="filter">Defines filter parameters.</param>
        /// <returns>Ok result.</returns>
        [HttpGet("/api/users/current/saved-content-counters")]
        public async Task<IActionResult> GetCurrentUserSavedContentCount([FromQuery] SavedContentFilter filter)
        {
            var currentUser = (UserModel)Request.HttpContext.Items["User"]!;

            UserSavedContentCountersResponse response = await _savedContentApiService.GetCurrentUserSavedContentCountAsync(currentUser.Id, filter);

            return Ok(response);
        }
    }
}