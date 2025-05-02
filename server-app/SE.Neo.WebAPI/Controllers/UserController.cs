using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Models.Role;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Models.UserProfile;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly IUserApiService _userApiService;
        private readonly IUserProfileApiService _userProfileApiService;
        private readonly INotificationApiService _notificationApiService;
        private readonly IConversationApiService _conversationApiService;
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userApiService">Service for user-related operations.</param>
        /// <param name="userProfileApiService">Service for user profile-related operations.</param>
        /// <param name="notificationApiService">Service for notification-related operations.</param>
        /// <param name="conversationApiService">Service for conversation-related operations.</param>
        /// <param name="logger">Logger instance for logging operations.</param>
        public UserController(
            IUserApiService userApiService,
            IUserProfileApiService userProfileApiService,
            INotificationApiService notificationApiService,
            IConversationApiService conversationApiService,
            ILogger<UserController> logger
            )
        {
            _userApiService = userApiService;
            _userProfileApiService = userProfileApiService;
            _notificationApiService = notificationApiService;
            _conversationApiService = conversationApiService;
            _logger = logger;
        }

        /// <summary>
        /// Get List of Users.
        /// </summary>
        /// <remarks>
        /// Search -&gt; FirstName + LastName <br/> FilterBy -&gt;
        /// statusids=5,6&amp;roleids=1&amp;regionids=1,2&amp;categoryids=1&amp;permissionids=1&amp;companyids=1&amp;companytypesids=1;companycategoryids=1<br/>
        /// OrderBy -&gt; company.asc, company.desc, statusname.asc, statusname.desc, lastname.asc,
        /// lastname.desc <br/> Expand -&gt; company, country, roles, permissions, image, timezone,
        /// userprofile, userprofile.followed, userprofile.state, userprofile.urllinks,
        /// userprofile.categories, userprofile.regions
        /// </remarks>
        [Active]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers([FromQuery] BaseSearchFilterModel filter)
        {
            var currentUser = (UserModel)Request.HttpContext.Items["User"]!;
            var users = await _userApiService.GetUsersAsync(
                filter,
                currentUser,
                false,
                currentUser.PermissionIds.Any(pi => pi == (int)PermissionType.UserAccessManagement));
            return Ok(users);
        }

        /// <summary>
        /// Get List of Specific Company Users.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Active, Permissions(PermissionType.ManageCompanyUsers, PermissionType.InitiativeManageOwn)]
        [HttpGet("users/company")]
        public async Task<IActionResult> GetCompanyUsers([FromQuery] BaseSearchFilterModel filter)
        {
            var currentUser = (UserModel)Request.HttpContext.Items["User"]!;
            var users = await _userApiService.GetUsersAsync(
                filter,
                currentUser,
                true);
            return Ok(users);
        }

        /// <summary>
        /// Get User By Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="expand">
        /// company, country, roles, permissions, image, timezone, userprofile, userprofile.state,userprofile.urllinks,userprofile.categories,userprofile.regions
        /// </param>
        /// <returns></returns>
        [Active]
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(int id, string? expand)
        {
            var currentUser = (UserModel)Request.HttpContext.Items["User"]!;
            var user = await _userApiService.GetUserAsync(
                id,
                expand,
                currentUser.Id == id || currentUser.PermissionIds.Any(pi => pi == (int)PermissionType.UserAccessManagement));
            return user != null ? Ok(user) : NotFound();
        }

        /// <summary>
        /// Export List of Users to csv file.
        /// </summary>
        /// <remarks>
        /// FilterBy -&gt; statusids=5,6&amp;roleids=1&amp;regionids=1,2&amp;categoryids=1 <br/>
        /// OrderBy -&gt; company.asc, company.desc, statusname.asc, statusname.desc, lastname.asc, lastname.desc
        /// </remarks>

        [Active, Permissions(PermissionType.UserAccessManagement)]
        [HttpGet("users/export")]
        public async Task<IActionResult> ExportUsers([FromQuery] BaseSearchFilterModel filter)
        {
            var userModel = (UserModel)Request.HttpContext.Items["User"]!;

            MemoryStream usersStream = new MemoryStream();
            int userCount = await _userApiService.ExportUsersAsync(filter, usersStream, userModel);
            EntityTagHeaderValue usersFoundCount = new EntityTagHeaderValue("\"" + $"Found {userCount} users" + "\"");
            return File(usersStream, "text/csv", $"Export file {DateTime.UtcNow.Month}_{DateTime.UtcNow.Day}_{DateTime.UtcNow.Year}.csv", null, usersFoundCount);
        }

        /// <summary>
        /// Export List of company Users to csv file.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Active, Permissions(PermissionType.ManageCompanyUsers)]
        [HttpGet("users/export/company")]
        public async Task<IActionResult> ExportCompanyUsers([FromQuery] BaseSearchFilterModel filter)
        {
            var userModel = (UserModel)Request.HttpContext.Items["User"]!;

            MemoryStream usersStream = new MemoryStream();
            int userCount = await _userApiService.ExportUsersAsync(filter, usersStream, userModel, true);
            EntityTagHeaderValue usersFoundCount = new EntityTagHeaderValue("\"" + $"Found {userCount} users" + "\"");
            return File(usersStream, "text/csv", $"Export file {DateTime.UtcNow.Month}_{DateTime.UtcNow.Day}_{DateTime.UtcNow.Year}.csv", null, usersFoundCount);
        }

        /// <summary>
        /// Get Authorized User.
        /// </summary>
        /// <param name="expand">
        /// company, country, roles, permissions, image, timezone, userprofile, userprofile.state,
        /// userprofile.urllinks, userprofile.categories,userprofile.regions
        /// </param>
        /// <returns></returns>
        [HttpGet("users/current")]
        public async Task<IActionResult> GetCurrentUser(string? expand)
        {
            var userModel = (UserModel)Request.HttpContext.Items["User"]!;
            if (userModel != null)
            {
                var user = await _userApiService.GetUserAsync(
                     userModel.Id,
                     expand,
                     true);

                if (user != null && user.Roles.Any(x => x.Id == (int)RoleType.SystemOwner) && !user.Roles.Any(x => x.Id == (int)RoleType.Admin))
                {
                    var rolesList = user.Roles.ToList();
                    rolesList.Add(new RoleResponse() { Id = (int)RoleType.Admin, Name = RoleType.Admin.ToString(), Permissions = user.Permissions, IsSpecial = true });
                    user.Roles = rolesList;
                }
                return Ok(user);
            }
            return Ok();
        }

        /// <summary>
        /// Gets number of unseen notifications and unread messages
        /// </summary>
        /// <returns></returns>
        [Active]
        [HttpGet("users/current/badge-counters")]
        [ExcludeLogging]
        public async Task<IActionResult> GetUnseenCurrentUserNotificationsAndUnreadMessagesCount()
        {
            var currentUser = (UserModel)Request.HttpContext.Items["User"]!;
            int notificationsCount = await _notificationApiService.GetUnseenUserNotificationsCountAsync(currentUser.Id);
            int messagesCount = await _conversationApiService.GetUnreadUserMessagesCountAsync(currentUser.Id);

            return Ok(new { notificationsCount, messagesCount });
        }

        /// <summary>
        /// Marks all notifications as seen, or all notification as read, or specific notification
        /// (by id) as read
        /// </summary>
        /// <param name="readSeen">Set 1 for marking as read and set 2 for marking as seen</param>
        /// <param name="id">
        /// Use it only for marking as read and it is not required. If value is set for the
        /// parameter - only specific notification will be marked as read, if there is no value -
        /// all notification will be marked as read in case when readSeen parameter is equal to 1
        /// </param>
        /// <returns>
        /// True if the action completed successful, False if the action was no executed because of
        /// an error
        /// </returns>
        [Active]
        [HttpPut("users/current/notifications")]
        public async Task<IActionResult> MarkUserNotificationsAsReadSeen(ReadSeenTypes readSeen, int? id)
        {
            var currentUser = (UserModel)Request.HttpContext.Items["User"]!;

            bool result = false;
            if (readSeen == ReadSeenTypes.Read)
            {
                result = await _notificationApiService.MarkNotificationsAsReadAsync(currentUser.Id, id);
            }
            if (readSeen == ReadSeenTypes.Seen)
            {
                result = await _notificationApiService.MarkNotificationsAsSeenAsync(currentUser.Id);
            }
            return Ok(result);
        }

        /// <summary>
        /// Gets list of all notifications
        /// </summary>
        /// <param name="filter">Provides paginating</param>
        /// <returns></returns>
        [Active]
        [HttpGet("users/current/notifications")]
        public async Task<IActionResult> GetUserNotifications([FromQuery] PaginationModel filter)
        {
            var currentUser = (UserModel)Request.HttpContext.Items["User"]!;

            var notifications = await _notificationApiService.GetUserNotificationsAsync(currentUser.Id, filter);
            return Ok(notifications);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="model">The user request model containing user details.</param>
        /// <returns>The ID of the created user.</returns>
        [HttpPost("users")]
        [Permissions(PermissionType.UserAccessManagement)]
        public async Task<IActionResult> PostUser(UserRequest model)
        {
            if (model.Username != model.Email)
                return UnprocessableEntity($"Email should match Username.");

            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;

            if (!currentUser.PermissionIds.Contains((int)PermissionType.TierManagement) && model.Roles.Any(c => c.Id == (int)RoleType.SystemOwner))
            {
                return UnprocessableEntity($"You are not allowed to create an user with System Owner role.");
            }

            var userId = await _userApiService.CreateUpdateUserAsync(0, model);
            return Ok(userId);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="model">The user request model containing updated user details.</param>
        /// <returns>The ID of the updated user.</returns>
        [HttpPut("users/{id}")]
        public async Task<IActionResult> PutUser(int id, UserRequest model)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            if (!currentUser.PermissionIds.Contains((int)PermissionType.UserAccessManagement))
            {
                if (id != currentUser.Id)
                    return Forbid($"Id should match current user.");
            }

            if (model.Username != model.Email)
                return UnprocessableEntity($"Email should match Username.");

            if (!currentUser.PermissionIds.Contains((int)PermissionType.TierManagement) && model.Roles.Any(c => c.Id == (int)RoleType.SystemOwner))
            {
                return UnprocessableEntity($"You are not allowed to create an user with System Owner role.");
            }

            if (model.Roles.Any(c => c.Id == (int)RoleType.SystemOwner) && model.Roles.Any(c => c.Id == (int)RoleType.Admin))
            {
                model.Roles = model.Roles.Where(c => c.Id != (int)RoleType.Admin);
            }

            var userId = await _userApiService.CreateUpdateUserAsync(id, model);
            return Ok(userId);
        }

        /// <summary>
        /// Patch a user.
        /// </summary>
        /// <remarks>
        /// JsonPatchDocument -&gt; [{op: "replace", "path": "/StatusId", "value": 4}] <br/> Only
        /// path /StatusId currently allowed, do not pass contractResolver, operationType, or from.
        /// </remarks>

        [Permissions(PermissionType.UserAccessManagement, PermissionType.ManageCompanyUsers)]
        [HttpPatch("users/{id}")]
        public async Task<IActionResult> PatchUser(int id, UserPatchRequest patchRequest)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            JsonPatchDocument patchDoc = patchRequest.JsonPatchDocument;
            var isUpdateSuccess = await _userApiService.PatchUserAsync(id, patchDoc, currentUser, isAdminRequest: true);
            return isUpdateSuccess ? Ok(id) : StatusCode(403);
        }

        /// <summary>
        /// Patch a user.
        /// </summary>
        /// <remarks>
        /// JsonPatchDocument -&gt; [{op: "replace", "path": "/CountryId", "value": 1}] <br/> Only
        /// path /CountryId or /RequestDeleteDate currently allowed, do not pass contractResolver,
        /// operationType, or from.
        /// </remarks>

        [HttpPatch("users/current")]
        public async Task<IActionResult> PatchCurrentUser(UserCurrentPatchRequest patchRequest)
        {
            JsonPatchDocument patchDoc = patchRequest.JsonPatchDocument;
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            await _userApiService.PatchUserAsync(currentUser.Id, patchDoc, currentUser);
            return Ok();
        }

        /// <summary>
        /// Request to delete a user.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("users/current")]
        public async Task<IActionResult> RequestDeleteCurrentUser()
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            await _userApiService.RequestToDeleteUserAsync(currentUser.Id, currentUser.Username);
            return Ok();
        }

        /// <summary>
        /// Delete a user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Permissions(PermissionType.UserAccessManagement, PermissionType.ManageCompanyUsers)]
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            var isDeleteSuccess = await _userApiService.DeleteUserAsync(id, currentUser);
            return isDeleteSuccess ? Ok(id) : StatusCode(403);
        }

        /// <summary>
        /// Get List of User Profiles.
        /// </summary>
        /// <remarks>
        /// FilterBy -&gt; regionids=1,2&amp;categoryids=1; companyIds=1 <br/> OrderBy -&gt;
        /// user.lastname.asc, user.lastname.desc <br/> Expand -&gt; country, state, user,
        /// user.company, user.role, user.image, categories, regions, urllinks, followers
        /// </remarks>
        [Active]
        [HttpGet("userprofiles")]
        public async Task<IActionResult> GetUserProfiles([FromQuery] BaseSearchFilterModel filter)
        {
            var currentUser = (UserModel)Request.HttpContext.Items["User"]!;
            var userProfiles = await _userProfileApiService.GetUserProfilesAsync(filter, currentUser.Id);
            return Ok(userProfiles);
        }

        /// <summary>
        /// Get User Profile by User Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="expand">
        /// country, state, user, user.company, user.role, user.image, categories, regions,
        /// urllinks, followers
        /// </param>
        /// <returns></returns>
        [Active]
        [HttpGet("userprofiles/{userId}")]
        public async Task<IActionResult> GetUserProfile(int userId, string? expand)
        {
            var currentUser = (UserModel)Request.HttpContext.Items["User"]!;
            var userProfile = await _userProfileApiService.GetUserProfileAsync(userId, currentUser.Id, expand);
            return userProfile != null ? Ok(userProfile) : NotFound();
        }

        /// <summary>
        /// Creates a new user profile.
        /// </summary>
        /// <param name="model">The user profile request model containing profile details.</param>
        /// <returns>The ID of the created user profile.</returns>
        [HttpPost("userprofiles")]
        public async Task<IActionResult> PostUserProfile(UserProfileRequest model)
        {
            string ConsentIp = string.Empty;
            string ConsentUserAgent = string.Empty;
            try
            {
                ConsentIp = HttpContext.Connection.RemoteIpAddress?.ToString();
                ConsentUserAgent = Request.Headers["User-Agent"];
            }
            catch (Exception)
            {
            }

            var userId = await _userProfileApiService.CreateUpdateUserProfileAsync(0, model, ConsentIp, ConsentUserAgent);
            return Ok(userId);
        }

        /// <summary>
        /// Updates the current user's profile.
        /// </summary>
        /// <param name="model">The user profile request model containing updated profile details.</param>
        /// <returns>The ID of the updated user profile.</returns>
        [Active]
        [HttpPut("userprofiles/current")]
        public async Task<IActionResult> PutCurrentUserProfile(UserProfileRequest model)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            model.UserId = currentUser.Id;
            var id = await _userProfileApiService.CreateUpdateUserProfileAsync(currentUser.Id, model,"","",true);
            return Ok(id);
        }

        /// <summary>
        /// Updates a user profile.
        /// </summary>
        /// <param name="model">The user profile request model containing updated profile details.</param>
        /// <returns>The ID of the updated user profile.</returns>
        [Active, Permissions(PermissionType.UserProfileEdit)]
        [HttpPut("userprofiles")]
        public async Task<IActionResult> PutUserProfile(UserProfileRequest model)
        {
            var id = await _userProfileApiService.CreateUpdateUserProfileAsync(model.UserId, model);
            return Ok(id);
        }

        /// <summary>
        /// Adds interests to the current user's profile.
        /// </summary>
        /// <param name="model">The user profile interest request model containing interest details.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [Active]
        [HttpPost("userprofiles/current/interests")]
        public async Task<IActionResult> PostUserProfileInterests(UserProfileInterestRequest model)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            await _userProfileApiService.CreateUserProfileInterestAsync(currentUser.Id, model);
            return Ok();
        }

        /// <summary>
        /// Gets suggestions for the current user's profile.
        /// </summary>
        /// <returns>A list of user profile suggestions.</returns>
        [Active]
        [HttpGet("userprofiles/current/suggestions")]
        public async Task<IActionResult> GetUserProfileSuggestions()
        {
            var currentUser = (UserModel)Request.HttpContext.Items["User"]!;
            var userProfile = await _userProfileApiService.GetUserProfileSuggestionsAsync(currentUser.Id);
            return userProfile != null ? Ok(userProfile) : NotFound();
        }

        /// <summary>
        /// Adds a follower to the current user.
        /// </summary>
        /// <param name="id">The ID of the user to follow.</param>
        /// <returns>The ID of the followed user.</returns>
        [Active]
        [HttpPost("users/current/followers/{id}")]
        public async Task<IActionResult> PostUserFollowers(int id)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            await _userApiService.CreateUserFollowerAsync(currentUser.Id, id);
            return Ok(id);
        }

        /// <summary>
        /// Removes a follower from the current user.
        /// </summary>
        /// <param name="id">The ID of the follower to remove.</param>
        /// <returns>The ID of the removed follower.</returns>
        [Active]
        [HttpDelete("users/current/followers/{id}")]
        public async Task<IActionResult> DeleteUserFollowers(int id)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            await _userApiService.RemoveUserFollowerAsync(currentUser.Id, id);
            return Ok(id);
        }

        /// <summary>
        /// Retrieves the SharePoint admin user information for a specific company.
        /// </summary>
        /// <param name="formData">The request data containing the company ID and user ID.</param>
        /// <returns>
        /// An IActionResult containing the SharePoint admin user information if found; otherwise, null.
        /// </returns>
        [Active]
        [HttpPost("users/getspadminbycompany")]
        public async Task<IActionResult> GetSPAdminByCompany([FromBody] SpAdminCompanyRequest formData)
        {
            var spAdminUserInfo = await _userApiService.GetSPAdminByCompany(
               formData.companyId,
               formData.userId);
            return spAdminUserInfo != null ? Ok(spAdminUserInfo) : Ok(null);
        }


        /// <summary>
        /// Sync Users Login Count
        /// </summary>
        /// <returns></returns>
        [Active]
        [HttpPost("users/synclogincount")]
        public async Task<IActionResult> SyncUserLoginCountAsync()
        {
            var rowsAffected = await _userProfileApiService.SyncUserLoginCountAsync();
            // Adding a service call to update the User status to Expire for approved Onboarded user for more than 20 days
            await _userApiService.UpdateOnboardUserStatus();
            return Ok(rowsAffected);
        }
        /// <summary>
        /// Function to get skills based on categories for SP users and to get all skills for corporate users
        /// </summary>
        /// <returns></returns>
        [Active]
        [HttpGet("userprofiles/getskillsbycategory")]
        public async Task<IActionResult> GetSkillsByCategory()
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            if (currentUser.RoleIds.Contains((int)RoleType.SolutionProvider) || currentUser.RoleIds.Contains((int)RoleType.Corporation) || currentUser.RoleIds.Contains((int)RoleType.SPAdmin))
            {
                return Ok(await _userApiService.GetSkillsByCategory(currentUser));
            }
            return BadRequest();
        }

        /// <summary>
        /// Function to get skills for onboarded users
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("userprofiles/getskillsbycategory/{userId}")]
        public async Task<IActionResult> GetSkillsByCategory(int userId,[FromQuery] int[] roleIds)
        {
            if (roleIds?.Count() > 0 && userId != 0)
            {
                var currentUser = new UserModel
                {
                    RoleIds = roleIds.ToList(),
                    Id = userId
                };
                return Ok(await _userApiService.GetSkillsByCategory(currentUser));
            }
            return BadRequest();
        }
    }
}