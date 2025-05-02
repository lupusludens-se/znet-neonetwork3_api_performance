using SE.Neo.Common.Models.Public;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services.Decorators
{
    /// <summary>
    /// 
    /// </summary>
    public class AbstractPublicDashboardServiceDecorator : IPublicDashboardService
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly IPublicDashboardService _publicDashboardService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicDashboardService"></param>
        public AbstractPublicDashboardServiceDecorator(IPublicDashboardService publicDashboardService)
        {
            _publicDashboardService = publicDashboardService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IList<DiscoverabilityProjectsDataDTO>> GetProjectsDataForDiscoverability()
        {
            return await _publicDashboardService.GetProjectsDataForDiscoverability();
        }
    }
}