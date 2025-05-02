using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Project;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Services.Decorators;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Decorators
{
    public class ProjectServiceBlobDecorator : AbstractProjectServiceDecorator
    {
        private readonly IBlobServicesFacade _blobServicesFacade;

        public ProjectServiceBlobDecorator(
            IProjectService projectService,
            IBlobServicesFacade blobServicesFacade)
            : base(projectService)
        {
            _blobServicesFacade = blobServicesFacade;
        }

        public override async Task<ProjectDTO?> GetProjectAsync(int id, int currentUserId, List<int> userRoleIds, int userCompanyId, string? expand = null, ProjectsPredicateFlag flag = default(ProjectsPredicateFlag))
        {
            ProjectDTO? projectDTO = await _projectService.GetProjectAsync(id, currentUserId, userRoleIds, userCompanyId, expand, flag);

            await _blobServicesFacade.PopulateWithBlobAsync(projectDTO, dto => dto?.Company?.Image, (dto, b) => { if (dto?.Company != null) dto.Company.Image = b; });

            return projectDTO;
        }

        public override async Task<WrapperModel<ProjectDTO>> GetRecommendedProjectsAsync(IEnumerable<int> projectIds, BaseSearchFilterModel filter, int currentUserId, List<int> userRoleIds, int userCompanyId)
        {
            WrapperModel<ProjectDTO> projectsResult = await _projectService.GetRecommendedProjectsAsync(projectIds, filter, currentUserId, userRoleIds, userCompanyId);

            List<ProjectDTO> projectsDTOs = projectsResult.DataList.ToList();
            await _blobServicesFacade.PopulateWithBlobAsync(projectsDTOs, dto => dto.Company?.Image, (dto, b) => { if (dto?.Company != null) dto.Company.Image = b; });
            projectsResult.DataList = projectsDTOs;

            return projectsResult;
        }

        public override async Task<WrapperModel<ProjectDTO>> GetProjectsAsync(BaseSearchFilterModel filter, int currentUserId, List<int> userRoleIds, int userCompanyId,
            ProjectsPredicateFlag flag = default(ProjectsPredicateFlag), bool isTrending = false)
        {
            WrapperModel<ProjectDTO> projectsResult = await _projectService.GetProjectsAsync(filter, currentUserId, userRoleIds, userCompanyId, flag, isTrending);

            List<ProjectDTO> projectsDTOs = projectsResult.DataList.ToList();
            await _blobServicesFacade.PopulateWithBlobAsync(projectsDTOs, dto => dto.Company?.Image, (dto, b) => { if (dto?.Company != null) dto.Company.Image = b; });
            projectsResult.DataList = projectsDTOs;

            return projectsResult;
        }

        public override async Task<IEnumerable<ProjectDTO>> GetProjectViewsAsync(int userId)
        {
            IEnumerable<ProjectDTO> projectResult = await base.GetProjectViewsAsync(userId);

            List<ProjectDTO> projects = projectResult.ToList();
            await _blobServicesFacade.PopulateWithBlobAsync(projects, dto => dto.Company!.Image, (dto, b) => dto.Company!.Image = b);

            return projects;
        }

        public override async Task<WrapperModel<ProjectDTO>> GetExportProjectsAsync(BaseSearchFilterModel filter, int currentUserId, List<int> userRoleIds, int userCompanyId, ProjectsPredicateFlag flag = default(ProjectsPredicateFlag))
        {
            WrapperModel<ProjectDTO> projectsResult = await _projectService.GetExportProjectsAsync(filter, currentUserId, userRoleIds, userCompanyId, flag);
            //projectsResult.DataList = projectsResult.DataList.ToList(); 
            return projectsResult;
        }
    }
}