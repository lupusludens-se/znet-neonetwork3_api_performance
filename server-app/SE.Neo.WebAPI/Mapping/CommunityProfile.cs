using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Community;
using SE.Neo.Core.Entities;
using SE.Neo.WebAPI.Models.Community;
namespace SE.Neo.WebAPI.Mapping
{
    public class CommunityProfile : Profile
    {
        public CommunityProfile()
        {
            CreateMap<CommunityItemDTO, CommunityItemResponse>()
                .ForMember(dest => dest.Role, opts => opts.MapFrom(src => src.Roles.Select(x => x).Where(x => x.Id != (int)RoleType.All).FirstOrDefault()));
            CreateMap<NetworkStatsDTO, NetworkStatsResponse>();
        }
    }
}
