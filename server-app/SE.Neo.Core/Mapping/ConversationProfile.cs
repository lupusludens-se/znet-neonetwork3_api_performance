using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models.Conversation;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Mapping
{
    public class ConversationProfile : Profile
    {
        public ConversationProfile()
        {
            CreateMap<Discussion, ConversationDTO>()
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.DiscussionUsers.Where(x => x.User != null).Select(x => x.User)))
                .ForMember(dest => dest.LastMessage, opt => { opt.PreCondition(src => src.Messages != null); opt.MapFrom(src => src.Messages.OrderByDescending(m => m.CreatedOn).FirstOrDefault()); })
                .ForMember(dest => dest.SourceTypeName, opts => opts.MapFrom(src => src.SourceTypeId != null
                    ? ((DiscussionSourceType)src.SourceTypeId).GetDescription()
                    : null));
            CreateMap<ConversationDTO, Discussion>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => DiscussionType.PrivateChat))
                .ForMember(dest => dest.Messages, opt => opt.Ignore())
                .ForMember(dest => dest.DiscussionUsers, opt => opt.Ignore());

            CreateMap<ConversationUserDTO, DiscussionUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Message, ConversationMessageDTO>()
                .ForMember(dest => dest.ConversationId, opts => opts.MapFrom(src => src.DiscussionId));

            CreateMap<ConversationMessageDTO, Message>()
                .ForMember(dest => dest.DiscussionId, opts => opts.MapFrom(src => src.ConversationId));

            CreateMap<Attachment, AttachmentDTO>().ReverseMap();

            CreateMap<User, ConversationUserDTO>()
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Company, opts => opts.MapFrom(src => src.Company.Name))
                .ForMember(dest => dest.IsSolutionProvider, opts => opts.MapFrom(src => src.Roles.Any(r =>
                    r.RoleId.Equals((int)Common.Enums.RoleType.SolutionProvider))));

        }
    }
}
