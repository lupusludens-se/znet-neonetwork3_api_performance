using AutoMapper;
using SE.Neo.Common.Models.Community;
using SE.Neo.Core.Models.Community;
namespace SE.Neo.Core.Mapping
{
    public class CommunityProfile : Profile
    {
        public CommunityProfile()
        {
            CreateMap<CommunityItem, CommunityItemDTO>()
                .ForMember(dest => dest.Roles, opts => opts.MapFrom(src => src.Roles.Select(o => o.Role)));
        }
    }
}
