using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Conversation;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services
{
    public partial class ConversationService : BaseFilterService, IConversationService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<ConversationService> _logger;
        private readonly IMapper _mapper;

        public ConversationService(ApplicationContext context, ILogger<ConversationService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<int> CreateConversationAsync(ConversationDTO model, ConversationMessageDTO initialConversationMessage)
        {
            using IDbContextTransaction? transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                Discussion? conversation = (await _context.Discussions.AddAsync(_mapper.Map<Discussion>(model))).Entity;

                // create init message
                var message = _mapper.Map<Message>(initialConversationMessage);
                message.Discussion = conversation;
                message.StatusId = MessageStatus.Active;
                await _context.Messages.AddAsync(message);

                // add all participants & owner
                var conversationUsers = _mapper.Map<List<DiscussionUser>>(model.Users);
                foreach (var conversationUser in conversationUsers)
                {
                    conversationUser.Discussion = conversation;
                    conversationUser.UnreadCount = 1;
                }

                conversationUsers.Add(new DiscussionUser
                {
                    UserId = model.CreatedByUserId,
                    Discussion = conversation
                });

                await _context.DiscussionUsers.AddRangeAsync(conversationUsers);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return conversation.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(CoreErrorMessages.ErrorOnSaving);
                throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
            }
        }

        public async Task<ConversationDTO?> GetConversationForUserAsync(int userId, int id, string? expand = null, bool allowedPrivate = false)
        {
            var conversationsQueryable = ExpandConversations(_context.Discussions.AsNoTracking(), expand);
            Discussion? conversation = await conversationsQueryable.FirstOrDefaultAsync(c => c.Id == id && c.Type == DiscussionType.PrivateChat
                && (c.DiscussionUsers.Any(du => du.UserId == userId) || allowedPrivate));

            return _mapper.Map<ConversationDTO>(conversation);
        }

        public async Task<WrapperModel<ConversationDTO>> GetConversationsAsync(ConversationsFilter filter, int? companyId)
        {
            return await GetConversationsAsync(filter, null, companyId);
        }

        public async Task<WrapperModel<ConversationDTO>> GetConversationsForUserAsync(int userId, ConversationsFilter filter)
        {
            return await GetConversationsAsync(filter, userId, null);
        }

        public async Task<int> UpdateConversationAsync(ConversationDTO model)
        {
            // only participants can update conversation
            Discussion? conversation = await _context.Discussions.FirstOrDefaultAsync(c => c.Id == model.Id
                && c.Type == DiscussionType.PrivateChat && c.DiscussionUsers.Any(u => u.UserId == model.CreatedByUserId));
            if (conversation == null)
            {
                _logger.LogError("Trying to update non-existing conversation {ConversationId}", model.Id);
                throw new CustomException($"{CoreErrorMessages.ErrorOnSaving} {CoreErrorMessages.EntityNotFound}");
            }

            // assign project id if not assigned yet
            if (!conversation.ProjectId.HasValue)
            {
                conversation.ProjectId = model.ProjectId;
            }

            _context.Discussions.Update(conversation);
            await _context.SaveChangesAsync();

            return conversation.Id;
        }

        public async Task<WrapperModel<ConversationMessageDTO>> GetConversationMessagesAsync(int id, ConversationMessagesFilterDTO filter)
        {
            return await GetConversationMessagesAsync(id, filter, null);
        }

        public async Task<WrapperModel<ConversationMessageDTO>> GetConversationMessagesForUserAsync(int id, ConversationMessagesFilterDTO filter, int userId)
        {
            return await GetConversationMessagesAsync(id, filter, userId);
        }

        public async Task<ConversationMessageDTO?> GetConversationMessageAsync(int id, string? expand = null)
        {
            Message? message = await ExpandMessages(_context.Messages.AsNoTracking(), expand).FirstOrDefaultAsync(c => c.Id == id);

            return _mapper.Map<ConversationMessageDTO>(message);
        }

        public async Task<bool?> UpdateMessageAsync(int id, string messageText)
        {
            try
            {
                var message = await _context.Messages.FirstOrDefaultAsync(du => du.Id == id);
                if (message != null)
                {
                    message.Text = messageText;
                    _context.Messages.Update(message);
                    await _context.SaveChangesAsync();
                    return true;
                }

            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error while updating the conversation message. Error : {ex.Message}", id);
                throw;
            }
            return false;
        }

        public async Task<bool?> DeleteMessageAsync(int id)
        {
            try
            {
                var message = await _context.Messages.FirstOrDefaultAsync(du => du.Id == id);
                if (message != null)
                {
                    message.StatusId = MessageStatus.Deleted;
                    _context.Messages.Update(message);
                    await _context.SaveChangesAsync();
                    return true;
                }

            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error while deleting the conversation message. Error : {ex.Message}", id);
                throw;
            }
            return false;
        }

        public async Task<int> GetUnreadUserMessagesCountAsync(int userId)
        {
            int count = await _context.DiscussionUsers.AsNoTracking().Where(du => du.UserId == userId && du.UnreadCount > 0).SumAsync(x => x.UnreadCount);

            return count;
        }

        public async Task MarkUserMessagesAsReadAsync(int userId, int conversationId)
        {
            DiscussionUser? discussionUser = await _context.DiscussionUsers.FirstOrDefaultAsync(du => du.UserId == userId && du.DiscussionId == conversationId);
            if (discussionUser != null)
            {
                discussionUser.UnreadCount = 0;
                _context.DiscussionUsers.Update(discussionUser);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CreateConversationMessageAsync(ConversationMessageDTO model)
        {
            Discussion? existingConversation = await _context.Discussions.Include(x => x.DiscussionUsers).AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == model.ConversationId
                    && c.Type == DiscussionType.PrivateChat && c.DiscussionUsers.Any(du => du.UserId == model.UserId));
            if (existingConversation == null)
            {
                _logger.LogWarning("Conversation {ConversationId} not found, or user {UserId} is not participant.", model.ConversationId, model.UserId);
                throw new CustomException($"Conversation {model.ConversationId} not found, or user {model.UserId} is not participant.");
            }

            var message = _mapper.Map<Message>(model);

            message.StatusId = MessageStatus.Active;

            using IDbContextTransaction? transaction = _context.Database.BeginTransaction();
            try
            {
                await _context.Messages.AddAsync(message);
                foreach (var user in existingConversation.DiscussionUsers.Where(du => du.UserId != model.UserId))
                {
                    user.UnreadCount++;
                }

                _context.DiscussionUsers.UpdateRange(existingConversation.DiscussionUsers);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return message.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(CoreErrorMessages.ErrorOnSaving);
                throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
            }
        }

        public async Task<List<ConversationUserDTO>> GetConversationUserAdminsAsync(int userId)
        {
            List<User>? users = await _context.Users
            .Where(x => x.Roles.Any(r => r.RoleId == (int)RoleType.Admin || r.RoleId == (int)RoleType.SystemOwner)
                && x.Id != userId
                && x.StatusId == Enums.UserStatus.Active)
            .ToListAsync();
            return users.Select(_mapper.Map<ConversationUserDTO>).ToList();
        }

        public async Task<List<ConversationUserDTO>> GetConversationContactProviderUserAsync(int userId)
        {
            int companyId = _context.Users.FirstOrDefault(x => x.Id.Equals(userId)).CompanyId;
            List<User>? users = await _context.Users.Where(x => x.CompanyId == companyId && x.StatusId.Equals(Enums.UserStatus.Active)).ToListAsync();
            return users.Select(_mapper.Map<ConversationUserDTO>).ToList();
        }

        public async Task<int?> GetContactProviderConversationAsync(int userId, int projectId)
        {
            return (await _context.Discussions.FirstOrDefaultAsync(x => x.CreatedByUserId == userId && x.ProjectId == projectId && x.SourceTypeId == Enums.DiscussionSourceType.ProviderContact))?.Id;
        }
    }
}