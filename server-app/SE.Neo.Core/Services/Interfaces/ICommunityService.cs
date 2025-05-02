using SE.Neo.Common.Models.Community;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface ICommunityService
    {
        public Task<WrapperModel<CommunityItemDTO>> GetCommunityAsync(int userId, CommunityFilter filter);
        public Task<NetworkStatsDTO> GetNetworkStats();
    }
}
