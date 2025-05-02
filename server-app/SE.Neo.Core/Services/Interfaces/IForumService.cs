using SE.Neo.Common.Models.Forum;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface IForumService
    {
        Task<int> CreateForumAsync(ForumDTO model, ForumMessageDTO initialMessage);

        Task<int> UpdateForumAsync(ForumDTO model);

        Task DeleteForumAsync(int id);

        Task<ForumDTO?> GetForumForUserAsync(int id, int userId, string? expand = null, bool allowedPrivate = false);

        Task<ForumDTO?> GetForumByIdAsync(int id, bool includeMessages = true);

        Task<WrapperModel<ForumDTO>> GetForumsForUserAsync(int userId, BaseSearchFilterModel filter, bool allowedPrivate = false);

        Task<int?> FollowForumAsync(ForumFollowDTO model, bool isAdminUser = false);

        Task UnFollowForumAsync(ForumFollowDTO model);

        Task<int?> CreateForumMessageAsync(ForumMessageDTO model, bool isAdminUser = false);

        Task<int> UpdateForumMessageAsync(ForumMessageDTO model);

        Task<int?> LikeForumMessageAsync(ForumMessageLikeDTO model, int forumId, bool isAdminUser = false);

        Task UnLikeForumMessageAsync(ForumMessageLikeDTO model);

        Task<ForumMessageDTO?> GetForumMessageAsync(int messageId, string? expand = null);

        Task<WrapperModel<ForumMessageDTO>> GetForumMessagesForUserAsync(int forumId, int userId, ForumMessagesFilter filter, bool allowedPrivate = false);

        Task<int> GetForumMessageOwnerId(int messageId);

        Task<IEnumerable<int>> GetForumFollowersIdsAsync(int forumId);

        Task RemoveForumMessageAsync(int id, int messageId);

        Task<List<Message>> GetMessageWithChildren(int id);

        Task<List<string?>> GetAttachmentsFromMessages(List<Message> messages);
        bool IsMessageExist(int messageId);

        bool IsForumExist(int forumId);

        Task<int> EditForumAsync(ForumDTO request, int userID);
    }
}
