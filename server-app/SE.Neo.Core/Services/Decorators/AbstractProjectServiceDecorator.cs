using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Project;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services.Decorators
{
    public class AbstractProjectServiceDecorator : IProjectService
    {
        protected readonly IProjectService _projectService;

        public AbstractProjectServiceDecorator(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public virtual async Task CreateProjectViewAsync(int userId, int projectId)
        {
            await _projectService.CreateProjectViewAsync(userId, projectId);
        }

        public virtual async Task<int> CreateUpdateProjectAsync(ProjectDTO projectDTO, BaseProjectDetailsDTO projectDetailsDTO, int currentUserId, int currentUserCompanyId, bool accessAll = false)
        {
            return await _projectService.CreateUpdateProjectAsync(projectDTO, projectDetailsDTO, currentUserId, currentUserCompanyId, accessAll);
        }

        public virtual async Task<int> DuplicateProjectAsync(int id, string projectName, int currentUserId, int currentUserCompanyId, List<int> userRoleIds, bool accessAll = false)
        {
            return await _projectService.DuplicateProjectAsync(id, projectName, currentUserId, currentUserCompanyId, userRoleIds, accessAll);
        }
        public virtual async Task<ProjectDTO?> GetProjectAsync(int id, int currentUserId, List<int> userRoleIds, int userCompanyId, string? expand = null, ProjectsPredicateFlag flag = default(ProjectsPredicateFlag))
        {
            return await _projectService.GetProjectAsync(id, currentUserId, userRoleIds, userCompanyId, expand, flag);
        }

        public virtual async Task<ProjectResourceResponseDTO?> GetProjectResourceDetails(int id, int currentUserId, List<int> userRoleIds, int userCompanyId, string? expand = null, ProjectsPredicateFlag flag = default(ProjectsPredicateFlag))
        {
            return await _projectService.GetProjectResourceDetails(id, currentUserId, userRoleIds, userCompanyId, expand, flag);
        }

        public virtual async Task<WrapperModel<ProjectDTO>> GetProjectsAsync(BaseSearchFilterModel filter, int currentUserId, List<int> userRoleIds, int userCompanyId,
                ProjectsPredicateFlag flag = default(ProjectsPredicateFlag), bool isTrending = false)
        {
            return await _projectService.GetProjectsAsync(filter, currentUserId, userRoleIds, userCompanyId, flag);
        }

        public virtual async Task<WrapperModel<ProjectDTO>> GetRecommendedProjectsAsync(IEnumerable<int> projectIds, BaseSearchFilterModel filter, int currentUserId, List<int> userRoleIds, int userCompanyId)
        {
            return await _projectService.GetRecommendedProjectsAsync(projectIds, filter, currentUserId, userRoleIds, userCompanyId);
        }

        public virtual async Task<IEnumerable<ProjectDTO>> GetProjectViewsAsync(int userId)
        {
            return await _projectService.GetProjectViewsAsync(userId);
        }

        public bool IsProjectActive(int id)
        {
            return _projectService.IsProjectActive(id);
        }

        public bool IsProjectExist(int id)
        {
            return _projectService.IsProjectExist(id);
        }

        public virtual async Task PatchProjectAsync(int id, JsonPatchDocument patchDoc, int currentUserId, int currentUserCompanyId, bool accessAll = false)
        {
            await _projectService.PatchProjectAsync(id, patchDoc, currentUserId, currentUserCompanyId, accessAll);
        }

        public bool IsNewUser(int userId)
        {
            return _projectService.IsNewUser(userId);
        }

        public IList<ProjectDTO> GetProjectsById(List<int> projectIds)
        {
            return _projectService.GetProjectsById(projectIds);
        }

        public IList<ProjectDTO> GetNewProjectsForCorporateDashboard(int currentUserId, List<int> trendingProjectIds)
        {
            return _projectService.GetNewProjectsForCorporateDashboard(currentUserId, trendingProjectIds);
        }

        public virtual async Task<WrapperModel<ProjectDTO>> GetExportProjectsAsync(BaseSearchFilterModel filter, int currentUserId, List<int> userRoleIds, int userCompanyId, ProjectsPredicateFlag flag = default(ProjectsPredicateFlag))
        {
            return await _projectService.GetExportProjectsAsync(filter, currentUserId, userRoleIds, userCompanyId, flag);
        }
        public async Task<IList<ProjectDTO>> GetActiveDraftProjectsForSPDashboard(int companyId)
        {
            return await _projectService.GetActiveDraftProjectsForSPDashboard(companyId);
        }

        public async Task<WrapperModel<ProjectDTO>> GetActiveProjectsForSpCompany(BaseSearchFilterModel filter, int currentUserId, int currentUserCompanyId, List<int> currentUserRoleIds, int companyId)
        {
            return await _projectService.GetActiveProjectsForSpCompany(filter, currentUserId, currentUserCompanyId, currentUserRoleIds, companyId);

        }
    }
}