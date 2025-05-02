using SE.Neo.Common.Models.Forum;
using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Models.Forum;
using SE.Neo.WebAPI.Models.User;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface IForumApiService
    {
        Task<WrapperModel<ForumResponse>> GetForumsAsync(UserModel user, BaseSearchFilterModel filter);

        Task<ForumResponse> GetForumAsync(UserModel user, int id, string? expand = null);

        Task<ForumResponse> CreateForumAsync(int userId, ForumRequest model, UserModel user);

        Task<ForumResponse> UpdateForumAsync(int userId, int id, ForumRequest model);

        Task DeleteForumAsync(int userId, int id);

        Task<WrapperModel<ForumMessageResponse>> GetForumMessagesAsync(UserModel user, int forumId, ForumMessagesFilter filter);

        Task<ForumMessageResponse?> CreateForumMessageAsync(UserModel user, int forumId, ForumMessageRequest model);

        Task<ForumMessageResponse> UpdateForumMessageAsync(int forumId, int messageId, ForumMessageRequest model);

        Task<int?> LikeForumMessageAsync(UserModel userId, int id, int messageId);

        Task UnLikeForumMessageAsync(int userId, int messageId);

        Task<int?> FollowForumAsync(UserModel user, int forumId);

        Task UnFollowForumAsync(int userId, int forumId);

        Task RemoveForumMessageAsync(int id, int messageId);
        Task<ForumResponse> EditForumAsync(int userId, ForumRequest model);
        Task<int> GetForumMessageOwnerIdAsync(int messageId);
    }
}