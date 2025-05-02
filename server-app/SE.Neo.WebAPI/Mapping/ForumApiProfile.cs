using AutoMapper;
using SE.Neo.Common.Models.Forum;
using SE.Neo.WebAPI.Models.Forum;

namespace SE.Neo.WebAPI.Mapping
{
    public class ForumApiProfile : Profile
    {
        public ForumApiProfile()
        {
            CreateMap<ForumRequest, ForumDTO>();
            CreateMap<ForumUserRequest, ForumUserDTO>();
            CreateMap<ForumMessageRequest, ForumMessageDTO>();
            CreateMap<ForumFirstMessageRequest, ForumMessageDTO>();

            CreateMap<ForumDTO, ForumResponse>();
            CreateMap<ForumMessageDTO, ForumMessageResponse>();
            CreateMap<ForumUserDTO, ForumUserResponse>();
        }
    }
}
