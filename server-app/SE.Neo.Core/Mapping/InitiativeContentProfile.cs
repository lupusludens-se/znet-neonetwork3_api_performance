using AutoMapper;
using SE.Neo.Common.Models.Initiative;


namespace SE.Neo.Core.Mapping
{
    public class InitiativeContentProfile : Profile
    {
        public InitiativeContentProfile()
        {
            CreateMap<InitiativeContentRequest, InitiativeContentDTO>();
        }
    }
}
