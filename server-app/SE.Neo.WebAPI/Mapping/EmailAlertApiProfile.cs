using AutoMapper;
using SE.Neo.Common.Models.EmailAlert;
using SE.Neo.WebAPI.Models.EmailAlert;

namespace SE.Neo.WebAPI.Mapping
{
    public class EmailAlertApiProfile : Profile
    {
        public EmailAlertApiProfile()
        {
            CreateMap<EmailAlertDTO, EmailAlertResponse>();
            CreateMap<EmailAlertItemRequest, EmailAlertDTO>();
        }
    }
}
