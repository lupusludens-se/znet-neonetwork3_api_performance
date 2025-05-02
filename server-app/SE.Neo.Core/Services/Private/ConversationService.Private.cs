using Microsoft.EntityFrameworkCore;
using SE.Neo.Common;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Conversation;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Models.Conversation;

namespace SE.Neo.Core.Services
{
    public partial class ConversationService
    {
        private async Task<WrapperModel<ConversationMessageDTO>> GetConversationMessagesAsync(int id, ConversationMessagesFilterDTO filter, int? userId = null)
        {
            var messageQuery = ExpandMessages(_context.Messages.AsNoTracking(), filter.Expand)
                .Where(m => m.DiscussionId == id && m.Discussion.Type == DiscussionType.PrivateChat);

            if (userId.HasValue)
            {
                messageQuery = messageQuery.Where(m => m.Discussion.DiscussionUsers.Any(u => u.UserId == userId.Value));
            }

            messageQuery = messageQuery.OrderByDescending(x => x.CreatedOn);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await messageQuery.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<ConversationMessageDTO> { Count = count, DataList = new List<ConversationMessageDTO>() };
                }
            }

            if (filter.Skip.HasValue)
            {
                messageQuery = messageQuery.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                messageQuery = messageQuery.Take(filter.Take.Value);
            }

            List<Message>? messages = await messageQuery
                .ToListAsync();

            //messages.Where(x => x.StatusId == MessageStatus.Deleted)?.ToList()?.ForEach(x => x.Text = ZnConstants.MessageDeletedText);
            foreach (var item in messages.Where(x => x.StatusId == MessageStatus.Deleted))
            {
                if (item.Attachments?.Any() == true)
                {
                    item.Attachments.Clear();
                }
                item.Id = 0;
                item.Text = ZnConstants.MessageDeletedText;
            }

            return new WrapperModel<ConversationMessageDTO> { Count = count, DataList = _mapper.Map<List<ConversationMessageDTO>>(messages) };
        }

        private async Task<WrapperModel<ConversationDTO>> GetConversationsAsync(ConversationsFilter filter, int? userId = null, int? companyId = null)
        {
            var messageSourceQuery = GetMessageQueryForConversations(filter, userId, companyId);
            var messageQuery = messageSourceQuery.Select(e => e.Key).Distinct()
                .SelectMany(key => messageSourceQuery
                    .Where(e => e.Key.ConversationId == key.ConversationId).Select(e => e.Message)
                    .OrderByDescending(m => m.CreatedOn)
                    .Take(1));

            var messageQueryBySortOrder = GenerateQueryBySortOrder(filter, messageQuery, userId);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await messageQueryBySortOrder.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<ConversationDTO> { Count = count, DataList = new List<ConversationDTO>() };
                }
            }

            if (filter.Skip.HasValue)
            {
                messageQueryBySortOrder = messageQueryBySortOrder.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                messageQueryBySortOrder = messageQueryBySortOrder.Take(filter.Take.Value);
            }

            List<Message>? messages = await messageQueryBySortOrder.ToListAsync();

            List<Discussion>? conversations = await ExpandConversations(_context.Discussions.AsNoTracking(), filter.Expand + ",discussionusers")
                .Where(x => messages.Select(y => y.DiscussionId).Contains(x.Id))
                .ToListAsync();

            var conversationDTOs = new List<ConversationDTO>();
            foreach (var message in messages)
            {
                var conversationEntity = conversations.First(c => c.Id == message.DiscussionId);
                var conversation = _mapper.Map<ConversationDTO>(conversationEntity);
                conversation.LastMessage = _mapper.Map<ConversationMessageDTO>(message);
                conversation.UnreadCount = userId.HasValue ? (conversationEntity.DiscussionUsers.FirstOrDefault(u => u.UserId == userId)?.UnreadCount ?? 0) : 0;
                conversationDTOs.Add(conversation);
            }

            return new WrapperModel<ConversationDTO> { Count = count, DataList = conversationDTOs };
        }

        private IQueryable<MessageGroupedByConversation> GetMessageQueryForConversations(ConversationsFilter filter, int? userId = null, int? companyId = null)
        {
            var messageSourceQuery = ExpandMessages(_context.Messages, "user,user.company,user.image")
                .Where(m => m.Discussion.Type == DiscussionType.PrivateChat);

            if (userId == null && (filter.ConversationType.Any() || !string.IsNullOrEmpty(filter.FilterBy)))//if (expand.Contains("category"))
            {
                List<int> categoryIds = new();
                messageSourceQuery = messageSourceQuery.Where(c => c.Discussion.SourceTypeId == DiscussionSourceType.General);
                messageSourceQuery = messageSourceQuery.Include(x => x.Discussion).ThenInclude(x => x.DiscussionUsers).ThenInclude(x => x.User).ThenInclude(u => u.Company).ThenInclude(i => i.Categories)
                    .Where(u => u.Discussion.DiscussionUsers.Count == 2 && !u.Discussion.DiscussionUsers.All(m1 => m1.User.StatusId == Enums.UserStatus.Deleted));

                if (!string.IsNullOrEmpty(filter.FilterBy))
                {
                    if (filter.FilterBy.Contains("category"))
                    {
                        foreach (string property in filter.FilterBy.Split("&").ToList())
                        {
                            var ids = ParseFilterByField(property);
                            if (ids != null && ids.Count > 0)
                            {
                                categoryIds = ids;
                            }
                        }
                    }
                }

                if (categoryIds?.Any() == true && filter.ConversationType?.Contains(ConversationBetweenType.CorpToCorp) == true)
                {
                    filter.ConversationType.Remove(ConversationBetweenType.CorpToCorp);
                }

                if (filter.ConversationType.Any())
                {
                    messageSourceQuery = messageSourceQuery.Where(u =>
                    (filter.ConversationType.Contains(ConversationBetweenType.CorpToCorp) && u.Discussion.DiscussionUsers.All(m => m.User.Company.TypeId == CompanyType.Corporation)) ||
                    (filter.ConversationType.Contains(ConversationBetweenType.SpToSp) && u.Discussion.DiscussionUsers.All(m => m.User.Company.TypeId == CompanyType.SolutionProvider)) ||
                    (filter.ConversationType.Contains(ConversationBetweenType.CorpToSp) && (u.Discussion.DiscussionUsers.OrderByDescending(m => m.Id).FirstOrDefault().User.Company.TypeId == CompanyType.Corporation && u.Discussion.DiscussionUsers.OrderByDescending(m => m.Id).LastOrDefault().User.Company.TypeId == CompanyType.SolutionProvider)) ||
                    (filter.ConversationType.Contains(ConversationBetweenType.SpToCorp) && (u.Discussion.DiscussionUsers.OrderByDescending(m => m.Id).FirstOrDefault().User.Company.TypeId == CompanyType.SolutionProvider && u.Discussion.DiscussionUsers.OrderByDescending(m => m.Id).LastOrDefault().User.Company.TypeId == CompanyType.Corporation))
                   );
                }

                if (categoryIds.Any())
                {
                    messageSourceQuery = messageSourceQuery.Where(u => categoryIds.Any() && u.Discussion.DiscussionUsers.Any(m => m.User.Company.Categories.Any(x => categoryIds.Contains(x.CategoryId))));
                }
            }

            if (userId.HasValue)
            {
                messageSourceQuery = messageSourceQuery.Where(m => m.Discussion.DiscussionUsers.Any(c => c.UserId == userId));
            }

            if (companyId.HasValue)
            {
                messageSourceQuery = messageSourceQuery.Where(x => x.Discussion.DiscussionUsers.Any(u => u.User.CompanyId == companyId));
            }

            if (filter.WithUserId.HasValue)
            {
                messageSourceQuery = messageSourceQuery.Where(m => m.Discussion.DiscussionUsers.Any(c => c.UserId == filter.WithUserId.Value));
            }

            if (filter.Individual.HasValue)
            {
                messageSourceQuery = filter.Individual.Value
                    ? messageSourceQuery.Where(m => m.Discussion.DiscussionUsers.Count() == 2)
                    : messageSourceQuery.Where(m => m.Discussion.DiscussionUsers.Count() > 2);
            }

            if (!string.IsNullOrEmpty(filter.Search))
            {
                string search = filter.Search.ToLower();
                messageSourceQuery = messageSourceQuery.Where(m => m.Discussion.Subject.ToLower().Contains(search)
                    || m.Discussion.DiscussionUsers.Any(c => (c.User.FirstName + " " + c.User.LastName).ToLower().Contains(search) || c.User.Company.Name.ToLower().Contains(search))
                    || ((m.Discussion.SourceTypeId == DiscussionSourceType.ProviderContact) && (m.Discussion.Project != null) && (m.Discussion.Project.Title.Contains(search))));
            }

            return messageSourceQuery.Select(m => new MessageGroupedByConversation
            {
                Key = new MessageGroupedByConversation.ConversationKey
                {
                    ConversationId = m.DiscussionId
                },
                Message = m
            });
        }

        private static IQueryable<Discussion> ExpandConversations(IQueryable<Discussion> conversationsQueryable, string? expand)
        {
            if (!string.IsNullOrEmpty(expand))
            {
                expand = expand.ToLower();
                if (expand.Contains("project"))
                {
                    conversationsQueryable = conversationsQueryable.Include(c => c.Project);
                }
                if (expand.Contains("discussionusers"))
                {
                    conversationsQueryable = conversationsQueryable.Include(c => c.DiscussionUsers);
                    if (expand.Contains("discussionusers.users"))
                    {
                        conversationsQueryable = conversationsQueryable.Include(c => c.DiscussionUsers).ThenInclude(cu => cu.User);
                        if (expand.Contains("discussionusers.users.image"))
                        {
                            conversationsQueryable = conversationsQueryable.Include(c => c.DiscussionUsers).ThenInclude(cu => cu.User).ThenInclude(u => u.Image);
                        }
                        if (expand.Contains("discussionusers.users.company"))
                        {
                            conversationsQueryable = conversationsQueryable.Include(c => c.DiscussionUsers).ThenInclude(cu => cu.User).ThenInclude(u => u.Company);
                        }
                        if (expand.Contains("discussionusers.users.roles"))
                        {
                            conversationsQueryable = conversationsQueryable.Include(c => c.DiscussionUsers).ThenInclude(cu => cu.User).ThenInclude(u => u.Roles);
                        }
                    }
                }
                if (expand.Contains("messages"))
                {
                    conversationsQueryable = conversationsQueryable.Include(c => c.Messages);
                    if (expand.Contains("messages.users"))
                    {
                        conversationsQueryable = conversationsQueryable.Include(c => c.Messages)
                            .ThenInclude(m => m.User);
                    }
                }
            }

            return conversationsQueryable;
        }

        private static IQueryable<Message> ExpandMessages(IQueryable<Message> messagesQueryable, string? expand)
        {
            if (!string.IsNullOrEmpty(expand))
            {
                expand = expand.ToLower();
                if (expand.Contains("user"))
                {
                    messagesQueryable = messagesQueryable.Include(m => m.User);
                    if (expand.Contains("user.company"))
                    {
                        messagesQueryable = messagesQueryable.Include(m => m.User).ThenInclude(u => u.Company);
                    }
                    if (expand.Contains("user.image"))
                    {
                        messagesQueryable = messagesQueryable.Include(m => m.User).ThenInclude(u => u.Image);
                    }
                    if (expand.Contains("user.roles"))
                    {
                        messagesQueryable = messagesQueryable.Include(m => m.User).ThenInclude(u => u.Roles);
                    }
                }
                if (expand.Contains("attachments"))
                {
                    messagesQueryable = messagesQueryable.Include(m => m.Attachments);
                    if (expand.Contains("attachments.image"))
                    {
                        messagesQueryable = messagesQueryable.Include(m => m.Attachments).ThenInclude(a => a.Image);
                    }
                }
            }

            return messagesQueryable;
        }

        private static IQueryable<Message> GenerateQueryBySortOrder(ConversationsFilter filter, IQueryable<Message> messageQuery, int? userId)
        {

            if (filter.OrderBy == "leads")
            {
                return messageQuery.OrderByDescending(e => e.Discussion.SourceTypeId == Enums.DiscussionSourceType.ProviderContact && e.Discussion.ProjectId.HasValue ? 1 : 0).ThenByDescending(m => m.CreatedOn);
            }
            else if (filter.OrderBy == "unread")
            {
                return messageQuery.OrderByDescending(e => (e.Discussion.DiscussionUsers.Where(y => y.UserId == userId).FirstOrDefault().UnreadCount > 0) ? 1 : 0).ThenByDescending(m => m.CreatedOn);
            }
            else
            {
                return messageQuery.OrderByDescending(e => e.CreatedOn);
            }
        }
    }
}