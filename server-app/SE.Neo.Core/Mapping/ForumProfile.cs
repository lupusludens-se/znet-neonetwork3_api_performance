using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Forum;
using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Mapping
{
    public class ForumProfile : Profile
    {
        public ForumProfile()
        {
            CreateMap<Discussion, ForumDTO>()
                .ForMember(dest => dest.IsPrivate, opt => opt.MapFrom(src => src.Type == DiscussionType.PrivateForum))
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.DiscussionUsers.Where(x => x.User != null).Select(x => x.User)))
                .ForMember(dest => dest.Regions, opt => opt.MapFrom(src => src.DiscussionRegions.Where(x => x.Region != null).Select(x => x.Region)))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.DiscussionCategories.Where(x => x.Category != null).Select(x => x.Category)))
                .ForMember(dest => dest.FirstMessage, opt => opt.MapFrom(src => src.Messages.Where(x => x.ParentMessage == null).OrderBy(x => x.CreatedOn).FirstOrDefault()))
                .ForMember(dest => dest.ResponsesCount, opt => opt.MapFrom(src => src.Messages.Count > 0 ? src.Messages.Count - 1 : 0))
                .ForMember(dest => dest.IsFollowed, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.IsPrivate ? DiscussionType.PrivateForum : DiscussionType.PublicForum))
                .ForMember(dest => dest.DiscussionRegions, opt => opt.MapFrom(src => src.Regions))
                .ForMember(dest => dest.DiscussionCategories, opt => opt.MapFrom(src => src.Categories))
                .ForMember(dest => dest.Messages, opt => opt.Ignore())
                .ForMember(dest => dest.DiscussionUsers, opt => opt.Ignore());

            CreateMap<RegionDTO, DiscussionRegion>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DisscussionId, opt => opt.Ignore())
                .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.Id));

            CreateMap<CategoryDTO, DiscussionCategory>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DisscussionId, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ForumUserDTO, DiscussionUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Message, ForumMessageDTO>()
                .ForMember(dest => dest.RepliesCount, opts => opts.MapFrom(src => src.Messages.Count))
                .ForMember(dest => dest.ForumId, opts => opts.MapFrom(src => src.DiscussionId))
                .ForMember(dest => dest.LikesCount, opts => opts.MapFrom(src => src.MessageLikes.Count));

            CreateMap<ForumMessageDTO, Message>()
                .ForMember(dest => dest.DiscussionId, opts => opts.MapFrom(src => src.ForumId))
                .ForMember(dest => dest.CreatedOn, opts => opts.Ignore());

            CreateMap<User, ForumUserDTO>()
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Company, opts => opts.MapFrom(src => src.Company.Name))
                .ForMember(dest => dest.JobTitle, opts => opts.MapFrom(src => src.UserProfile.JobTitle));

            CreateMap<ForumFollowDTO, DiscussionFollower>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.DisscussionId, opts => opts.MapFrom(src => src.ForumId)); ;

            CreateMap<ForumMessageLikeDTO, MessageLike>()
                .ForMember(dest => dest.Id, opts => opts.Ignore());
        }
    }
}