using AutoMapper;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Entities;
using File = SE.Neo.Core.Entities.File;

namespace SE.Neo.Core.Mapping
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDTO>()
                .ForMember(dest => dest.TypeName, opts => opts.MapFrom(src => src.TypeId.GetDescription()))
                .ForMember(dest => dest.Categories, opts => opts.MapFrom(src => src.Categories.Select(o => o.Category)))
                .ForMember(dest => dest.IndustryName, opts => opts.MapFrom(src => src.Industry.Name))
                .ForMember(dest => dest.StatusName, opts => opts.MapFrom(src => src.StatusId.ToString()))
                .ForMember(dest => dest.OffsitePPAs, opts => opts.MapFrom(src => src.OffsitePPAs.Select(o => o.OffsitePPA)));

            CreateMap<CompanyUrlLink, CompanyUrlLinkDTO>();

            CreateMap<CompanyDTO, Company>()
                .ForMember(m => m.Industry, i => i.Ignore())
                .ForMember(m => m.Categories, i => i.Ignore())
                .ForMember(m => m.UrlLinks, i => i.Ignore())
                .ForMember(m => m.Projects, i => i.Ignore())
                .ForMember(m => m.Users, i => i.Ignore())
                .ForMember(m => m.OffsitePPAs, i => i.Ignore())
                .ForMember(m => m.Country, i => i.Ignore());

            CreateMap<Industry, BaseIdNameDTO>();

            CreateMap<OffsitePPA, BaseIdNameDTO>();

            CreateMap<BaseIdNameDTO, Industry>();

            CreateMap<BaseIdNameDTO, OffsitePPA>();

            CreateMap<CompanyFollower, CompanyFollowerDTO>();


            CreateMap<CompanyFollower, CompanyFollowerDTO>();

            CreateMap<CompanyFileDTO, File>();

            CreateMap<CompanyAnnouncementRegion, RegionDTO>();

            CreateMap<CompanyAnnouncementDTO, CompanyAnnouncement>();
            CreateMap<CompanyAnnouncement, CompanyAnnouncementDTO>().ForMember(dest => dest.Regions, opt => opt.MapFrom(src => src.Regions.Select(id => new RegionDTO { Id = id.RegionId, Name = id.Region.Name })))
                .ForMember(dest => dest.RegionIds, opts => opts.MapFrom(src => src.Regions.Select(o => o.RegionId)));
        }
    }
}