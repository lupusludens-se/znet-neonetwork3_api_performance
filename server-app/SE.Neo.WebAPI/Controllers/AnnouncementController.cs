
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Models.Announcement;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Controllers
{
    [Authorize, Active]
    [ApiController]
    [Route("api")]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementApiService _announcementApiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnouncementController"/> class.
        /// </summary>
        /// <param name="announcementApiService">The announcement API service.</param>
        public AnnouncementController(IAnnouncementApiService announcementApiService)
        {
            _announcementApiService = announcementApiService;
        }

        /// <summary>
        /// Creates or updates an announcement.
        /// </summary>
        /// <param name="model">The announcement request model.</param>
        /// <param name="forceActivate">Indicates whether to force activate the announcement.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPost("announcements")]
        [Permissions(PermissionType.AnnouncementManagement)]
        public async Task<IActionResult> PostAnnouncement(AnnouncementRequest model, [FromQuery] bool forceActivate = false)
        {
            int announcementId = await _announcementApiService.CreateUpdateAnnouncementAsync(model, forceActivate: forceActivate);
            return announcementId == 0 ? Conflict(announcementId) : Ok(announcementId);
        }

        /// <summary>
        /// Updates an existing announcement.
        /// </summary>
        /// <param name="id">The announcement ID.</param>
        /// <param name="model">The announcement request model.</param>
        /// <param name="forceActivate">Indicates whether to force activate the announcement.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPut("announcements/{id}")]
        [Permissions(PermissionType.AnnouncementManagement)]
        public async Task<IActionResult> PutAnnouncement(int id, AnnouncementRequest model, [FromQuery] bool forceActivate = false)
        {
            int announcementId = await _announcementApiService.CreateUpdateAnnouncementAsync(model, id, forceActivate);
            return announcementId == 0 ? Conflict(id) : Ok(announcementId);
        }

        /// <summary>
        /// Patches an announcement.
        /// </summary>
        /// <param name="id">The announcement ID.</param>
        /// <param name="patchRequest">The patch request model.</param>
        /// <param name="forceActivate">Indicates whether to force activate the announcement.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        /// <remarks>
        /// JsonPatchDocument -> [{op: "replace", "path": "/IsActive", "value": true}]
        /// <br />
        /// Only path /IsActive currently allowed, do not pass contractResolver, operationType, or from.
        /// </remarks>
        [HttpPatch("announcements/{id}")]
        [Permissions(PermissionType.AnnouncementManagement)]
        public async Task<IActionResult> PatchAnnouncement(int id, AnnouncementJsonPatchRequest patchRequest, [FromQuery] bool forceActivate = false)
        {
            JsonPatchDocument patchDoc = patchRequest.JsonPatchDocument;
            int announcementId = await _announcementApiService.PatchAnnouncementAsync(id, patchDoc, forceActivate);
            return announcementId == 0 ? Conflict(id) : Ok(announcementId);
        }

        /// <summary>
        /// Gets the latest announcement for the current user.
        /// </summary>
        /// <param name="expand">The expand parameter to include additional details.</param>
        /// <returns>An <see cref="IActionResult"/> containing the latest announcement.</returns>
        [Active(skipAuthorize: true)]
        [HttpGet("announcements/latest")]
        public async Task<IActionResult> GetLatestAnnouncement(string? expand)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            var announcement = await _announcementApiService.GetLatestAnnouncementAsync(currentUser?.RoleIds?.Where(y => y != (int)RoleType.All).First() ?? -1, expand);

            return Ok(announcement);
        }

        /// <summary>
        /// Gets an announcement by ID.
        /// </summary>
        /// <param name="id">The announcement ID.</param>
        /// <param name="expand">The expand parameter to include additional details.</param>
        /// <returns>An <see cref="IActionResult"/> containing the announcement.</returns>
        [HttpGet("announcements/{id}")]
        [Permissions(PermissionType.AnnouncementManagement)]
        public async Task<IActionResult> GetAnnouncement(int id, string? expand)
        {
            var announcement = await _announcementApiService.GetAnnouncementAsync(id, expand);
            return announcement != null ? Ok(announcement) : NotFound();
        }

        /// <summary>
        /// Gets a list of announcements.
        /// </summary>
        /// <param name="expandOrderModel">The expand and order model for filtering and sorting.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of announcements.</returns>
        /// <remarks>
        /// OrderBy -> name.asc, name.desc, isactive.asc, isactive.desc, audience.asc, audience.desc
        /// <br/>
        /// Expand -> audience
        /// </remarks>
        [HttpGet("announcements")]
        [Permissions(PermissionType.AnnouncementManagement)]
        public async Task<IActionResult> GetAnnouncements([FromQuery] ExpandOrderModel expandOrderModel)
        {
            WrapperModel<AnnouncementResponse> announcements = await _announcementApiService.GetAnnouncementsAsync(expandOrderModel);
            return Ok(announcements);
        }

        /// <summary>
        /// Deletes an announcement by ID.
        /// </summary>
        /// <param name="id">The announcement ID.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpDelete("announcements/{id}")]
        [Permissions(PermissionType.AnnouncementManagement)]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            await _announcementApiService.RemoveAnnouncementAsync(id);
            return Ok(id);
        }
    }
}
