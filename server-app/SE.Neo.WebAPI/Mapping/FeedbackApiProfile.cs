using AutoMapper;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models.Feedback;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;
using SE.Neo.WebAPI.Models.Feedback;
using SE.Neo.WebAPI.Models.Role;

namespace SE.Neo.WebAPI.Mapping
{
    public class FeedbackApiProfile : Profile
    {

        public FeedbackApiProfile()
        {
            CreateMap<FeedbackRequest, FeedbackDTO>();
            CreateMap<FeedbackRequest, CreateFeedbackDTO>();
            CreateMap<CreateFeedbackDTO, FeedbackResponse>();
            CreateMap<FeedbackDTO, FeedbackResponse>()
                .ForMember(dest => dest.FeedbackUser, opts => opts.MapFrom(src => src.User));
            CreateMap<UserDTO, FeedbackUserResponse>()
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => $"{src.LastName} {src.FirstName}"))
                .ForMember(dest => dest.Company, opts => opts.MapFrom(src => src.Company.Name))
                .ForMember(dest => dest.Image, opts => opts.MapFrom(src => src.Image))
                .ForMember(dest => dest.Roles, opts => opts.MapFrom(src => src.Roles.Select(x => x)));
            CreateMap<RoleDTO, RoleResponse>();
            CreateMap<FeedbackDTO, FeedbackExportResponse>()
                .ForMember(dest => dest.Comments, opts => opts.MapFrom(src => string.IsNullOrEmpty(src.Comments) ? string.Empty : HTMLExtensions.RemoveAllHTML(src.Comments)))
                .ForMember(dest => dest.Rating, opts => opts.MapFrom(src => src.Rating == 1 ? $"{src.Rating + " Star"}" : $"{src.Rating + " Stars"}"))
                .ForMember(dest => dest.CreatedOn, opts => opts.MapFrom(src => src.CreatedOn.Value.ToString("dd-MM-yyyy, hh:mm:ss:tt")))
                .ForMember(dest => dest.User, opts => opts.MapFrom(src => $"{src.User.LastName} {src.User.FirstName}"))
            .ForMember(dest => dest.Company, opts => opts.MapFrom(src => src.User.Company.Name))
            .ForMember(dest => dest.Role, opts => opts.MapFrom(src => src.User.Roles.Select(x => x.Name).FirstOrDefault()));


        }
    }
}
