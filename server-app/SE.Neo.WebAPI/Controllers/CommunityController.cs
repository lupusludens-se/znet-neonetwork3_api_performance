using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SE.Neo.Common.Models.Community;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;
namespace SE.Neo.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class CommunityController : ControllerBase
    {
        private readonly ICommunityApiService _communityApiService;
        public CommunityController(ICommunityApiService communityApiService)
        {
            _communityApiService = communityApiService;
        }
        /// <summary>
        /// Get community members
        /// </summary>
        /// <remarks>
        /// Search -&gt; Name <br/>
        /// FilterBy -&gt; regionids=1,2&amp;industryids=1&amp;companytype=1; 
        /// OnlyFollowed: boolean for only returning followed users/companies <br/>
        /// ForYou: boolean to return users/companies based on matching region and category of user <br/>
        /// CommunityItemType: pass 0 to return only companies, 1 to return only users CommunityItemType=1; <br/>
        /// OrderBy -&gt; string for sort order, "asc" or "desc". Ascending is default
        /// </remarks>
        [HttpGet("community")]
        [Active]
        public async Task<IActionResult> GetCommunity([FromQuery] CommunityFilter filter)
        {
            var currentUser = (UserModel)HttpContext.Items["User"];
            var communityItems = await _communityApiService.GetCommunityAsync(currentUser.Id, filter);
            return Ok(communityItems);
        }

        /// <summary>
        /// Get network statistics
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves the network statistics without requiring authorization.
        /// </remarks>
        /// <returns>
        /// Returns an IActionResult containing the network statistics.
        /// </returns>
        [HttpGet("network-stats")]
        [Active(skipAuthorize: true)]
        public async Task<IActionResult> GetNetworkStats()
        {
            var networkStats = await _communityApiService.GetNetworkStats();
            return Ok(networkStats);
        }
    }
}
