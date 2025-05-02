using AutoMapper;
using SE.Neo.Common.Models.Conversation;
using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Models;
using SE.Neo.WebAPI.Models.Conversation;
using SE.Neo.WebAPI.Models.Shared;

namespace SE.Neo.WebAPI.Mapping
{
    public class ConversationApiProfile : Profile
    {
        public ConversationApiProfile()
        {
            CreateMap<ConversationRequest, ConversationDTO>();
            CreateMap<ConversationUserRequest, ConversationUserDTO>();
            CreateMap<ConversationMessageRequest, ConversationMessageDTO>();
            CreateMap<MessageAttachmentRequest, AttachmentDTO>();

            CreateMap<ConversationDTO, ConversationResponse>();
            CreateMap<ConversationMessageDTO, ConversationMessageResponse>();
            CreateMap<AttachmentDTO, AttachmentResponse>();
            CreateMap<ConversationUserDTO, ConversationUserResponse>();
            CreateMap<ConversationUserDTO, ConversationUserRequest>();
        }
    }
}
