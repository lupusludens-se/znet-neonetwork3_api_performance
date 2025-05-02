using AutoMapper;
using SE.Neo.Common.Models.Tool;
using SE.Neo.EmailTemplates.Models;
namespace SE.Neo.Infrastructure.Mapping
{
    public class SolarInfrastructureProfile : Profile
    {
        public SolarInfrastructureProfile()
        {
            CreateMap<SolarQuoteDTO, SolarQuoteEmailTemplatedModel>();
        }
    }
}
