using SE.Neo.Common.Models.Conversation;
using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Models.Conversation;
using SE.Neo.WebAPI.Models.User;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface IConversationApiService
    {
        Task<WrapperModel<ConversationResponse>> GetConversationsAsync(UserModel user, ConversationsFilter filter);

        Task<ConversationResponse> CreateConversationAsync(UserModel user, ConversationRequest model);

        Task<ConversationResponse> UpdateConversationAsync(UserModel user, int id, ConversationRequest model);

        Task<WrapperModel<ConversationMessageResponse>> GetConversationMessagesAsync(UserModel user, int conversationId, ConversationMessagesFilterDTO filter);

        Task<int> GetUnreadUserMessagesCountAsync(int userId);

        Task<ConversationMessageResponse> CreateConversationMessageAsync(UserModel user, int conversationId, ConversationMessageRequest model);

        Task MarkUserMessagesAsReadAsync(UserModel user, int conversationId);

        Task<ConversationResponse?> GetConversationAsync(UserModel user, int id, string? expand = null);

        Task<ConversationResponse> CreateContactUsConversationAsync(UserModel user, ConversationContactUsRequest model);
        Task<bool?> DeleteMessageAsync(int messageId, UserModel currentUser);

        Task<bool?> UpdateMessageAsync(int messageId, UserModel currentUser, ConversationMessageRequest model);

        Task<int?> GetContactProviderConversationAsync(int userId, int projectId);
    }
}
