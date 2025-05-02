using SE.Neo.Common.Models.Forum;
using SE.Neo.Common.Models.Media;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Decorators;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Decorators
{
    public class ForumServiceBlobDecorator : AbstractForumServiceDecorator
    {
        private readonly IBlobServicesFacade _blobServicesFacade;

        public ForumServiceBlobDecorator(
            IForumService forumService,
            IBlobServicesFacade blobServicesFacade)
            : base(forumService)
        {
            _blobServicesFacade = blobServicesFacade;
        }

        public override async Task<ForumDTO?> GetForumForUserAsync(int id, int userId, string? expand = null, bool allowedPrivate = false)
        {
            ForumDTO? forum = await base.GetForumForUserAsync(id, userId, expand, allowedPrivate);
            if (forum != null)
            {
                await AssignImagesToForumsAsync(new List<ForumDTO> { forum });
            }

            return forum;
        }

        public override async Task<ForumMessageDTO?> GetForumMessageAsync(int messageId, string? expand = null)
        {
            ForumMessageDTO? message = await base.GetForumMessageAsync(messageId, expand);
            if (message != null)
            {
                await AssignImagesToMessagesAsync(new List<ForumMessageDTO> { message });
            }

            return message;
        }

        public override async Task<WrapperModel<ForumMessageDTO>> GetForumMessagesForUserAsync(int forumId, int userId, ForumMessagesFilter filter, bool allowedPrivate = false)
        {
            WrapperModel<ForumMessageDTO> wrapperModel = await base.GetForumMessagesForUserAsync(forumId, userId, filter, allowedPrivate);

            await AssignImagesToMessagesAsync(wrapperModel.DataList.ToList());

            return wrapperModel;
        }

        public override async Task<WrapperModel<ForumDTO>> GetForumsForUserAsync(int userId, BaseSearchFilterModel filter, bool allowedPrivate = false)
        {
            WrapperModel<ForumDTO> wrapperModel = await base.GetForumsForUserAsync(userId, filter, allowedPrivate);

            await AssignImagesToForumsAsync(wrapperModel.DataList.ToList());

            return wrapperModel;
        }

        public override async Task RemoveForumMessageAsync(int id, int messageId)
        {
            List<Message> messages = await base.GetMessageWithChildren(messageId);
            List<string?> oldMessageAtachmetImageNames = await base.GetAttachmentsFromMessages(messages);

            await base.RemoveForumMessageAsync(id, messageId);

            foreach (string? oldMessageAtachmetImageName in oldMessageAtachmetImageNames)
            {
                if (!string.IsNullOrEmpty(oldMessageAtachmetImageName))
                    await _blobServicesFacade.DeleteBlobAsync(new BlobBaseDTO() { Name = oldMessageAtachmetImageName, ContainerName = BlobType.Forums.ToString() });
            }
        }

        private async Task AssignImagesToMessagesAsync(List<ForumMessageDTO> messages)
        {
            if (!messages.Any())
                return;

            foreach (ForumMessageDTO message in messages)
            {
                List<AttachmentDTO> attachmentDTOs = message.Attachments.ToList();
                await _blobServicesFacade.PopulateWithBlobAsync(attachmentDTOs, dto => dto.Image, (dto, b) => dto.Image = b);
                message.Attachments = attachmentDTOs;
            }

            await _blobServicesFacade.PopulateWithBlobAsync(messages.Where(x => x.User != null).ToList(), dto => dto.User!.Image, (dto, b) => dto.User!.Image = b);
        }

        private async Task AssignImagesToForumsAsync(List<ForumDTO> forums)
        {
            if (!forums.Any())
                return;

            foreach (ForumDTO forum in forums)
            {
                if (forum.FirstMessage != null)
                {
                    List<AttachmentDTO> attachmentDTOs = forum.FirstMessage.Attachments.ToList();
                    await _blobServicesFacade.PopulateWithBlobAsync(attachmentDTOs, dto => dto.Image, (dto, b) => dto.Image = b);
                    forum.FirstMessage.Attachments = attachmentDTOs;
                }

                List<ForumUserDTO> forumUserDTOs = forum.Users.ToList();
                await _blobServicesFacade.PopulateWithBlobAsync(forumUserDTOs, dto => dto.Image, (dto, b) => dto.Image = b);
                forum.Users = forumUserDTOs;
            }

            await _blobServicesFacade.PopulateWithBlobAsync(
                forums,
                dto => dto.FirstMessage?.User.Image,
                (dto, b) =>
                {
                    if (dto.FirstMessage?.User != null)
                        dto.FirstMessage.User.Image = b;
                });
        }
    }
}