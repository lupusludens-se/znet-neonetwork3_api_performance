using AutoMapper;
using SE.Neo.Common.Models.Feedback;
using SE.Neo.Common.Models.User;
using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Mapping
{
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<Feedback, FeedbackDTO>()
                .ForMember(dest => dest.User, opts => opts.MapFrom(src => src.User))
                .ForPath(dest => dest.User.Roles, opts => opts.MapFrom(src => src.User.Roles.Select(o => o.Role)));
            CreateMap<FeedbackDTO, Feedback>()
                .ForMember(dest => dest.Rating, opts => opts.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Comments, opts => opts.MapFrom(src => src.Comments));
            CreateMap<CreateFeedbackDTO, Feedback>()
                 .ForMember(dest => dest.Rating, opts => opts.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Comments, opts => opts.MapFrom(src => src.Comments));
            CreateMap<User, UserDTO>();
        }

    }
}
