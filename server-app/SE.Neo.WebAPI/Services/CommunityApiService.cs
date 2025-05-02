using AutoMapper;
using SE.Neo.Common.Models.Community;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Models.Community;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Services
{
    public class CommunityApiService : ICommunityApiService
    {
        private readonly ICommunityService _communityService;
        private readonly IMapper _mapper;
        public CommunityApiService(ICommunityService communityService, IMapper mapper)
        {
            _communityService = communityService;
            _mapper = mapper;
        }
        public async Task<WrapperModel<CommunityItemResponse>> GetCommunityAsync(int userId, CommunityFilter filter)
        {
            WrapperModel<CommunityItemDTO> communityDTOs = await _communityService.GetCommunityAsync(userId, filter);
            return new WrapperModel<CommunityItemResponse>
            {
                Count = communityDTOs.Count,
                DataList = communityDTOs.DataList.Select(_mapper.Map<CommunityItemResponse>)
            };
        }

        public async Task<NetworkStatsResponse> GetNetworkStats()
        {
            NetworkStatsDTO networkStats = await _communityService.GetNetworkStats();
            return _mapper.Map<NetworkStatsResponse>(networkStats);
        }
    }
}
