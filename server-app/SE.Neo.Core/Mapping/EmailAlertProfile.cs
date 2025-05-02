using AutoMapper;
using SE.Neo.Common.Models.EmailAlert;
using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Mapping
{
    public class EmailAlertProfile : Profile
    {
        public EmailAlertProfile()
        {
            CreateMap<EmailAlert, EmailAlertDTO>()
                .ReverseMap();
        }
    }
}