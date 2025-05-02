using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Notifications.Details.Multiple;
using SE.Neo.Common.Models.Notifications.Details.Single;
using SE.Neo.Common.Models.Project;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Services.Interfaces;
using SE.Neo.WebAPI.Constants;
using SE.Neo.WebAPI.Models.Project;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;
using System.Globalization;
using System.Text;

namespace SE.Neo.WebAPI.Services
{
    public class ProjectApiService : BaseProjectApiService, IProjectApiService
    {
        private readonly ILogger<ProjectApiService> _logger;
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;
        private readonly INotificationService _notificationService;
        private readonly ICompanyService _companyService;
        private readonly ICommonService _commonService;
        private readonly ProjectTagsConfig _projectTagsConfig;
        private readonly IRecommenderSystemService _recommenderSystemService;
        private readonly IBlobServicesFacade _blobServicesFacade;
        private const string FilterForYou = "foryou";

        public ProjectApiService(
            ILogger<ProjectApiService> logger,
            IMapper mapper,
            IProjectService projectService,
            INotificationService notificationService,
            ICompanyService companyService,
            ICommonService commonService,
            IOptions<ProjectTagsConfig> projectTagsConfig,
            IRecommenderSystemService recommenderSystemService,
            IBlobServicesFacade blobServicesFacade) : base(logger)
        {
            _logger = logger;
            _mapper = mapper;
            _projectService = projectService;
            _notificationService = notificationService;
            _companyService = companyService;
            _commonService = commonService;
            _projectTagsConfig = projectTagsConfig.Value;
            _recommenderSystemService = recommenderSystemService;
            _blobServicesFacade = blobServicesFacade;
        }

        public async Task<int> CreateUpdateProjectAsync(ProjectRequest project, BaseProjectDetailsRequest projectDetails, UserModel currentUser, bool accessAll = false, int id = 0)
        {
            ProjectDTO projectDTO = _mapper.Map<ProjectDTO>(project);
            _mapper.Map(projectDetails, projectDTO);
            BaseProjectDetailsDTO projectDetailsDTO = _mapper.Map<BaseProjectDetailsDTO>(projectDetails);
            projectDTO.Id = id;

            string categorySlug = (await _commonService.GetCategory(projectDTO.CategoryId)).Slug;
            switch (categorySlug)
            {
                case CategoriesSlugs.OffsitePowerPurchaseAgreement:
                    projectDTO.Tags = _projectTagsConfig.OffsitePPATags;
                    break;
                case CategoriesSlugs.OnsiteSolar:
                    projectDTO.Tags = _projectTagsConfig.OnsiteSolarTags;
                    break;
                case CategoriesSlugs.BatteryStorage:
                    projectDTO.Tags = _projectTagsConfig.BatteryStorageTags;
                    break;
                case CategoriesSlugs.FuelCells:
                    projectDTO.Tags = _projectTagsConfig.FuelCellsTags;
                    break;
                case CategoriesSlugs.CommunitySolar:
                    projectDTO.Tags = _projectTagsConfig.CommunitySolarTags;
                    break;
                case CategoriesSlugs.CarbonOffsets:
                    projectDTO.Tags = _projectTagsConfig.CarbonOffsetsTags;
                    break;
                case CategoriesSlugs.EAC:
                    projectDTO.Tags = _projectTagsConfig.EACTags;
                    break;
                case CategoriesSlugs.EfficiencyAuditsAndConsulting:
                    projectDTO.Tags = _projectTagsConfig.EfficiencyAuditsAndConsultingTags;
                    break;
                case CategoriesSlugs.EfficiencyEquipmentMeasures:
                    projectDTO.Tags = _projectTagsConfig.EfficiencyEquipmentMeasuresTags;
                    break;
                case CategoriesSlugs.EmergingTechnology:
                    projectDTO.Tags = _projectTagsConfig.EmergingTechnologyTags;
                    break;
                case CategoriesSlugs.EVCharging:
                    projectDTO.Tags = _projectTagsConfig.EVChargingTags;
                    break;
                case CategoriesSlugs.GreenTariffs:
                    projectDTO.Tags = _projectTagsConfig.GreenTariffsTags;
                    break;
                case CategoriesSlugs.RenewableRetail:
                    projectDTO.Tags = _projectTagsConfig.RenewableRetailTags;
                    break;
                case CategoriesSlugs.AggregatedPowerPurchaseAgreement:
                    projectDTO.Tags = _projectTagsConfig.AggregatedPowerPurchaseAgreementTags;
                    break;
                case CategoriesSlugs.MarketBrief:
                    projectDTO.Tags = _projectTagsConfig.MarketBriefTags;
                    break;
            }

            bool isEdit = id != 0;
            bool firstTimePublished = isEdit ? false : true;

            ProjectDTO? oldProjectDTO = null;
            if (isEdit)
            {
                oldProjectDTO = await _projectService.GetProjectAsync(id, currentUser.Id, currentUser.RoleIds, currentUser.CompanyId, expand: "regions,technologies,projectdetails", ProjectsPredicateFlag.AccessAll);
                if ((oldProjectDTO.StatusId == (int)ProjectStatus.Draft) && (projectDTO.StatusId == (int)ProjectStatus.Active))
                {
                    firstTimePublished = true;
                }
            }
            // projectDTO.FirstTimePublishedOn = oldProjectDTO?.FirstTimePublishedOn;
            int projectId = await _projectService.CreateUpdateProjectAsync(projectDTO, projectDetailsDTO, currentUser.Id, currentUser.CompanyId, accessAll);

            // notifications
            try
            {
                if (projectDTO.StatusId == (int)ProjectStatus.Active)
                {
                    ProjectDTO? newProjectDTO =
                                        await _projectService.GetProjectAsync(projectId, currentUser.Id, currentUser.RoleIds, currentUser.CompanyId, isEdit ? "regions,technologies,saved,projectdetails" : "company", ProjectsPredicateFlag.AccessAll);

                    if (isEdit && !firstTimePublished)
                    {
                        List<string>? changedProperties = oldProjectDTO!.FindChangedProperties(newProjectDTO!,
                            (oldCollection, newCollection, type) =>
                            {
                                return CompareEnumerableProjectProperty(oldCollection, newCollection, type);
                            });
                        if (changedProperties != null)
                        {
                            List<int> projectUsersSavedIds = newProjectDTO!.ProjectSaved.Select(ps => ps.UserId).ToList();
                            if (changedProperties.Count > 1)
                            {
                                await _notificationService.AddNotificationsAsync(projectUsersSavedIds,
                                    NotificationType.ChangesToProjectISaved, new ProjectNotificationDetails
                                    {
                                        ProjectId = projectId,
                                        ProjectTitle = newProjectDTO!.Title
                                    });
                            }
                            else if (changedProperties.Count == 1)
                            {
                                await _notificationService.AddNotificationsAsync(projectUsersSavedIds,
                                    NotificationType.ChangesToProjectISaved, new ChangeProjectNotificationDetails
                                    {
                                        ProjectId = projectId,
                                        ProjectTitle = newProjectDTO!.Title,
                                        FieldName = changedProperties.First()
                                    });
                            }
                        }
                    }
                    else
                    {
                        CompanyDTO? companyDTO = await _companyService.GetCompanyAsync(newProjectDTO!.CompanyId, expand: "followers.user.roles");
                        if (companyDTO != null && companyDTO.Followers != null)
                        {
                            List<int> usersIdsToNotify = companyDTO.Followers
                                .Where(f => !f.Follower.Roles.Any(r => r.Id == (int)RoleType.SolutionProvider || r.Id == (int)RoleType.SPAdmin))
                                .Select(cf => cf.FollowerId).Where(fId => fId != currentUser.Id)
                                .ToList();
                            await _notificationService.AddNotificationsAsync(usersIdsToNotify, NotificationType.CompanyIFollowPostProject,
                                    new CompanyProjectNotificationDetails
                                    {
                                        ProjectId = projectId,
                                        ProjectTitle = projectDTO.Title,
                                        CompanyId = companyDTO!.Id,
                                        CompanyName = companyDTO!.Name
                                    });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorAddingNotification);
            }
            return projectId;
        }

        public async Task CreateProjectViewAsync(int userId, int projectId)
        {
            await _projectService.CreateProjectViewAsync(userId, projectId);
        }

        public async Task<IEnumerable<RecentProjectResponse>> GetProjectViewsAsync(int userId)
        {
            IEnumerable<ProjectDTO> projectsResult = await _projectService.GetProjectViewsAsync(userId);

            List<RecentProjectResponse> recentProjectsResult = projectsResult.Select(_mapper.Map<RecentProjectResponse>).ToList();
            if (projectsResult.Count() > 0)
            {
                Dictionary<string, List<string>> projectTypeToImages = new Dictionary<string, List<string>>();
                Dictionary<string, List<string>> technologyTypeToImages = new Dictionary<string, List<string>>();

                Dictionary<string, int> projectTypeIndex = new Dictionary<string, int>();
                Dictionary<string, int> technologyTypeIndex = new Dictionary<string, int>();

                List<string> projectCategories = new List<string>();
                List<string> projectTechnologies = new List<string>();

                InitializeProjectImages(projectCategories, projectTypeToImages, projectTypeIndex);
                InitializeProjectTechnologyImages(projectTechnologies, technologyTypeToImages, technologyTypeIndex);
                foreach (RecentProjectResponse resultItem in recentProjectsResult)
                {
                    if(resultItem.Technologies?.Count() > 0)
                    {
                        resultItem.ProjectCategoryImage = GetNextImageForProjectType(resultItem.Technologies.ElementAt(0).Slug, technologyTypeToImages, technologyTypeIndex);
                    }
                    else
                    {
                        resultItem.ProjectCategoryImage = GetNextImageForProjectType(resultItem.Category.Slug, projectTypeToImages, projectTypeIndex);
                    }                  
                }
            }
            return recentProjectsResult;
        }

        public async Task<int> DuplicateProjectAsync(int id, string projectName, UserModel currentUser, bool accessAll = false)
        {
            int projectId = await _projectService.DuplicateProjectAsync(id, projectName, currentUser.Id, currentUser.CompanyId, currentUser.RoleIds, accessAll);
            return projectId;
        }

        public async Task PatchProjectAsync(int id, JsonPatchDocument patchDoc, UserModel currentUser, bool accessAll = false)
        {
            await _projectService.PatchProjectAsync(id, patchDoc, currentUser.Id, currentUser.CompanyId, accessAll);
        }

        public async Task<WrapperModel<ProjectResponse>> GetProjectsAsync(BaseSearchFilterModel filter, UserModel currentUser, ProjectsPredicateFlag flag = default(ProjectsPredicateFlag), ProjectsViewType projectsViewType = default(ProjectsViewType))
        {
            // filter by active projects for project catalog
            if (projectsViewType == ProjectsViewType.Catalog)
            {
                filter.FilterBy = $"statusids={(int)ProjectStatus.Active}&${(filter.FilterBy == null ? string.Empty : filter.FilterBy)}";
            }

            WrapperModel<ProjectDTO> projectsResult = await _projectService.GetProjectsAsync(filter, currentUser.Id, currentUser.RoleIds, currentUser.CompanyId, flag);

            var wrapper = new WrapperModel<ProjectResponse>
            {
                Count = projectsResult.Count,
                DataList = projectsResult.DataList.Select(_mapper.Map<ProjectResponse>),
            };

            List<ProjectResponse> modelResponses = wrapper.DataList.ToList();
            for (int index = 0; index < modelResponses.Count; ++index)
                _mapper.Map(projectsResult.DataList.ElementAt(index), modelResponses[index].ProjectDetails);
            wrapper.DataList = modelResponses;

            return wrapper;
        }

        public async Task<WrapperModel<ProjectResponse>> GetRecommendedProjectsAsync(IEnumerable<int> projectIds, BaseSearchFilterModel filter, UserModel currentUser)
        {
            WrapperModel<ProjectDTO> projectsResult = await _projectService.GetRecommendedProjectsAsync(projectIds, filter, currentUser.Id, currentUser.RoleIds, currentUser.CompanyId);

            var wrapper = new WrapperModel<ProjectResponse>
            {
                Count = projectsResult.Count,
                DataList = projectsResult.DataList.Select(_mapper.Map<ProjectResponse>),
            };

            List<ProjectResponse> modelResponses = wrapper.DataList.ToList();
            for (int index = 0; index < modelResponses.Count; ++index)
                _mapper.Map(projectsResult.DataList.ElementAt(index), modelResponses[index].ProjectDetails);
            wrapper.DataList = modelResponses;

            return wrapper;
        }

        public async Task<ProjectResponse?> GetProjectAsync(int id, UserModel currentUser, string? expand = null, ProjectsPredicateFlag flag = default(ProjectsPredicateFlag))
        {
            ProjectDTO? modelDTO = await _projectService.GetProjectAsync(id, currentUser.Id, currentUser.RoleIds, currentUser.CompanyId, expand, flag);
            ProjectResponse modelResponse = _mapper.Map<ProjectResponse>(modelDTO);
            if (modelDTO != null)
                _mapper.Map(modelDTO, modelResponse.ProjectDetails);
            return modelResponse;
        }


        public async Task<ProjectResourceResponse?> GetProjectResourceDetails(int id, UserModel currentUser, string? expand = null, ProjectsPredicateFlag flag = default(ProjectsPredicateFlag))
        {
            ProjectResourceResponseDTO? modelDTO = await _projectService.GetProjectResourceDetails(id, currentUser.Id, currentUser.RoleIds, currentUser.CompanyId, expand, flag);
            ProjectResourceResponse modelResponse = _mapper.Map<ProjectResourceResponse>(modelDTO);
            return modelResponse;
        }

        public async Task<WrapperModel<NewTrendingProjectResponse>> GetNewTrendingProjectsAsync(UserModel currentUser, bool isNew, ProjectsPredicateFlag flag)
        {
            List<ProjectDTO> newProjects = new List<ProjectDTO>();
            List<ProjectDTO> trendingProjects = new List<ProjectDTO>();
            List<NewTrendingProjectResponse> result = new List<NewTrendingProjectResponse>();
            IEnumerable<int> recommenderProjectIds = await _recommenderSystemService.GetRecommendedProjectIds(currentUser.Id);

            if (recommenderProjectIds.Any())
            {
                List<int> recommenderTopProjectIds = recommenderProjectIds.Take(9).ToList();
                List<int> trendingProjectIds = recommenderTopProjectIds.TakeLast(6).ToList();
                IList<ProjectDTO> projectDTOs = _projectService.GetProjectsById(recommenderTopProjectIds);
                if (isNew)
                {
                    result = await NewTrendingResponse(projectDTOs);
                }
                else
                {
                    IEnumerable<ProjectDTO> topThreeRecommenderProjectDTO = projectDTOs.Take(3);
                    IList<ProjectDTO> newProjectsFromRecommenderDTO = new List<ProjectDTO>();
                    int newProjectCount = 0;
                    foreach (ProjectDTO project in topThreeRecommenderProjectDTO)
                    {
                        if (project.FirstTimePublishedOn >= DateTime.UtcNow.AddMonths(-3))
                        {
                            newProjectCount += 1;
                            newProjectsFromRecommenderDTO.Add(project);
                        }
                    }
                    if (newProjectCount == 3)
                    {
                        newProjects.AddRange(projectDTOs.Take(3));
                    }
                    else
                    {
                        IList<ProjectDTO> newAllProjects = _projectService.GetNewProjectsForCorporateDashboard(currentUser.Id, trendingProjectIds);
                        newProjects.AddRange(newProjectsFromRecommenderDTO);
                        newProjects.AddRange(newAllProjects.Take(3 - newProjectCount).ToList());
                    }
                    List<NewTrendingProjectResponse> newProjectResponse = NewTrendingResponse(newProjects, DashboardProjectType.New.ToString()).Result.ToList();
                    trendingProjects = projectDTOs.TakeLast(6).ToList();
                    List<NewTrendingProjectResponse> trendingProjectResponse = NewTrendingResponse(trendingProjects, DashboardProjectType.Trending.ToString()).Result.ToList();

                    result = MergeList(trendingProjectResponse, newProjectResponse);
                }
            }
            else
            {
                BaseSearchFilterModel filter = new BaseSearchFilterModel()
                {
                    FilterBy = FilterForYou
                };
                filter.Expand = "category.technologies";
                WrapperModel<ProjectDTO> projectsResult = await _projectService.GetProjectsAsync(filter, currentUser.Id, currentUser.RoleIds, currentUser.CompanyId, flag, true);
                List<ProjectResponse> projectsResultResponse = projectsResult.DataList.Select(_mapper.Map<ProjectResponse>).ToList();
                bool canViewProjectPrivateDetails = currentUser.PermissionIds.Any(pi => pi == (int)PermissionType.ProjectPrivateDetailsView);
                if (!canViewProjectPrivateDetails)
                {
                    List<ProjectResponse> projectResponses = projectsResultResponse.ToList();
                    for (int index = 0; index < projectResponses.Count; ++index)
                        if (!(projectResponses[index].OwnerId == currentUser.Id
                            || canViewProjectPrivateDetails)
                            && projectResponses[index].ProjectDetails is ProjectOffsitePowerPurchaseAgreementDetailsResponse)
                            projectResponses[index].ProjectDetails = new BaseProjectOffsitePowerPurchaseAgreementDetailsResponse(
                                (ProjectOffsitePowerPurchaseAgreementDetailsResponse)projectResponses[index].ProjectDetails);
                    projectsResult.DataList = projectResponses.Select(_mapper.Map<ProjectDTO>).ToList();
                }

                newProjects = projectsResult.DataList.Take(3).ToList();
                trendingProjects = projectsResult.DataList.Skip(3).Take(6).ToList();
                List<NewTrendingProjectResponse> newProjectResponse = NewTrendingResponse(newProjects, DashboardProjectType.New.ToString()).Result.ToList();
                List<NewTrendingProjectResponse> trendingProjectResponse = NewTrendingResponse(trendingProjects, DashboardProjectType.Trending.ToString()).Result.ToList();
                result = MergeList(trendingProjectResponse, newProjectResponse);
            }

            Dictionary<string, List<string>> projectTypeToImages = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> technologyTypeToImages = new Dictionary<string, List<string>>();

            Dictionary<string, int> projectTypeIndex = new Dictionary<string, int>();
            Dictionary<string, int> technologyTypeIndex = new Dictionary<string, int>();

            List<string> projectCategories = new List<string>();
            List<string> projectTechnologies = new List<string>();

            InitializeProjectImages(projectCategories, projectTypeToImages, projectTypeIndex);
            InitializeProjectTechnologyImages(projectTechnologies, technologyTypeToImages, technologyTypeIndex);

            foreach (NewTrendingProjectResponse resultItem in result)
            {
                if (resultItem.Technologies.Count > 0)
                {
                    resultItem.ProjectCategoryImage = GetNextImageForProjectTechnology(resultItem.Technologies.ElementAt(0), technologyTypeToImages, technologyTypeIndex);
                }
                else
                {
                    resultItem.ProjectCategoryImage = GetNextImageForProjectType(resultItem.ProjectCategorySlug, projectTypeToImages, projectTypeIndex);
                }
                }
            var wrapper = new WrapperModel<NewTrendingProjectResponse>
            {
                Count = result.Count,
                DataList = result
            };
            return wrapper;
        }

        private static List<NewTrendingProjectResponse> MergeList(List<NewTrendingProjectResponse> trendingProjectResponse, List<NewTrendingProjectResponse> newProjectResponse)
        {
            List<NewTrendingProjectResponse> result = new List<NewTrendingProjectResponse>();
            int TermLengthType = Math.Min(newProjectResponse.Count, trendingProjectResponse.Count);

            for (int i = 0; i < TermLengthType; i++)
            {
                result.Add(trendingProjectResponse.ElementAt(i));
                result.Add(newProjectResponse.ElementAt(i));
            }
            result.AddRange(trendingProjectResponse.TakeLast(trendingProjectResponse.Count - TermLengthType));
            return result;
        }

        private async Task<List<NewTrendingProjectResponse>> NewTrendingResponse(IList<ProjectDTO> projectDTOs, string tag = null)
        {
            List<NewTrendingProjectResponse> result = new List<NewTrendingProjectResponse>();
            foreach (ProjectDTO project in projectDTOs)
            {
                await _blobServicesFacade.PopulateWithBlobAsync(project, dto => dto?.Company.Image, (dto, b) => { if (dto != null) dto.Company.Image = b; });
                result.Add(new NewTrendingProjectResponse
                {
                    Id = project.Id,
                    Title = project.Title,
                    SubTitle = project.SubTitle,
                    Description = project.Description,
                    CompanyImage = project.Company.Image?.Uri.ToString(),
                    Tag = tag,
                    ProjectCategoryId = project.CategoryId,
                    ProjectCategorySlug = project.Category.Slug,
                    Technologies = project.Technologies.Select(x => x.Slug).ToList()
                });
            }
            return result;
        }

        private bool CompareEnumerableProjectProperty(IEnumerable<dynamic> oldCollection, IEnumerable<dynamic> newCollection, Type type)
        {
            if (type == typeof(TechnologyDTO) || type == typeof(RegionDTO) || type == typeof(BaseIdNameDTO))
                return oldCollection.Select(el => el.Id).SequenceEqual(newCollection.Select(el => el.Id));
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="currentUser"></param>
        /// <param name="stream"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public async Task<int> ExportProjectsAsync(BaseSearchFilterModel filter, UserModel currentUser, MemoryStream stream, ProjectsPredicateFlag flag = default(ProjectsPredicateFlag))
        {
            string[] projectsWithTechnologies = new string[] { CategoriesSlugs.EfficiencyEquipmentMeasures, CategoriesSlugs.CarbonOffsets };
            filter.FilterBy = $"{(filter.FilterBy ?? string.Empty)}";
            List<string> projectTypesIncluded = new List<string>();
            bool isAdmin = currentUser.RoleIds.Contains((int)Common.Enums.RoleType.Admin);
            WrapperModel<ProjectDTO> projectsResult = await _projectService.GetExportProjectsAsync(filter, currentUser.Id, currentUser.RoleIds, currentUser.CompanyId, flag);
            var wrapper = new WrapperModel<ProjectResponse>
            {
                Count = projectsResult.Count,
                DataList = projectsResult.DataList.Select(_mapper.Map<ProjectResponse>),
            };
            List<ProjectResponse> modelResponses = wrapper.DataList.ToList();
            for (int index = 0; index < modelResponses.Count; ++index)
                _mapper.Map(projectsResult.DataList.ElementAt(index), modelResponses[index].ProjectDetails);
            wrapper.DataList = modelResponses;
            List<ProjectExportResponse> projects = new();
            bool hasOnlyPPA = true;
            for (int index = 0; index < modelResponses.Count; ++index)
            {
                var desc = string.IsNullOrEmpty(modelResponses[index].Description) ? string.Empty : HTMLExtensions.RemoveAllHTML(modelResponses[index].Description);
                var opportunity = string.IsNullOrEmpty(modelResponses[index].Opportunity) ? string.Empty : HTMLExtensions.RemoveAllHTML(modelResponses[index].Opportunity);
                string projectDetails = JsonConvert.SerializeObject(modelResponses[index].ProjectDetails, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                var project = new ProjectExportResponse()
                {
                    Description = Encoding.ASCII.GetString(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(Encoding.ASCII.EncodingName, new EncoderReplacementFallback(string.Empty), new DecoderExceptionFallback()), Encoding.UTF8.GetBytes(desc))),
                    Opportunity = Encoding.ASCII.GetString(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(Encoding.ASCII.EncodingName, new EncoderReplacementFallback(string.Empty), new DecoderExceptionFallback()), Encoding.UTF8.GetBytes(opportunity))),
                    SubTitle = modelResponses[index].SubTitle,
                    Title = modelResponses[index].Title,
                    Category = modelResponses[index].Category.Name,
                    CategorySlug = modelResponses[index].Category.Slug,
                    Technologies = string.Join(",", modelResponses[index].Technologies?.Select(x => x.Name)),
                    Regions = string.Join(",", modelResponses[index].Regions?.Select(x => x.Name)),
                    StatusName = modelResponses[index].StatusName,
                    ChangedOn = modelResponses[index].ChangedOn.Year > 0001 ? modelResponses[index].ChangedOn.ToString("dd-MM-yyyy hh:mm:ss:tt") : string.Empty,
                    Company = modelResponses[index].Company?.Name,
                    PublishedBy = modelResponses[index].Owner?.FirstName + " " + modelResponses[index].Owner?.LastName,
                    PublishedOn = modelResponses[index].FirstTimePublishedOn.HasValue ? modelResponses[index].FirstTimePublishedOn.Value.ToString("dd-MM-yyyy hh:mm:ss:tt") : string.Empty
                };

                switch (modelResponses[index].ProjectDetails)
                {
                    case ProjectOffsitePowerPurchaseAgreementDetailsResponse:
                        {
                            projectTypesIncluded.Add(CategoriesSlugs.OffsitePowerPurchaseAgreement);
                            var ppaDetails = JsonConvert.DeserializeObject<ProjectOffsitePowerPurchaseAgreementDetailsResponse>(projectDetails);
                            project.SettlementTypeName = ppaDetails.SettlementTypeName;
                            project.SettlementHubOrLoadZoneName = ppaDetails.SettlementHubOrLoadZoneName ?? "None";
                            project.ForAllPriceEntriesCurrencyName = ppaDetails.ForAllPriceEntriesCurrencyName;
                            project.ContractPricePerMWh = ppaDetails.ContractPricePerMWh;
                            project.FloatingMarketSwapIndexDiscount = ppaDetails.FloatingMarketSwapIndexDiscount;
                            project.FloatingMarketSwapFloor = ppaDetails.FloatingMarketSwapFloor;
                            project.FloatingMarketSwapCap = ppaDetails.FloatingMarketSwapCap;
                            project.PricingStructureName = ppaDetails.PricingStructureName;
                            project.UpsidePercentageToDeveloper = ppaDetails.UpsidePercentageToDeveloper;
                            project.UpsidePercentageToOfftaker = ppaDetails.UpsidePercentageToOfftaker;
                            project.DiscountAmount = ppaDetails.DiscountAmount;
                            project.EACName = ppaDetails.EACName ?? ppaDetails.EACCustom;
                            project.EACValue = ppaDetails.EACValue;
                            project.SettlementPriceIntervalName = ppaDetails.SettlementPriceIntervalName;
                            project.SettlementPriceIntervalCustom = ppaDetails.SettlementPriceIntervalCustom;
                            project.AdditionalNotesForSEOperationsTeam = ppaDetails.AdditionalNotesForSEOperationsTeam;
                            project.SettlementCalculationIntervalName = ppaDetails.SettlementCalculationIntervalName;
                            project.ProjectMWCurrentlyAvailable = ppaDetails.ProjectMWCurrentlyAvailable;
                            project.IsoRtoName = ppaDetails.IsoRtoName ?? "None";
                            project.ProductTypeName = ppaDetails.ProductTypeName;
                            project.CommercialOperationDate = ppaDetails.CommercialOperationDate.HasValue ? ppaDetails.CommercialOperationDate.Value.ToString("dd-MM-yyyy hh:mm:ss:tt") : string.Empty;
                            project.PPATermYearsLength = ppaDetails.PPATermYearsLength;
                            project.TotalProjectNameplateMWACCapacity = ppaDetails.TotalProjectNameplateMWACCapacity;
                            project.TotalProjectExpectedAnnualMWhProductionP50 = ppaDetails.TotalProjectExpectedAnnualMWhProductionP50;
                            project.MinimumOfftakeMWhVolumeRequired = ppaDetails.MinimumOfftakeMWhVolumeRequired;
                            project.NotesForPotentialOfftakers = ppaDetails.NotesForPotentialOfftakers;
                            project.ValuesToOfftakers = string.Join(", ", ppaDetails.ValuesToOfftakers?.Select(x => x.Name) ?? Enumerable.Empty<string>());

                            break;
                        }

                    case ProjectOnsiteSolarDetailsResponse:
                        {
                            projectTypesIncluded.Add(CategoriesSlugs.OnsiteSolar);
                            var projectOnsiteSolarDetails = JsonConvert.DeserializeObject<ProjectOnsiteSolarDetailsResponse>(projectDetails);
                            project.TimeAndUrgencyConsiderations = projectOnsiteSolarDetails.TimeAndUrgencyConsiderations;
                            project.AdditionalComments = projectOnsiteSolarDetails.AdditionalComments;
                            project.ContractStructures = string.Join(", ", projectOnsiteSolarDetails.ContractStructures?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            project.MinimumTermLengthAvailable = projectOnsiteSolarDetails.MinimumTermLength;
                            project.MinimumAnnualSiteOnSiteSolar = projectOnsiteSolarDetails.MinimumAnnualSiteKWh;
                            project.ValuesProvided = string.Join(", ", projectOnsiteSolarDetails.ValuesProvided?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            hasOnlyPPA = false;
                            break;
                        }

                    case ProjectEACDetailsResponse:
                        {
                            projectTypesIncluded.Add(CategoriesSlugs.EAC);
                            var projectEACDetails = JsonConvert.DeserializeObject<ProjectEACDetailsResponse>(projectDetails);
                            project.TimeAndUrgencyConsiderations = projectEACDetails.TimeAndUrgencyConsiderations;
                            project.AdditionalCommentsEAC = projectEACDetails.AdditionalComments;
                            project.StripLengths = string.Join(", ", projectEACDetails.StripLengths?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            project.ValuesProvided = string.Join(", ", projectEACDetails.ValuesProvided?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            project.MinimumTermLengthAvailable = projectEACDetails.MinimumTermLength;
                            project.MinimumPurchaseVolume = projectEACDetails.MinimumPurchaseVolume;
                            hasOnlyPPA = false;
                            break;
                        }

                    case ProjectEfficiencyAuditsAndConsultingDetailsResponse:
                        {
                            projectTypesIncluded.Add(CategoriesSlugs.EfficiencyAuditsAndConsulting);
                            var projectEfficiencyAuditsAndConsultingDetails = JsonConvert.DeserializeObject<ProjectEfficiencyAuditsAndConsultingDetailsResponse>(projectDetails);
                            project.TimeAndUrgencyConsiderations = projectEfficiencyAuditsAndConsultingDetails.TimeAndUrgencyConsiderations;
                            project.AdditionalComments = projectEfficiencyAuditsAndConsultingDetails.AdditionalComments;
                            project.ContractStructures = string.Join(", ", projectEfficiencyAuditsAndConsultingDetails.ContractStructures?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            project.ValuesProvided = string.Join(", ", projectEfficiencyAuditsAndConsultingDetails.ValuesProvided?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            project.IsInvestmentGradeCreditOfOfftakerRequired = projectEfficiencyAuditsAndConsultingDetails.IsInvestmentGradeCreditOfOfftakerRequired == true ? "Yes" : "No";
                            project.MinimumTermLengthAvailable = projectEfficiencyAuditsAndConsultingDetails.MinimumTermLength;
                            hasOnlyPPA = false;
                            break;
                        }

                    case ProjectEfficiencyEquipmentMeasuresDetailsResponse:
                        {
                            projectTypesIncluded.Add(CategoriesSlugs.EfficiencyEquipmentMeasures);
                            var projectEfficiencyEquipmentMeasuresDetails = JsonConvert.DeserializeObject<ProjectEfficiencyEquipmentMeasuresDetailsResponse>(projectDetails);
                            project.TimeAndUrgencyConsiderations = projectEfficiencyEquipmentMeasuresDetails.TimeAndUrgencyConsiderations;
                            project.AdditionalComments = projectEfficiencyEquipmentMeasuresDetails.AdditionalComments;
                            project.ContractStructures = string.Join(", ", projectEfficiencyEquipmentMeasuresDetails.ContractStructures?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            project.ValuesProvided = string.Join(", ", projectEfficiencyEquipmentMeasuresDetails.ValuesProvided?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            project.IsInvestmentGradeCreditOfOfftakerRequired = projectEfficiencyEquipmentMeasuresDetails.IsInvestmentGradeCreditOfOfftakerRequired == true ? "Yes" : "No";
                            project.MinimumTermLengthAvailable = projectEfficiencyEquipmentMeasuresDetails.MinimumTermLength;
                            hasOnlyPPA = false;
                            break;
                        }
                    case ProjectEmergingTechnologyDetailsResponse:
                        {
                            projectTypesIncluded.Add(CategoriesSlugs.EmergingTechnology);
                            var projectEmergingTechnologyDetails = JsonConvert.DeserializeObject<ProjectEmergingTechnologyDetailsResponse>(projectDetails);
                            project.TimeAndUrgencyConsiderations = projectEmergingTechnologyDetails.TimeAndUrgencyConsiderations;
                            project.AdditionalComments = projectEmergingTechnologyDetails.AdditionalComments;
                            project.MinimumVolumeLoadRequired = projectEmergingTechnologyDetails.MinimumAnnualValue + EnumHelper.GetEnumValue<EnergyUnit>(Convert.ToInt32(projectEmergingTechnologyDetails.EnergyUnitId)).GetDescription<EnergyUnit>();
                            project.MinimumTermLengthAvailable = projectEmergingTechnologyDetails.MinimumTermLength;
                            project.ContractStructures = string.Join(", ", projectEmergingTechnologyDetails.ContractStructures?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            project.ValuesProvided = string.Join(", ", projectEmergingTechnologyDetails.ValuesProvided?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            hasOnlyPPA = false;
                            break;
                        }
                    case ProjectEVChargingDetailsResponse:
                        {
                            projectTypesIncluded.Add(CategoriesSlugs.EVCharging);
                            var projectEVChargingDetails = JsonConvert.DeserializeObject<ProjectEVChargingDetailsResponse>(projectDetails);
                            project.TimeAndUrgencyConsiderations = projectEVChargingDetails.TimeAndUrgencyConsiderations;
                            project.AdditionalComments = projectEVChargingDetails.AdditionalComments;
                            project.MinimumChargingStationsRequired = projectEVChargingDetails.MinimumChargingStationsRequired;
                            project.ContractStructures = string.Join(", ", projectEVChargingDetails.ContractStructures?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            project.MinimumTermLengthAvailable = projectEVChargingDetails.MinimumTermLength;
                            project.ValuesProvided = string.Join(", ", projectEVChargingDetails.ValuesProvided?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            hasOnlyPPA = false;
                            break;
                        }
                    case ProjectGreenTariffsDetailsResponse:
                        {
                            projectTypesIncluded.Add(CategoriesSlugs.GreenTariffs);
                            var projectGreenTariffsDetails = JsonConvert.DeserializeObject<ProjectGreenTariffsDetailsResponse>(projectDetails);
                            project.TimeAndUrgencyConsiderations = projectGreenTariffsDetails.TimeAndUrgencyConsiderations;
                            project.AdditionalComments = projectGreenTariffsDetails.AdditionalComments;
                            project.UtilityName = projectGreenTariffsDetails.UtilityName;
                            project.ProgramWebsite = projectGreenTariffsDetails.ProgramWebsite;
                            project.MinimumPurchaseVolume = projectGreenTariffsDetails.MinimumPurchaseVolume;
                            project.ValuesProvided = string.Join(", ", projectGreenTariffsDetails.ValuesProvided?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            project.TermLength = projectGreenTariffsDetails.TermLengthName;
                            hasOnlyPPA = false;
                            break;
                        }
                    case ProjectRenewableRetailDetailsResponse:
                        {
                            projectTypesIncluded.Add(CategoriesSlugs.RenewableRetail);
                            var projectRenewableRetailDetails = JsonConvert.DeserializeObject<ProjectRenewableRetailDetailsResponse>(projectDetails);
                            project.TimeAndUrgencyConsiderations = projectRenewableRetailDetails.TimeAndUrgencyConsiderations;
                            project.AdditionalComments = projectRenewableRetailDetails.AdditionalComments;
                            project.ValuesProvided = string.Join(", ", projectRenewableRetailDetails.ValuesProvided?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            project.MinimumTermLengthAvailable = projectRenewableRetailDetails.MinimumTermLength;
                            project.MinimumAnnualSiteLoadRenewableRetailElectricity = projectRenewableRetailDetails.MinimumAnnualSiteKWh;
                            project.PurchaseOptions = string.Join(", ", projectRenewableRetailDetails.PurchaseOptions?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            hasOnlyPPA = false;
                            break;
                        }
                    case ProjectCarbonOffsetsDetailsResponse:
                        {
                            projectTypesIncluded.Add(CategoriesSlugs.CarbonOffsets);
                            var projectCarbonOffsetsDetails = JsonConvert.DeserializeObject<ProjectCarbonOffsetsDetailsResponse>(projectDetails);
                            project.TimeAndUrgencyConsiderations = projectCarbonOffsetsDetails.TimeAndUrgencyConsiderations;
                            project.AdditionalComments = projectCarbonOffsetsDetails.AdditionalComments;
                            project.MinimumPurchaseVolume = projectCarbonOffsetsDetails.MinimumPurchaseVolume;
                            project.StripLengths = string.Join(", ", projectCarbonOffsetsDetails.StripLengths?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            project.ValuesProvided = string.Join(", ", projectCarbonOffsetsDetails.ValuesProvided?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            hasOnlyPPA = false;
                            break;
                        }
                    case ProjectBatteryStorageDetailsResponse:
                        {
                            projectTypesIncluded.Add(CategoriesSlugs.BatteryStorage);
                            var projectBatteryStorageDetails = JsonConvert.DeserializeObject<ProjectBatteryStorageDetailsResponse>(projectDetails);
                            project.TimeAndUrgencyConsiderations = projectBatteryStorageDetails.TimeAndUrgencyConsiderations;
                            project.AdditionalComments = projectBatteryStorageDetails.AdditionalComments;
                            project.MinimumAnnualPeakBatteryStorage = projectBatteryStorageDetails.MinimumAnnualPeakKW;
                            project.MinimumTermLengthAvailable = projectBatteryStorageDetails.MinimumTermLength;
                            project.ValuesProvided = string.Join(", ", projectBatteryStorageDetails.ValuesProvided?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            project.ContractStructures = string.Join(", ", projectBatteryStorageDetails.ContractStructures?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            hasOnlyPPA = false;
                            break;
                        }
                    case ProjectFuelCellsDetailsResponse:
                        {
                            projectTypesIncluded.Add(CategoriesSlugs.FuelCells);
                            var projectFuelCellsDetails = JsonConvert.DeserializeObject<ProjectFuelCellsDetailsResponse>(projectDetails);
                            project.TimeAndUrgencyConsiderations = projectFuelCellsDetails.TimeAndUrgencyConsiderations;
                            project.AdditionalComments = projectFuelCellsDetails.AdditionalComments;
                            project.ValuesProvided = string.Join(", ", projectFuelCellsDetails.ValuesProvided?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            project.MinimumAnnualSiteLoadFuelCells = projectFuelCellsDetails.MinimumAnnualSiteKWh;
                            project.MinimumTermLengthAvailable = projectFuelCellsDetails.MinimumTermLength;
                            project.AdditionalComments = projectFuelCellsDetails.AdditionalComments;
                            project.ContractStructures = string.Join(", ", projectFuelCellsDetails.ContractStructures?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            hasOnlyPPA = false;
                            break;
                        }
                    case ProjectCommunitySolarDetailsResponse:
                        {
                            projectTypesIncluded.Add(CategoriesSlugs.CommunitySolar);
                            var projectCommunitySolarDetails = JsonConvert.DeserializeObject<ProjectCommunitySolarDetailsResponse>(projectDetails);
                            project.TimeAndUrgencyConsiderations = projectCommunitySolarDetails.TimeAndUrgencyConsiderations;
                            project.AdditionalComments = projectCommunitySolarDetails.AdditionalComments;
                            project.TotalAnnualMWhAvailable = projectCommunitySolarDetails.TotalAnnualMWh;
                            project.UtilityTerritory = projectCommunitySolarDetails.UtilityTerritory;
                            project.ProjectCurrentlyAvailable = projectCommunitySolarDetails.ProjectAvailable == true ? "Yes" : "No";
                            project.ProjectAvailabilityApproximateDate = projectCommunitySolarDetails.ProjectAvailabilityApproximateDate.HasValue ? projectCommunitySolarDetails.ProjectAvailabilityApproximateDate.Value.ToString("dd-MM-yyyy hh:mm:ss:tt") : "Currently Available";
                            project.IsInvestmentGradeCreditOfOfftakerRequired = projectCommunitySolarDetails.IsInvestmentGradeCreditOfOfftakerRequired == true ? "Yes" : "No";
                            project.MinimumTermLengthAvailable = projectCommunitySolarDetails.MinimumTermLength;
                            project.ContractStructures = string.Join(", ", projectCommunitySolarDetails.ContractStructures?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            project.ValuesProvided = string.Join(", ", projectCommunitySolarDetails.ValuesProvided?.Select(x => x.Name) ?? Enumerable.Empty<string>());
                            project.MinimumAnnualkWhPurchaseCommunitySolar = projectCommunitySolarDetails.MinimumAnnualMWh;
                            hasOnlyPPA = false;
                            break;
                        }
                }

                projects.Add(project);
            }

            projectTypesIncluded = projectTypesIncluded.Distinct().ToList();

            using (var writeFile = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true))
            {
                using (var csv = new CsvWriter(writeFile, new CsvConfiguration(CultureInfo.InvariantCulture) { Encoding = Encoding.UTF8, LeaveOpen = true }))
                {
                    var classMap = new DefaultClassMap<ProjectExportResponse>();
                    writeFile.Write($"Exported from Zeigo Network on {DateTime.Now.ToString("MM/dd/yyyy")}\n");
                    writeFile.Write($"Exported By User: {currentUser.FirstName} {currentUser.LastName} \n");
                    if (!string.IsNullOrEmpty(filter.Search))
                    {
                        writeFile.Write($"Filtered Records By: Search Text - {filter.Search}\n");
                    }
                    csv.NextRecord();

                    classMap.Map(o => o.Title).Name("Project Title").Index(0);
                    classMap.Map(o => o.Category).Name("Project Type").Index(1);
                    var countOfEfficiencyAuditsAndConsulting = projects.Count(x => x.CategorySlug == CategoriesSlugs.EfficiencyAuditsAndConsulting);
                    var countOfCarbonOffsets = projects.Count(x => x.CategorySlug == CategoriesSlugs.CarbonOffsets);
                    if (projects.Count() > (countOfEfficiencyAuditsAndConsulting + countOfCarbonOffsets))
                    {
                        classMap.Map(o => o.Technologies).Name("Technologies").Index(2);
                    }

                    classMap.Map(o => o.Regions).Name("Project Geography").Index(3);
                    classMap.Map(o => o.SubTitle).Name("Sub-Title").Index(4);
                    classMap.Map(o => o.Opportunity).Name("Describe the Opportunity").Index(5);


                    if (projectTypesIncluded?.Any(x => x == CategoriesSlugs.OnsiteSolar) == true)
                    {
                        classMap.Map(o => o.MinimumAnnualSiteOnSiteSolar).Name("Minimum Annual Site (OnSite Solar)").Index(7);
                        classMap.Map(o => o.MinimumTermLengthAvailable).Name("Minimum Term Length Available").Index(12);// (Years)
                        classMap.Map(o => o.ContractStructures).Name("Contract Structures Available").Index(14);
                    }
                    if (projectTypesIncluded?.Any(x => x == CategoriesSlugs.EAC) == true)
                    {
                        classMap.Map(o => o.MinimumTermLengthAvailable).Name("Minimum Term Length Available").Index(12);// (Years)
                        classMap.Map(o => o.StripLengths).Name("Strip Lengths Available").Index(17);
                        classMap.Map(o => o.MinimumPurchaseVolume).Name("Minimum Purchase Volume Available (1 EAC = 1 MWh)").Index(18);
                        classMap.Map(o => o.AdditionalCommentsEAC).Name("Additional Comments for the Zeigo Network / SE Team (not visible to corporate members)").Index(28);
                    }
                    if (projectTypesIncluded?.Any(x => x == CategoriesSlugs.EfficiencyAuditsAndConsulting) == true ||
                        projectTypesIncluded?.Any(x => x == CategoriesSlugs.EfficiencyEquipmentMeasures) == true)
                    {
                        classMap.Map(o => o.MinimumTermLengthAvailable).Name("Minimum Term Length Available").Index(12);// (Years)
                        classMap.Map(o => o.ContractStructures).Name("Contract Structures Available").Index(14);
                        classMap.Map(o => o.IsInvestmentGradeCreditOfOfftakerRequired).Name("Requires Investment Grade Credit of Offtaker").Index(16);
                    }
                    if (projectTypesIncluded?.Any(x => x == CategoriesSlugs.EmergingTechnology) == true)
                    {
                        classMap.Map(o => o.MinimumTermLengthAvailable).Name("Minimum Term Length Available").Index(12);// (Years)
                        classMap.Map(o => o.ContractStructures).Name("Contract Structures Available").Index(14);
                        classMap.Map(o => o.MinimumVolumeLoadRequired).Name("Minimum Volume/Load Required").Index(6);
                    }
                    if (projectTypesIncluded?.Any(x => x == CategoriesSlugs.EVCharging) == true)
                    {
                        classMap.Map(o => o.MinimumTermLengthAvailable).Name("Minimum Term Length Available").Index(12);// (Years)
                        classMap.Map(o => o.ContractStructures).Name("Contract Structures Available").Index(14);
                        classMap.Map(o => o.MinimumChargingStationsRequired).Name("Minimum Charging Stations Required").Index(21);
                    }
                    if (projectTypesIncluded?.Any(x => x == CategoriesSlugs.GreenTariffs) == true)
                    {
                        classMap.Map(o => o.TermLength).Name("Term Length").Index(15);
                        classMap.Map(o => o.MinimumPurchaseVolume).Name("Minimum Purchase Volume Available (1 EAC = 1 MWh)").Index(18);
                        classMap.Map(o => o.UtilityName).Name("Utility Name").Index(19);
                        classMap.Map(o => o.ProgramWebsite).Name("Program Website").Index(20);
                    }
                    if (projectTypesIncluded?.Any(x => x == CategoriesSlugs.RenewableRetail) == true)
                    {
                        classMap.Map(o => o.MinimumAnnualSiteLoadRenewableRetailElectricity).Name("Minimum Annual Site Load (kWh)").Index(10);
                        classMap.Map(o => o.MinimumTermLengthAvailable).Name("Minimum Term Length Available").Index(12);// (Years)
                        classMap.Map(o => o.PurchaseOptions).Name("Purchase Options - Additional Details").Index(24);
                    }
                    if (projectTypesIncluded?.Any(x => x == CategoriesSlugs.CarbonOffsets) == true)
                    {
                        classMap.Map(o => o.StripLengths).Name("Strip Lengths Available").Index(17);
                        classMap.Map(o => o.MinimumPurchaseVolume).Name("Minimum Purchase Volume Available (1 EAC = 1 MWh)").Index(18);
                    }
                    if (projectTypesIncluded?.Any(x => x == CategoriesSlugs.BatteryStorage) == true)
                    {
                        classMap.Map(o => o.MinimumAnnualPeakBatteryStorage).Name("Minimum Annual Peak (kW)").Index(9);
                        classMap.Map(o => o.ContractStructures).Name("Contract Structures Available").Index(14);
                        classMap.Map(o => o.MinimumTermLengthAvailable).Name("Minimum Term Length Available").Index(12);// (Years)
                    }
                    if (projectTypesIncluded?.Any(x => x == CategoriesSlugs.FuelCells) == true)
                    {
                        classMap.Map(o => o.MinimumAnnualSiteLoadFuelCells).Name("Minimum Annual Site Load (MWh)").Index(8);//FuelCells - Minimum Annual Site Load (MWh)
                        classMap.Map(o => o.MinimumTermLengthAvailable).Name("Minimum Term Length Available").Index(12);// (Years)
                        classMap.Map(o => o.ContractStructures).Name("Contract Structures Available").Index(14);
                    }
                    if (projectTypesIncluded?.Any(x => x == CategoriesSlugs.CommunitySolar) == true)
                    {
                        classMap.Map(o => o.MinimumAnnualkWhPurchaseCommunitySolar).Name("Minimum Annual kWh Purchase").Index(11);
                        classMap.Map(o => o.MinimumTermLengthAvailable).Name("Minimum Term Length Available").Index(12);// (Years)
                        classMap.Map(o => o.ContractStructures).Name("Contract Structures Available").Index(14);
                        classMap.Map(o => o.IsInvestmentGradeCreditOfOfftakerRequired).Name("Requires Investment Grade Credit of Offtaker").Index(16);
                        classMap.Map(o => o.UtilityTerritory).Name("Utility Territory").Index(22);
                        classMap.Map(o => o.ProjectAvailabilityApproximateDate).Name("Approximate Date of Project Availability").Index(23);
                        classMap.Map(o => o.TotalAnnualMWhAvailable).Name("Total Annual kWh Available").Index(25);
                    }

                    if (hasOnlyPPA == false)
                    {
                        classMap.Map(o => o.ValuesProvided).Name("Value Provided").Index(13);
                        classMap.Map(o => o.TimeAndUrgencyConsiderations).Name("Time & Urgency Considerations").Index(26);
                        classMap.Map(o => o.AdditionalComments).Name("Additional Comments").Index(27);
                    }

                    if (projectTypesIncluded.Count == 1 && projectTypesIncluded.Any(a => a == CategoriesSlugs.EAC))
                    {
                        classMap.Map(o => o.AdditionalComments).Name("Additional Comments").Ignore();
                    }

                    if (projectTypesIncluded?.Any(x => x == CategoriesSlugs.OffsitePowerPurchaseAgreement) == true)
                    {
                        classMap.Map(o => o.IsoRtoName).Name("ISO / RTO");
                        classMap.Map(o => o.ProductTypeName).Name("Product Type");
                        classMap.Map(o => o.CommercialOperationDate).Name("Commercial Operation Date");
                        classMap.Map(o => o.ValuesToOfftakers).Name("Value to Offtaker");
                        classMap.Map(o => o.PPATermYearsLength).Name("PPA Term Length");//(Years)
                        classMap.Map(o => o.TotalProjectNameplateMWACCapacity).Name("Total Project Nameplate Capacity");//(MWAC)
                        classMap.Map(o => o.TotalProjectExpectedAnnualMWhProductionP50).Name("Total Project Expected Annual Production - P50");//(MWh)
                        classMap.Map(o => o.MinimumOfftakeMWhVolumeRequired).Name("Minimum Offtake Volume Required");// (MWh)
                        classMap.Map(o => o.NotesForPotentialOfftakers).Name("Notes for Potential Offtakers");
                        classMap.Map(o => o.SettlementTypeName).Name("Settlement Type");
                        classMap.Map(o => o.SettlementHubOrLoadZoneName).Name("Settlement Hub/ Load Zone");
                        classMap.Map(o => o.ForAllPriceEntriesCurrencyName).Name("Currency for all Price Entries");
                        classMap.Map(o => o.ContractPricePerMWh).Name("Contract Price");
                        classMap.Map(o => o.FloatingMarketSwapIndexDiscount).Name("Floating Market Swap (Index, Discount)");
                        classMap.Map(o => o.FloatingMarketSwapFloor).Name("Floating Market Swap (Floor)");
                        classMap.Map(o => o.FloatingMarketSwapCap).Name("Floating Market Swap (Cap)");
                        classMap.Map(o => o.PricingStructureName).Name("Pricing Structure");
                        classMap.Map(o => o.UpsidePercentageToDeveloper).Name("Upside Percentage To Developer");
                        classMap.Map(o => o.UpsidePercentageToOfftaker).Name("Upside Percentage To Offtaker");
                        classMap.Map(o => o.DiscountAmount).Name("Discount amount");
                        classMap.Map(o => o.EACName).Name("EAC Type");
                        classMap.Map(o => o.EACValue).Name("EAC Value");
                        classMap.Map(o => o.SettlementPriceIntervalName).Name("Settlement Price Interval");
                        classMap.Map(o => o.SettlementPriceIntervalCustom).Name("Custom Settlement Price Interval");
                        classMap.Map(o => o.SettlementCalculationIntervalName).Name("Settlement Calculation Interval");
                        classMap.Map(o => o.ProjectMWCurrentlyAvailable).Name("Project MW Currently Available");
                        classMap.Map(o => o.AdditionalNotesForSEOperationsTeam).Name("Notes For SE Operations Team");
                    }

                    classMap.Map(o => o.PublishedBy).Name("Published By");
                    if (isAdmin)
                    {
                        classMap.Map(o => o.Company).Name("Company");
                    }
                    classMap.Map(o => o.Description).Name("About the Provider");
                    classMap.Map(o => o.StatusName).Name("Status");
                    classMap.Map(o => o.PublishedOn).Name("First Published On");
                    classMap.Map(o => o.ChangedOn).Name("Modified On");
                    csv.Context.RegisterClassMap(classMap);
                    csv.WriteRecords(projects);
                }
                stream.Position = 0;
            }
            return modelResponses.Count;
        }

        private bool CheckIfOnlyValuesFromList(List<string> items, string[] projectsWithTechnologies)
        {
            return items.Any(projectsWithTechnologies.Contains) && !projectsWithTechnologies.All(items.Contains);
        }
        /// <summary>
        /// Returns list of all active and draft project
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<IList<SPDashboardProjectDetailsResponse>> GetActiveandDraftProjects(UserModel currentUser)
        {
            Dictionary<string, List<string>> projectTypeToImages = new Dictionary<string, List<string>>();
            Dictionary<string, int> projectTypeIndex = new Dictionary<string, int>();
            List<string> projectCategories = new List<string>();

            Dictionary<string, List<string>> technologyTypeToImages = new Dictionary<string, List<string>>();
            Dictionary<string, int> technologyTypeIndex = new Dictionary<string, int>();
            List<string> technologyCategories = new List<string>();

            IEnumerable<ProjectDTO> projectsResults = await _projectService.GetActiveDraftProjectsForSPDashboard(currentUser.CompanyId);
            List<SPDashboardProjectDetailsResponse> projectResponse = projectsResults.Select(_mapper.Map<SPDashboardProjectDetailsResponse>).ToList();
            InitializeProjectImages(projectCategories, projectTypeToImages, projectTypeIndex);
            InitializeProjectTechnologyImages(technologyCategories, technologyTypeToImages, technologyTypeIndex);
            foreach (SPDashboardProjectDetailsResponse resultItem in projectResponse)
            {
                if(resultItem.Technologies.Count() > 0)
                {
                    resultItem.ProjectCategoryImage = GetNextImageForProjectType(resultItem.Technologies.ElementAt(0).Slug, technologyTypeToImages, technologyTypeIndex);
                }
                else
                {
                    resultItem.ProjectCategoryImage = GetNextImageForProjectType(resultItem.ProjectCategorySlug, projectTypeToImages, projectTypeIndex);
                }
                
            }
            return projectResponse;

        }

        /// <summary>
        /// Returns list of all active projects for a SP company
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="currentUser"></param>
        /// <param name="companyId">The id of the company from query string</param>
        /// <returns></returns>
        public async Task<WrapperModel<SPCompanyProjectResponse>> GetActiveProjectsForSPCompany(BaseSearchFilterModel filter, UserModel currentUser,  int companyId)
        {
            Dictionary<string, List<string>> projectTypeToImages = new Dictionary<string, List<string>>();
            Dictionary<string, int> projectTypeIndex = new Dictionary<string, int>();
            List<string> projectCategories = new List<string>();

            Dictionary<string, List<string>> technologyTypeToImages = new Dictionary<string, List<string>>();
            Dictionary<string, int> technologyTypeIndex = new Dictionary<string, int>();
            List<string> technologyCategories = new List<string>();

            WrapperModel<ProjectDTO> projectsResults = await _projectService.GetActiveProjectsForSpCompany(filter, currentUser.Id, currentUser.CompanyId, currentUser.RoleIds, companyId);
            List<SPCompanyProjectResponse> projectResponse = projectsResults.DataList.Select(_mapper.Map<SPCompanyProjectResponse>).ToList();
            InitializeProjectImages(projectCategories, projectTypeToImages, projectTypeIndex);
            InitializeProjectTechnologyImages(technologyCategories, technologyTypeToImages, technologyTypeIndex);
            foreach (SPCompanyProjectResponse resultItem in projectResponse)
            {
                if(resultItem.Technologies.Count() > 0)
                {
                    resultItem.ProjectCategoryImage = GetNextImageForProjectTechnology(resultItem.Technologies.ElementAt(0).Slug, technologyTypeToImages, technologyTypeIndex);
                }
                else
                {
                    resultItem.ProjectCategoryImage = GetNextImageForProjectType(resultItem.ProjectCategorySlug, projectTypeToImages, projectTypeIndex);
                }
                
            }
            return new WrapperModel<SPCompanyProjectResponse>
            {
                Count = projectsResults.Count,
                DataList = projectResponse,
            };

        }

    }
}