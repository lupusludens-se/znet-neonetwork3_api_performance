using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Models.Search;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Controllers
{
    /// <summary>
    /// Controller for handling search-related API requests.
    /// </summary>
    [Authorize, Active]
    [ApiController]
    [Route("api/search")]
    public class SearchContoller : ControllerBase
    {
        private readonly ISearchApiService _searchApiService;
        private readonly ILogger<SearchContoller> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchContoller"/> class.
        /// </summary>
        /// <param name="searchApiService">The search API service.</param>
        /// <param name="logger">The logger instance.</param>
        public SearchContoller(
            ISearchApiService searchApiService,
            ILogger<SearchContoller> logger
            )
        {
            _searchApiService = searchApiService;
            _logger = logger;
        }

        /// <summary>
        /// Performs a search based on the provided filter.
        /// </summary>
        /// <param name="filter">The search filter.</param>
        /// <returns>An <see cref="IActionResult"/> containing the search results.</returns>
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] GlobalSearchFilter filter)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            if (filter.EntityType == AzureSearchEntityType.Project && currentUser.RoleIds.Any(rId => rId == (int)RoleType.SolutionProvider || rId == (int)RoleType.SPAdmin))
            {
                return BadRequest();
            }

            FacetWrapperModel<SearchDocument> searchResult = await _searchApiService.SearchAsync(filter, currentUser.Id);

            return Ok(searchResult);
        }
    }
}