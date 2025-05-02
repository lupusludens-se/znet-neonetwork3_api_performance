using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Forum;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Models.Forum;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Controllers
{

    [ApiController]
    [Authorize, Active]
    [Route("api/forums")]
    public class ForumController : ControllerBase
    {
        private readonly IForumApiService _forumApiService;
        private readonly ILogger<ForumController> _logger;

        /// <summary>  
        /// Initializes a new instance of the <see cref="ForumController"/> class.  
        /// </summary>  
        /// <param name="forumApiService">The forum API service instance.</param>  
        /// <param name="logger">The logger instance.</param>
        public ForumController(
            IForumApiService forumApiService,
            ILogger<ForumController> logger
            )
        {
            _forumApiService = forumApiService;
            _logger = logger;
        }

        /// <summary>
        /// Get list of the forums by filters.
        /// </summary>
        /// <param name="filter">Defines filter parameters.</param>
        /// <returns>List of the forums.</returns>
        /// <remarks>
        /// Search: search by forum subject (case insensitive).
        /// <br />
        /// FilterBy: foryou&amp;saved&amp;myonly&amp;foryou.popular&amp;solutionids=1,2&amp;categoryids=1&amp;technologyids=1&amp;regionids=1
        /// <br />
        /// OrderBy: no values available.
        /// <br />
        /// Expand values: discussionusers.users,discussionusers.users.image,categories,regions,saved.
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> GetForumsAsync([FromQuery] BaseSearchFilterModel filter)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            var items = await _forumApiService.GetForumsAsync(currentUser, filter);

            return Ok(items);
        }

        /// <summary>
        /// Get forum by given the id.
        /// </summary>
        /// <param name="id">Unique identifier of the forum.</param>
        /// <param name="expand">Parameter to attach related entities. Possible values: discussionusers.users,discussionusers.users.image,categories,regions,saved.</param>
        /// <returns>Forum object, or 404 NotFound if nothing found.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetForumAsync(int id, [FromQuery] string? expand = null)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            var forum = await _forumApiService.GetForumAsync(currentUser, id, expand);

            return forum != null ? Ok(forum) : NotFound();
        }

        /// <summary>
        /// Create new forum.
        /// </summary>
        /// <param name="model">Forum definition.</param>
        /// <returns>Newly created forum object.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateForumAsync(ForumRequest model)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            if (!currentUser.PermissionIds.Contains((int)PermissionType.ForumManagement) && model.IsPrivate)
            {
                _logger.LogWarning("User {Username} ({UserId}) trying to create Private Forum without permission", currentUser.Username, currentUser.Id);
                return Forbid();
            }

            if (!currentUser.PermissionIds.Contains((int)PermissionType.ForumManagement) && model.IsPinned)
            {
                model.IsPinned = false;
            }

            var forum = await _forumApiService.CreateForumAsync(currentUser.Id, model, currentUser);

            return Ok(forum);
        }

        /// <summary>
        /// Update existing forum.
        /// </summary>
        /// <param name="id">Unique identifier of the forum.</param>
        /// <param name="model">Forum definition.</param>
        /// <returns>Updated forum object.</returns>
        [HttpPut("{id}")]
        [Permissions(PermissionType.ForumManagement)]
        public async Task<IActionResult> UpdateForumAsync(int id, ForumRequest model)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            var forum = await _forumApiService.UpdateForumAsync(currentUser.Id, id, model);

            return Ok(forum);
        }

        /// <summary>
        /// Delete existing forum.
        /// </summary>
        /// <param name="id">Unique identifier of the forum.</param>
        /// <returns>Empty response.</returns>
        [HttpDelete("{id}")]
        [Permissions(PermissionType.ForumManagement)]
        public async Task<IActionResult> DeleteForumAsync(int id)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            await _forumApiService.DeleteForumAsync(currentUser.Id, id);

            return NoContent();
        }

        /// <summary>
        /// Follow forum (subscribe for notifications).
        /// </summary>
        /// <param name="id">Unique identifier of the forum.</param>
        /// <returns>Empty response.</returns>
        [HttpPost("{id}/followers")]
        public async Task<IActionResult> FollowForumAsync(int id)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            int? responseId = await _forumApiService.FollowForumAsync(currentUser, id);

            return responseId == null ? StatusCode(403) : Ok();
        }

        /// <summary>
        /// Unfollow forum (unsubscribe for notifications).
        /// </summary>
        /// <param name="id">Unique identifier of the forum.</param>
        /// <returns>Empty response.</returns>
        [HttpDelete("{id}/followers")]
        public async Task<IActionResult> UnFollowForumAsync(int id)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            await _forumApiService.UnFollowForumAsync(currentUser.Id, id);

            return Ok();
        }

        /// <summary>
        /// Get forum messages/replies.
        /// </summary>
        /// <param name="id">Unique identifier of the forum.</param>
        /// <param name="filter">Defines filter parameters.</param>
        /// <returns>List of the forum messages/replies.</returns>
        /// <remarks>Expand values: user,user.company,user.image,user.follower,attachments,attachments.image,messagelikes,replies.</remarks>
        [HttpGet("{id}/messages")]
        public async Task<IActionResult> GetForumMessagesAsync(int id, [FromQuery] ForumMessagesFilter filter)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            var items = await _forumApiService.GetForumMessagesAsync(currentUser, id, filter);

            return Ok(items);
        }

        /// <summary>
        /// Send message/reply to the forum/message.
        /// </summary>
        /// <param name="id">Unique identifier of the forum.</param>
        /// <param name="model">Message definition.</param>
        /// <returns>Newly created message object.</returns>
        [HttpPost("{id}/messages")]
        public async Task<IActionResult> CreateForumMessageAsync(int id, ForumMessageRequest model)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            if (!currentUser.PermissionIds.Contains((int)PermissionType.ForumManagement) && model.IsPinned)
            {
                model.IsPinned = false;
            }

            var message = await _forumApiService.CreateForumMessageAsync(currentUser, id, model);

            return message == null ? StatusCode(403) : Ok(message);
        }

        /// <summary>
        /// Update existing forum message
        /// </summary>
        /// <param name="id">Unique identifier of the forum.</param>
        /// <param name="messageId">Unique identifier of the message.</param>
        /// <param name="model">Message definition.</param>
        /// <returns>Updated message object.</returns>
        [HttpPut("{id}/messages/{messageId}")]
        //[Permissions(PermissionType.ForumManagement)]
        public async Task<IActionResult> UpdateForumMessageAsync(int id, int messageId, ForumMessageRequest model)
        {
            try
            {
                var currentUser = (UserModel)HttpContext.Items["User"]!;
                if (id == 0)
                {
                    _logger.LogWarning("Forum Message Cannot be editted, Valid Forum Message ID was not passed for User {Username}", currentUser.Id);
                    return BadRequest();
                }
                if (model.UserId > 0 && currentUser.Id != model.UserId)
                {
                    _logger.LogWarning("No permission to edit the Forum message", currentUser.Id);
                    return Forbid();
                }

                var message = await _forumApiService.UpdateForumMessageAsync(id, messageId, model);

                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Add like to the message/reply.
        /// </summary>
        /// <param name="id">Unique identifier of the forum.</param>
        /// <param name="messageId">Unique identifier of the message.</param>
        /// <returns>Empty response.</returns>
        [HttpPost("{id}/messages/{messageId}/likes")]
        public async Task<IActionResult> LikeForumMessageAsync(int id, int messageId)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            int? responseId = await _forumApiService.LikeForumMessageAsync(currentUser, id, messageId);

            return responseId == null ? StatusCode(403) : Ok();
        }

        /// <summary>
        /// Removes like to the message/reply.
        /// </summary>
        /// <param name="id">Unique identifier of the forum.</param>
        /// <param name="messageId">Unique identifier of the message.</param>
        /// <returns>Empty response.</returns>
        [HttpDelete("{id}/messages/{messageId}/likes")]
        public async Task<IActionResult> UnLikeForumMessageAsync(int id, int messageId)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            await _forumApiService.UnLikeForumMessageAsync(currentUser.Id, messageId);

            return Ok();
        }

        /// <summary>
        /// Remove messages with childs by the given message id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}/messages/{messageId}")]
        public async Task<IActionResult> RemoveForumMessageAsync(int id, int messageId)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            if (!currentUser.RoleIds.Contains((int)RoleType.Admin))
            {
                int messageOwnerId = await _forumApiService.GetForumMessageOwnerIdAsync(messageId);
                if (messageOwnerId != currentUser?.Id)
                {
                    return StatusCode(403);
                }

            }
            await _forumApiService.RemoveForumMessageAsync(id, messageId);

            return Ok();
        }
        /// <summary>
        /// Edit forum Discussion.
        /// </summary>
        /// <param name="model">Forum definition.</param>
        /// <returns>Newly created forum object.</returns>
        [HttpPut("edit")]
        public async Task<IActionResult> EditForumAsync(ForumRequest model)
        {
            try
            {
                var currentUser = (UserModel)HttpContext.Items["User"]!;
                if (model.Id == 0)
                {
                    _logger.LogWarning("Forum Cannot be editted, Valid Forum ID was not passed for User {Username}", currentUser.Id);
                    return BadRequest();
                }
                if (!currentUser.PermissionIds.Contains((int)PermissionType.ForumManagement) && model.IsPrivate)
                {
                    _logger.LogWarning("User {Username} ({UserId}) trying to edit Private Forum without permission", currentUser.Username, currentUser.Id);
                    return Forbid();
                }

                if (model.FirstMessage?.Attachments?.Count > 0)
                {
                    foreach (var attachment in model.FirstMessage.Attachments)
                        if (attachment.ImageName == null)
                        {
                            _logger.LogWarning("Forum Cannot be editted, Because Image Name can not be null in attachments.");
                            return BadRequest("Forum Cannot be editted, Because Image Name can not be null in attachments.");
                        }
                }

                var forum = await _forumApiService.EditForumAsync(currentUser.Id, model);

                return Ok(forum);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}