using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Forum;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services
{
    public partial class ForumService : IForumService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<ForumService> _logger;
        private readonly IMapper _mapper;
        private readonly ICommonService _commonService;

        public ForumService(ApplicationContext context, ILogger<ForumService> logger, IMapper mapper, ICommonService commonService)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _commonService = commonService;
        }

        public async Task<int> CreateForumAsync(ForumDTO model, ForumMessageDTO initialMessage)
        {
            using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var discussion = _mapper.Map<Discussion>(model);
                // all Update related to Discussion 
                discussion.DiscussionUpdatedOn = DateTime.UtcNow;
                Discussion forum = (await _context.Discussions.AddAsync(discussion)).Entity;

                // create init message
                var message = _mapper.Map<Message>(initialMessage);
                message.Discussion = forum;
                await _context.Messages.AddAsync(message);

                // add all participants & owner
                if (forum.Type == DiscussionType.PrivateForum)
                {
                    var conversationUsers = _mapper.Map<List<DiscussionUser>>(model.Users);
                    foreach (var conversationUser in conversationUsers)
                    {
                        conversationUser.Discussion = forum;
                    }

                    await _context.DiscussionUsers.AddRangeAsync(conversationUsers);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return forum.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
            }
        }

        public async Task<int> UpdateForumAsync(ForumDTO model)
        {
            try
            {
                Discussion? forum = await _context.Discussions.FirstOrDefaultAsync(d => d.Id == model.Id && !d.IsDeleted
                    && (d.Type == DiscussionType.PrivateForum || d.Type == DiscussionType.PublicForum));
                if (forum != null)
                {
                    if (forum.IsPinned != model.IsPinned)
                    {
                        var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@Discussion_Id",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = model.Id
                        },
                        new SqlParameter() {
                            ParameterName = "@IsPinned",
                            SqlDbType =  System.Data.SqlDbType.Bit,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = model.IsPinned
                        }};
                        await _context.Database.ExecuteSqlRawAsync("[dbo].[sp_UpdateDiscussionPinnedStatus] @Discussion_Id, @IsPinned", param);
                    }

                }
                else
                {
                    _logger.LogWarning(CoreErrorMessages.ForumNotFoundOrDeleted, model.Id);
                }

                return model.Id;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(CoreErrorMessages.ErrorOnSaving);
                throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
            }

        }

        public async Task DeleteForumAsync(int id)
        {
            Discussion? forum = await _context.Discussions.FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted
                && (d.Type == DiscussionType.PrivateForum || d.Type == DiscussionType.PublicForum));

            if (forum != null)
            {
                forum.IsDeleted = true;

                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning(CoreErrorMessages.ForumNotFoundOrDeleted, id);
            }
        }

        public async Task<ForumDTO?> GetForumForUserAsync(int id, int userId, string? expand = null, bool allowedPrivate = false)
        {
            expand += ",toplevelmessages,discussionfollowers";
            var forumsQueryable = ExpandForums(_context.Discussions.AsNoTracking(), expand)
                .Where(x => !x.IsDeleted);

            Discussion? forum = await forumsQueryable.FirstOrDefaultAsync(c => c.Id == id
                    && (c.Type == DiscussionType.PublicForum || (c.Type == DiscussionType.PrivateForum && (c.DiscussionUsers.Any(du => du.UserId == userId) || allowedPrivate))));

            if (forum == null)
            {
                return null;
            }

            return MapForum(forum, userId);
        }

        public async Task<ForumDTO?> GetForumByIdAsync(int id, bool includeMessages = true)
        {
            IQueryable<Discussion> discussionQueriable = _context.Discussions.AsNoTracking();
            if (includeMessages)
            {
                discussionQueriable = discussionQueriable.Include(d => d.Messages);
            }
            Discussion? forum = await discussionQueriable.SingleOrDefaultAsync(d => d.Id == id && !d.IsDeleted && d.Type != DiscussionType.PrivateChat);
            if (forum == null)
            {
                return null;
            }
            return _mapper.Map<ForumDTO>(forum);
        }

        public async Task<WrapperModel<ForumDTO>> GetForumsForUserAsync(int userId, BaseSearchFilterModel filter, bool allowedPrivate = false)
        {
            return await GetForumsAsync(userId, filter, allowedPrivate);
        }

        public async Task<int?> FollowForumAsync(ForumFollowDTO model, bool isAdminUser = false)
        {
            Discussion? forum = await ExpandForums(_context.Discussions.AsNoTracking(), "discussionusers,discussionfollowers")
                .FirstOrDefaultAsync(d => d.Id == model.ForumId && !d.IsDeleted);

            if (forum == null)
            {
                throw new CustomException(CoreErrorMessages.ErrorOnSaving);
            }

            if (!isAdminUser)
            {
                if (forum.Type == DiscussionType.PrivateChat || (forum.Type == DiscussionType.PrivateForum &&
                    !forum.DiscussionUsers.Any(du => du.UserId == model.UserId)))
                {
                    _logger.LogWarning("User {UserId} is trying to follow not allowed Forum ({ForumId})", model.UserId, model.ForumId);
                    return null;
                }
            }

            if (!forum.DiscussionFollowers.Any(df => df.UserId == model.UserId))
            {
                await _context.DiscussionFollowers.AddAsync(_mapper.Map<DiscussionFollower>(model));

                await _context.SaveChangesAsync();

                return model.ForumId;
            }
            return null;
        }

        public async Task UnFollowForumAsync(ForumFollowDTO model)
        {
            DiscussionFollower? follower = await _context.DiscussionFollowers.SingleOrDefaultAsync(df => df.UserId == model.UserId && df.DisscussionId == model.ForumId);
            if (follower != null)
            {
                _context.DiscussionFollowers.Remove(follower);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<int>> GetForumFollowersIdsAsync(int forumId)
        {
            return _context.DiscussionFollowers.Where(df => df.DisscussionId == forumId).Select(df => df.UserId).ToList();
        }

        public async Task<int?> CreateForumMessageAsync(ForumMessageDTO model, bool isAdminUser = false)
        {

            Discussion? forum = await ExpandForums(_context.Discussions.AsNoTracking(), "discussionusers")
                .FirstOrDefaultAsync(d => d.Id == model.ForumId && !d.IsDeleted);

            if (forum == null)
            {
                _logger.LogWarning("User {UserId} is trying to send message to the Forum ({ForumId}). Forum not found, private or deleted.", model.UserId, model.ForumId);
                return null;
            }
            if (!isAdminUser)
            {
                if (forum.Type == DiscussionType.PrivateChat || (forum.Type == DiscussionType.PrivateForum &&
                    !forum.DiscussionUsers.Any(du => du.UserId == model.UserId)))
                {
                    _logger.LogError("User {UserId} is trying to send message to the private Forum ({ForumId})", model.UserId, model.ForumId);
                    return null;
                }
            }
            using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var message = _mapper.Map<Message>(model);

                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
                var param = GetParamsForDiscussionUpdatedOn(model.ForumId);
                await _context.Database.ExecuteSqlRawAsync("[dbo].[sp_UpdateDiscussionUpdatedOn] @Discussion_Id, @Discussion_UpdatedOn", param);
                await transaction.CommitAsync();
                return message.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
            }
        }

        public async Task<int> UpdateForumMessageAsync(ForumMessageDTO model)
        {
            Message? message = await _context.Messages
                .Include(m => m.Discussion)
                .ThenInclude(d => d.Messages)
                .Include(d => d.ParentMessage)
                .FirstOrDefaultAsync(d => d.Id == model.Id && !d.Discussion.IsDeleted);
            if (message != null)
            {
                using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // can only pin reply to top level forum message
                    int firstMessageId = message.Discussion.Messages.OrderBy(m => m.CreatedOn).First().Id;
                    if (model.ParentMessageId != firstMessageId && model.IsPinned)
                        throw new CustomException("Can only pin a reply to the top level message.");
                    // can't change message user
                    if (model.ParentMessageId == 0 || model.ParentMessageId == null)
                    {
                        model.ParentMessageId = message.ParentMessageId > 0 ? message.ParentMessageId : message.ParentMessage?.Id;
                    }
                    model.UserId = message.UserId;
                    _mapper.Map(model, message);
                    await _context.SaveChangesAsync();
                    var param = GetParamsForDiscussionUpdatedOn(model.ForumId);
                    await _context.Database.ExecuteSqlRawAsync("[dbo].[sp_UpdateDiscussionUpdatedOn] @Discussion_Id, @Discussion_UpdatedOn", param);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }
            else
            {
                _logger.LogWarning("Forum ({ForumId}) message {MessageId} not found or forum deleted", model.ForumId, model.Id);
            }

            return model.Id;
        }

        public async Task<int?> LikeForumMessageAsync(ForumMessageLikeDTO model, int forumId, bool isAdminUser = false)
        {
            Message? message = await ExpandMessages(_context.Messages.AsNoTracking(), "messagelikes, discussionusers")
                .FirstOrDefaultAsync(d => d.Id == model.MessageId && !d.Discussion.IsDeleted);
            if (message == null)
                throw new CustomException(CoreErrorMessages.ErrorOnSaving);

            if (!isAdminUser)
            {
                if (message.Discussion.Type == DiscussionType.PrivateChat || (message.Discussion.Type == DiscussionType.PrivateForum &&
                    !message.Discussion.DiscussionUsers.Any(du => du.UserId == model.UserId)))
                {
                    _logger.LogWarning("User {UserId} is trying to like forum message {MessageId} that not found or the forum deleted", model.UserId, model.MessageId);
                    return null;
                }
            }

            if (!message.MessageLikes.Any(df => df.UserId == model.UserId))
            {
                using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _context.MessageLikes.AddAsync(_mapper.Map<MessageLike>(model));

                    await _context.SaveChangesAsync();

                    var param = GetParamsForDiscussionUpdatedOn(forumId);

                    await _context.Database.ExecuteSqlRawAsync("[dbo].[sp_UpdateDiscussionUpdatedOn] @Discussion_Id, @Discussion_UpdatedOn", param);
                    await transaction.CommitAsync();

                    return model.MessageId;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }
            return null;
        }

        public async Task UnLikeForumMessageAsync(ForumMessageLikeDTO model)
        {
            MessageLike? messageLike = await _context.MessageLikes.Where(ml => ml.UserId == model.UserId && ml.MessageId == model.MessageId).FirstOrDefaultAsync();

            if (messageLike != null)
            {
                _context.MessageLikes.Remove(messageLike);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<ForumMessageDTO?> GetForumMessageAsync(int messageId, string? expand = null)
        {
            Message? message = await ExpandMessages(_context.Messages.AsNoTracking(), expand).FirstOrDefaultAsync(m => m.Id == messageId);

            return _mapper.Map<ForumMessageDTO>(message);
        }

        public async Task<WrapperModel<ForumMessageDTO>> GetForumMessagesForUserAsync(int forumId, int userId, ForumMessagesFilter filter, bool allowedPrivate = false)
        {
            return await GetForumMessagesAsync(forumId, userId, filter, allowedPrivate);
        }

        public async Task<int> GetForumMessageOwnerId(int messageId)
        {
            Message? message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
            return message?.CreatedByUserId ?? 0;
        }

        public async Task RemoveForumMessageAsync(int id, int messageId)
        {
            if (!IsMessageExist(messageId))
                throw new CustomException(CoreErrorMessages.ErrorOnRemoving);

            List<Message> messages = await GetMessageWithChildren(messageId);
            List<MessageLike> messageLikes = await _context.MessageLikes.Where(x => messages.Select(s => s.Id).Contains(x.MessageId)).AsNoTracking().ToListAsync();
            List<Attachment> messageAtachments = await _context.Attachments.Where(x => messages.Select(s => s.Id).Contains(x.MessageId)).AsNoTracking().ToListAsync();

            if (messages.Any())
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Attachments.RemoveRange(messageAtachments);
                        _context.MessageLikes.RemoveRange(messageLikes);
                        _context.Messages.RemoveRange(messages);

                        await _context.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new CustomException(CoreErrorMessages.ErrorOnRemoving, ex);
                    }
                }
            }
        }

        public async Task<List<Message>> GetMessageWithChildren(int id)
        {
            return await _context.Messages.FromSqlRaw(";WITH CTE AS (SELECT M.* FROM [MESSAGE] AS M WHERE M.[MESSAGE_ID] = @Id " +
                     "UNION ALL SELECT M.* FROM[MESSAGE] AS M INNER JOIN CTE AS C ON M.PARENT_MESSAGE_ID = C.[MESSAGE_ID]) " +
                     "SELECT* FROM CTE", new SqlParameter("@Id", id)).ToListAsync();
        }

        public async Task<List<string?>> GetAttachmentsFromMessages(List<Message> messages)
        {
            return await _context.Attachments
                .Where(x => messages.Select(s => s.Id).Contains(x.MessageId) && x.ImageName != null)
                    .AsNoTracking().Select(s => s.ImageName).ToListAsync();
        }

        public bool IsMessageExist(int messageId)
        {
            return _context.Messages.AsNoTracking().Any(m => m.Id == messageId && !m.Discussion.IsDeleted
                && (m.Discussion.Type == DiscussionType.PublicForum || m.Discussion.Type == DiscussionType.PrivateForum));
        }

        public bool IsForumExist(int forumId)
        {
            return _context.Discussions.AsNoTracking().Any(m => m.Id == forumId && !m.IsDeleted
                && (m.Type == DiscussionType.PublicForum || m.Type == DiscussionType.PrivateForum));
        }

        public async Task<int> EditForumAsync(ForumDTO request, int userID)
        {
            using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();

            Discussion? forum = await _context.Discussions.FirstOrDefaultAsync(d => d.Id == request.Id);
            try
            {
                if (forum != null)
                {
                    forum.UpdatedByUserId = userID;
                    forum.Subject = request.Subject;
                    forum.ModifiedOn = new DateTime();

                    // all Update related to Discussion 
                    forum.DiscussionUpdatedOn = DateTime.UtcNow;

                    _context.RemoveRange(_context.DiscussionRegions.Where(a => a.DisscussionId == forum.Id));
                    IEnumerable<int> RegionIds = request.Regions.Select(item => item.Id);
                    _context.DiscussionRegions.AddRange(RegionIds.Select(id => new DiscussionRegion() { RegionId = id, DisscussionId = forum.Id }));

                    _context.RemoveRange(_context.DiscussionCategories.Where(a => a.DisscussionId == forum.Id));
                    IEnumerable<int> CategoryIds = request.Categories.Select(item => item.Id);
                    _context.DiscussionCategories.AddRange(CategoryIds.Select(id => new DiscussionCategory() { CategoryId = id, DisscussionId = forum.Id }));

                    if (forum.Type == DiscussionType.PrivateForum)
                    {
                        _context.RemoveRange(_context.DiscussionUsers.Where(a => a.DiscussionId == forum.Id));
                        IEnumerable<int> UserIds = request.Users.Select(item => item.Id);
                        _context.DiscussionUsers.AddRange(UserIds.Select(id => new DiscussionUser() { UserId = id, DiscussionId = forum.Id }));
                    }
                    await _context.SaveChangesAsync();
                }
                Message? message = await _context.Messages.FirstOrDefaultAsync(d => d.Id == request.FirstMessage.Id);
                if (message != null)
                {
                    message.UpdatedByUserId = request.FirstMessage.UserId;
                    message.Text = request.FirstMessage.Text;
                    message.ModifiedOn = new DateTime();

                    var attachments = new List<Attachment>();
                    _mapper.Map(request.FirstMessage.Attachments, attachments);
                    _context.RemoveRange(_context.Attachments.Where(a => a.MessageId == message.Id));
                    _context.Attachments.AddRange(attachments);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }


            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
            }
            return forum.Id;
        }

        private SqlParameter[] GetParamsForDiscussionUpdatedOn(int discussionId)
        {

            return new SqlParameter[] {
                         new SqlParameter() {
                            ParameterName = "@Discussion_Id",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = discussionId
                        }, new SqlParameter() {
                            ParameterName = "@Discussion_UpdatedOn",
                            SqlDbType =  System.Data.SqlDbType.DateTime2,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = DateTime.UtcNow
                        }, };
        }
    }
}