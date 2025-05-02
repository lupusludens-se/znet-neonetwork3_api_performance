using SE.Neo.Common.Models.Conversation;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface IConversationService
    {
        Task<WrapperModel<ConversationDTO>> GetConversationsAsync(ConversationsFilter filter, int? companyId = null);

        Task<WrapperModel<ConversationDTO>> GetConversationsForUserAsync(int userId, ConversationsFilter filter);

        Task<ConversationDTO?> GetConversationForUserAsync(int userId, int id, string? expand = null, bool allowedPrivate = false);

        Task<int> CreateConversationAsync(ConversationDTO conversation, ConversationMessageDTO initialConversationMessage);

        Task<int> UpdateConversationAsync(ConversationDTO conversation);

        Task<WrapperModel<ConversationMessageDTO>> GetConversationMessagesAsync(int id, ConversationMessagesFilterDTO filter);

        Task<WrapperModel<ConversationMessageDTO>> GetConversationMessagesForUserAsync(int id, ConversationMessagesFilterDTO filter, int userId);

        Task<ConversationMessageDTO?> GetConversationMessageAsync(int id, string? expand = null);

        Task<int> GetUnreadUserMessagesCountAsync(int userId);

        Task<int> CreateConversationMessageAsync(ConversationMessageDTO model);

        Task MarkUserMessagesAsReadAsync(int userId, int conversationId);

        Task<List<ConversationUserDTO>> GetConversationUserAdminsAsync(int userId);

        Task<bool?> DeleteMessageAsync(int messageId);

        Task<bool?> UpdateMessageAsync(int messageId, string messageText);

        Task<List<ConversationUserDTO>> GetConversationContactProviderUserAsync(int userId);

        Task<int?> GetContactProviderConversationAsync(int userId, int projectId);
    }
}
