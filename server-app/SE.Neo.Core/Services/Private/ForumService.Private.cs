using Microsoft.EntityFrameworkCore;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Forum;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services
{
    public partial class ForumService : BaseFilterService, IForumService
    {
        private const string FilterForYouPopular = "foryou.popular";
        private const string FilterForYou = "foryou";
        private const string FilterSaved = "saved";
        private const string FilterMyOnly = "myonly";

        private async Task<WrapperModel<ForumMessageDTO>> GetForumMessagesAsync(int id, int userId, ForumMessagesFilter filter, bool allowedPrivate)
        {
            IQueryable<Message> messageQuery = ExpandMessages(_context.Messages.AsNoTracking(), filter.Expand)
                .Where(m => m.DiscussionId == id && !m.Discussion.IsDeleted
                    && (m.Discussion.Type == DiscussionType.PublicForum || m.Discussion.Type == DiscussionType.PrivateForum && m.Discussion.DiscussionUsers.Any(du => du.UserId == userId) || allowedPrivate));

            if (filter.ParentMessageId.HasValue)
            {
                messageQuery = messageQuery.Where(m => m.ParentMessageId == filter.ParentMessageId);
            }
            else
            {
                messageQuery = messageQuery.Where(m => m.ParentMessageId == null);
            }

            messageQuery = messageQuery.OrderByDescending(x => x.IsPinned).ThenBy(x => x.CreatedOn);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await messageQuery.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<ForumMessageDTO>
                    {
                        DataList = new List<ForumMessageDTO>()
                    };
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

            List<Message> messages = await messageQuery
                .ToListAsync();

            var messageDTOs = new List<ForumMessageDTO>();

            foreach (var message in messages)
            {
                var messageDTO = _mapper.Map<ForumMessageDTO>(message);
                messageDTO.IsLiked = message.MessageLikes?.Any(ml => ml.UserId == userId) ?? false;
                if (messageDTO.User != null)
                {
                    messageDTO.User.IsFollowed = message.User?.FollowerUsers?.Any(ml => ml.FollowerId == userId) ?? false;
                }
                messageDTOs.Add(messageDTO);
            }

            return new WrapperModel<ForumMessageDTO>
            {
                Count = count,
                DataList = messageDTOs
            };
        }

        private async Task<WrapperModel<ForumDTO>> GetForumsAsync(int userId, BaseSearchFilterModel filter, bool allowedPrivate)
        {
            IQueryable<Discussion> forumsQuery = ExpandForums(_context.Discussions.AsNoTracking(), filter.Expand + ",toplevelmessages")
                .Where(x => !x.IsDeleted);

            forumsQuery = await CreateFilteredForumQuery(forumsQuery, userId, filter.Search, filter.FilterBy, allowedPrivate);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await forumsQuery.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<ForumDTO>
                    {
                        DataList = new List<ForumDTO>()
                    };
                }
            }

            if (filter.Skip.HasValue)
            {
                forumsQuery = forumsQuery.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                forumsQuery = forumsQuery.Take(filter.Take.Value);
            }

            List<Discussion> forums = await forumsQuery
                .ToListAsync();

            var forumDTOs = new List<ForumDTO>();

            foreach (var forum in forums)
            {
                forumDTOs.Add(MapForum(forum, userId));
            }

            return new WrapperModel<ForumDTO>
            {
                Count = count,
                DataList = forumDTOs
            };
        }

        private async Task<IQueryable<Discussion>> CreateFilteredForumQuery(IQueryable<Discussion> forumsQueryable, int userId, string? search, string? filter, bool allowedPrivate)
        {
            forumsQueryable = forumsQueryable.Where(f => f.Type == DiscussionType.PublicForum || (f.Type == DiscussionType.PrivateForum && (f.DiscussionUsers.Any(du => du.UserId == userId) || allowedPrivate)));

            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                if (filter == FilterForYou)
                {
                    List<int> userProfileRegions = await _commonService
                        .GetRegionListForUserProfile(userId, true);
                    IQueryable<int> userProfileCategories = _context.UserProfileCategories
                        .Where(upc => upc.UserProfile.UserId == userId)
                        .Select(x => x.CategoryId);

                    //forumsQueryable = forumsQueryable
                    //   .Where(f => !f.IsDeleted
                    //       && (((f.Messages.Any(m => m.CreatedOn.Value >= DateTime.UtcNow.AddDays(-7))
                    //               || f.Messages.SelectMany(m => m.MessageLikes).Any(m => m.CreatedOn.Value >= DateTime.UtcNow.AddDays(-7)))
                    //               && (f.Type == DiscussionType.PublicForum && f.DiscussionCategories.Any(dr => userProfileCategories.Contains(dr.CategoryId)) || f.Type == DiscussionType.PrivateForum))
                    //           || f.DiscussionRegions.Any(dr => userProfileRegions.Contains(dr.RegionId))
                    //           || f.DiscussionCategories.Any(dr => userProfileCategories.Contains(dr.CategoryId))))
                    //   .OrderByDescending(f => f.IsPinned)
                    //   .ThenByDescending(f => f.Type == DiscussionType.PrivateForum)
                    //   .ThenByDescending(f => f.Messages.Where(m => m.CreatedOn.Value >= DateTime.UtcNow.AddDays(-7)).Count()
                    //       + f.Messages.SelectMany(m => m.MessageLikes).Where(l => l.CreatedOn.Value >= DateTime.UtcNow.AddDays(-7)).Count())
                    //   .ThenByDescending(f => f.DiscussionRegions.Any(dr => userProfileRegions.Contains(dr.RegionId))
                    //       && f.DiscussionCategories.Any(dr => userProfileCategories.Contains(dr.CategoryId)))
                    //   .ThenByDescending(f => f.DiscussionCategories.Any(dr => userProfileCategories.Contains(dr.CategoryId)))
                    //   .ThenByDescending(f => f.DiscussionRegions.Any(dr => userProfileRegions.Contains(dr.RegionId)))
                    //   .ThenByDescending(f => f.ModifiedOn);

                    //Keeping this code for now
                    forumsQueryable = forumsQueryable
                        .Where(f => !f.IsDeleted && (f.Type == DiscussionType.PrivateForum || (f.Type == DiscussionType.PublicForum && (((f.Messages.Any(m => m.CreatedOn.Value >= DateTime.UtcNow.AddDays(-7))
                                    || f.Messages.SelectMany(m => m.MessageLikes).Any(m => m.CreatedOn.Value >= DateTime.UtcNow.AddDays(-7))) && f.DiscussionCategories.Any(dr => userProfileCategories.Contains(dr.CategoryId)))
                                || f.DiscussionRegions.Any(dr => userProfileRegions.Contains(dr.RegionId))
                                || f.DiscussionCategories.Any(dr => userProfileCategories.Contains(dr.CategoryId))))))
                        .OrderByDescending(f => f.IsPinned)
                        .ThenByDescending(f => f.Type == DiscussionType.PrivateForum)
                        .ThenByDescending(f => f.Type == DiscussionType.PrivateForum ? f.DiscussionUpdatedOn : default)
                        .ThenByDescending(f => f.Type == DiscussionType.PublicForum ? (f.Messages.Where(m => m.CreatedOn.Value >= DateTime.UtcNow.AddDays(-7)).Count() + f.Messages.SelectMany(m => m.MessageLikes).Where(l => l.CreatedOn.Value >= DateTime.UtcNow.AddDays(-7)).Count()) : default)
                        .ThenByDescending(f => f.Type == DiscussionType.PublicForum && f.DiscussionRegions.Any(dr => userProfileRegions.Contains(dr.RegionId)) && f.DiscussionCategories.Any(dr => userProfileCategories.Contains(dr.CategoryId)))
                        .ThenByDescending(f => f.Type == DiscussionType.PublicForum && f.DiscussionCategories.Any(dr => userProfileCategories.Contains(dr.CategoryId)))
                        .ThenByDescending(f => f.Type == DiscussionType.PublicForum && f.DiscussionRegions.Any(dr => userProfileRegions.Contains(dr.RegionId)))
                        .ThenByDescending(f => f.Type == DiscussionType.PublicForum ? f.ModifiedOn : default);


                }
                else if (filter == FilterSaved)
                {
                    forumsQueryable = forumsQueryable.Where(f => f.DiscussionSaved.Any(ds => ds.UserId == userId));
                }
                else if (filter == FilterMyOnly)
                {
                    forumsQueryable = forumsQueryable.Where(f => f.CreatedByUserId == userId);
                }
                else if (filter == FilterForYouPopular)
                {
                    var discussionIds = _context.MessageLikes.Include(m => m.Message).Where(p => !p.Message.Discussion.IsDeleted && p.Message.Discussion.Type != DiscussionType.PrivateForum && !p.Message.ParentMessageId.HasValue
                        && p.CreatedOn.Value >= DateTime.UtcNow.AddDays(-7))
                        .GroupBy(x => x.MessageId).Select(s => new { discussionId = s.First().Message.DiscussionId, count = s.Count() }).AsNoTracking()
                             .OrderByDescending(o => o.count).Select(s => s.discussionId).ToList();
                    var privateDiscussionIds = _context.Discussions.Where(f => !f.IsDeleted && (f.Type == DiscussionType.PrivateForum && (f.DiscussionUsers.Any(du => du.UserId == userId)))
                            && (f.CreatedOn.Value >= DateTime.UtcNow.AddDays(-7) || f.ModifiedOn.Value >= DateTime.UtcNow.AddDays(-7) || f.Messages.Any(m => m.CreatedOn.Value >= DateTime.UtcNow.AddDays(-7)) || f.Messages.SelectMany(m => m.MessageLikes).Any(l => l.CreatedOn.Value >= DateTime.UtcNow.AddDays(-7)))).Select(s => s.Id).ToList();
                    discussionIds.AddRange(privateDiscussionIds);


                    forumsQueryable = forumsQueryable.Where(f => (f.Type == DiscussionType.PrivateForum || f.DiscussionCategories.Any(dr =>
                       _context.UserProfileCategories.Where(upc => upc.UserProfile.UserId == userId).Select(x => x.CategoryId).Contains(dr.CategoryId)))
                             && discussionIds.Contains(f.Id)).OrderByDescending(s => s.Type == DiscussionType.PrivateForum);
                }
                else
                {
                    foreach (string property in filter.Split("&").ToList())
                    {
                        var ids = ParseFilterByField(property);
                        if (ids != null && ids.Count > 0)
                        {
                            if (property.Contains("regionids"))
                            {
                                forumsQueryable = forumsQueryable.Where(f => f.DiscussionRegions.Any(dr => ids.Contains(dr.RegionId)));
                            }

                            if (property.Contains("categoryids"))
                            {
                                forumsQueryable = forumsQueryable.Where(f => f.DiscussionCategories.Any(dr => ids.Contains(dr.CategoryId)));
                            }

                            if (property.Contains("solutionids"))
                            {
                                forumsQueryable = forumsQueryable.Where(f => f.DiscussionCategories.Any(dr => _context.Categories.Where(c => c.SolutionId != null && ids.Contains(c.SolutionId.Value)).Select(x => x.Id).Contains(dr.CategoryId)));
                            }

                            if (property.Contains("technologyids"))
                            {
                                forumsQueryable = forumsQueryable.Where(f => f.DiscussionCategories.Any(dr => _context.Categories.Where(x => x.Technologies.Any(t => ids.Contains(t.TechnologyId))).Select(x => x.Id).Contains(dr.CategoryId)));
                            }
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                forumsQueryable = forumsQueryable.Where(f => f.Subject.ToLower().Contains(search.ToLower()));
            }

            // default sorting
            if (filter != FilterForYou)
            {
                forumsQueryable = forumsQueryable.OrderByDescending(x => x.IsPinned).ThenByDescending(x => x.ModifiedOn).ThenByDescending(x => x.CreatedOn);
            }

            return forumsQueryable;
        }

        private IQueryable<Discussion> ExpandForums(IQueryable<Discussion> forumsQueryable, string? expand)
        {
            if (!string.IsNullOrEmpty(expand))
            {
                expand = expand.ToLower();
                if (expand.Contains("discussionusers"))
                {
                    forumsQueryable = forumsQueryable.Include(c => c.DiscussionUsers);

                    if (expand.Contains("discussionusers.users"))
                    {
                        forumsQueryable = forumsQueryable.Include(c => c.DiscussionUsers)
                            .ThenInclude(cu => cu.User)
                            .ThenInclude(u => u.UserProfile);

                        if (expand.Contains("discussionusers.users.image"))
                        {
                            forumsQueryable = forumsQueryable.Include(c => c.DiscussionUsers)
                                .ThenInclude(cu => cu.User)
                                .ThenInclude(u => u.Image);
                        }
                    }
                }
                if (expand.Contains("categories"))
                {
                    forumsQueryable = forumsQueryable.Include(p => p.DiscussionCategories
                        .Where(s => _context.Categories.Where(s => !s.IsDeleted)
                            .Select(es => es.Id).Contains(s.CategoryId)))
                        .ThenInclude(s => s.Category);
                }
                if (expand.Contains("regions"))
                {
                    forumsQueryable = forumsQueryable.Include(p => p.DiscussionRegions
                        .Where(s => _context.Regions.Where(s => !s.IsDeleted)
                            .Select(es => es.Id).Contains(s.RegionId)))
                        .ThenInclude(s => s.Region);
                }
                if (expand.Contains("toplevelmessages"))
                {
                    forumsQueryable = forumsQueryable
                        .Include(p => p.Messages)
                        .ThenInclude(m => m.Attachments)
                        .ThenInclude(a => a.Image)
                        .Include(p => p.Messages)
                        .ThenInclude(m => m.User)
                        .ThenInclude(a => a.Image)
                        .Include(p => p.Messages)
                        .ThenInclude(m => m.User)
                        .ThenInclude(a => a.Company)
                        .Include(p => p.Messages)
                        .ThenInclude(m => m.User)
                        .ThenInclude(a => a.FollowerUsers)
                        .Include(p => p.Messages)
                        .ThenInclude(m => m.User)
                        .ThenInclude(a => a.UserProfile)
                        .Include(p => p.Messages)
                        .ThenInclude(m => m.MessageLikes);
                }
                if (expand.Contains("discussionfollowers"))
                {
                    forumsQueryable = forumsQueryable.Include(c => c.DiscussionFollowers);
                }
                if (expand.Contains("saved"))
                {
                    forumsQueryable = forumsQueryable.Include(c => c.DiscussionSaved);
                }
            }

            return forumsQueryable;
        }

        private static IQueryable<Message> ExpandMessages(IQueryable<Message> messagesQueryable, string? expand)
        {
            if (!string.IsNullOrEmpty(expand))
            {
                expand = expand.ToLower();
                if (expand.Contains("user"))
                {
                    messagesQueryable = messagesQueryable.Include(m => m.User).ThenInclude(u => u.UserProfile);

                    if (expand.Contains("user.company"))
                    {
                        messagesQueryable = messagesQueryable.Include(m => m.User).ThenInclude(u => u.Company);
                    }
                    if (expand.Contains("user.image"))
                    {
                        messagesQueryable = messagesQueryable.Include(m => m.User).ThenInclude(u => u.Image);
                    }
                    if (expand.Contains("user.follower"))
                    {
                        messagesQueryable = messagesQueryable.Include(m => m.User).ThenInclude(u => u.FollowerUsers);
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
                if (expand.Contains("messagelikes"))
                {
                    messagesQueryable = messagesQueryable.Include(m => m.MessageLikes);
                }
                if (expand.Contains("replies"))
                {
                    messagesQueryable = messagesQueryable.Include(m => m.Messages);
                }
                if (expand.Contains("discussions"))
                {
                    messagesQueryable = messagesQueryable.Include(m => m.Discussion);
                }
                if (expand.Contains("discussionusers"))
                {
                    messagesQueryable = messagesQueryable.Include(m => m.Discussion).ThenInclude(c => c.DiscussionUsers);
                }
            }

            return messagesQueryable;
        }

        private ForumDTO MapForum(Discussion forum, int userId)
        {
            var forumDTO = _mapper.Map<ForumDTO>(forum);
            Message? firstMessage = forum.Messages.OrderBy(x => x.CreatedOn).FirstOrDefault();
            forumDTO.IsSaved = forum.DiscussionSaved?.Any(ds => ds.UserId == userId) ?? false;
            forumDTO.IsFollowed = forum.DiscussionFollowers?.Any(df => df.UserId == userId) ?? false;
            if (firstMessage != null)
            {
                forumDTO.FirstMessage.IsLiked = firstMessage.MessageLikes.Any(df => df.UserId == userId);
                forumDTO.FirstMessage.User.IsFollowed = firstMessage.User.FollowerUsers.Any(fu => fu.FollowerId == userId);
            }
            return forumDTO;
        }
    }
}