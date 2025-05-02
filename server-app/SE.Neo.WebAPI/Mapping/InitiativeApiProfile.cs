using AutoMapper;
using SE.Neo.Common.Models.Initiative;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;
using SE.Neo.Core.Entities;
using SE.Neo.WebAPI.Models.Conversation;
using SE.Neo.WebAPI.Models.Initiative;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.User;

namespace SE.Neo.WebAPI.Mapping
{
    public class InitiativeApiProfile : Profile
    {
        public InitiativeApiProfile()
        {
            CreateMap<InitiativeDTO, InitiativeCreateOrUpdateRequest>();
            CreateMap<InitiativeCreateOrUpdateRequest, InitiativeDTO>();
            CreateMap<InitiativeDTO, InitiativeResponse>();
            CreateMap<ArticleForInitiativeDTO, InitiativeArticleResponse>();
            CreateMap<InitiativeAdminDTO, InitiativeAdminResponse>()
             .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => $"{src.User.LastName} {src.User.FirstName}"));

            CreateMap<InitiativeAndProgressDetailsDTO, InitiativeAndProgressDetailsResponse>().ForMember(dest => dest.InitiativeId, opts => opts.MapFrom(src => src.Id));
            CreateMap<InitiativeStepDTO, InitiativeStepResponse>();
            CreateMap<InitiativeSubStepDTO, InitiativeSubStepResponse>();
            CreateMap<InitiativeProgressDetails, InitiativeSubStepProgressDTO>();
            CreateMap<InitiativeSubStepRequest, InitiativeSubStepProgressDTO>();
            CreateMap<UserDTO, UserResponse>();


            CreateMap<ConversationUserForInitiativeDTO, ConversationUserResponse>();

            CreateMap<ConversationForInitiativeDTO, InitiativeConversationResponse>()
             .ForMember(dest => dest.Users, opts => opts.MapFrom(src => src.Users));

            CreateMap<ToolForInitiativeDTO, InitiativeToolResponse>();
            CreateMap<InitiativesAttachedContentDTO, InitiativesAttachedContentResponse>();
            CreateMap<AttachContentToInitiativeRequest, AttachContentToInitiativeDTO>();

            CreateMap<CommunityUserForInitiativeDTO, InitiativeCommunityUserResponse>();

            CreateMap<InitiativeFileRequest, InitiativeFileDTO>()
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.BlobName));
            CreateMap<InitiativeFileDTO, InitiativeFileRequest>()
                .ForMember(dest => dest.BlobName, opts => opts.MapFrom(src => src.Name));

            CreateMap<InitiativeFileDTO, InitiativeFileResponse>()
                .ForMember(dest => dest.BlobName, opts => opts.MapFrom(src => src.Name));
            CreateMap<InitiativeFileResponse, InitiativeFileDTO>()
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.BlobName));

            CreateMap<ProjectForInitiativeDTO, InitiativeProjectResponse>();
            CreateMap<FileExistResponseDTO, FileExistResponse>();
            CreateMap<InitiativeAdminDTO, InitiativeExportResponse>()
                .ForMember(dest => dest.RegionsString, opts => opts.MapFrom(src => string.Join(",", src.Regions.Select(r=> r.Name))))
                .ForMember(dest => dest.ChangedOn, opts => opts.MapFrom(src => src.ModifiedOn.Value.ToString("dd-MM-yyyy, hh:mm:ss:tt")))
                .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => $"{src.User.LastName} {src.User.FirstName}"));

        }

    }
}
