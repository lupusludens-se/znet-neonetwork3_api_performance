using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Project;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface IProjectService
    {
        Task<int> CreateUpdateProjectAsync(ProjectDTO projectDTO, BaseProjectDetailsDTO projectDetailsDTO, int currentUserId, int currentUserCompanyId, bool accessAll = false);

        Task CreateProjectViewAsync(int userId, int projectId);

        Task<IEnumerable<ProjectDTO>> GetProjectViewsAsync(int userId);

        bool IsProjectExist(int id);

        Task<int> DuplicateProjectAsync(int id, string projectName, int currentUserId, int currentUserCompanyId, List<int> userRoleIds, bool accessAll = false);

        Task PatchProjectAsync(int id, JsonPatchDocument patchDoc, int currentUserId, int currentUserCompanyId, bool accessAll = false);

        Task<WrapperModel<ProjectDTO>> GetProjectsAsync(BaseSearchFilterModel filter, int currentUserId, List<int> userRoleIds, int userCompanyId, ProjectsPredicateFlag flag = default(ProjectsPredicateFlag), bool isTrending = false);

        Task<ProjectDTO?> GetProjectAsync(int id, int currentUserId, List<int> userRoleIds, int userCompanyId, string? expand = null, ProjectsPredicateFlag flag = default(ProjectsPredicateFlag));

        Task<ProjectResourceResponseDTO?> GetProjectResourceDetails(int id, int currentUserId, List<int> userRoleIds, int userCompanyId, string? expand = null, ProjectsPredicateFlag flag = default(ProjectsPredicateFlag));


        bool IsProjectActive(int id);
        Task<WrapperModel<ProjectDTO>> GetRecommendedProjectsAsync(IEnumerable<int> projectIds, BaseSearchFilterModel filter, int currentUserId, List<int> userRoleIds, int userCompanyId);

        bool IsNewUser(int userId);
        IList<ProjectDTO> GetProjectsById(List<int> projectIds);
        IList<ProjectDTO> GetNewProjectsForCorporateDashboard(int currentUserId, List<int> trendingProjectIds);

        Task<WrapperModel<ProjectDTO>> GetExportProjectsAsync(BaseSearchFilterModel filter, int currentUserId, List<int> userRoleIds, int userCompanyId, ProjectsPredicateFlag flag = default(ProjectsPredicateFlag));
        Task<IList<ProjectDTO>> GetActiveDraftProjectsForSPDashboard(int companyId);

        Task<WrapperModel<ProjectDTO>> GetActiveProjectsForSpCompany(BaseSearchFilterModel filter, int currentUserId, int currentUserCompanyId, List<int> currentUserRoleIds, int companyId);

    }
}