using SE.Neo.Common.Models.Conversation;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services.Decorators
{
    public abstract class AbstractConversationServiceDecorator : IConversationService
    {
        protected readonly IConversationService _conversationService;

        public AbstractConversationServiceDecorator(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        public virtual async Task<int> CreateConversationAsync(ConversationDTO conversation, ConversationMessageDTO initialConversationMessage)
        {
            return await _conversationService.CreateConversationAsync(conversation, initialConversationMessage);
        }

        public virtual async Task<int> CreateConversationMessageAsync(ConversationMessageDTO model)
        {
            return await _conversationService.CreateConversationMessageAsync(model);
        }

        public virtual async Task<ConversationDTO?> GetConversationForUserAsync(int userId, int id, string? expand = null, bool allowedPrivate = false)
        {
            return await _conversationService.GetConversationForUserAsync(userId, id, expand, allowedPrivate);
        }

        public virtual async Task<ConversationMessageDTO?> GetConversationMessageAsync(int id, string? expand = null)
        {
            return await _conversationService.GetConversationMessageAsync(id, expand);
        }

        public virtual async Task<bool?> UpdateMessageAsync(int id, string messageText)
        {
            return await _conversationService.UpdateMessageAsync(id, messageText);
        }

        public virtual async Task<bool?> DeleteMessageAsync(int id)
        {
            return await _conversationService.DeleteMessageAsync(id);
        }

        public virtual async Task<WrapperModel<ConversationMessageDTO>> GetConversationMessagesAsync(int id, ConversationMessagesFilterDTO filter)
        {
            return await _conversationService.GetConversationMessagesAsync(id, filter);
        }

        public virtual async Task<WrapperModel<ConversationMessageDTO>> GetConversationMessagesForUserAsync(int id, ConversationMessagesFilterDTO filter, int userId)
        {
            return await _conversationService.GetConversationMessagesForUserAsync(id, filter, userId);
        }

        public virtual async Task<WrapperModel<ConversationDTO>> GetConversationsAsync(ConversationsFilter filter, int? companyId)
        {
            return await _conversationService.GetConversationsAsync(filter, companyId);
        }

        public virtual async Task<WrapperModel<ConversationDTO>> GetConversationsForUserAsync(int userId, ConversationsFilter filter)
        {
            return await _conversationService.GetConversationsForUserAsync(userId, filter);
        }

        public virtual async Task<int> GetUnreadUserMessagesCountAsync(int userId)
        {
            return await _conversationService.GetUnreadUserMessagesCountAsync(userId);
        }

        public virtual async Task MarkUserMessagesAsReadAsync(int userId, int conversationId)
        {
            await _conversationService.MarkUserMessagesAsReadAsync(userId, conversationId);
        }

        public virtual async Task<int> UpdateConversationAsync(ConversationDTO conversation)
        {
            return await _conversationService.UpdateConversationAsync(conversation);
        }

        public virtual async Task<List<ConversationUserDTO>> GetConversationUserAdminsAsync(int userId)
        {
            return await _conversationService.GetConversationUserAdminsAsync(userId);
        }

        public async Task<List<ConversationUserDTO>> GetConversationContactProviderUserAsync(int userId)
        {
            return await _conversationService.GetConversationContactProviderUserAsync(userId);
        }

        public async Task<int?> GetContactProviderConversationAsync(int userId, int projectId)
        {
            return await _conversationService.GetContactProviderConversationAsync(userId, projectId);
        }
    }
}