using SE.Neo.Common.Models.Community;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services.Decorators
{
    public class AbstractCommunityServiceDecorator : ICommunityService
    {
        protected readonly ICommunityService _communityService;

        public AbstractCommunityServiceDecorator(ICommunityService communityService)
        {
            _communityService = communityService;
        }
        public virtual async Task<WrapperModel<CommunityItemDTO>> GetCommunityAsync(int userId, CommunityFilter filter)
        {
            return await _communityService.GetCommunityAsync(userId, filter);
        }

        public Task<NetworkStatsDTO> GetNetworkStats()
        {
            return _communityService.GetNetworkStats();
        }
    }
}
