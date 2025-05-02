using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SE.Neo.Common.Models;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Models.Article;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Controllers
{
    [Authorize, Active]
    [ApiController]
    [Route("api")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleApiService _articleApiService;
        private readonly ILogger<ArticleController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleController"/> class.
        /// </summary>
        /// <param name="articleApiService">The article API service.</param>
        /// <param name="logger">The logger instance.</param>
        public ArticleController(IArticleApiService articleApiService, ILogger<ArticleController> logger)
        {
            _articleApiService = articleApiService;
            _logger = logger;
        }

        /// <summary>
        /// Get list of Articles.
        /// </summary>
        /// <remarks>
        /// Search: Title and Content. <br/>
        /// FilterBy: foryou, saved,
        ///           solutionids=1,2&amp;categoryids=1&amp;technologyids=1&amp;regionids=1 <br/>
        /// OrderBy: id.asc, id.desc, date.asc, date.desc, modified.asc, modified.desc. <br/> Expand
        /// values: categories, regions, solutions, technologies.
        /// </remarks>
        /// <param name="filter">The filter model for searching and filtering articles.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of articles.</returns>
        [HttpGet]
        [Route("articles")]
        [Active(skipAuthorize: true)]
        public async Task<IActionResult> GetArticles([FromQuery] BaseSearchFilterModel filter)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            WrapperModel<ArticleResponse> result = await _articleApiService.GetArticlesAsync(filter, currentUser);

            return Ok(result);
        }

        /// <summary>
        /// Get Article By Id.
        /// </summary>
        /// <param name="id">Unique identifier of the article.</param>
        /// <param name="expand">Possible values: categories, regions, solutions, technologies.</param>
        /// <returns>An <see cref="IActionResult"/> containing the article.</returns>
        [HttpGet]
        [Route("articles/{id}")]
        [Active(skipAuthorize: true)]
        public async Task<IActionResult> GetArticle(int id, string? expand)
        {
            var currentUser = (UserModel)Request.HttpContext.Items["User"]!;
            var article = await _articleApiService.GetArticleAsync(id, currentUser, expand);

            return article != null ? Ok(article) : currentUser == null ? StatusCode(401) : NotFound();
        }

        /// <summary>
        /// Sync CMS articles and taxonomies.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPost]
        [Permissions(PermissionType.DataSync)]
        [Route("articles/sync")]
        public async Task<IActionResult> SyncContents()
        {
            await _articleApiService.SyncContentsAsync();
            return Ok();
        }

        /// <summary>
        /// Marks an article as trending.
        /// </summary>
        /// <param name="id">The article ID.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPost("articles/{id}/trendings")]
        public async Task<IActionResult> PostArticleTrending(int id)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            await _articleApiService.CreateArticleTrendingAsync(currentUser.Id, id);
            return Ok(id);
        }

        /// <summary>
        /// Get list of article trendings.
        /// </summary>
        /// <param name="filter">The pagination model for filtering trendings.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of trending articles.</returns>
        [HttpGet("articles/trendings")]
        [Active(skipAuthorize: true)]
        public async Task<IActionResult> GetArticleTrendings([FromQuery] PaginationModel filter)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            WrapperModel<ArticleResponse> articles = await _articleApiService.GetArticleTrendingsAsync(filter, currentUser);
            return Ok(articles);
        }

        /// <summary>
        /// Get list of new and noteworthy articles.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of new and noteworthy articles.</returns>
        [Active(skipAuthorize: true)]
        [HttpGet("articles/newandnoteworthy")]
        public async Task<IActionResult> GetNewAndNoteworthyArticles()
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            WrapperModel<NewAndNoteworthyArticlesResponse> result = await _articleApiService.GetNewAndNoteworthyArticlesAsync(currentUser);
            return Ok(result);
        }

        [Active(skipAuthorize: true)]
        [HttpGet("articles/get-public-initiatiative-article-content")]
        public async Task<IActionResult> GetInitiativeArticle()
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            if (currentUser == null)
            {
                var result = await _articleApiService.GetPublicInitiativeArticleContent();
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
