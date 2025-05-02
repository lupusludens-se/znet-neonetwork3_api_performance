using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SE.Neo.Common.Models.Conversation;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Models.Conversation;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Controllers
{
    [Authorize, Active]
    [ApiController]
    [Route("api/conversations")]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationApiService _conversationsApiService;
        private readonly ILogger<ConversationController> _logger;
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationController"/> class.
        /// </summary>
        /// <param name="conversationsApiService">The service to manage conversations.</param>
        /// <param name="logger">The logger instance.</param>
        /// <param name="userService">The user service instance.</param>
        public ConversationController(
            IConversationApiService conversationsApiService,
            ILogger<ConversationController> logger,
            IUserService userService
            )
        {
            _conversationsApiService = conversationsApiService;
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Get list of the conversations by filters.
        /// </summary>
        /// <param name="filter">Defines filter parameters.</param>
        /// <returns>List of the conversations.</returns>
        /// <remarks>
        /// Expand values: discussionusers,discussionusers.users,discussionusers.users.image,discussionusers.users.company,project.
        /// WithUserId: set user id who you want to get the conversations with and leave it empty to
        /// get the conversations with all users.
        /// Individual: filter list by individual (if set true), or group (if set false), or all (if
        /// not set) conversations.
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> GetConversationsAsync([FromQuery] ConversationsFilter filter)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            bool hasPermission = (currentUser.PermissionIds.Contains((int)PermissionType.MessagesAll) ||
                         currentUser.PermissionIds.Contains((int)PermissionType.ViewCompanyMessages));

            if (!hasPermission && filter.IncludeAll)
            {
                _logger.LogError("User {Username} ({UserId}) trying to get all conversations without permission", currentUser.Username, currentUser.Id);
                return Forbid();
            }

            WrapperModel<ConversationResponse> items = await _conversationsApiService.GetConversationsAsync(currentUser, filter);

            return Ok(items);
        }

        /// <summary>
        /// Create new conversation.
        /// </summary>
        /// <param name="model">Conversation definition.</param>
        /// <returns>Newly created conversation object.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateConversationAsync(ConversationRequest model)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            // make sure current user not in the list of participants
            model.Users = model.Users.Where(u => u.Id != currentUser.Id).ToList();

            if (!model.Users.Any())
            {
                ModelState.AddModelError(nameof(model.Users), "User to start conversation with is not selected.");

                return UnprocessableEntity(new ValidationResponse(ModelState));
            }

            if (!currentUser.PermissionIds.Contains((int)PermissionType.MessagesAll) && model.Users.Count > 1)
            {
                _logger.LogError("User {Username} ({UserId}) trying to create group conversation without permission", currentUser.Username, currentUser.Id);
                return Forbid();
            }
            if (CheckAccessToAttachProject(model, currentUser))
            {
                ModelState.AddModelError(nameof(model.Users), "User is not granted to attach a project.");
                return UnprocessableEntity(new ValidationResponse(ModelState));
            }

            ConversationResponse conversation = await _conversationsApiService.CreateConversationAsync(currentUser, model);

            return Ok(conversation);
        }

        /// <summary>
        /// Get conversation by the given id.
        /// </summary>
        /// <param name="id">Unique identifier of the conversation.</param>
        /// <param name="expand">Parameter to attach related entities. Possible values: discussionusers,discussionusers.users,discussionusers.users.roles,discussionusers.users.image,discussionusers.users.company,project.</param>
        /// <returns>Conversation object, or 404 NotFound if nothing found.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConversationsAsync(int id, string? expand = null)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            ConversationResponse? item = await _conversationsApiService.GetConversationAsync(currentUser, id, expand);

            return item != null ? Ok(item) : NotFound();
        }

        /// <summary>
        /// Update existing conversation.
        /// </summary>
        /// <param name="id">Unique identifier of the conversation.</param>
        /// <param name="model">Conversation definition.</param>
        /// <returns>Updated conversation object.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConversationAsync(int id, ConversationRequest model)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            // make sure current user not in the list of participants
            model.Users = model.Users.Where(u => u.Id != currentUser.Id).ToList();
            if (CheckAccessToAttachProject(model, currentUser))
            {
                ModelState.AddModelError(nameof(model.Users), "User is not granted to attach a project.");
                return UnprocessableEntity(new ValidationResponse(ModelState));
            }
            ConversationResponse conversation = await _conversationsApiService.UpdateConversationAsync(currentUser, id, model);

            return Ok(conversation);
        }

        /// <summary>
        /// Get conversation messages.
        /// </summary>
        /// <param name="id">Unique identifier of the conversation.</param>
        /// <param name="filter">Defines filter parameters.</param>
        /// <returns>List of the conversation messages.</returns>
        /// <remarks>Expand values: user,user.image,user.company,user.roles,attachments,attachments.image.</remarks>
        [HttpGet("{id}/messages")]
        public async Task<IActionResult> GetConversationMessagesAsync(int id, [FromQuery] ConversationMessagesFilterDTO filter)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            WrapperModel<ConversationMessageResponse> items = await _conversationsApiService.GetConversationMessagesAsync(currentUser, id, filter);

            return Ok(items);
        }

        /// <summary>
        /// Send message to the conversation.
        /// </summary>
        /// <param name="id">Unique identifier of the conversation.</param>
        /// <param name="model">Message definition.</param>
        /// <returns>Newly created message object.</returns>
        [HttpPost("{id}/messages")]
        public async Task<IActionResult> CreateConversationMessageAsync(int id, ConversationMessageRequest model)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            ConversationMessageResponse item = await _conversationsApiService.CreateConversationMessageAsync(currentUser, id, model);

            return Ok(item);
        }

        /// <summary>
        /// Marks all messages as read for the given conversation.
        /// </summary>
        /// <param name="id">Unique identifier of the conversation.</param>
        /// <returns>Empty response.</returns>
        [HttpPut("{id}/messages")]
        public async Task<IActionResult> MarkUserMessagesAsReadAsync(int id)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            await _conversationsApiService.MarkUserMessagesAsReadAsync(currentUser, id);

            return Ok();
        }

        /// <summary>
        /// Create new conversation to contact us.
        /// </summary>
        /// <param name="model">Conversation definition.</param>
        /// <returns>Newly created conversation object.</returns>
        [HttpPost("contact-us")]
        public async Task<IActionResult> CreateContactUsConversationAsync(ConversationContactUsRequest model)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            ConversationResponse? conversation = await _conversationsApiService.CreateContactUsConversationAsync(currentUser, model);
            return conversation != null ? Ok(conversation) : NotFound();
        }

        /// <summary>
        /// Checks if the current user has access to attach a project to the conversation.
        /// </summary>
        /// <param name="model">The conversation request model containing the project information.</param>
        /// <param name="currentUser">The current user attempting to attach the project.</param>
        /// <returns>True if the user has access to attach the project, otherwise false.</returns>
        private bool CheckAccessToAttachProject(ConversationRequest model, UserModel currentUser)
        {
            if (model.ProjectId.HasValue)
            {
                if (currentUser.RoleIds.Contains((int)Common.Enums.RoleType.SolutionProvider))
                {
                    List<int> userIds = model.Users.Select(u => u.Id).ToList();
                    return _userService.IsSolutionProviderUserExist(userIds);
                }
            }
            return false;
        }


        /// <summary>
        /// Remove message for the given message id.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [HttpDelete("delete-message/{messageId}")]
        public async Task<IActionResult> DeleteMessageAsync(int messageId)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            bool? result = await _conversationsApiService.DeleteMessageAsync(messageId, currentUser);
            return result != null ? Ok(result) : Unauthorized(403);
        }


        /// <summary>
        /// Edit message for the given message id.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch("edit-message/{messageId}")]
        public async Task<IActionResult> UpdateMessageAsync(int messageId, ConversationMessageRequest model)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            bool? result = await _conversationsApiService.UpdateMessageAsync(messageId, currentUser, model);
            return result != null ? Ok(result) : Unauthorized(403);
        }

        /// <summary>
        /// Get conversation id for a project for contact provider button
        /// </summary>
        /// <param name="projectId">project id attached to the conversation</param>
        /// <returns>Conversation id, or null if nothing found.</returns>
        [HttpGet("contact-provider/{projectId}")]
        public async Task<IActionResult> GetContactProviderConversationAsync(int projectId)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;

            int? item = await _conversationsApiService.GetContactProviderConversationAsync(currentUser.Id, projectId);

            return Ok(item);
        }
    }
}