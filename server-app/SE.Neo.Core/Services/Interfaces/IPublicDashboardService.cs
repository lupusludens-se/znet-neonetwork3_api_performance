using SE.Neo.Common.Models.Public;

namespace SE.Neo.Core.Services.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPublicDashboardService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IList<DiscoverabilityProjectsDataDTO>> GetProjectsDataForDiscoverability();
    }
}