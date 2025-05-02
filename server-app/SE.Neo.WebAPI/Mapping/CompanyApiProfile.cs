using AutoMapper;
using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;
using SE.Neo.WebAPI.Models;
using SE.Neo.WebAPI.Models.Company;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.UserProfile;

namespace SE.Neo.WebAPI.Mapping
{
    public class CompanyApiProfile : Profile
    {
        public CompanyApiProfile()
        {
            CreateMap<CompanyDTO, CompanyResponse>();
            CreateMap<CompanyFollowerDTO, CompanyFollowerResponse>();
            CreateMap<UserDTO, UserFollowerResponse>()
                .ForMember(dest => dest.Company, opts => opts.MapFrom(src => src.Company.Name))
                .ForMember(dest => dest.JobTitle, opts => opts.MapFrom(src => src.UserProfile.JobTitle));

            CreateMap<CompanyRequest, CompanyDTO>()
                .ForMember(dest => dest.About, opts => opts.NullSubstitute(string.Empty))
                .ForMember(dest => dest.LinkedInUrl, opts => opts.NullSubstitute(string.Empty))
                .ForMember(dest => dest.CompanyUrl, opts => opts.NullSubstitute(string.Empty))
                .ForMember(dest => dest.MDMKey, opts => opts.NullSubstitute(string.Empty));

            CreateMap<CompanyUrlLinkDTO, UrlLinkModel>();

            CreateMap<UrlLinkModel, CompanyUrlLinkDTO>();

            CreateMap<OffsitePPAsRequest, BaseIdNameDTO>();

            CreateMap<CompanyResponse, CompanyDTO>();

            CreateMap<CompanyFileDTO, CompanyFileResponse>()
            .ForMember(dest => dest.BlobName, opts => opts.MapFrom(src => src.Name));
            CreateMap<CompanyFileResponse, CompanyFileDTO>()
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.BlobName));


            CreateMap<CompanyFileRequest, CompanyFileDTO>()
                 .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.BlobName));


            CreateMap<CompanyAnnouncementCreateOrUpdateRequest, CompanyAnnouncementDTO>();
            CreateMap<CompanyAnnouncementDTO, CompanyAnnouncementResponse>();            
        }
    }
}