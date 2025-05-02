using SE.Neo.Common.Models.Community;
using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Models.Community;
namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface ICommunityApiService
    {
        public Task<WrapperModel<CommunityItemResponse>> GetCommunityAsync(int userId, CommunityFilter filter);
        public Task<NetworkStatsResponse> GetNetworkStats();
    }
}
