using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Forum;
using SE.Neo.Common.Models.Notifications.Details.Single;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;
using SE.Neo.Core.Entities.CMS;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Constants;
using SE.Neo.WebAPI.Models.Forum;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;
using System.Text;

namespace SE.Neo.WebAPI.Services
{
    public class ForumApiService : IForumApiService
    {
        private readonly ILogger<ForumApiService> _logger;
        private readonly IMapper _mapper;
        private readonly IForumService _forumService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly ICommonService _commonService;

        public ForumApiService(ILogger<ForumApiService> logger,
            IMapper mapper,
            IForumService forumService,
            INotificationService notificationService,
            IUserService userService,
            ICommonService commonService)
        {
            _logger = logger;
            _mapper = mapper;
            _forumService = forumService;
            _notificationService = notificationService;
            _userService = userService;
            _commonService = commonService;
        }

        public async Task<ForumResponse> CreateForumAsync(int userId, ForumRequest model, UserModel user)
        {
            var forumDTO = _mapper.Map<ForumDTO>(model);
            forumDTO.CreatedByUserId = userId;
            var initMessage = _mapper.Map<ForumMessageDTO>(model.FirstMessage);
            initMessage.UserId = userId;

            _logger.LogInformation("User {UserId} is trying to create new forum", userId);

            int forumId = await _forumService.CreateForumAsync(forumDTO, initMessage);
            forumDTO.Id = forumId;
            ForumDTO? forum = await _forumService.GetForumForUserAsync(forumId, userId, "discussionusers,discussionusers.users,discussionusers.users.image,categories,regions,solutions,technologies", true);

            if (forum != null)
            {
                if (!forum.IsPrivate)
                {
                    await CreateForumNotification(userId, user, forum);
                }
                else
                {
                    await CreatePrivateForumNotification(userId, user, forum);
                }
            }


            return _mapper.Map<ForumResponse>(forum);
        }

        private async Task CreateForumNotification(int userId, UserModel user, ForumDTO? forum)
        {
            //notification
            try
            {
                if (forum != null)
                {
                    BaseSearchFilterModel filter = new BaseSearchFilterModel();
                    string? forumCategories = "";
                    string? forumRegions = "";

                    List<int> forumCategoryList = forum.Categories.ToList().Where(x => x.Name.Length > 0).Select(x => x.Id).ToList();
                    forumCategories = string.Join(",", forumCategoryList);

                    List<int> forumRegionList = forum.Regions.Where(x => x.Name.Length > 0).Select(x => x.Id).ToList();
                    if (forumRegionList.Count > 0)
                    {
                        forumRegions = await GetForumRegionIds(forumRegionList);
                    }

                    if (!string.IsNullOrEmpty(forumCategories) || !string.IsNullOrEmpty(forumRegions))
                    {
                        filter.FilterBy = string.Format("statusids=2&categoryids={0}&regionids={1}", forumCategories, forumRegions);
                        WrapperModel<UserDTO> usersResult = await _userService.GetUsersAsync(filter, userId);
                        List<int> notifiedUserIds = usersResult.DataList.Select(u => u.Id).ToList();
                        if (notifiedUserIds.Contains(userId))
                        {
                            notifiedUserIds.Remove(userId);
                        }
                        if (notifiedUserIds.Count > 0)
                        {
                            await _notificationService
                               .AddNotificationRangeAsync(notifiedUserIds, NotificationType.NewForumCreated, BuildNotificationModel(forum, user));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorAddingNotification);
            }
        }

        private async Task CreatePrivateForumNotification(int userId, UserModel user, ForumDTO forum)
        {
            try
            {
                List<int> notifiedPrivateForumUserIds = forum.Users.Select(u => u.Id).Where(u => u != userId).ToList();
                await _notificationService.AddNotificationRangeAsync(notifiedPrivateForumUserIds, NotificationType.NewPrivateForumCreated, BuildNotificationModel(forum, user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorAddingNotification);
            }
        }

        public async Task<ForumMessageResponse> CreateForumMessageAsync(UserModel user, int forumId, ForumMessageRequest model)
        {
            bool isAdminUser = user.RoleIds.Any(r => r.Equals((int)RoleType.Admin));
            var messageDTO = _mapper.Map<ForumMessageDTO>(model);
            messageDTO.ForumId = forumId;
            messageDTO.UserId = user.Id;

            int? messageId = await _forumService.CreateForumMessageAsync(messageDTO, isAdminUser);
            if (messageId == null)
                return null;
            // notifications
            try
            {
                ForumDTO? forumDTO = await _forumService.GetForumByIdAsync(forumId);
                if (forumDTO != null)
                {
                    List<int> notifiedUsersIds = new List<int>(user.Id);
                    if (messageDTO.Attachments != null && messageDTO.Attachments.Any(a => a.Type == AttachmentType.Mention))
                    {
                        List<int> mentionsUsersIds = mentionsUsersIds = messageDTO.Attachments
                            .Where(a => a.Type == AttachmentType.Mention)
                            .Select(a => int.Parse(a.Link.Split("/").Last()))
                            .ToList();

                        await _notificationService
                            .AddNotificationsAsync(mentionsUsersIds, NotificationType.MentionsMeInComment, BuildNotificationModel(forumDTO, user));

                        notifiedUsersIds.AddRange(mentionsUsersIds);
                    }

                    if (messageDTO.ParentMessageId.HasValue && forumDTO.FirstMessage.Id != messageDTO.ParentMessageId.Value)
                    {
                        int parentMessageOwnerId = await _forumService.GetForumMessageOwnerId(messageDTO.ParentMessageId.Value);
                        if (parentMessageOwnerId > 0 && parentMessageOwnerId != messageDTO.UserId && !notifiedUsersIds.Contains(parentMessageOwnerId))
                        {
                            UserDTO? parentMessageOwner = await _userService.GetUserAsync(forumDTO.CreatedByUserId);
                            if (parentMessageOwner != null && parentMessageOwner.StatusId != (int)UserStatus.Deleted)
                            {
                                await _notificationService
                                .AddNotificationAsync(parentMessageOwner.Id, NotificationType.RepliesToMyComment, BuildNotificationModel(forumDTO, user));

                                notifiedUsersIds.Add(parentMessageOwner.Id);
                            }
                        }
                    }
                    else
                    {
                        if (forumDTO.CreatedByUserId != messageDTO.UserId && !notifiedUsersIds.Contains(forumDTO.CreatedByUserId))
                        {
                            UserDTO? forumOwner = await _userService.GetUserAsync(forumDTO.CreatedByUserId);
                            if (forumOwner != null && forumOwner.StatusId != (int)UserStatus.Deleted)
                            {
                                await _notificationService
                            .AddNotificationAsync(forumOwner.Id, NotificationType.CommentsMyTopic, BuildNotificationModel(forumDTO, user));

                                notifiedUsersIds.Contains(forumOwner.Id);
                            }
                        }

                        IEnumerable<int> followersIds = await _forumService.GetForumFollowersIdsAsync(forumId);
                        List<int> followersToNotifyIds = followersIds.Where(fi => !notifiedUsersIds.Contains(fi) && fi != messageDTO.UserId).ToList();

                        await _notificationService
                            .AddNotificationsAsync(followersToNotifyIds, NotificationType.RepliesToTopicIFollow, BuildNotificationModel(forumDTO, user));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorAddingNotification);
            }

            ForumMessageDTO? message = await _forumService.GetForumMessageAsync(messageId.Value, "attachments,attachments.image,user");

            return _mapper.Map<ForumMessageResponse>(message);
        }

        public async Task<ForumMessageResponse> UpdateForumMessageAsync(int forumId, int messageId, ForumMessageRequest model)
        {
            var messageDTO = _mapper.Map<ForumMessageDTO>(model);
            messageDTO.ForumId = forumId;
            messageDTO.Id = messageId;

            int updatedMessageId = await _forumService.UpdateForumMessageAsync(messageDTO);
            ForumMessageDTO? message = await _forumService.GetForumMessageAsync(updatedMessageId, "attachments,attachments.image,user");

            return _mapper.Map<ForumMessageResponse>(message);
        }

        public async Task<int?> LikeForumMessageAsync(UserModel user, int forumId, int messageId)
        {
            bool isAdminUser = user.RoleIds.Any(r => r.Equals((int)RoleType.Admin));

            var forumMessageLikeDTO = new ForumMessageLikeDTO
            {
                MessageId = messageId,
                UserId = user.Id
            };

            int? responseId = await _forumService.LikeForumMessageAsync(forumMessageLikeDTO, forumId, isAdminUser);
            if (responseId == null)
                return null;
            // notifications
            try
            {
                ForumDTO? forumDTO = await _forumService.GetForumByIdAsync(forumId);
                if (forumDTO != null && forumDTO.CreatedByUserId != forumMessageLikeDTO.UserId && forumDTO.FirstMessage.Id == forumMessageLikeDTO.MessageId)
                {
                    UserDTO? forumOwner = await _userService.GetUserAsync(forumDTO.CreatedByUserId);
                    if (forumOwner != null && forumOwner.StatusId != (int)UserStatus.Deleted)
                    {
                        await _notificationService
                            .AddNotificationAsync(forumOwner.Id, NotificationType.LikesMyTopic, BuildNotificationModel(forumDTO, user));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorAddingNotification);
            }
            return responseId.Value;
        }

        public async Task UnLikeForumMessageAsync(int userId, int messageId)
        {
            var forumMessageLikeDTO = new ForumMessageLikeDTO
            {
                MessageId = messageId,
                UserId = userId
            };

            await _forumService.UnLikeForumMessageAsync(forumMessageLikeDTO);
        }

        public async Task<int?> FollowForumAsync(UserModel user, int forumId)
        {
            bool isAdminUser = user.RoleIds.Any(r => r.Equals((int)RoleType.Admin));
            var model = new ForumFollowDTO
            {
                UserId = user.Id,
                ForumId = forumId
            };

            return await _forumService.FollowForumAsync(model, isAdminUser);
        }

        public async Task UnFollowForumAsync(int userId, int forumId)
        {
            var model = new ForumFollowDTO
            {
                UserId = userId,
                ForumId = forumId
            };

            await _forumService.UnFollowForumAsync(model);
        }

        public async Task<ForumResponse> GetForumAsync(UserModel user, int id, string? expand = null)
        {
            var hasPermissionToViewAllForums = user.PermissionIds.Contains((int)PermissionType.ForumManagement);
            ForumDTO? forum = await _forumService.GetForumForUserAsync(id, user.Id, expand, hasPermissionToViewAllForums);

            return _mapper.Map<ForumResponse>(forum);
        }

        public async Task<WrapperModel<ForumMessageResponse>> GetForumMessagesAsync(UserModel user, int forumId, ForumMessagesFilter filter)
        {
            bool hasPermissionToViewAllForums = user.PermissionIds.Contains((int)PermissionType.ForumManagement);
            WrapperModel<ForumMessageDTO> wrapperModel = await _forumService.GetForumMessagesForUserAsync(forumId, user.Id, filter, hasPermissionToViewAllForums);

            return new WrapperModel<ForumMessageResponse>
            {
                Count = wrapperModel.Count,
                DataList = _mapper.Map<List<ForumMessageResponse>>(wrapperModel.DataList)
            };
        }

        public async Task<WrapperModel<ForumResponse>> GetForumsAsync(UserModel user, BaseSearchFilterModel filter)
        {
            bool hasPermissionToViewAllForums = user.PermissionIds.Contains((int)PermissionType.ForumManagement);
            WrapperModel<ForumDTO> wrapperModel = await _forumService.GetForumsForUserAsync(user.Id, filter, hasPermissionToViewAllForums);

            return new WrapperModel<ForumResponse>
            {
                Count = wrapperModel.Count,
                DataList = _mapper.Map<List<ForumResponse>>(wrapperModel.DataList)
            };
        }

        public async Task<ForumResponse> UpdateForumAsync(int userId, int id, ForumRequest model)
        {
            var forumDTO = _mapper.Map<ForumDTO>(model);
            forumDTO.Id = id;
            _logger.LogInformation("User {UserId} is trying to update forum {ForumId}", userId, id);

            int forumId = await _forumService.UpdateForumAsync(forumDTO);
            ForumDTO? forum = await _forumService.GetForumForUserAsync(forumId, userId, "discussionusers,discussionusers.users,discussionusers.users.image,categories,regions,solutions,technologies", true);

            return _mapper.Map<ForumResponse>(forum);
        }

        public async Task DeleteForumAsync(int userId, int id)
        {
            _logger.LogInformation("User {UserId} is trying to delete forum {ForumId}", userId, id);

            await _forumService.DeleteForumAsync(id);
        }

        private UserTopicNotificationDetails BuildNotificationModel(ForumDTO forumDTO, UserModel currentUser)
        {
            return new UserTopicNotificationDetails
            {
                TopicId = forumDTO.Id,
                TopicTitle = forumDTO.Subject,
                UserId = currentUser.Id,
                UserName = $"{currentUser.FirstName} {currentUser.LastName}"
            };
        }

        public async Task RemoveForumMessageAsync(int id, int messageId)
        {
            await _forumService.RemoveForumMessageAsync(id, messageId);
        }

        public async Task<ForumResponse> EditForumAsync(int userId, ForumRequest model)
        {
            var forumDTO = _mapper.Map<ForumDTO>(model);
            forumDTO.UpdatedByUserId = userId;
            forumDTO.Id = model.Id;
            _logger.LogInformation("User {UserId} is trying to edit user", userId);

            await _forumService.EditForumAsync(forumDTO, userId);
            ForumDTO? forum = await _forumService.GetForumForUserAsync(model.Id, userId, "discussionusers,discussionusers.users,discussionusers.users.image,categories,regions,solutions,technologies", true);

            return _mapper.Map<ForumResponse>(forum);
        }


        public async Task<int> GetForumMessageOwnerIdAsync(int messageId)
        {
            return await _forumService.GetForumMessageOwnerId(messageId);
        }

        private async Task<string> GetForumRegionIds(List<int> forumRegionIdList)
        {
            List<int> childRegionsIds = new List<int>();
            var resultBuilder = new StringBuilder();
            try
            {
                List<Region> regions = await _commonService.GetAllRegions();
                foreach (var parentRegionId in forumRegionIdList)
                {
                    var children = regions.Where(r => r.ParentId == parentRegionId).Select(x => x.Id).ToList();
                    childRegionsIds.AddRange(children);
                }
                string forumParentIds = string.Join(',', forumRegionIdList);
                string forumChildIds = string.Join(',', childRegionsIds);

                resultBuilder.Append(forumParentIds).Append(',').Append(forumChildIds);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error while getting Forum Region Ids", ex.InnerException);
            }
            return resultBuilder.ToString();
        }
    }
}