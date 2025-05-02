using AutoMapper;
using SE.Neo.EmailTemplates.Models;
using SE.Neo.WebAPI.Models.ScheduleDemo;

namespace SE.Neo.WebAPI.Mapping
{
    public class ScheduleDemoProfile : Profile
    {
        public ScheduleDemoProfile()
        {
            CreateMap<ScheduleDemoRequest, ScheduleDemoToUserTemplateModel>();

            CreateMap<ScheduleDemoRequest, ScheduleDemoToAdminTemplateModel>();

        }
    }
}
