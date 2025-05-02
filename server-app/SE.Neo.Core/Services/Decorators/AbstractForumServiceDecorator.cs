using SE.Neo.Common.Models.Forum;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services.Decorators
{
    public abstract class AbstractForumServiceDecorator : IForumService
    {
        protected readonly IForumService _forumService;

        public AbstractForumServiceDecorator(IForumService forumService)
        {
            _forumService = forumService;
        }

        public virtual async Task<int> CreateForumAsync(ForumDTO model, ForumMessageDTO initialMessage)
        {
            return await _forumService.CreateForumAsync(model, initialMessage);
        }

        public virtual async Task<int?> CreateForumMessageAsync(ForumMessageDTO model, bool isAdminUser = false)
        {
            return await _forumService.CreateForumMessageAsync(model, isAdminUser);
        }

        public virtual async Task DeleteForumAsync(int id)
        {
            await _forumService.DeleteForumAsync(id);
        }

        public virtual async Task<int?> FollowForumAsync(ForumFollowDTO model, bool isAdminUser = false)
        {
            return await _forumService.FollowForumAsync(model, isAdminUser);
        }

        public virtual async Task<ForumDTO?> GetForumForUserAsync(int id, int userId, string? expand = null, bool allowedPrivate = false)
        {
            return await _forumService.GetForumForUserAsync(id, userId, expand, allowedPrivate);
        }

        public virtual async Task<ForumDTO?> GetForumByIdAsync(int id, bool includeMessages = true)
        {
            return await _forumService.GetForumByIdAsync(id, includeMessages);
        }

        public virtual async Task<ForumMessageDTO?> GetForumMessageAsync(int messageId, string? expand = null)
        {
            return await _forumService.GetForumMessageAsync(messageId, expand);
        }

        public virtual async Task<WrapperModel<ForumMessageDTO>> GetForumMessagesForUserAsync(int forumId, int userId, ForumMessagesFilter filter, bool allowedPrivate = false)
        {
            return await _forumService.GetForumMessagesForUserAsync(forumId, userId, filter, allowedPrivate);
        }

        public virtual async Task<WrapperModel<ForumDTO>> GetForumsForUserAsync(int userId, BaseSearchFilterModel filter, bool allowedPrivate = false)
        {
            return await _forumService.GetForumsForUserAsync(userId, filter, allowedPrivate);
        }

        public virtual bool IsMessageExist(int messageId)
        {
            return _forumService.IsMessageExist(messageId);
        }

        public virtual async Task<int?> LikeForumMessageAsync(ForumMessageLikeDTO model, int forumId, bool isAdminUser = false)
        {
            return await _forumService.LikeForumMessageAsync(model, forumId, isAdminUser);
        }

        public virtual async Task UnLikeForumMessageAsync(ForumMessageLikeDTO model)
        {
            await _forumService.UnLikeForumMessageAsync(model);
        }

        public virtual async Task UnFollowForumAsync(ForumFollowDTO model)
        {
            await _forumService.UnFollowForumAsync(model);
        }

        public virtual async Task<IEnumerable<int>> GetForumFollowersIdsAsync(int forumId)
        {
            return await _forumService.GetForumFollowersIdsAsync(forumId);
        }

        public virtual async Task<int> UpdateForumAsync(ForumDTO model)
        {
            return await _forumService.UpdateForumAsync(model);
        }

        public virtual async Task<int> GetForumMessageOwnerId(int messageId)
        {
            return await _forumService.GetForumMessageOwnerId(messageId);
        }

        public virtual async Task<int> UpdateForumMessageAsync(ForumMessageDTO model)
        {
            return await _forumService.UpdateForumMessageAsync(model);
        }

        public virtual async Task RemoveForumMessageAsync(int id, int messageId)
        {
            await _forumService.RemoveForumMessageAsync(id, messageId);
        }

        public virtual async Task<List<Message>> GetMessageWithChildren(int id)
        {
            return await _forumService.GetMessageWithChildren(id);
        }

        public virtual async Task<List<string?>> GetAttachmentsFromMessages(List<Message> messages)
        {
            return await _forumService.GetAttachmentsFromMessages(messages);
        }

        public virtual bool IsForumExist(int forumId)
        {
            return _forumService.IsForumExist(forumId);
        }
        public virtual async Task<int> EditForumAsync(ForumDTO model, int userID)
        {
            return await _forumService.EditForumAsync(model, userID);
        }
    }
}