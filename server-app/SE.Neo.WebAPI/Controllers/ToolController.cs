using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Models.Tool;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Controllers
{
    [Authorize, Active]
    [ApiController]
    [Route("api")]
    public class ToolController : ControllerBase
    {
        private readonly IToolApiService _toolApiService;
        private readonly ILogger<ToolController> _logger;

        public ToolController(IToolApiService toolApiService, ILogger<ToolController> logger)
        {
            _toolApiService = toolApiService;
            _logger = logger;
        }

        /// <summary>
        /// Get Tool By Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="expand">roles, companies, icon</param>
        /// <returns></returns>
        [HttpGet("tools/{id}")]
        public async Task<IActionResult> GetTool(int id, string? expand)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            bool isToolViewAll = currentUser.PermissionIds.Contains((int)PermissionType.ToolManagement);
            if (_toolApiService.IsToolExist(id, isToolViewAll) == false)
            {
                return NotFound();
            }
            ToolResponse tool = await _toolApiService.GetToolAsync(id, expand, currentUser.Id, isToolViewAll);
            return tool != null ? Ok(tool) : StatusCode(403);
        }

        /// <summary>
        /// Get List of Tools.
        /// </summary>
        /// <remarks>
        /// Search -> Title
        /// <br />
        /// FilterBy -> IsActive
        /// <br />
        /// OrderBy -> title.asc, title.desc, status.asc, status.desc, pinned.asc, pinned.desc
        /// <br />
        /// Expand -> roles, companies, icon
        /// </remarks>
        [Active(skipAuthorize: true)]
        [HttpGet("tools")]
        public async Task<IActionResult> GetUserTools([FromQuery] BaseSearchFilterModel filter)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            int userId = currentUser?.Id ?? 0;
            bool isToolViewAll = (currentUser?.PermissionIds?.Contains((int)PermissionType.ToolManagement)) ?? false;
            WrapperModel<ToolResponse> tools = await _toolApiService.GetToolsAsync(filter, userId, isToolViewAll);
            return Ok(tools);
        }

        /// <summary>
        /// Create a new tool.
        /// </summary>
        /// <param name="model">The tool request model.</param>
        /// <returns>The ID of the created tool.</returns>
        [HttpPost("tools")]
        [Permissions(PermissionType.ToolManagement)]
        public async Task<IActionResult> PostTool(ToolRequest model)
        {
            int toolId = await _toolApiService.CreateUpdateToolAsync(0, model);
            return Ok(toolId);
        }

        /// <summary>
        /// Update an existing tool.
        /// </summary>
        /// <param name="id">The ID of the tool to update.</param>
        /// <param name="model">The tool request model.</param>
        /// <returns>The ID of the updated tool.</returns>
        [HttpPut("tools/{id}")]
        [Permissions(PermissionType.ToolManagement)]
        public async Task<IActionResult> PutToolAsync(int id, ToolRequest model)
        {
            int toolId = await _toolApiService.CreateUpdateToolAsync(id, model);
            return Ok(toolId);
        }

        /// <summary>
        /// Delete an existing tool.
        /// </summary>
        /// <param name="id">The ID of the tool to delete.</param>
        /// <returns>An Ok result.</returns>
        [Permissions(PermissionType.ToolManagement)]
        [HttpDelete("tools/{id}")]
        public async Task<IActionResult> DeleteTool(int id)
        {
            await _toolApiService.RemoveToolAsync(id);
            return Ok();
        }

        /// <summary>
        /// Patch a tool
        /// </summary>
        /// <remarks>
        /// JsonPatchDocument -> [{op: "replace", "path": "/IsActive", "value": true}]
        /// <br />
        /// Only path /IsActive currently allowed, do not pass contractResolver, operationType, or from.
        /// </remarks>
        [Permissions(PermissionType.ToolManagement)]
        [HttpPatch("tools/{id}")]
        public async Task<IActionResult> PatchTool(int id, ToolJsonPatchRequest patchRequest)
        {
            JsonPatchDocument patchDoc = patchRequest.JsonPatchDocument;
            await _toolApiService.PatchToolAsync(id, patchDoc);
            return Ok();
        }

        /// <summary>
        /// Create a new solar quote.
        /// </summary>
        /// <param name="model">The solar quote request model.</param>
        /// <returns>The ID of the created solar quote.</returns>
        [HttpPost("tools/solarquote")]
        [Permissions(PermissionType.SendQuote)]
        public async Task<IActionResult> PostSolarQuote(SolarQuoteRequest model)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"];
            int quoteId = await _toolApiService.CreateSolarQuoteAsync(model, currentUser.Id);

            return Ok(quoteId);
        }

        /// <summary>
        /// Create a new solar portfolio review.
        /// </summary>
        /// <param name="model">The solar portfolio review request model.</param>
        /// <returns>The ID of the created solar portfolio review.</returns>
        [HttpPost("tools/solarportfolioreview")]
        [Permissions(PermissionType.SendQuote)]
        public async Task<IActionResult> PostSolarPortfolioReview(SolarPortfolioReviewRequest model)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"];
            int quoteId = await _toolApiService.CreateSolarPortfolioReviewAsync(model, currentUser.Id);
            return Ok(quoteId);
        }

        /// <summary>
        /// Get List of Pinned Tools.
        /// </summary>
        /// <remarks>
        /// Search -> Title
        /// <br />
        /// FilterBy -> IsActive
        /// <br />
        /// OrderBy -> title.asc, title.desc, status.asc, status.desc
        /// <br />
        /// Expand -> roles, companies, icon
        /// </remarks>
        [HttpGet("tools/users/current/pinned")]
        public async Task<IActionResult> GetPinnedTools([FromQuery] BaseSearchFilterModel filter)
        {
            UserModel currentUser = (UserModel)Request.HttpContext.Items["User"]!;
            IEnumerable<ToolResponse> tools = await _toolApiService.GetPinnedToolsAsync(filter, currentUser.Id);
            return Ok(tools);
        }

        /// <summary>
        /// Create pinned tools for the current user.
        /// </summary>
        /// <param name="model">The list of tool pinned requests.</param>
        /// <returns>An Ok result.</returns>
        [HttpPost("tools/users/current/pinned")]
        public async Task<IActionResult> PostPinnedTools(IList<ToolPinnedRequest> model)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            await _toolApiService.CreatePinnedToolsAsync(currentUser.Id, model);
            return Ok();
        }
    }
}