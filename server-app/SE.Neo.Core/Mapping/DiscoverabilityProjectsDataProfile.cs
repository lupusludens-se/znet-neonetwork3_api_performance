using AutoMapper;
using SE.Neo.Common.Models.Public;
using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Mapping
{
    public class DiscoverabilityProjectsDataProfile : Profile
    {
        public DiscoverabilityProjectsDataProfile()
        {

            CreateMap<DiscoverabilityProjectsData, DiscoverabilityProjectsDataDTO>();
        }
    }
}
