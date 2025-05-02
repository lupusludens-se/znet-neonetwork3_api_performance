using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Project;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Factories.Interfaces;
using SE.Neo.Core.Models.Project;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services
{
    public partial class ProjectService : BaseService, IProjectService
    {
        private readonly ApplicationContext _context;
        private readonly IProjectDetailsFactory _projectDetailsFactory;
        private readonly ILogger<ProjectService> _logger;
        private readonly IMapper _mapper;
        private readonly ICommonService _commonService;


        public ProjectService(
            ApplicationContext context,
            IProjectDetailsFactory projectDetailsFactory,
            ILogger<ProjectService> logger,
            IMapper mapper,
            IDistributedCache cache,
            ICommonService commonService) : base(cache)
        {
            _context = context;
            _projectDetailsFactory = projectDetailsFactory;
            _logger = logger;
            _mapper = mapper;
            _commonService = commonService;
        }

        public async Task<int> CreateUpdateProjectAsync(ProjectDTO projectDTO, BaseProjectDetailsDTO projectDetailsDTO, int currentUserId, int currentUserCompanyId, bool accessAll = false)
        {
            Project project;

            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (projectDTO.Id == 0)
                    {
                        project = await AddProjectAsync(
                            _mapper.Map<Project>(projectDTO),
                            _mapper.Map<BaseProjectDetails>(projectDetailsDTO));
                    }
                    else
                    {
                        project = await EnsureAccessibleProjectExistsAsync(projectDTO.Id, currentUserId, currentUserCompanyId, accessAll);

                        _context.RemoveRange(_context.ProjectRegions.Where(pr => pr.ProjectId == project.Id));
                        _context.RemoveRange(_context.ProjectTechnologies.Where(pt => pt.ProjectId == project.Id));
                        _context.RemoveRange(_context.ProjectContractStructures.Where(pt => pt.ProjectId == project.Id));
                        _context.RemoveRange(_context.ProjectValuesProvided.Where(pt => pt.ProjectId == project.Id));

                        BaseProjectDetails? projectDetails = await _projectDetailsFactory.GetProjectDetailsAsync(project);
                        projectDetails = await _projectDetailsFactory.RemoveManyRelationsAsync(projectDetails);

                        if (projectDTO.CategoryId != project.CategoryId)
                        {
                            _context.Remove(projectDetails);
                            projectDetails = _mapper.Map<BaseProjectDetails>(projectDetailsDTO);

                            await AddProjectDetailsAsync(project.Id, projectDetails);
                        }
                        else
                        {
                            _mapper.Map(projectDetailsDTO, projectDetails);
                        }

                        _mapper.Map(projectDTO, project);

                        project.ChangedOn = DateTime.UtcNow;
                    }

                    await ApplyProjectStatusAsync(project);

                    await _context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError($"Error while publishing project with Title {projectDTO.Title}.Error: " + ex.Message);
                    _logger.LogError($"Error while publishing project with Title Inner {projectDTO.Title}.Error: " + ex.InnerException?.Message);
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }
            return project.Id;
        }

        public async Task CreateProjectViewAsync(int userId, int projectId)
        {
            if (_context.Projects.SingleOrDefault(b => b.Id == projectId) == null)
                throw new CustomException($"{CoreErrorMessages.ErrorOnSaving} Project does not exist.");

            try
            {
                var model = new ProjectView { UserId = userId, ProjectId = projectId };
                _context.ProjectViews.Add(model);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
            }
        }

        public async Task<IEnumerable<ProjectDTO>> GetProjectViewsAsync(int userId)
        {
            List<ProjectView>? projectViews = await _context.ProjectViews.Where(pv => pv.UserId.Equals(userId) && pv.Project.StatusId == Enums.ProjectStatus.Active)
                .GroupBy(x => x.ProjectId, (key, g) => g.OrderByDescending(e => e.Id).First()).ToListAsync();

            List<int> projectIds = projectViews.OrderByDescending(o => o.Id).Take(6).Select(s => s.ProjectId).ToList();

            IQueryable<Project> projectsQueryable = _context.Projects.Where(s => projectIds.Contains(s.Id)).AsNoTracking();

            projectsQueryable = projectsQueryable.Include(p => p.Company).ThenInclude(c => c.Image).Include(i => i.Category).Include(i => i.Technologies).ThenInclude(i => i.Technology).Include(c => c.ProjectSaved);

            IEnumerable<Project> projects = await projectsQueryable.ToListAsync();

            List<ProjectDTO> projectDTOs = new List<ProjectDTO>();
            foreach (Project project in projectsQueryable)
                projectDTOs.Add(
                    _mapper.Map<Project, ProjectDTO>(
                        project,
                        opts => opts.AfterMap((src, dest) => dest.IsSaved = src.ProjectSaved?.Any(dest => dest.UserId == userId) ?? false)));

            return projectDTOs.OrderBy(x => projectIds.IndexOf(x.Id));
        }

        public bool IsProjectExist(int id)
        {
            return _context.Projects.AsNoTracking().Any(p => p.Id == id && p.StatusId != Enums.ProjectStatus.Deleted);
        }

        public async Task<int> DuplicateProjectAsync(int id, string projectName, int currentUserId, int currentUserCompanyId, List<int> userRoleIds, bool accessAll = false)
        {
            Project project;
            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    project = await EnsureAccessibleProjectExistsAsync(id, currentUserId, currentUserCompanyId, accessAll);
                    BaseProjectDetails? projectDetails = await _projectDetailsFactory.GetProjectDetailsAsync(project);

                    _logger.LogInformation($"Attempt to duplicate project {project.Title} - {id}.");

                    _context.Entry(project).State = EntityState.Detached;
                    _context.Entry(projectDetails).State = EntityState.Detached;

                    // Remapping resets relations' keys
                    ProjectDTO projectDTO = _mapper.Map<ProjectDTO>(project);
                    project = _mapper.Map<Project>(projectDTO);
                    BaseProjectDetailsDTO projectDetailsDTO = _mapper.Map<BaseProjectDetailsDTO>(projectDetails);
                    projectDetails = _mapper.Map<BaseProjectDetails>(projectDetailsDTO);

                    project.Title = projectName;
                    project.PublishedOn = null;
                    project.FirstTimePublishedOn = null;
                    project.StatusId = Enums.ProjectStatus.Draft;
                    if (userRoleIds.Any(r => r.Equals((int)RoleType.SolutionProvider)))
                        project.OwnerId = currentUserId;
                    project = await AddProjectAsync(project, projectDetails);

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }

            return project.Id;
        }

        public async Task PatchProjectAsync(int id, JsonPatchDocument patchDoc, int currentUserId, int currentUserCompanyId, bool accessAll = false)
        {
            Project model = await EnsureAccessibleProjectExistsAsync(id, currentUserId, currentUserCompanyId, accessAll, false);

            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    patchDoc.ApplyTo(model);

                    await ApplyProjectStatusAsync(model);

                    if (!(patchDoc.Operations.SingleOrDefault()?.path.Equals($"/{nameof(model.IsPinned)}", StringComparison.InvariantCultureIgnoreCase) ?? false))
                        model.ChangedOn = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }
        }

        public async Task<WrapperModel<ProjectDTO>> GetProjectsAsync(BaseSearchFilterModel filter, int currentUserId, List<int> userRoleIds, int userCompanyId,
                ProjectsPredicateFlag flag = default(ProjectsPredicateFlag), bool isTrending = false)
        {
            IQueryable<Project> projectsQueryable = _context.Projects.Where(p => (flag.HasFlag(ProjectsPredicateFlag.AccessAll) || p.OwnerId == currentUserId || p.CompanyId == userCompanyId)
                && (!flag.HasFlag(ProjectsPredicateFlag.ActiveOnly)
                    || (flag.HasFlag(ProjectsPredicateFlag.ActiveOnly) && p.StatusId != Enums.ProjectStatus.Inactive && p.StatusId != Enums.ProjectStatus.Draft))
                && p.StatusId != Enums.ProjectStatus.Deleted).AsNoTracking();

            if (userRoleIds.Contains((int)Common.Enums.RoleType.SolutionProvider))
            {
                projectsQueryable = projectsQueryable.Where(p => p.CompanyId == userCompanyId &&
                (p.StatusId == Enums.ProjectStatus.Active || p.StatusId == Enums.ProjectStatus.Inactive || p.StatusId == Enums.ProjectStatus.Draft));

            }
            projectsQueryable = ExpandSortProjects(projectsQueryable, currentUserId, userRoleIds, userCompanyId, filter.Expand, filter.OrderBy, true);

            projectsQueryable = SearchProjects(projectsQueryable, currentUserId, filter.Search, filter.FilterBy);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await projectsQueryable.CountAsync();
                if (count == 0)
                    return new WrapperModel<ProjectDTO> { Count = count, DataList = new List<ProjectDTO>() };
            }

            if (filter.Skip.HasValue)
                projectsQueryable = projectsQueryable.Skip(filter.Skip.Value);

            if (filter.Take.HasValue)
                projectsQueryable = projectsQueryable.Take(filter.Take.Value);

            if (isTrending)
            {
                projectsQueryable = projectsQueryable.Include(p => p.Company)
                                                     .Include(p => p.Company.Image)
                                                     .Include(p => p.Category);
            }

            List<ProjectDTO> projectDTOs = new List<ProjectDTO>();
            var projects = await projectsQueryable.ToListAsync();
            foreach (Project project in projects)
                projectDTOs.Add(
                    _mapper.Map<Project, ProjectDTO>(
                        project,
                        opts => opts.AfterMap((src, dest) => dest.IsSaved = src.ProjectSaved?.Any(dest => dest.UserId == currentUserId) ?? false)));

            return new WrapperModel<ProjectDTO> { Count = count, DataList = projectDTOs };
        }

        public async Task<WrapperModel<ProjectDTO>> GetExportProjectsAsync(BaseSearchFilterModel filter, int currentUserId, List<int> userRoleIds, int userCompanyId, ProjectsPredicateFlag flag = default(ProjectsPredicateFlag))
        {
            IQueryable<Project> projectsQueryable = _context.Projects.Where(p => (flag.HasFlag(ProjectsPredicateFlag.AccessAll) || p.OwnerId == currentUserId || p.CompanyId == userCompanyId)
                && (!flag.HasFlag(ProjectsPredicateFlag.ActiveOnly)
                    || (flag.HasFlag(ProjectsPredicateFlag.ActiveOnly) && p.StatusId != Enums.ProjectStatus.Inactive && p.StatusId != Enums.ProjectStatus.Draft))
                && p.StatusId != Enums.ProjectStatus.Deleted).AsNoTracking();

            if (userRoleIds.Contains((int)Common.Enums.RoleType.SolutionProvider))
                projectsQueryable = projectsQueryable.Where(p => p.StatusId == Enums.ProjectStatus.Active || p.StatusId == Enums.ProjectStatus.Inactive
                || (p.StatusId == Enums.ProjectStatus.Draft && (userRoleIds.Contains((int)Common.Enums.RoleType.SPAdmin) ? (p.OwnerId == currentUserId || p.CompanyId == userCompanyId) :
                p.OwnerId == currentUserId)));

            projectsQueryable = ExpandSortProjects(projectsQueryable, currentUserId, userRoleIds, userCompanyId, filter.Expand, filter.OrderBy, true);

            projectsQueryable = SearchProjects(projectsQueryable, currentUserId, filter.Search, filter.FilterBy);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await projectsQueryable.CountAsync();
                if (count == 0)
                    return new WrapperModel<ProjectDTO> { Count = count, DataList = new List<ProjectDTO>() };
            }
            if (filter.Skip.HasValue)
                projectsQueryable = projectsQueryable.Skip(filter.Skip.Value);
            if (filter.Take.HasValue)
                projectsQueryable = projectsQueryable.Take(filter.Take.Value);
            projectsQueryable = projectsQueryable.Include(x => x.ContractStructures).Include(x => x.ValuesProvided)
                    .Include(p => p.ProjectBatteryStorageDetails)
                    .Include(p => p.ProjectFuelCellsDetails)
                    .Include(p => p.ProjectGreenTariffsDetails)
                    .Include(p => p.ProjectEmergingTechnologyDetails)
                    .Include(p => p.ProjectEfficiencyEquipmentMeasuresDetails)
                    .Include(p => p.ProjectEVChargingDetails)
                    .Include(p => p.ProjectRenewableRetailDetails).ThenInclude(y => y.PurchaseOptions)
                    .Include(p => p.ProjectEACDetails).ThenInclude(y => y.StripLengths)
                    .Include(p => p.ProjectOnsiteSolarDetails)
                    .Include(p => p.ProjectCommunitySolarDetails)
                    .Include(p => p.ProjectEfficiencyAuditsAndConsultingDetails)
                    .Include(p => p.ProjectOffsitePowerPurchaseAgreementDetails).ThenInclude(y => y.ValuesToOfftakers)
                    .Include(p => p.ProjectCarbonOffsetsDetails).ThenInclude(y => y.StripLengths);
            //.ThenInclude(m=>m.TermLength);
            List<ProjectDTO> exportProjectDTOs = new List<ProjectDTO>();
            foreach (Project project in projectsQueryable)
            {
                exportProjectDTOs.Add(
                    _mapper.Map<Project, ProjectDTO>(
                        project,
                        opts => opts.AfterMap((src, dest) => dest.IsSaved = src.ProjectSaved?.Any(dest => dest.UserId == currentUserId) ?? false)));
            }
            return new WrapperModel<ProjectDTO> { Count = count, DataList = exportProjectDTOs };
        }

        public async Task<WrapperModel<ProjectDTO>> GetRecommendedProjectsAsync(IEnumerable<int> projectIds, BaseSearchFilterModel filter, int currentUserId, List<int> userRoleIds, int userCompanyId)
        {
            IQueryable<int> userProfileRegions = _context.UserProfileRegions
                  .Where(upc => upc.UserProfile.UserId == currentUserId)
                  .Select(x => x.RegionId);
            IQueryable<int> userProfileCategories = _context.UserProfileCategories
                .Where(upc => upc.UserProfile.UserId == currentUserId)
                .Select(x => x.CategoryId);

            IQueryable<Project> projectsQueryable = _context.Projects
                .Where(p => projectIds.Contains(p.Id) || (p.IsPinned && p.StatusId == Enums.ProjectStatus.Active && (p.Regions.Any(dr => userProfileRegions.Contains(dr.RegionId))
                     || userProfileCategories.Contains(p.CategoryId))))
                .OrderBy(m => m.IsPinned).AsNoTracking();

            projectsQueryable = ExpandSortProjects(projectsQueryable, currentUserId, userRoleIds, userCompanyId, filter.Expand, null, false);// ignore sort for recommended projects

            projectsQueryable = SearchProjects(projectsQueryable, currentUserId, filter.Search, "recommondedProjects");

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await projectsQueryable.CountAsync();
                if (count == 0)
                    return new WrapperModel<ProjectDTO> { Count = count, DataList = new List<ProjectDTO>() };
            }

            var allProjects = await projectsQueryable.ToListAsync();
            var recommendedProjectsPinned = allProjects.Where(x => x.IsPinned == true).OrderByDescending(p => p.IsPinned)
                            .ThenByDescending(p => p.Regions.Any(dr => userProfileRegions.Contains(dr.RegionId))
                                && userProfileCategories.Contains(p.CategoryId))
                            .ThenByDescending(p => userProfileCategories.Contains(p.CategoryId))
                            .ThenByDescending(f => f.Regions.Any(dr => userProfileRegions.Contains(dr.RegionId)))
                            .ThenByDescending(f => f.ModifiedOn).ToList();

            List<ProjectDTO> projectDTOs = new();
            foreach (Project project in recommendedProjectsPinned)
                projectDTOs.Add(
                    _mapper.Map<Project, ProjectDTO>(
                        project,
                        opts => opts.AfterMap((src, dest) => dest.IsSaved = src.ProjectSaved?.Any(dest => dest.UserId == currentUserId) ?? false)));

            var recommendedProjectsUnPinned = allProjects.Where(x => x.IsPinned == false).ToList();

            foreach (var projectId in projectIds)
            {
                var project = recommendedProjectsUnPinned.FirstOrDefault(x => x.Id == projectId);
                if (project != null && !projectDTOs.Any(y => y.Id == projectId))
                {
                    projectDTOs.Add(
                        _mapper.Map<Project, ProjectDTO>(
                            project,
                            opts => opts.AfterMap((src, dest) => dest.IsSaved = src.ProjectSaved?.Any(dest => dest.UserId == currentUserId) ?? false)));
                }
            }

            if (filter.Skip.HasValue)
                projectDTOs = projectDTOs.Skip(filter.Skip.Value).ToList();

            if (filter.Take.HasValue)
                projectDTOs = projectDTOs.Take(filter.Take.Value).ToList();

            return new WrapperModel<ProjectDTO> { Count = count, DataList = projectDTOs };
        }

        public async Task<ProjectResourceResponseDTO?> GetProjectResourceDetails(int id, int currentUserId, List<int> userRoleIds, int userCompanyId, string? expand = null,
                ProjectsPredicateFlag flag = default(ProjectsPredicateFlag))
        {
            IQueryable<Project> projectsQueryable = _context.Projects.Where(p => (flag.HasFlag(ProjectsPredicateFlag.AccessAll) || p.OwnerId == currentUserId || p.CompanyId == userCompanyId)
                    && (!flag.HasFlag(ProjectsPredicateFlag.ActiveOnly)
                        || (flag.HasFlag(ProjectsPredicateFlag.ActiveOnly) && p.StatusId != Enums.ProjectStatus.Inactive && p.StatusId != Enums.ProjectStatus.Draft))
                    && p.StatusId != Enums.ProjectStatus.Deleted).AsNoTracking();

            if (!string.IsNullOrEmpty(expand))
            {
                expand = expand.ToLower();
                if (expand.Contains("category"))
                {
                    projectsQueryable = projectsQueryable.Include(p => p.Category);
                    if (expand.Contains("category.resources"))
                    {
                        projectsQueryable = projectsQueryable.Include(p => p.Category)
                            .ThenInclude(c => c.CategoryResources.Where(t =>
                            userRoleIds.Contains((int)Common.Enums.RoleType.Admin) || (t.Resource.Article == null && t.Resource.Tool == null)
                            || (t.Resource.Article != null && t.Resource.Article.ArticleRoles.Any(a => userRoleIds.Contains(a.RoleId)))
                            || (t.Resource.Tool != null && (t.Resource.Tool.Roles.Any(a => userRoleIds.Contains(a.RoleId))
                                || t.Resource.Tool.Companies.Any(c => c.CompanyId.Equals(userCompanyId))))
                            ))
                            .ThenInclude(cr => cr.Resource);
                    } 
                } 
                if (expand.Contains("technologies"))
                {
                    projectsQueryable = projectsQueryable.Include(p => p.Technologies).ThenInclude(pl => pl.Technology);
                    if (expand.Contains("technologies.resources"))
                    {
                        projectsQueryable = projectsQueryable.Include(p => p.Technologies)
                            .ThenInclude(pl => pl.Technology)
                            .ThenInclude(t => t.TechnologyResources.Where(t =>
                             userRoleIds.Contains((int)Common.Enums.RoleType.Admin) || (t.Resource.Article == null && t.Resource.Tool == null)
                            || (t.Resource.Article != null && t.Resource.Article.ArticleRoles.Any(a => userRoleIds.Contains(a.RoleId)))
                            || (t.Resource.Tool != null && (t.Resource.Tool.Roles.Any(a => userRoleIds.Contains(a.RoleId))
                                || t.Resource.Tool.Companies.Any(c => c.Id.Equals(userCompanyId))))
                            ))
                            .ThenInclude(tr => tr.Resource);
                    }
                } 
            }

            Project? project = await projectsQueryable.FirstOrDefaultAsync(p => p.Id == id);
            ProjectResourceResponseDTO? projectResourceResponseDTO = null;

            if (project != null)
            {
                projectResourceResponseDTO = _mapper.Map<Project, ProjectResourceResponseDTO>(project);
            }

            return projectResourceResponseDTO;
        }

        public async Task<ProjectDTO?> GetProjectAsync(int id, int currentUserId, List<int> userRoleIds, int userCompanyId, string? expand = null,
                ProjectsPredicateFlag flag = default(ProjectsPredicateFlag))
        {
            IQueryable<Project> projectsQueryable = ExpandSortProjects(
                _context.Projects.Where(p => (flag.HasFlag(ProjectsPredicateFlag.AccessAll) || p.OwnerId == currentUserId || p.CompanyId == userCompanyId)
                    && (!flag.HasFlag(ProjectsPredicateFlag.ActiveOnly)
                        || (flag.HasFlag(ProjectsPredicateFlag.ActiveOnly) && p.StatusId != Enums.ProjectStatus.Inactive && p.StatusId != Enums.ProjectStatus.Draft))
                    && p.StatusId != Enums.ProjectStatus.Deleted).AsNoTracking(),
                currentUserId, userRoleIds, userCompanyId, expand);

            Project? project = await projectsQueryable.FirstOrDefaultAsync(p => p.Id == id);
            ProjectDTO? projectDTO = null;

            if (project != null)
            {
                projectDTO = _mapper.Map<Project, ProjectDTO>(
                    project,
                    opts => opts.AfterMap((src, dest) => dest.IsSaved = src.ProjectSaved?.Any(dest => dest.UserId == currentUserId) ?? false));
            }

            return projectDTO;
        }

        public bool IsProjectActive(int id)
        {
            return _context.Projects.AsNoTracking().Any(p => p.Id == id && p.StatusId == Enums.ProjectStatus.Active);
        }

        public bool IsNewUser(int userId)
        {
            ProjectView? projectView = _context.ProjectViews.Where(pv => pv.UserId == userId).FirstOrDefault();
            if ((projectView == null) ||
                (projectView != null && (DateTime.UtcNow < projectView?.CreatedOn?.AddMinutes(60))))
            {
                return true;
            }
            else return false;
        }

        public IList<ProjectDTO> GetProjectsById(List<int> projectIds)
        {
            var projectsQueryable = _context.Projects.Where(pId => projectIds.Contains(pId.Id))
                                    .Include(p => p.Company)
                                    .Include(p => p.Company.Image)
                                    .Include(p => p.Category)
                                    .Include(p => p.Technologies).ThenInclude(p => p.Technology)
                                    .ToList().OrderBy(pId => projectIds.IndexOf(pId.Id));

            List<ProjectDTO> projectDTOs = new List<ProjectDTO>();
            foreach (Project project in projectsQueryable)
                projectDTOs.Add(
                    _mapper.Map<Project, ProjectDTO>(project));

            return projectDTOs;
        }

        public IList<ProjectDTO> GetNewProjectsForCorporateDashboard(int currentUserId, List<int> trendingProjectIds)
        {
            DateTime threeMonthsAgo = DateTime.UtcNow.AddMonths(-3);

            IQueryable<int> userProfileRegions = _context.UserProfileRegions
                  .Where(upc => upc.UserProfile.UserId == currentUserId)
                  .Select(x => x.RegionId);

            IQueryable<int> userProfileCategories = _context.UserProfileCategories
                .Where(upc => upc.UserProfile.UserId == currentUserId)
                .Select(x => x.CategoryId);


            IList<Project> newProjects = _context.Projects.Where(project => threeMonthsAgo <= project.FirstTimePublishedOn
                                                      && project.StatusId == Enums.ProjectStatus.Active
                                                      && project.IsPinned == false
                                                      && !trendingProjectIds.Contains(project.Id))
                     .OrderBy(p => p.Regions.Any(dr => userProfileRegions.Contains(dr.RegionId) && userProfileCategories.Contains(p.CategoryId)) ? 1 :
                     (p.Regions.Any(dr => userProfileRegions.Contains(dr.RegionId)) ? 2 :
                         (userProfileCategories.Contains(p.CategoryId) ? 3 :
                         (!(p.Regions.Any(dr => userProfileRegions.Contains(dr.RegionId)) ||
                         (!userProfileCategories.Contains(p.CategoryId))) &&
                         p.FirstTimePublishedOn >= threeMonthsAgo) ? 4 : 5)
                     )).ThenByDescending(x => x.FirstTimePublishedOn)
                     .Include(p => p.Company)
                     .Include(p => p.Category)
                     .Include(p => p.Technologies).ThenInclude(p => p.Technology)
                     .ToList();

            List<ProjectDTO> projectDTOs = new List<ProjectDTO>();
            foreach (Project project in newProjects)
                projectDTOs.Add(
                    _mapper.Map<Project, ProjectDTO>(project));

            return projectDTOs;
        }
        public async Task<IList<ProjectDTO>> GetActiveDraftProjectsForSPDashboard(int CompanyId)
        {
            int count = 0;
            IQueryable<Project> projectsQueryable = _context.Projects
                .Where(p => (p.StatusId == Enums.ProjectStatus.Active || (p.StatusId == Enums.ProjectStatus.Draft)) && (p.Company.Id == CompanyId)).OrderBy(x => x.StatusId).
                ThenByDescending(p => p.ModifiedOn).Take(9);
            projectsQueryable = projectsQueryable.Include(p => p.Company).Include(p => p.Category).Include(p => p.Technologies).ThenInclude(x => x.Technology);

            List<ProjectDTO> projectDTOs = new List<ProjectDTO>();
            foreach (Project project in projectsQueryable)
                projectDTOs.Add(
                    _mapper.Map<Project, ProjectDTO>(project));

            return projectDTOs;

        }

        public async Task<WrapperModel<ProjectDTO>> GetActiveProjectsForSpCompany(BaseSearchFilterModel filter, int currentUserId, int currentUserCompanyId, List<int> currentUserRoleIds, int companyId)
        {
            int count = 0;
            IQueryable<Project> projectsQueryable = _context.Projects
                .Where(p => (p.StatusId == Enums.ProjectStatus.Active) && (((currentUserRoleIds.Contains((int) RoleType.SolutionProvider) || currentUserRoleIds.Contains((int)RoleType.SPAdmin)) && p.Company.Id == currentUserCompanyId) || (!currentUserRoleIds.Contains((int) RoleType.SPAdmin) && !currentUserRoleIds.Contains((int)RoleType.SolutionProvider))) && p.Company.Id == companyId && p.Company.TypeId == CompanyType.SolutionProvider).OrderByDescending(p => p.ModifiedOn);

            projectsQueryable = projectsQueryable.Include(y => y.ProjectSaved).Include(p => p.Company).ThenInclude(p => p.Image).Include(p => p.Category).Include(p => p.Regions).ThenInclude(y => y.Region).Include(p => p.Technologies).ThenInclude(p => p.Technology);

            if (filter.IncludeCount)
            {
                count = await projectsQueryable.CountAsync();
                if (count == 0)
                    return new WrapperModel<ProjectDTO> { Count = count, DataList = new List<ProjectDTO>() };
            }


            if (filter.Skip.HasValue)
                projectsQueryable = projectsQueryable.Skip(filter.Skip.Value);

            if (filter.Take.HasValue)
                projectsQueryable = projectsQueryable.Take(filter.Take.Value);
            List<ProjectDTO> projectDTOs = new List<ProjectDTO>();
            foreach (Project project in projectsQueryable)
            {
                projectDTOs.Add(_mapper.Map<Project, ProjectDTO>(
                    project, opts => opts.AfterMap((src, dest) =>
                    {
                        dest.IsSaved = src.ProjectSaved?.Any(ps => ps.UserId == currentUserId) ?? false;
                    })));
            }

            return new WrapperModel<ProjectDTO> {
                Count = count,
                DataList = projectDTOs
            };

        }
    }
}