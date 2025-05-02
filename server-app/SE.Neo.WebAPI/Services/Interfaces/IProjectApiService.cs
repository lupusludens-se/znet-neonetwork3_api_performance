using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Models.Project;
using SE.Neo.WebAPI.Models.User;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface IProjectApiService
    {
        Task<int> CreateUpdateProjectAsync(ProjectRequest project, BaseProjectDetailsRequest projectDetails, UserModel currentUser, bool accessAll = false, int id = 0);

        Task CreateProjectViewAsync(int userId, int projectId);

        Task<IEnumerable<RecentProjectResponse>> GetProjectViewsAsync(int userId);

        Task<int> DuplicateProjectAsync(int id, string projectName, UserModel currentUser, bool accessAll = false);

        Task PatchProjectAsync(int id, JsonPatchDocument patchDoc, UserModel currentUser, bool accessAll = false);

        Task<WrapperModel<ProjectResponse>> GetProjectsAsync(BaseSearchFilterModel filter, UserModel currentUser,
                ProjectsPredicateFlag flag = default(ProjectsPredicateFlag), ProjectsViewType projectsViewType = default(ProjectsViewType));

        Task<ProjectResponse?> GetProjectAsync(int id, UserModel currentUser, string? expand = null, ProjectsPredicateFlag flag = default(ProjectsPredicateFlag));

        Task<ProjectResourceResponse?> GetProjectResourceDetails(int id, UserModel currentUser, string? expand = null, ProjectsPredicateFlag flag = default(ProjectsPredicateFlag));
        Task<WrapperModel<ProjectResponse>> GetRecommendedProjectsAsync(IEnumerable<int> projectIds, BaseSearchFilterModel filter, UserModel currentUser);
        Task<WrapperModel<NewTrendingProjectResponse>> GetNewTrendingProjectsAsync(UserModel currentUser, bool isNew, ProjectsPredicateFlag flag);
        Task<int> ExportProjectsAsync(BaseSearchFilterModel filter, UserModel currentUser, MemoryStream stream, ProjectsPredicateFlag flag = default(ProjectsPredicateFlag));
        Task<IList<SPDashboardProjectDetailsResponse>> GetActiveandDraftProjects(UserModel currentUser);

        Task<WrapperModel<SPCompanyProjectResponse>> GetActiveProjectsForSPCompany(BaseSearchFilterModel filter, UserModel currentUser, int companyId);

    }
}