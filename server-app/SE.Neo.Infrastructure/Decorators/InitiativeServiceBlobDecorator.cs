
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Initiative;
using SE.Neo.Common.Models.Media;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Decorators;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Decorators
{
    public class InitiativeServiceBlobDecorator : AbstractInitiativeServiceDecorator
    {
        private readonly IBlobServicesFacade _blobServicesFacade;

        public InitiativeServiceBlobDecorator(
            IInitiativeService initiativeService,
            IBlobServicesFacade blobServicesFacade)
            : base(initiativeService)
        {
            _blobServicesFacade = blobServicesFacade;
        }
        public override async Task<InitiativeDTO> CreateOrUpdateInitiativeAsync(int id, InitiativeDTO modelDTO)
        {
            return await base.CreateOrUpdateInitiativeAsync(id, modelDTO);
        }

        /// <summary>
        /// Get all initiatives for admin module.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>

        public override async Task<WrapperModel<InitiativeAdminDTO>> GetAllInitiativesAsync(BaseSearchFilterModel filter)
        {
            WrapperModel<InitiativeAdminDTO> initiatives = await base.GetAllInitiativesAsync(filter);

            List<InitiativeAdminDTO> initiativeDTOs = initiatives.DataList.ToList();

            await _blobServicesFacade.PopulateWithBlobAsync(initiativeDTOs, dto => dto.User.Image, (dto, b) => dto.User.Image = b);
            initiatives.DataList = initiativeDTOs;

            return initiatives;
        }

        public override async Task<WrapperModel<ConversationForInitiativeDTO>> GetSavedConversationsForInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin)
        {
            WrapperModel<ConversationForInitiativeDTO> conversationsWrapper = await base.GetSavedConversationsForInitiativeAsync(initiativeId, filter, userId, isAdmin);

            List<ConversationForInitiativeDTO> conversations = conversationsWrapper.DataList.ToList();

            await AssignImagesToConversationsAsync(conversationsWrapper.DataList.ToList());

            conversationsWrapper.DataList = conversations;

            return conversationsWrapper;
        }

        public override async Task<WrapperModel<ConversationForInitiativeDTO>> GetRecommendedConversationsForInitiativeAsync(InitiativeRecommendationRequest request, int userId, int initiativeId)
        {
            WrapperModel<ConversationForInitiativeDTO> conversationsWrapper = await base.GetRecommendedConversationsForInitiativeAsync(request, userId, initiativeId);

            List<ConversationForInitiativeDTO> conversations = conversationsWrapper.DataList.ToList();

            await AssignImagesToConversationsAsync(conversationsWrapper.DataList.ToList());

            conversationsWrapper.DataList = conversations;

            return conversationsWrapper;
        }

        public override async Task<InitiativeContentsWrapperModel<ToolForInitiativeDTO>> GetRecommendedToolsForInitiativeAsync(InitiativeRecommendationRequest request, int userId, int companyId, int initiativeId)
        {
           InitiativeContentsWrapperModel<ToolForInitiativeDTO> toolsWrapper = await base.GetRecommendedToolsForInitiativeAsync(request, userId, companyId, initiativeId);
            List<ToolForInitiativeDTO> toolsDTO = toolsWrapper.DataList.ToList();
            await _blobServicesFacade.PopulateWithBlobAsync(toolsDTO, dto => dto?.ImageUrl, (dto, b) => { if (dto?.ImageUrl != null) dto.ImageUrl = b; });
            toolsWrapper.DataList = toolsDTO;
            return toolsWrapper;
        }

        public override async Task<WrapperModel<ToolForInitiativeDTO>> GetSavedToolsForInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin)
        {
            WrapperModel<ToolForInitiativeDTO> toolsWrapper = await base.GetSavedToolsForInitiativeAsync(initiativeId, filter, userId, isAdmin);
            List<ToolForInitiativeDTO> toolsDTO = toolsWrapper.DataList.ToList();
            await _blobServicesFacade.PopulateWithBlobAsync(toolsDTO, dto => dto?.ImageUrl, (dto, b) => { if (dto?.ImageUrl != null) dto.ImageUrl = b; });
            toolsWrapper.DataList = toolsDTO;
            return toolsWrapper;
        }

        private async Task AssignImagesToConversationsAsync(List<ConversationForInitiativeDTO> conversations)
        {
            if (!conversations.Any())
                return;
            ///Solution 2
            {
                var tasks = new List<Task>();
                var imageCache = new Dictionary<int, Uri>();
                foreach (ConversationForInitiativeDTO conversation in conversations)
                {
                    if (conversation.LastMessage != null)
                    {
                        var attachmentDTOs = conversation.LastMessage.Attachments?.ToList();
                        if (attachmentDTOs?.Any() == true)
                        {
                            tasks.Add(_blobServicesFacade.PopulateWithBlobAsync(attachmentDTOs, dto => dto.Image, (dto, b) => dto.Image = b)
                                .ContinueWith(_ => conversation.LastMessage.Attachments = attachmentDTOs));
                        }
                    }

                    var distinctConversationUserDTOs = conversation.Users.DistinctBy(x => x.Id).Take(5).ToList();
                    foreach (var item in distinctConversationUserDTOs)
                    {
                        if (!imageCache.ContainsKey(item.Id))
                        {
                            tasks.Add(_blobServicesFacade.PopulateWithBlobAsync(item, dto => dto.Image, (dto, b) =>
                            {
                                dto.Image = b;
                                imageCache[item.Id] = b.Uri;
                            }));
                        }
                        else
                        {
                            item.Image.Uri = imageCache[item.Id];
                        }
                    }
                }
                await Task.WhenAll(tasks);
            }


            ///Solution 1
            //{

            //    foreach (ConversationForInitiativeDTO conversation in conversations)
            //    {
            //        if (conversation.LastMessage != null)
            //        {
            //            List<AttachmentDTO> attachmentDTOs = conversation.LastMessage.Attachments.ToList();
            //            await _blobServicesFacade.PopulateWithBlobAsync(attachmentDTOs, dto => dto.Image, (dto, b) => dto.Image = b);
            //            conversation.LastMessage.Attachments = attachmentDTOs;
            //        }

            //        List<ConversationUserForInitiativeDTO> conversationUserDTOs = conversation.Users.ToList();
            //        await _blobServicesFacade.PopulateWithBlobAsync(conversationUserDTOs, dto => dto.Image, (dto, b) => dto.Image = b);
            //        conversation.Users = conversationUserDTOs;
            //    }
            //    await _blobServicesFacade.PopulateWithBlobAsync(
            //        conversations,
            //        dto => dto.LastMessage?.User?.Image,
            //        (dto, b) =>
            //        {
            //            if (dto.LastMessage?.User != null)
            //                dto.LastMessage.User.Image = b;
            //        });
            //}
        }

        public override async Task<bool> RemoveContentFromInitiativeAsync(int userId, int initiativeId, int contentId, InitiativeModule contentType, bool isAdmin)
        {
            string filename = await base.GetBlobFileName(contentId);
            bool isRemoved = await base.RemoveContentFromInitiativeAsync(userId, initiativeId, contentId, contentType, isAdmin);

            if (isRemoved && contentType == InitiativeModule.Files && !string.IsNullOrEmpty(filename))
            {
                await _blobServicesFacade.DeleteBlobAsync(new BlobBaseDTO()
                {
                    Name = filename,
                    ContainerName = BlobType.Initiative.ToString()
                });
            }
            return true;
        }

        public override async Task<InitiativeContentsWrapperModel<CommunityUserForInitiativeDTO>> GetRecommendedCommunityUsersForInitiativeAsync(InitiativeRecommendationRequest request, int userId, int initiativeId)
        {
            InitiativeContentsWrapperModel<CommunityUserForInitiativeDTO> usersWrapper = await base.GetRecommendedCommunityUsersForInitiativeAsync(request, userId, initiativeId);

            List<CommunityUserForInitiativeDTO> users = usersWrapper.DataList.ToList();
            await _blobServicesFacade.PopulateWithBlobAsync(users, dto => dto.Image, (dto, b) => dto.Image = b);
            usersWrapper.DataList = users;

            return usersWrapper;
        }

        public override async Task<WrapperModel<CommunityUserForInitiativeDTO>> GetSavedCommunityUsersForInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin)
        {
            WrapperModel<CommunityUserForInitiativeDTO> usersWrapper = await base.GetSavedCommunityUsersForInitiativeAsync(initiativeId, filter, userId, isAdmin);

            List<CommunityUserForInitiativeDTO> users = usersWrapper.DataList.ToList();
            await _blobServicesFacade.PopulateWithBlobAsync(users, dto => dto.Image, (dto, b) => dto.Image = b);
            usersWrapper.DataList = users;
            return usersWrapper;
        }

        public override async Task<InitiativeContentsWrapperModel<ProjectForInitiativeDTO>> GetRecommendedProjectsForInitiativeAsync(InitiativeRecommendationRequest request, int userId, List<int> roleIds, int intiativeId)
        {
            InitiativeContentsWrapperModel<ProjectForInitiativeDTO> projectsWrapper = await base.GetRecommendedProjectsForInitiativeAsync(request, userId, roleIds, intiativeId);
            List<ProjectForInitiativeDTO> projectsDTO = projectsWrapper.DataList.ToList();
            foreach (var project in projectsDTO)
            {
                await _blobServicesFacade.PopulateWithBlobAsync(project, dto => dto?.Company.Image, (dto, b) => { if (dto != null) dto.Company.Image = b; });
            }
            projectsWrapper.DataList = projectsDTO;
            return projectsWrapper;
        }

        public override async Task<WrapperModel<ProjectForInitiativeDTO>> GetSavedProjectsForInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin)
        {
            WrapperModel<ProjectForInitiativeDTO> projectsWrapper = await base.GetSavedProjectsForInitiativeAsync(initiativeId, filter, userId, isAdmin);
            List<ProjectForInitiativeDTO> projectsDTOs = projectsWrapper.DataList.ToList();
            foreach (var project in projectsDTOs)
            {
                await _blobServicesFacade.PopulateWithBlobAsync(project, dto => dto?.Company.Image, (dto, b) => { if (dto != null) dto.Company.Image = b; });
            }
            projectsWrapper.DataList = projectsDTOs;

            return projectsWrapper;
        }

        public override async Task<WrapperModel<InitiativeAndProgressDetailsDTO>> GetInitiativesAndProgressDetailsByUserIdAsync(int userId, InitiativeViewSource initiativeType, BaseSearchFilterModel request)
        {
            WrapperModel<InitiativeAndProgressDetailsDTO> initiativesAndProgressDetailsDTOWrapper = await base.GetInitiativesAndProgressDetailsByUserIdAsync(userId, initiativeType, request);
            if (initiativeType == InitiativeViewSource.YourInitiatives)
            {
                List<InitiativeAndProgressDetailsDTO> initiativeAndProgressDetailsDTOs = initiativesAndProgressDetailsDTOWrapper.DataList.ToList();
                foreach (var initiativeAndProgressDetailsDTO in initiativeAndProgressDetailsDTOs)
                {
                    List<UserDTO> userDTOs = initiativeAndProgressDetailsDTO.Collaborators.ToList();
                    await _blobServicesFacade.PopulateWithBlobAsync(userDTOs, dto => dto?.Image, (dto, b) => { if (dto != null) dto.Image = b; });
                }
                initiativesAndProgressDetailsDTOWrapper.DataList = initiativeAndProgressDetailsDTOs;
            }
            return initiativesAndProgressDetailsDTOWrapper;
        }


    }
}