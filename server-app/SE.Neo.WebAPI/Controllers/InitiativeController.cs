
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models;
using SE.Neo.Common.Models.Initiative;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Models.Initiative;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;

using Microsoft.Net.Http.Headers;

namespace SE.Neo.WebAPI.Controllers
{
    [ApiController]
    [Authorize, Active]
    [Route("api/initiative")]

    public class InitiativeController : ControllerBase
    {
        private readonly IInitiativeApiService _initiativeApiService;

        #region Constructor
        public InitiativeController(IInitiativeApiService InitiativeApiService)
        {
            _initiativeApiService = InitiativeApiService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create or Update an initiative
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Roles(RoleType.Corporation)]
        [Route("create-or-update/{id?}")]
        public async Task<IActionResult> CreateOrUpdateInitiativeAsync(InitiativeCreateOrUpdateRequest request, int id = 0)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            List<int> initiativeIds = await _initiativeApiService.GetInitiativeIdsByUserId(currentUser.Id);
            // The below condition applies to only create part
            if (initiativeIds?.Count >= 3 && id == 0)
            {
                return BadRequest(CoreErrorMessages.ErrorOnInitiativeCreation);
            }
            InitiativeResponse result = await _initiativeApiService.CreateOrUpdateInitiativeAsync(id, request, currentUser);
            return result == null ? StatusCode(500) : Ok(result);
        }


        /// <summary>
        /// Get All Initiatives for admin module
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Roles(RoleType.Admin)]
        [HttpGet]
        [Route("get-all-initiatives")]
        public async Task<IActionResult> GetAllInitiativesAsync([FromQuery] BaseSearchFilterModel filter)
        {
            var intitiatives = await _initiativeApiService.GetAllInitiativesAsync(filter);
            return Ok(intitiatives);
        }


        /// <summary>
        /// Get list of Recommended Articles
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Roles(RoleType.Corporation)]
        [HttpGet]
        [Route("get-recommended-articles/{initiativeId}")]
        public async Task<IActionResult> GetRecommendedArticlesForInitiativeAsync(int initiativeId, [FromQuery] InitiativeRecommendationRequest request)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            InitiativeContentsWrapperModel<InitiativeArticleResponse> result = await _initiativeApiService.GetRecommendedArticlesForInitiativeAsync(request, currentUser, initiativeId);
            return Ok(result);
        }


        /// <summary>
        /// Get list of Saved Recommended Articles for an Initiative
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Roles(RoleType.Corporation, RoleType.Admin)]
        [HttpGet]
        [Route("get-saved-articles/{initiativeId}")]
        public async Task<IActionResult> GetSavedArticlesForInitiativeAsync(int initiativeId, [FromQuery] BaseSearchFilterModel filter)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            WrapperModel<InitiativeArticleResponse> response = await _initiativeApiService.GetSavedArticlesForInitiativeAsync(initiativeId, currentUser, filter, currentUser.RoleIds.Contains((int)RoleType.Admin));
            return Ok(response);
        }

        /// <summary>
        /// Save Initiative Contents for an initiative
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Roles(RoleType.Corporation)]
        [HttpPost]
        [Route("save-selected-content-items")]
        public async Task<IActionResult> SaveContentsToAnInitiativeAsync([FromBody] InitiativeContentRequest request)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            var result = await _initiativeApiService.SaveContentsToAnInitiativeAsync(request, currentUser.Id);
            return result == false ? StatusCode(500) : Ok(result);
        }


        /// <summary>
        /// Update sub step of an initiative progress tracker
        /// </summary>
        /// <param name="initiativeProgressRequest"></param>
        /// <returns></returns>
        [Roles(RoleType.Corporation)]
        [HttpPost]
        [Route("update-initiative-substep")]
        public async Task<IActionResult> UpdateInitiativeSubStepProgressAsync([FromBody] InitiativeSubStepRequest initiativeProgressRequest)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            var result = await _initiativeApiService.UpdateInitiativeSubStepProgressAsync(initiativeProgressRequest, currentUser.Id);
            return Ok(result);
        }

        /// <summary>
        /// Delete Initiative
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Roles(RoleType.Corporation)]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteInitiativeAsync(int id)
        {
            try
            {
                var currentUser = (UserModel)HttpContext.Items["User"]!;
                bool isDeleted = await _initiativeApiService.DeleteInitiativeAsync(currentUser.Id, id);
                return isDeleted ? Ok(isDeleted) : BadRequest("Record already marked as Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Remove content from an initiative
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contentId"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        [HttpDelete]
        [Roles(RoleType.Corporation, RoleType.Admin)]
        [Route("remove-saved-module-content-item/{id}/content/{contentId}/contentType/{contentType}")]
        public async Task<IActionResult> RemoveContentFromInitiativeAsync(int id, int contentId, InitiativeModule contentType)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            bool isRemoved = await _initiativeApiService.RemoveContentFromInitiativeAsync(currentUser.Id, id, contentId, contentType, currentUser.RoleIds.Contains((int)RoleType.Admin));
            return Ok(isRemoved);
        }

        /// <summary>
        /// Get Progress Tracker Details for all Initiatives for the user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Roles(RoleType.Corporation, RoleType.Admin)]
        [Route("get-user-all-initiatives/{initiativeType}")]
        public async Task<IActionResult> GetInitiativesAndProgressDetailsByUserIdAsync(InitiativeViewSource initiativeType, [FromQuery] BaseSearchFilterModel filter)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            WrapperModel<InitiativeAndProgressDetailsResponse> result = await _initiativeApiService.GetInitiativesAndProgressTrackerDetailsByUserIdAsync(currentUser.Id, currentUser.RoleIds, currentUser.CompanyId, initiativeType, filter);
            return Ok(result);
        }

        /// <summary>
        /// Get Progress Tracker Details for an Initiative
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Roles(RoleType.Corporation, RoleType.Admin)]
        [Route("get-initiative-and-progress-details/{id}/{isEditMode?}")]
        [HttpGet]
        public async Task<IActionResult> GetInitiativeAndProgressTrackerDetailsByIdAsync(int id, bool isEditMode = false)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            InitiativeAndProgressDetailsResponse result = await _initiativeApiService.GetInitiativeAndProgressTrackerDetailsByIdAsync(id, currentUser.Id, isEditMode, currentUser.RoleIds.Contains((int)RoleType.Admin));
            return result != null ? Ok(result) : NotFound();
        }
        /// <summary>
        /// Get the count of new recommendations for an initiative.
        /// </summary>
        /// <param name="request">The request containing the initiative details.</param>
        /// <returns>The count of new recommendations.</returns>
        [HttpPost]
        [Roles(RoleType.Corporation, RoleType.Admin)]
        [Route("get-new-recommendations-count")]
        public async Task<IActionResult> GetInitiativeRecommendationCountAsync([FromBody] InitiativeRecommendationCountRequest request)
        {
            var result = await _initiativeApiService.GetNewRecommendationsCountAsync((UserModel)HttpContext.Items["User"]!, request);
            return Ok(result);
        }


        /// <summary>
        /// Get list of Recommended Messages for the Initiative
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Roles(RoleType.Corporation)]
        [HttpGet]
        [Route("get-recommended-conversations/{initiativeId}")]
        public async Task<IActionResult> GetRecommendedConversationsForInitiativeAsync(int initiativeId, [FromQuery] InitiativeRecommendationRequest request)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            WrapperModel<InitiativeConversationResponse> result = await _initiativeApiService.GetRecommendedConversationsForInitiativeAsync(request, currentUser, initiativeId);
            return Ok(result);
        }


        /// <summary>
        /// Get list of Saved Recommended Conversations for an Initiative
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Roles(RoleType.Corporation, RoleType.Admin)]
        [HttpGet]
        [Route("get-saved-conversations/{initiativeId}")]
        public async Task<IActionResult> GetSavedConversationsForInitiativeAsync(int initiativeId, [FromQuery] BaseSearchFilterModel filter)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            WrapperModel<InitiativeConversationResponse> response = await _initiativeApiService.GetSavedConversationsForInitiativeAsync(initiativeId, currentUser, filter, currentUser.RoleIds.Contains((int)RoleType.Admin));
            return Ok(response);
        }

        /// <summary>
        /// Get list of Recommended Tools
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Roles(RoleType.Corporation)]
        [HttpGet]
        [Route("get-recommended-tools/{initiativeId}")]
        public async Task<IActionResult> GetRecommendedToolsForInitiativeAsync(int initiativeId, [FromQuery] InitiativeRecommendationRequest request)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            InitiativeContentsWrapperModel<InitiativeToolResponse> result = await _initiativeApiService.GetRecommendedToolsForInitiativeAsync(request, currentUser, initiativeId);
            return Ok(result);
        }

        /// <summary>
        /// Get list of Saved Recommended Tools for an Initiative
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Roles(RoleType.Corporation, RoleType.Admin)]
        [HttpGet]
        [Route("get-saved-tools/{initiativeId}")]
        public async Task<IActionResult> GetSavedToolsForInitiativeAsync(int initiativeId, [FromQuery] BaseSearchFilterModel filter)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            WrapperModel<InitiativeToolResponse> response = await _initiativeApiService.GetSavedToolsForInitiativeAsync(initiativeId, currentUser, filter, currentUser.RoleIds.Contains((int)RoleType.Admin));
            return Ok(response);
        }

        /// <summary>
        /// Get list of Recommended Community Users
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Roles(RoleType.Corporation)]
        [HttpGet]
        [Route("get-recommended-community-users/{initiativeId}")]
        public async Task<IActionResult> GetRecommendedCommunityUsersForInitiativeAsync(int initiativeId, [FromQuery] InitiativeRecommendationRequest request)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            InitiativeContentsWrapperModel<InitiativeCommunityUserResponse> result = await _initiativeApiService.GetRecommendedCommunityUsersForInitiativeAsync(request, currentUser, initiativeId);
            return Ok(result);
        }

        /// <summary>
        /// Add contnent to initiatives from details page
        /// </summary>
        /// <param name="attachContentToInitiativeRequest">contains contnent details with initiaitve ids</param>
        /// <returns></returns>
        [HttpPut]
        [Roles(RoleType.Corporation)]
        [Route("attach-content-to-initiative")]
        public async Task<IActionResult> AttachContentToInitiativeAsync(AttachContentToInitiativeRequest attachContentToInitiativeRequest)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            bool isAdded = await _initiativeApiService.AttachContentToInitiativeAsync(currentUser.Id, attachContentToInitiativeRequest);
            return Ok(isAdded);
        }

        /// <summary>
        /// Get initiatives attachment details for content
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        [HttpGet]
        [Roles(RoleType.Corporation)]
        [Route("get-initiatives-with-attached-content-details/{contentId}/contentType/{contentType}")]
        public async Task<IActionResult> GetInitiativesByContentIdAsync(int contentId, InitiativeModule contentType)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            var result = await _initiativeApiService.GetInitiativesByContentIdAsync(currentUser.Id, contentId, contentType);
            return Ok(result);
        }

        /// <summary>
        /// Upload initiative files
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Roles(RoleType.Corporation, RoleType.Admin)]
        [HttpPost]
        [Route("upload-file/{initiativeId}")]
        public async Task<IActionResult> UploadFileByInitiativeId(int initiativeId, [FromBody] InitiativeFileRequest request)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            var result = await _initiativeApiService.UploadFileToAnInitiativeAsync(request, currentUser.Id, initiativeId, currentUser.RoleIds.Contains((int)RoleType.Admin));
            return result == false ? StatusCode(500) : Ok(result);
        }

        /// <summary>
        /// Get list of Saved Files of an Initiative
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="filter"></param>
        /// <remarks>
        /// </remarks>
        [Roles(RoleType.Corporation, RoleType.Admin)]
        [HttpGet]
        [Route("get-saved-files/{initiativeId}")]
        public async Task<IActionResult> GetSavedFilesOfAnInitiativeAsync(int initiativeId, [FromQuery] BaseSearchFilterModel filter)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            WrapperModel<InitiativeFileResponse> response = await _initiativeApiService.GetSavedFilesOfAnInitiativeAsync(initiativeId, currentUser, filter, currentUser.RoleIds.Contains((int)RoleType.Admin));
            return Ok(response);
        }
        /// <summary>
        /// Get list of Recommended Projects
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Roles(RoleType.Corporation)]
        [HttpGet]
        [Route("get-recommended-projects/{initiativeId}")]
        public async Task<IActionResult> GetRecommendedProjectsForInitiativeAsync(int initiativeId, [FromQuery] InitiativeRecommendationRequest request)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            InitiativeContentsWrapperModel<InitiativeProjectResponse> result = await _initiativeApiService.GetRecommendedProjectsForInitiativeAsync(request, currentUser, initiativeId);
            return Ok(result);
        }

        /// <summary>
        /// Check the file exist or not
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="fileName"></param>
        /// <remarks>
        /// </remarks>
        [Roles(RoleType.Corporation, RoleType.Admin)]
        [HttpGet]
        [Route("validate-file-count-and-file-existence/{initiativeId}/{fileName}")]
        public async Task<IActionResult> ValidateFileCountAndIfExistsByInitiativeIdAsync(int initiativeId, string fileName)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            var result = await _initiativeApiService.ValidateFileCountAndIfExistsByInitiativeIdAsync(initiativeId, fileName, currentUser, currentUser.RoleIds.Contains((int)RoleType.Admin));
            return Ok(result);
        }

        /// <summary>
        /// Get list of Saved Projects of an Initiative
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="filter"></param>
        /// <remarks>
        /// </remarks>
        [Roles(RoleType.Corporation, RoleType.Admin)]
        [HttpGet]
        [Route("get-saved-projects/{initiativeId}")]
        public async Task<IActionResult> GetSavedProjectsForAnInitiativeAsync(int initiativeId, [FromQuery] BaseSearchFilterModel filter)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            WrapperModel<InitiativeProjectResponse> response = await _initiativeApiService.GetSavedProjectsForInitiativeAsync(initiativeId, currentUser, filter, currentUser.RoleIds.Contains((int)RoleType.Admin));
            return Ok(response);
        }


        /// <summary>
        /// Get list of Saved Community Users for an Initiative
        /// </summary>
        /// <param name="initiativeId">The ID of the initiative.</param>
        /// <param name="filter">The filter criteria for the search.</param>
        /// <returns>A list of saved community users for the specified initiative.</returns>
        [Roles(RoleType.Corporation, RoleType.Admin)]
        [HttpGet]
        [Route("get-saved-community-users/{initiativeId}")]
        public async Task<IActionResult> GetSavedCommunityUsersForInitiativeAsync(int initiativeId, [FromQuery] BaseSearchFilterModel filter)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            WrapperModel<InitiativeCommunityUserResponse> response = await _initiativeApiService.GetSavedCommunityUsersForInitiativeAsync(initiativeId, currentUser, filter, currentUser.RoleIds.Contains((int)RoleType.Admin));
            return Ok(response);
        }
        /// <summary>
        /// Update the last viewed date of initiative content.
        /// </summary>
        /// <param name="request">The request containing the initiative content recommendation activity details.</param>
        /// <returns>The result of the update operation.</returns>
        [HttpPost]
        [Roles(RoleType.Corporation)]
        [Route("update-initiative-content-lastViewedDate")]
        public async Task<IActionResult> UpdateInitiativeContentLastViewedDate([FromBody] InitiativeContentRecommendationActivityRequest request)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            var result = await _initiativeApiService.UpdateInitiativeContentLastViewedDate(request);
            return Ok(result);
        }
        /// <summary>
        /// Export all the initiative details.
        /// </summary>
        /// <param name="filter">The filter criteria for the search.</param>
        /// <returns>A file containing the exported initiative details.</returns>
        [Roles(RoleType.Admin)]
        [HttpGet]
        [Route("export-all-initiatives")]
        [Permissions(PermissionType.AdminAll)]
        public async Task<IActionResult> ExportInitiativesAsync([FromQuery] BaseSearchFilterModel filter)
        {
            var currentUser = (UserModel)Request.HttpContext.Items["User"]!;
            MemoryStream intitiativesStream = new MemoryStream();
            int intitiativesCount = await _initiativeApiService.ExportInitiativesAsync(filter, currentUser, intitiativesStream);
            EntityTagHeaderValue intitiativesFoundCount = new EntityTagHeaderValue("\"" + $"Found {intitiativesCount} Feedbacks" + "\"");
            return File(intitiativesStream, "text/csv", $"Export file {DateTime.UtcNow.Month}_{DateTime.UtcNow.Day}_{DateTime.UtcNow.Year}.csv", null, intitiativesFoundCount);
        }

        /// <summary>
        /// Update the modified date of an initiative file.
        /// </summary>
        /// <param name="fileName">The name of the file to update.</param>
        /// <param name="fileSize">The size of the file to update.</param>
        /// <param name="initiativeId">The ID of the initiative</param>
        /// <returns>The result of the update operation.</returns>
        [HttpPut]
        [Roles(RoleType.Corporation, RoleType.Admin)]
        [Route("update-initiative-file-modifiedDate-size/{initiativeId}")]
        public async Task<IActionResult> UpdateInitiativeFileModifiedDateAndSize(int initiativeId, [FromQuery] InitiativeFileUpdateRequest initiativeFile)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            var result = await _initiativeApiService.UpdateInitiativeFileModifiedDateAndSize(initiativeFile.FileName, initiativeFile.FileSize, initiativeId,currentUser.Id);
            return Ok(result);
        }

        #endregion
    }
}
