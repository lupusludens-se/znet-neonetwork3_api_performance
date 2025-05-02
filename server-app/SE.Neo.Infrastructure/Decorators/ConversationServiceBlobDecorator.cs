using SE.Neo.Common.Models.Conversation;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Services.Decorators;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Decorators
{
    public class ConversationServiceBlobDecorator : AbstractConversationServiceDecorator
    {
        private readonly IBlobServicesFacade _blobServicesFacade;

        public ConversationServiceBlobDecorator(
            IConversationService conversationService,
            IBlobServicesFacade blobServicesFacade)
            : base(conversationService)
        {
            _blobServicesFacade = blobServicesFacade;
        }

        public override async Task<ConversationDTO?> GetConversationForUserAsync(int userId, int id, string? expand = null, bool allowedPrivate = false)
        {
            ConversationDTO? conversation = await _conversationService.GetConversationForUserAsync(userId, id, expand, allowedPrivate);

            if (conversation != null)
            {
                await AssignImagesToConversationsAsync(new List<ConversationDTO> { conversation });
            }

            return conversation;
        }

        public override async Task<ConversationMessageDTO?> GetConversationMessageAsync(int id, string? expand = null)
        {
            ConversationMessageDTO? message = await _conversationService.GetConversationMessageAsync(id, expand);

            if (message != null)
            {
                await AssignImagesToMessagesAsync(new List<ConversationMessageDTO> { message });
            }

            return message;
        }

        public override async Task<WrapperModel<ConversationMessageDTO>> GetConversationMessagesAsync(int id, ConversationMessagesFilterDTO filter)
        {
            WrapperModel<ConversationMessageDTO> wrapperModel = await _conversationService.GetConversationMessagesAsync(id, filter);

            await AssignImagesToMessagesAsync(wrapperModel.DataList.ToList());

            return wrapperModel;
        }

        public override async Task<WrapperModel<ConversationMessageDTO>> GetConversationMessagesForUserAsync(int id, ConversationMessagesFilterDTO filter, int userId)
        {
            WrapperModel<ConversationMessageDTO>? wrapperModel = await _conversationService.GetConversationMessagesForUserAsync(id, filter, userId);

            await AssignImagesToMessagesAsync(wrapperModel.DataList.ToList());

            return wrapperModel;
        }

        public override async Task<WrapperModel<ConversationDTO>> GetConversationsAsync(ConversationsFilter filter, int? companyId)
        {
            WrapperModel<ConversationDTO>? wrapperModel = await _conversationService.GetConversationsAsync(filter, companyId);

            await AssignImagesToConversationsAsync(wrapperModel.DataList.ToList());

            return wrapperModel;
        }

        public override async Task<WrapperModel<ConversationDTO>> GetConversationsForUserAsync(int userId, ConversationsFilter filter)
        {
            WrapperModel<ConversationDTO>? wrapperModel = await _conversationService.GetConversationsForUserAsync(userId, filter);

            await AssignImagesToConversationsAsync(wrapperModel.DataList.ToList());

            return wrapperModel;
        }

        private async Task AssignImagesToMessagesAsync(List<ConversationMessageDTO> messages)
        {
            if (!messages.Any())
                return;

            foreach (ConversationMessageDTO message in messages)
            {
                List<AttachmentDTO> attachmentDTOs = message.Attachments.ToList();
                await _blobServicesFacade.PopulateWithBlobAsync(attachmentDTOs, dto => dto.Image, (dto, b) => dto.Image = b);
                message.Attachments = attachmentDTOs;
            }

            await _blobServicesFacade.PopulateWithBlobAsync(messages.Where(m => m.User != null).ToList(), dto => dto.User!.Image, (dto, b) => dto.User!.Image = b);
        }

        private async Task AssignImagesToConversationsAsync(List<ConversationDTO> conversations)
        {
            if (!conversations.Any())
                return;

            ///Solution 2
            //{
            //    var tasks = new List<Task>();

            //    foreach (ConversationDTO conversation in conversations)
            //    {
            //        if (conversation.LastMessage != null)
            //        {
            //            var attachmentDTOs = conversation.LastMessage.Attachments.ToList();
            //            tasks.Add(_blobServicesFacade.PopulateWithBlobAsync(attachmentDTOs, dto => dto.Image, (dto, b) => dto.Image = b)
            //                .ContinueWith(_ => conversation.LastMessage.Attachments = attachmentDTOs));

            //        }

            //        var conversationUserDTOs = conversation.Users.ToList();
            //        tasks.Add(_blobServicesFacade.PopulateWithBlobAsync(conversationUserDTOs, dto => dto.Image, (dto, b) => dto.Image = b)
            //                .ContinueWith(_ => conversation.Users = conversationUserDTOs));
            //    }

            //    tasks.Add(
            //        _blobServicesFacade.PopulateWithBlobAsync(
            //        conversations,
            //        dto => dto.LastMessage?.User?.Image,
            //        (dto, b) =>
            //        {
            //            if (dto.LastMessage?.User != null)
            //                dto.LastMessage.User.Image = b;
            //        }));

            //    await Task.WhenAll(tasks);
            //}

            // Solution 1
            {
                foreach (ConversationDTO conversation in conversations)
                {
                    if (conversation.LastMessage != null)
                    {
                        List<AttachmentDTO> attachmentDTOs = conversation.LastMessage.Attachments.ToList();
                        await _blobServicesFacade.PopulateWithBlobAsync(attachmentDTOs, dto => dto.Image, (dto, b) => dto.Image = b);
                        conversation.LastMessage.Attachments = attachmentDTOs;
                    }

                    List<ConversationUserDTO> conversationUserDTOs = conversation.Users.ToList();
                    await _blobServicesFacade.PopulateWithBlobAsync(conversationUserDTOs, dto => dto.Image, (dto, b) => dto.Image = b);
                    conversation.Users = conversationUserDTOs;
                }
                await _blobServicesFacade.PopulateWithBlobAsync(
                    conversations,
                    dto => dto.LastMessage?.User?.Image,
                    (dto, b) =>
                    {
                        if (dto.LastMessage?.User != null)
                            dto.LastMessage.User.Image = b;
                    });
            }
        }
    }
}