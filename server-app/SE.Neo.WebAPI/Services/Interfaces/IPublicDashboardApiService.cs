using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Models.Project;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface IPublicDashboardApiService
    {
        Task<WrapperModel<NewTrendingProjectResponse>> GetProjectsDataForDiscoverability();
    }
}