using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Constants;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class UserPendingController : ControllerBase
    {
        private readonly IUserPendingApiService _userPendingApiService;
        private readonly IActivityApiService _activityApiService;
        private readonly ILogger<UserPendingController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPendingController"/> class.
        /// </summary>
        /// <param name="userPendingApiService">The service for handling user pending operations.</param>
        /// <param name="activityApiService">The service for handling activity operations.</param>
        /// <param name="logger">The logger instance for logging information.</param>
        public UserPendingController(
            IUserPendingApiService userPendingApiService,
            IActivityApiService activityApiService,
            ILogger<UserPendingController> logger
            )
        {
            _userPendingApiService = userPendingApiService;
            _activityApiService = activityApiService;
            _logger = logger;
        }

        /// <summary>
        /// Gets list of pending users
        /// </summary>
        /// <remarks>
        /// OrderBy -> name.asc, name.desc, company.asc, company.desc, role.asc, role.desc, createddate.asc, createddate.desc
        /// <br />
        /// Expand -> company, role
        /// </remarks>
        [Active, Permissions(PermissionType.UserAccessManagement)]
        [HttpGet("userpendings")]
        public async Task<IActionResult> GetUserPendings([FromQuery] ExpandOrderModel filter)
        {
            WrapperModel<UserPendingListItemResponse> userPendings = await _userPendingApiService.GetUserPendingsAsync(filter);
            //Delete denied user pendings
            try
            {
                await _userPendingApiService.DeleteDeniedUserPendingsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, CoreErrorMessages.ErrorOnRemoving);
            }

            return Ok(userPendings);
        }

        /// <summary>
        /// Gets specific pending user
        /// </summary>
        /// <param name="id">identificatior of pending user</param>
        /// <param name="expand">company, country, role</param>
        [Active, Permissions(PermissionType.UserAccessManagement)]
        [HttpGet("userpendings/{id}")]
        public async Task<IActionResult> GetUserPending(int id, string? expand)
        {
            UserPendingItemResponse? userPending = await _userPendingApiService.GetUserPendingAsync(id, expand);
            return userPending != null ? Ok(userPending) : NotFound();
        }

        /// <summary>
        /// Creates new user pending
        /// </summary>
        [AllowAnonymous]
        [HttpPost("userpendings")]
        public async Task<IActionResult> CreateUserPending(UserPendingAddWithActivityRequest formData)
        {
            int userPendingId = await _userPendingApiService.CreateUpdateUserPendingAsync(formData.SignUpData);
            if (userPendingId > 0)
            {
                try
                {
                    await _activityApiService.CreatePublicActivityAsync(formData.ActivityData);
                }
                catch (Exception)
                {
                }

            }
            return Ok(userPendingId);
        }

        /// <summary>
        /// Edits existing user pending
        /// </summary>
        /// <param name="id">identificatior of pending user</param>
        [Active, Permissions(PermissionType.UserAccessManagement)]
        [HttpPut("userpendings/{id}")]
        public async Task<IActionResult> EditUserPending(int id, UserPendingEditRequest model)
        {
            if (id <= 0)
            {
                ModelState.AddModelError(nameof(id), ErrorMessages.PositiveIdValue);
                return UnprocessableEntity(new ValidationResponse(ModelState));
            }
            int userPendingId = await _userPendingApiService.CreateUpdateUserPendingAsync(model, id);
            return Ok(userPendingId);
        }

        /// <summary>
        /// Approves existing user pending - creates new user and deletes the user pending afterwards
        /// </summary>
        /// <param name="id">identificatior of pending user</param>
        [Active, Permissions(PermissionType.UserAccessManagement)]
        [HttpPost("userpendings/{id}/approval")]
        public async Task<IActionResult> ApproveUserPending(int id)
        {
            int userId = await _userPendingApiService.ApproveUserPending(id);
            return Ok(userId);
        }

        /// <summary>
        /// Denies existing user pending - marks the user pending as denied
        /// </summary>
        /// <param name="id">identificatior of pending user</param>
        [Active, Permissions(PermissionType.UserAccessManagement)]
        [HttpPatch("userpendings/{id}/denial")]
        public async Task<IActionResult> DenyUserPending(int id, bool isDenied)
        {
            bool result = await _userPendingApiService.DenyUserPending(id, isDenied);
            return Ok(result);
        }

        /// <summary>
        /// Gets number of pending users
        /// </summary>
        /// <returns></returns>
        [Active, Permissions(PermissionType.UserAccessManagement)]
        [HttpGet("userpendings/pending-counter")]
        public async Task<IActionResult> GetPendingUserCount()
        {
            int pendingUserCount = await _userPendingApiService.GetPendingUserCount();
            return Ok(new { pendingUserCount });
        }
    }
}
