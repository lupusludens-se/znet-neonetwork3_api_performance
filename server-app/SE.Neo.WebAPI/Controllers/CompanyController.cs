
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Constants;
using SE.Neo.WebAPI.Models;
using SE.Neo.WebAPI.Models.Company;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces; 

namespace SE.Neo.WebAPI.Controllers
{
    [Authorize, Active]
    [ApiController]
    [Route("api")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyApiService _companyApiService;
        private readonly int _seCompanyId = 1;

        /// <summary>  
        /// Initializes a new instance of the <see cref="CompanyController"/> class.  
        /// </summary>  
        /// <param name="companyApiService">The company API service.</param>  
        public CompanyController(ICompanyApiService companyApiService)
        {
            _companyApiService = companyApiService;
        }

        /// <summary>  
        /// Checks if the company request is unprocessable.  
        /// </summary>  
        /// <param name="company">The company request.</param>  
        /// <returns>An <see cref="UnprocessableEntityObjectResult"/> if the request is unprocessable; otherwise, null.</returns>  
        private UnprocessableEntityObjectResult? CheckUnprocessableEntity(CompanyRequest company)
        {
            if (company.TypeId == Common.Enums.CompanyType.Corporation && (company.Categories != null && company.Categories.Any()))
            {
                ModelState.AddModelError(nameof(company.Categories), ErrorMessages.CategoriesNotAllowedForCorporation);
                if (company.OffsitePPAs != null && company.OffsitePPAs.Any())
                    ModelState.AddModelError(nameof(company.OffsitePPAs), ErrorMessages.OffsitePPAsNotAllowedForCorporation);

                return UnprocessableEntity(new ValidationResponse(ModelState));
            }

            if (company.TypeId != Common.Enums.CompanyType.Corporation)
            {
                if (company.Categories == null || !company.Categories.Any())
                {
                    ModelState.AddModelError(nameof(company.Categories), ErrorMessages.CategoriesNotEmpty);
                    return UnprocessableEntity(new ValidationResponse(ModelState));
                }
            }
            if (company.About != null && (company.About?.Length > 12000))
            {
                ModelState.AddModelError(nameof(company.About), ErrorMessages.AboutMaxLength);
                return UnprocessableEntity(new ValidationResponse(ModelState));
            }

            if (company.AboutText != null && (company.AboutText?.Length > 4000))
            {
                ModelState.AddModelError(nameof(company.AboutText), ErrorMessages.AboutTextMaxLength);
                return UnprocessableEntity(new ValidationResponse(ModelState));
            }
            return null;
        }

        /// <summary>  
        /// Get List of Companies.  
        /// </summary>  
        /// <remarks>  
        /// Search -> Name  
        /// <br />  
        /// FilterBy -> statusids=1,2&amp;typeids=1&amp;categoryids=1  
        /// <br />  
        /// OrderBy -> name.asc, name.desc, type.asc, type.desc, status.asc, status.desc  
        /// <br />  
        /// Expand -> country, image, projects, users, categories, urllinks, offsiteppas, followers  
        /// </remarks>  
        /// <param name="filter">The filter model for searching and filtering companies.</param>  
        /// <returns>An <see cref="IActionResult"/> containing the list of companies.</returns>  
        [HttpGet("companies")]
        public async Task<IActionResult> GetCompanies([FromQuery] BaseSearchFilterModel filter)
        {
            UserModel currentUser = (UserModel)Request.HttpContext.Items["User"]!;
            var companies = await _companyApiService.GetCompaniesAsync(filter, currentUser.Id);
            return Ok(companies);
        }

        /// <summary>  
        /// Get Company By Id.  
        /// </summary>  
        /// <param name="id">The company ID.</param>  
        /// <param name="expand">The fields to expand.</param>  
        /// <returns>An <see cref="IActionResult"/> containing the company details.</returns>  
        [HttpGet("companies/{id}")]
        public async Task<IActionResult> GetCompany(int id, string? expand)
        {
            UserModel currentUser = (UserModel)Request.HttpContext.Items["User"]!;
            var company = await _companyApiService.GetCompanyAsync(id, currentUser.Id, expand);
            return company != null ? Ok(company) : NotFound();
        }

        /// <summary>  
        /// Patch a company.  
        /// </summary>  
        /// <remarks>  
        /// JsonPatchDocument -> [{op: "replace", "path": "/StatusId", "value": 1}]  
        /// <br />  
        /// Only path /StatusId currently allowed, do not pass contractResolver, operationType, or from.  
        /// </remarks>  
        /// <param name="id">The company ID.</param>  
        /// <param name="patchRequest">The patch request model.</param>  
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>  
        [Permissions(PermissionType.CompanyManagement)]
        [HttpPatch("companies/{id}")]
        public async Task<IActionResult> PatchCompany(int id, CompanyJsonPatchRequest patchRequest)
        {
            if (id == _seCompanyId &&
                 int.Parse(patchRequest.JsonPatchDocument.Operations.FirstOrDefault().value.ToString())
                    != (int)CompanyStatus.Active)
                return StatusCode(403);

            JsonPatchDocument patchDoc = patchRequest.JsonPatchDocument;
            await _companyApiService.PatchCompanyAsync(id, patchDoc);
            return Ok(id);
        }

        /// <summary>  
        /// Create a new company.  
        /// </summary>  
        /// <param name="model">The company request model.</param>  
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>  
        [Permissions(PermissionType.CompanyManagement)]
        [HttpPost("companies")]
        public async Task<IActionResult> PostCompany(CompanyRequest model)
        {
            return CheckUnprocessableEntity(model) ??
                (IActionResult)Ok(await _companyApiService.CreateUpdateCompanyAsync(0, model));
        }

        /// <summary>  
        /// Update an existing company.  
        /// </summary>  
        /// <param name="id">The company ID.</param>  
        /// <param name="model">The company request model.</param>  
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>  
        [Permissions(PermissionType.CompanyManagement, PermissionType.ManageOwnCompany)]
        [HttpPut("companies/{id}")]
        public async Task<IActionResult> PutCompany(int id, CompanyRequest model)
        {
            UserModel currentUser = (UserModel)Request.HttpContext.Items["User"]!;
            if ((currentUser.RoleIds.Contains((int)RoleType.SPAdmin)) && currentUser.CompanyId != model.CompanyId)
            {
                return StatusCode(403);
            }

            if (id == _seCompanyId &&
                (model.StatusId != CompanyStatus.Active || model.TypeId != Common.Enums.CompanyType.Corporation))
                return StatusCode(403);

            return CheckUnprocessableEntity(model) ??
                (IActionResult)Ok(await _companyApiService.CreateUpdateCompanyAsync(id, model));
        }

        /// <summary>  
        /// Get List of Industries.  
        /// </summary>  
        /// <returns>An <see cref="IActionResult"/> containing the list of industries.</returns>  
        [HttpGet("industries")]
        public async Task<IActionResult> GetIndustries()
        {
            var industries = await _companyApiService.GetIndustriesAsync();
            return Ok(industries);
        }

        /// <summary>  
        /// Get List of OffsitePPAs.  
        /// </summary>  
        /// <returns>An <see cref="IActionResult"/> containing the list of offsite PPAs.</returns>  
        [HttpGet("offsiteppas")]
        public async Task<IActionResult> GetOffsitePPAs()
        {
            var offsiteppas = await _companyApiService.GetOffsitePPAsAsync();
            return Ok(offsiteppas);
        }

        /// <summary>  
        /// Follow a company.  
        /// </summary>  
        /// <param name="id">The company ID.</param>  
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>  
        [HttpPost("companies/{id}/followers")]
        public async Task<IActionResult> PostUserFollowers(int id)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            await _companyApiService.CreateCompanyFollowerAsync(currentUser.Id, id);
            return Ok(id);
        }

        /// <summary>  
        /// Unfollow a company.  
        /// </summary>  
        /// <param name="id">The company ID.</param>  
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>  
        [HttpDelete("companies/{id}/followers")]
        public async Task<IActionResult> DeleteCompanyFollowers(int id)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            await _companyApiService.RemoveCompanyFollowerAsync(currentUser.Id, id);
            return Ok(id);
        }

        /// <summary>  
        /// Get List of Followed Companies.  
        /// </summary>  
        /// <remarks>  
        /// For Dashboard we should Take(8), Skip(0)  
        /// </remarks>  
        /// <param name="filter">The pagination model for filtering followed companies.</param>  
        /// <returns>An <see cref="IActionResult"/> containing the list of followed companies.</returns>  
        [HttpGet("companies/users/current/followed")]
        public async Task<IActionResult> GetFollowedCompanies([FromQuery] PaginationModel filter)
        {
            UserModel currentUser = (UserModel)Request.HttpContext.Items["User"]!;
            var companies = await _companyApiService.GetFollowedCompaniesAsync(currentUser.Id, filter);
            return Ok(companies);
        }

        /// <summary>
        /// Get list of Saved Private Files of a company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="filter"></param>
        /// <remarks>
        /// </remarks>
        [Roles(RoleType.SPAdmin, RoleType.Admin, RoleType.SolutionProvider)]
        [HttpGet]
        [Route("get-saved-private-files/{companyId}")]
        public async Task<IActionResult> GetSavedPrivateFilesOfACompanyAsync(int companyId, [FromQuery] BaseSearchFilterModel filter)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            WrapperModel<CompanyFileResponse> response = await _companyApiService.GetSavedFilesOfACompanyAsync(companyId, currentUser, filter, true);
            return Ok(response);
        }

        /// <summary>
        /// Get list of Saved Public Files of a company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-saved-public-files/{companyId}")]
        public async Task<IActionResult> GetSavedPublicFilesOfACompanyAsync(int companyId, [FromQuery] BaseSearchFilterModel filter)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            WrapperModel<CompanyFileResponse> response = await _companyApiService.GetSavedFilesOfACompanyAsync(companyId, currentUser, filter, false);
            return Ok(response);
        }

        /// <summary>
        /// Delete file of a company
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="isPrivate"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-file/{fileId}")]
        [Roles(RoleType.SPAdmin, RoleType.Admin)]
        public async Task<IActionResult> DeleteCompanyFileByTypeAsync(int fileId, bool isPrivate)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            bool isRemoved = await _companyApiService.DeleteCompanyFileByTypeAsync(currentUser.Id, currentUser.CompanyId, fileId, isPrivate, currentUser.RoleIds.Contains((int)RoleType.Admin));
            return Ok(isRemoved);
        }

        /// <summary>
        /// Check the file exist or not
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="fileName"></param>
        /// <param name="isPrivate"></param>
        /// <remarks>
        /// </remarks>
        [Roles(RoleType.SPAdmin, RoleType.Admin)]
        [HttpPost]
        [Route("validate-file-count-and-file-existence")]
        public async Task<IActionResult> ValidateFileCountAndIsFileExistByCompanyId([FromBody] SE.Neo.WebAPI.Models.Company.ValidateCompanyFileRequest request)

        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            var result = await _companyApiService.ValidateFileCountAndIfExistsByCompanyIdAsync(request.CompanyId, request.FileName, currentUser, request.IsPrivate);
            return Ok(result);
        }

        /// <summary>
        /// Upload company files
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Roles(RoleType.SPAdmin, RoleType.Admin)]
        [HttpPost]
        [Route("upload-file/{companyId}")]
        public async Task<IActionResult> UploadFileByCompanyIdAsync(int companyId, [FromBody] CompanyFileRequest request)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            var result = await _companyApiService.UploadFileByCompanyIdAsync(request, currentUser.Id, companyId);
            return result == false ? StatusCode(500) : Ok(result);
        }

        /// <summary>
        /// Update the modified date of a company file.
        /// </summary>
        /// <param name="fileName">The name of the file to update.</param>
        /// <returns>The result of the update operation.</returns>
        [HttpPut]
        [Roles(RoleType.SPAdmin, RoleType.Admin)]
        [Route("update-company-file-modified-date-size/{fileName}/{fileSize}")]
        public async Task<IActionResult> UpdateCompanyFileModifiedDateAndSize(string fileName, int fileSize)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            var result = await _companyApiService.UpdateCompanyFileModifiedDateAndSize(fileName, fileSize);
            return Ok(result);
        }

        /// <summary>
        /// Update the modified date of a company file.
        /// </summary>
        /// <param name="fileId">File Id.</param>
        /// <param name="fileTitle">The name of the file to update.</param>
        /// <returns>The result of the update operation.</returns>
        [HttpPut]
        [Roles(RoleType.SPAdmin, RoleType.Admin)]
        [Route("update-company-file-title/{fileId}")]
        public async Task<IActionResult> UpdateFileTitleOfSelectedFileByCompany(int fileId, string fileTitle)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            var result = await _companyApiService.UpdateFileTitleOfSelectedFileByCompany(fileTitle, fileId, currentUser.Id, currentUser.RoleIds.Contains((int)RoleType.Admin), currentUser.CompanyId);
            return Ok(result);
        }


        /// <summary>
        /// Create or Update an Company Announcement
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Roles(RoleType.SPAdmin, RoleType.Admin)]
        [Route("create-or-update/{id?}")]
        public async Task<IActionResult> CreateOrUpdateCompanyAnnouncementAsync(CompanyAnnouncementCreateOrUpdateRequest request, int id = 0)
        {
            var currentUser = (UserModel)HttpContext.Items["User"]!;
            if (request.CompanyId == currentUser.CompanyId || currentUser.RoleIds.Contains((int)RoleType.Admin))
            {
                bool result = await _companyApiService.CreateOrUpdateCompanyAnnouncementAsync(id, request, currentUser);

                return Ok(result);
            }
            return BadRequest();
        }


        /// <summary>
        /// Get All Company Announcements for admin module
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-all-company-announcements/{companyId}")]
        public async Task<IActionResult> GetAllCompanyAnnouncementsAsync([FromQuery] BaseSearchFilterModel filter, int companyId)
        {
            var companyAnnouncements = await _companyApiService.GetAllCompanyAnnouncementsAsync(filter, companyId);
            return Ok(companyAnnouncements);
        }

        /// <summary>
        /// Get Company Announcement by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-company-announcement-by-id/{id}")]
        public async Task<IActionResult> GetCompanyAnnouncementByIdAsync(int id)
        {
            var companyAnnouncement = await _companyApiService.GetCompanyAnnouncementByIdAsync(id);
            return Ok(companyAnnouncement);
        }

        /// <summary>
        /// Delete Company Announcement
        /// </summary>
        /// <param name="announcementId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Roles(RoleType.SPAdmin, RoleType.Admin)]
        [Route("delete-announcement/{announcementId}")]
        public async Task<IActionResult> DeleteCompanyAnnouncementAsync(int announcementId)
        {
            try
            {
                var currentUser = (UserModel)HttpContext.Items["User"]!;
                bool isDeleted = await _companyApiService.DeleteCompanyAnnouncementAsync(currentUser.Id, announcementId, currentUser.RoleIds.Contains((int)RoleType.Admin));
                return isDeleted ? Ok(isDeleted) : BadRequest("Record already marked as Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
