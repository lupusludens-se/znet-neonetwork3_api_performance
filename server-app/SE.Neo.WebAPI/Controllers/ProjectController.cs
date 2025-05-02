using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Extensions;
using SE.Neo.WebAPI.Models.Project;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Controllers
{
    [ApiController]
    [Authorize, Active]
    [Route("api")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectApiService _projectCatalogApiService;
        private readonly ILogger<ProjectController> _logger;
        private readonly IUserApiService _userApiService;
        private readonly IRecommenderSystemService _recommenderSystemService;

        public ProjectController(
            IProjectApiService projectCatalogApiService,
            ILogger<ProjectController> logger,
            IRecommenderSystemService recommenderSystemService,
            IUserApiService userApiService)
        {
            _projectCatalogApiService = projectCatalogApiService;
            _recommenderSystemService = recommenderSystemService;
            _logger = logger;
            _userApiService = userApiService;
        }

        /// <summary>
        /// Checks if the current user has access to all projects.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        /// <param name="projectsViewType">The type of projects view.</param>
        /// <returns>True if the user has access, otherwise false.</returns>
        private bool CheckAllProjectsAccess(UserModel currentUser, ProjectsViewType projectsViewType = default(ProjectsViewType))
        {
            return ((projectsViewType == default(ProjectsViewType) || projectsViewType == ProjectsViewType.Catalog)
                    && currentUser.PermissionIds.Any(pi => pi == (int)PermissionType.ProjectCatalogView))
                || ((projectsViewType == default(ProjectsViewType) || projectsViewType == ProjectsViewType.Library)
                    && currentUser.PermissionIds.Any(pi => pi == (int)PermissionType.ProjectManagementAll));
        }

        /// <summary>
        /// Checks if the current user has access to a specific project view type.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        /// <param name="projectsViewType">The type of projects view.</param>
        /// <returns>True if the user has access, otherwise false.</returns>

        private bool CheckProjectViewTypeAccess(UserModel currentUser, ProjectsViewType projectsViewType)
        {
            return (projectsViewType == ProjectsViewType.Catalog && currentUser.PermissionIds.Any(pi => pi == (int)PermissionType.ProjectCatalogView))
                || (projectsViewType == ProjectsViewType.Library
                    && currentUser.PermissionIds.Any(pi => pi == (int)PermissionType.ProjectManagementOwn || pi == (int)PermissionType.ProjectManagementAll)
                    && !_userApiService.IsInternalCorporationUser(currentUser.Id));
        }

        /// <summary>
        /// Builds the project predicate flag based on the current user and project view type.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        /// <param name="projectsViewType">The type of projects view.</param>
        /// <returns>The project predicate flag.</returns>

        private ProjectsPredicateFlag BuildProjectPredicateFlag(UserModel currentUser, ProjectsViewType projectsViewType = default(ProjectsViewType))
        {
            ProjectsPredicateFlag flag = default(ProjectsPredicateFlag);
            if (CheckAllProjectsAccess(currentUser, projectsViewType))
                flag |= ProjectsPredicateFlag.AccessAll;

            if (projectsViewType == ProjectsViewType.Catalog)
                flag |= ProjectsPredicateFlag.ActiveOnly;

            return flag;
        }

        /// <summary>
        /// Creates or updates a project asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of project details request.</typeparam>
        /// <param name="model">The project detailed request model.</param>
        /// <param name="validator">The validator for the project detailed request model.</param>
        /// <param name="id">The project ID (0 for create).</param>
        /// <returns>An IActionResult representing the result of the operation.</returns>

        private async Task<IActionResult> CreateUpdateProjectAsync<T>(
            BaseProjectDetailedRequest<T> model,
            IValidator<BaseProjectDetailedRequest<T>> validator, int id = 0) where T : BaseProjectDetailsRequest
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            bool isAbleToEditOthers = currentUser.PermissionIds.Any(pi => pi == (int)PermissionType.ProjectManagementAll);
            bool isSPAdmin = currentUser.PermissionIds.Any(pi => pi == (int)PermissionType.ManageCompanyProjects);

            if (isSPAdmin)
            {
                if (id > 0)
                {
                    if (model.Project.CompanyId == currentUser.CompanyId)
                    {
                        if (!model.Project.OwnerId.HasValue)
                        {
                            model.Project.OwnerId = currentUser.Id;
                        }
                    }
                    else
                    {
                        return Unauthorized(403);
                    }
                }
                else
                {
                    model.Project.OwnerId = currentUser.Id;
                }
            }
            else
            {
                if (!isAbleToEditOthers || !model.Project.OwnerId.HasValue)
                {
                    model.Project.OwnerId = currentUser.Id;
                }
            }

            try
            {
                return await this.ManualValidation(model, validator)
                    ?? (IActionResult)Ok(
                        await _projectCatalogApiService.CreateUpdateProjectAsync(
                            model.Project,
                            model.ProjectDetails,
                            currentUser,
                            isAbleToEditOthers,
                            id));
            }
            catch (CustomException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new Offsite Power Purchase Agreement project.
        /// </summary>
        /// <param name="model">The project request model.</param>
        /// <param name="validator">The validator for the project request model.</param>
        /// <returns>An IActionResult representing the result of the operation.</returns>

        [HttpPost($"projects/{CategoriesSlugs.OffsitePowerPurchaseAgreement}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PostProjectOffsitePowerPurchaseAgreement(
            ProjectOffsitePowerPurchaseAgreementRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectOffsitePowerPurchaseAgreementDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator);
        }

        /// <summary>
        /// Updates an existing Offsite Power Purchase Agreement project.
        /// </summary>
        /// <param name="id">The project ID.</param>
        /// <param name="model">The project request model.</param>
        /// <param name="validator">The validator for the project request model.</param>
        /// <returns>An IActionResult representing the result of the operation.</returns>
        [HttpPut($"projects/{{id}}/{CategoriesSlugs.OffsitePowerPurchaseAgreement}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PutProjectOffsitePowerPurchaseAgreement(
            int id,
            ProjectOffsitePowerPurchaseAgreementRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectOffsitePowerPurchaseAgreementDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator, id);
        }

        /// <summary>
        /// Creates a new Onsite Solar project.
        /// </summary>
        /// <param name="model">The project request model.</param>
        /// <param name="validator">The validator for the project request model.</param>
        /// <returns>An IActionResult representing the result of the operation.</returns>
        [HttpPost($"projects/{CategoriesSlugs.OnsiteSolar}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PostProjectOnsiteSolar(
            ProjectOnsiteSolarRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectOnsiteSolarDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator);
        }

        /// <summary>
        /// Updates an existing Onsite Solar project.
        /// </summary>
        /// <param name="id">The project ID.</param>
        /// <param name="model">The project request model.</param>
        /// <param name="validator">The validator for the project request model.</param>
        /// <returns>An IActionResult representing the result of the operation.</returns>

        [HttpPut($"projects/{{id}}/{CategoriesSlugs.OnsiteSolar}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PutProjectOnsiteSolar(
            int id,
            ProjectOnsiteSolarRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectOnsiteSolarDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator, id);
        }

        /// <summary>
        /// Creates a new EV Charging project.
        /// </summary>
        /// <param name="model">The project request model.</param>
        /// <param name="validator">The validator for the project request model.</param>
        /// <returns>An IActionResult representing the result of the operation.</returns>

        [HttpPost($"projects/{CategoriesSlugs.EVCharging}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PostProjectEVCharging(
            ProjectEVChargingRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectEVChargingDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator);
        }

        /// <summary>
        /// Updates an existing EV Charging project.
        /// </summary>
        /// <param name="id">The project ID.</param>
        /// <param name="model">The project request model.</param>
        /// <param name="validator">The validator for the project request model.</param>
        /// <returns>An IActionResult representing the result of the operation.</returns>
        [HttpPut($"projects/{{id}}/{CategoriesSlugs.EVCharging}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PutProjectEVCharging(
            int id,
            ProjectEVChargingRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectEVChargingDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator, id);
        }

        /// <summary>
        /// Creates a new Battery Storage project.
        /// </summary>
        /// <param name="model">The project request model.</param>
        /// <param name="validator">The validator for the project request model.</param>
        /// <returns>An IActionResult representing the result of the operation.</returns>

        [HttpPost($"projects/{CategoriesSlugs.BatteryStorage}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PostProjectBatteryStorage(
            ProjectBatteryStorageRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectBatteryStorageDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator);
        }

        /// <summary>
        /// Updates an existing Battery Storage project.
        /// </summary>
        /// <param name="id">The project ID.</param>
        /// <param name="model">The project request model.</param>
        /// <param name="validator">The validator for the project request model.</param>
        /// <returns>An IActionResult representing the result of the operation.</returns>

        [HttpPut($"projects/{{id}}/{CategoriesSlugs.BatteryStorage}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PutProjectBatteryStorage(
            int id,
            ProjectBatteryStorageRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectBatteryStorageDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator, id);
        }

        /// <summary>
        /// Creates a new Emerging Technology project.
        /// </summary>
        /// <param name="model">The project request model.</param>
        /// <param name="validator">The validator for the project request model.</param>
        /// <returns>An IActionResult representing the result of the operation.</returns>

        [HttpPost($"projects/{CategoriesSlugs.EmergingTechnology}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PostProjectEmergingTechnology(
            ProjectEmergingTechnologyRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectEmergingTechnologyDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator);
        }

        /// <summary>
        /// Updates an existing Emerging Technology project.
        /// </summary>
        /// <param name="id">The project ID.</param>
        /// <param name="model">The project request model.</param>
        /// <param name="validator">The validator for the project request model.</param>
        /// <returns>An IActionResult representing the result of the operation.</returns>

        [HttpPut($"projects/{{id}}/{CategoriesSlugs.EmergingTechnology}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PutProjectEmergingTechnology(
            int id,
            ProjectEmergingTechnologyRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectEmergingTechnologyDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator, id);
        }

        /// <summary>
        /// Creates a new Community Solar project.
        /// </summary>
        /// <param name="model">The request model containing the details of the Community Solar project.</param>
        /// <param name="validator">The validator for the request model.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the asynchronous operation.</returns>
        [HttpPost($"projects/{CategoriesSlugs.CommunitySolar}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PostProjectCommunitySolar(
            ProjectCommunitySolarRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectCommunitySolarDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator);
        }

        [HttpPut($"projects/{{id}}/{CategoriesSlugs.CommunitySolar}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PutProjectCommunitySolar(
            int id,
            ProjectCommunitySolarRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectCommunitySolarDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator, id);
        }

        /// <summary>
        /// Creates a new Fuel Cells project.
        /// </summary>
        /// <param name="model">The project details.</param>
        /// <param name="validator">The validator for the project details.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost($"projects/{CategoriesSlugs.FuelCells}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PostProjectFuelCells(
            ProjectFuelCellsRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectFuelCellsDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator);
        }

        /// <summary>
        /// Updates an existing Fuel Cells project.
        /// </summary>
        /// <param name="id">The project ID.</param>
        /// <param name="model">The project details.</param>
        /// <param name="validator">The validator for the project details.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut($"projects/{{id}}/{CategoriesSlugs.FuelCells}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PutProjectFuelCells(
            int id,
            ProjectFuelCellsRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectFuelCellsDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator, id);
        }

        /// <summary>
        /// Creates a new EAC project.
        /// </summary>
        /// <param name="model">The project details.</param>
        /// <param name="validator">The validator for the project details.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost($"projects/{CategoriesSlugs.EAC}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PostProjectEAC(
            ProjectEACRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectEACDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator);
        }

        /// <summary>
        /// Updates an existing EAC project.
        /// </summary>
        /// <param name="id">The ID of the project to update.</param>
        /// <param name="model">The project details to update.</param>
        /// <param name="validator">The validator for the project details.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut($"projects/{{id}}/{CategoriesSlugs.EAC}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PutProjectEAC(
            int id,
            ProjectEACRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectEACDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator, id);
        }

        /// <summary>
        /// Creates a new Carbon Offsets project.
        /// </summary>
        /// <param name="model">The project details to create.</param>
        /// <param name="validator">The validator for the project details.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost($"projects/{CategoriesSlugs.CarbonOffsets}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PostProjectCarbonOffsets(
            ProjectCarbonOffsetsRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectCarbonOffsetsDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator);
        }

        /// <summary>
        /// Updates an existing Carbon Offsets project.
        /// </summary>
        /// <param name="id">The ID of the project to update.</param>
        /// <param name="model">The project details to update.</param>
        /// <param name="validator">The validator for the project details.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut($"projects/{{id}}/{CategoriesSlugs.CarbonOffsets}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PutProjectCarbonOffsets(
            int id,
            ProjectCarbonOffsetsRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectCarbonOffsetsDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator, id);
        }

        /// <summary>
        /// Creates a new Efficiency Equipment Measures project.
        /// </summary>
        /// <param name="model">The project details to create.</param>
        /// <param name="validator">The validator for the project details.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost($"projects/{CategoriesSlugs.EfficiencyEquipmentMeasures}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PostProjectEfficiencyEquipmentMeasures(
            ProjectEfficiencyEquipmentMeasuresRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectEfficiencyEquipmentMeasuresDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator);
        }

        /// <summary>
        /// Updates an existing Efficiency Equipment Measures project.
        /// </summary>
        /// <param name="id">The ID of the project to update.</param>
        /// <param name="model">The project details to update.</param>
        /// <param name="validator">The validator for the project details.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut($"projects/{{id}}/{CategoriesSlugs.EfficiencyEquipmentMeasures}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PutProjectCarbonOffsets(
            int id,
            ProjectEfficiencyEquipmentMeasuresRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectEfficiencyEquipmentMeasuresDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator, id);
        }

        /// <summary>
        /// Creates a new Efficiency Audits and Consulting project.
        /// </summary>
        /// <param name="model">The project details to create.</param>
        /// <param name="validator">The validator for the project details.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost($"projects/{CategoriesSlugs.EfficiencyAuditsAndConsulting}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PostProjectEfficiencyAuditsAndConsulting(
            ProjectEfficiencyAuditsAndConsultingRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectEfficiencyAuditsAndConsultingDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator);
        }

        /// <summary>
        /// Updates an existing Efficiency Audits and Consulting project.
        /// </summary>
        /// <param name="id">The ID of the project to update.</param>
        /// <param name="model">The project details to update.</param>
        /// <param name="validator">The validator for the project details.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut($"projects/{{id}}/{CategoriesSlugs.EfficiencyAuditsAndConsulting}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PutProjectEfficiencyAuditsAndConsulting(
            int id,
            ProjectEfficiencyAuditsAndConsultingRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectEfficiencyAuditsAndConsultingDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator, id);
        }

        /// <summary>
        /// Creates a new Green Tariffs project.
        /// </summary>
        /// <param name="model">The project details.</param>
        /// <param name="validator">The validator for the project details.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPost($"projects/{CategoriesSlugs.GreenTariffs}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PostProjectGreenTariffs(
            ProjectGreenTariffsRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectGreenTariffsDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator);
        }

        /// <summary>
        /// Updates an existing Green Tariffs project.
        /// </summary>
        /// <param name="id">The project ID.</param>
        /// <param name="model">The project details.</param>
        /// <param name="validator">The validator for the project details.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPut($"projects/{{id}}/{CategoriesSlugs.GreenTariffs}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PutProjectGreenTariffs(
            int id,
            ProjectGreenTariffsRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectGreenTariffsDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator, id);
        }

        /// <summary>
        /// Creates a new Renewable Retail project.
        /// </summary>
        /// <param name="model">The project details.</param>
        /// <param name="validator">The validator for the project details.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPost($"projects/{CategoriesSlugs.RenewableRetail}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PostProjectRenewableRetail(
            ProjectRenewableRetailRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectRenewableRetailDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator);
        }

        /// <summary>
        /// Updates an existing Renewable Retail project.
        /// </summary>
        /// <param name="id">The project ID.</param>
        /// <param name="model">The project details.</param>
        /// <param name="validator">The validator for the project details.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPut($"projects/{{id}}/{CategoriesSlugs.RenewableRetail}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PutProjectRenewableRetail(
            int id,
            ProjectRenewableRetailRequest model,
            [FromServices] IValidator<BaseProjectDetailedRequest<ProjectRenewableRetailDetailsRequest>> validator)
        {
            return await CreateUpdateProjectAsync(model, validator, id);
        }

        /// <summary>
        /// Duplicate a project.
        /// </summary>
        [HttpPost("projects/{id}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> DuplicateProject(int id, string projectName)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            int projectId =
                await _projectCatalogApiService.DuplicateProjectAsync(
                    id,
                    projectName,
                    currentUser,
                    currentUser.PermissionIds.Any(pi => pi == (int)PermissionType.ProjectManagementAll));
            return Ok(projectId);
        }

        /// <summary>
        /// Patch a project.
        /// </summary>
        /// <remarks>
        /// JsonPatchDocument -&gt; [{op: "replace", "path": "/StatusId", "value": 1}], [{op:
        /// "replace", "path": "/IsPinned", "value": 1}]
        /// </remarks>
        [HttpPatch("projects/{id}")]
        [Permissions(PermissionType.ProjectManagementOwn, PermissionType.ProjectManagementAll)]
        public async Task<IActionResult> PatchProjectStatus(int id, ProjectPatchRequest patchRequest)
        {
            JsonPatchDocument patchDoc = patchRequest.JsonPatchDocument;
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;

            if (!currentUser.PermissionIds.Any(pi => pi == (int)PermissionType.ProjectManagementAll)
                && (patchDoc.Operations.SingleOrDefault()?.path.Equals("/ispinned", StringComparison.InvariantCultureIgnoreCase) ?? false))
                return StatusCode(403);

            await _projectCatalogApiService.PatchProjectAsync(
                id,
                patchDoc,
                currentUser,
                currentUser.PermissionIds.Any(pi => pi == (int)PermissionType.ProjectManagementAll));
            return Ok(id);
        }

        /// <summary>
        /// Get list of projects
        /// </summary>
        /// <remarks>
        /// OrderBy -&gt; title.asc, title.desc, category.asc, category.desc, publishedon.asc,
        /// publishedon.desc, changedon.asc, changedon.desc, ispinned.asc, ispinned.desc,
        /// company.asc, company.desc, status.asc, status.desc, regions.asc, regions.desc <br/>
        /// Expand -&gt; category, category.resources, category.technologies, company,
        /// company.image, owner, technologies, technologies.resources, regions, projectdetails,
        /// saved <br/> FilterBy -&gt; foryou&amp;saved&amp;solutionids=1,2&amp;categoryids=1&amp;technologyids=1&amp;regionids=1&amp;statusids=1
        /// </remarks>
        [HttpGet("projects")]
        [Permissions(PermissionType.ProjectCatalogView, PermissionType.ProjectManagementAll, PermissionType.ProjectManagementOwn)]
        public async Task<IActionResult> GetProjects([FromQuery] BaseSearchFilterModel filter, [FromQuery] ProjectsViewType projectsViewType)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;

            if (!(projectsViewType == default(ProjectsViewType) || CheckProjectViewTypeAccess(currentUser, projectsViewType)))
                return StatusCode(403);

            if (projectsViewType == ProjectsViewType.Catalog && filter?.FilterBy?.Split("&").Contains("foryou") == true)
            {
                IEnumerable<int> projectIds = await _recommenderSystemService.GetRecommendedProjectIds(currentUser.Id);
                if (projectIds.Any())
                {
                    WrapperModel<ProjectResponse> recommendedProjects = await _projectCatalogApiService.GetRecommendedProjectsAsync(projectIds, filter, currentUser);

                    if (recommendedProjects?.Count > 0)
                    {
                        return Ok(recommendedProjects);
                    }
                }
            }
            ProjectsPredicateFlag flag = BuildProjectPredicateFlag(currentUser, projectsViewType);
            WrapperModel<ProjectResponse> projects =
                await _projectCatalogApiService.GetProjectsAsync(
                    filter,
                    currentUser,
                    flag,
                    projectsViewType);

            bool canViewProjectPrivateDetails = currentUser.PermissionIds.Any(pi => pi == (int)PermissionType.ProjectPrivateDetailsView);
            if (!canViewProjectPrivateDetails)
            {
                List<ProjectResponse> projectResponses = projects.DataList.ToList();
                for (int index = 0; index < projectResponses.Count; ++index)
                    if (!(projectResponses[index].OwnerId == currentUser.Id
                        || canViewProjectPrivateDetails)
                        && projectResponses[index].ProjectDetails is ProjectOffsitePowerPurchaseAgreementDetailsResponse)
                        projectResponses[index].ProjectDetails = new BaseProjectOffsitePowerPurchaseAgreementDetailsResponse(
                            (ProjectOffsitePowerPurchaseAgreementDetailsResponse)projectResponses[index].ProjectDetails);
                projects.DataList = projectResponses;
            }

            return Ok(projects);
        }

        /// <summary>
        /// Get a project
        /// </summary>
        /// <param name="id">Identification of the project</param>
        /// <param name="expand">
        /// category, category.resources, company, company.image, owner, technologies,
        /// technologies.resources, regions, projectdetails, saved
        /// </param>
        /// <returns></returns>
        [HttpGet("projects/{id}")]
        [Permissions(PermissionType.ProjectCatalogView, PermissionType.ProjectManagementAll, PermissionType.ProjectManagementOwn)]
        public async Task<IActionResult> GetProject(int id, string? expand, ProjectsViewType projectsViewType)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;

            if (!(projectsViewType == default(ProjectsViewType) || CheckProjectViewTypeAccess(currentUser, projectsViewType)))
                return StatusCode(403);

            ProjectsPredicateFlag flag = BuildProjectPredicateFlag(currentUser, projectsViewType);
            ProjectResponse? project = await _projectCatalogApiService
                .GetProjectAsync(
                    id,
                    currentUser,
                    expand,
                    flag);

            if (!(project?.OwnerId == currentUser.Id
                || currentUser.PermissionIds.Any(pi => pi == (int)PermissionType.ProjectPrivateDetailsView)) && !(project?.CompanyId == currentUser.CompanyId || currentUser.RoleIds.Any(p => p == (int)RoleType.SolutionProvider))
                && project?.ProjectDetails is ProjectOffsitePowerPurchaseAgreementDetailsResponse)
                project.ProjectDetails = new BaseProjectOffsitePowerPurchaseAgreementDetailsResponse(
                    (ProjectOffsitePowerPurchaseAgreementDetailsResponse)project.ProjectDetails);

            return project != null ? Ok(project) : NotFound();
        }

        /// <summary>
        /// Get a project
        /// </summary>
        /// <param name="id">Identification of the project</param>
        /// <param name="expand">
        /// category.resources, technologies.resources
        /// </param>
        /// <returns></returns>
        [HttpGet("project-resources/{id}")]
        [Permissions(PermissionType.ProjectCatalogView, PermissionType.ProjectManagementAll, PermissionType.ProjectManagementOwn)]
        public async Task<IActionResult> GetProjectResourceDetails(int id, string? expand, ProjectsViewType projectsViewType)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;

            if (!(projectsViewType == default(ProjectsViewType) || CheckProjectViewTypeAccess(currentUser, projectsViewType)))
                return StatusCode(403);

            ProjectsPredicateFlag flag = BuildProjectPredicateFlag(currentUser, projectsViewType);
            ProjectResourceResponse? projectResources = await _projectCatalogApiService
                .GetProjectResourceDetails(
                    id,
                    currentUser,
                    expand,
                    flag); 

            return projectResources != null ? Ok(projectResources) : NotFound();
        }

        /// <summary>
        /// Get list of recently viewed projects.
        /// </summary>
        /// <returns></returns>
        [HttpGet("projects/views")]
        [Permissions(PermissionType.ProjectCatalogView)]
        public async Task<IActionResult> GetProjectViews()
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            var projects = await _projectCatalogApiService.GetProjectViewsAsync(currentUser.Id);
            return Ok(projects);
        }

        /// <summary>
        /// Creates a new project view for the specified project.
        /// </summary>
        /// <param name="id">The ID of the project.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("projects/{id}/views")]
        [Permissions(PermissionType.ProjectCatalogView)]
        public async Task<IActionResult> PostProjectViews(int id)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            await _projectCatalogApiService.CreateProjectViewAsync(currentUser.Id, id);
            return Ok(id);
        }

        /// <summary>
        /// Retrieves new and trending projects for the current user.
        /// </summary>
        /// <returns>An IActionResult containing a wrapper model with the new and trending projects.</returns>
        [Roles(RoleType.Corporation)]
        [HttpGet("trending")]
        public async Task<IActionResult> GetNewAndTrendingProjects()
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            WrapperModel<NewTrendingProjectResponse> response = new WrapperModel<NewTrendingProjectResponse>();
            if (currentUser != null)
            {
                int userId = currentUser.Id;
                bool isNewUser = _userApiService.IsNewUser(userId);
                ProjectsPredicateFlag flag = BuildProjectPredicateFlag(currentUser, ProjectsViewType.Catalog);
                response = await _projectCatalogApiService.GetNewTrendingProjectsAsync(currentUser, isNewUser, flag);
            }
            return Ok(response);
        }

        /// <summary>
        /// Exports the projects based on the provided filter.
        /// </summary>
        /// <param name="filter">The filter criteria for exporting projects.</param>
        /// <returns>A CSV file containing the exported projects.</returns>
        [HttpGet("projects/export")]
        public async Task<IActionResult> ExportProjects([FromQuery] BaseSearchFilterModel filter)
        {
            var currentUser = (UserModel)Request.HttpContext.Items["User"]!;
            ProjectsPredicateFlag flag = BuildProjectPredicateFlag(currentUser, ProjectsViewType.Library);
            MemoryStream projectsStream = new MemoryStream();
            int projectCount = await _projectCatalogApiService.ExportProjectsAsync(filter, currentUser, projectsStream, flag);
            EntityTagHeaderValue projectsFoundCount = new EntityTagHeaderValue("\"" + $"Found {projectCount} Projects" + "\"");
            return File(projectsStream, "text/csv", $"Export file {DateTime.UtcNow.Month}_{DateTime.UtcNow.Day}_{DateTime.UtcNow.Year}.csv", null, projectsFoundCount);
        }
        /// <summary>
        /// Endpoint to get loggedin user's company Projects which are active and draft 
        /// </summary>
        /// <returns>Specific Project Details to be displayed on Carousal of SP dashboard</returns>
        [Roles(RoleType.SolutionProvider, RoleType.SPAdmin)]
        [HttpGet("dashboard/sp-projects")]
        public async Task<IActionResult> SolutionProviderProjects()
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            IList<SPDashboardProjectDetailsResponse> response = null;
            if (!(currentUser is null))
            {
                response = await _projectCatalogApiService.GetActiveandDraftProjects(currentUser);
            }
            return Ok(response);
        }


        /// <summary>
        /// Endpoint to get Solution Provider company Projects which are active
        /// </summary>
        /// <param name="id">The ID of the company.</param>
        /// <returns> List of  active projects part of the sp company </returns>
        [HttpGet("get-sp-company-active-projects/{id}")]
        public async Task<IActionResult> GetSPCompanyActiveProjects(int id, [FromQuery] BaseSearchFilterModel filter)
        {
            UserModel currentUser = (UserModel)HttpContext.Items["User"]!;
            WrapperModel<SPCompanyProjectResponse> response = await _projectCatalogApiService.GetActiveProjectsForSPCompany(filter, currentUser, id);
            return Ok(response);
        }
    }
}