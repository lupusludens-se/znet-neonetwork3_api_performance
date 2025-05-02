using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Conversation;
using SE.Neo.Common.Models.Notifications.Details.Single;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Constants;
using SE.Neo.WebAPI.Models.Conversation;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Services
{
    public class ConversationApiService : IConversationApiService
    {
        private readonly ILogger<ConversationApiService> _logger;
        private readonly IMapper _mapper;
        private readonly IConversationService _conversationService;
        private readonly INotificationService _notificationService;

        public ConversationApiService(ILogger<ConversationApiService> logger,
            IMapper mapper,
            IConversationService conversationService,
            INotificationService notificationService)
        {
            _logger = logger;
            _mapper = mapper;
            _conversationService = conversationService;
            _notificationService = notificationService;
        }

        public async Task<ConversationResponse> CreateConversationAsync(UserModel user, ConversationRequest model)
        {
            ConversationMessageDTO initMessage = _mapper.Map<ConversationMessageDTO>(model.Message);
            initMessage.UserId = user.Id;
            if (model.SourceTypeId.Equals(DiscussionSourceType.ProviderContact) && model.Users.Any())
            {
                var companyUserList = await _conversationService.GetConversationContactProviderUserAsync(model.Users[0].Id);
                model.Users = companyUserList.Select(_mapper.Map<ConversationUserRequest>).ToList();
            }
            ConversationDTO modelDTO = _mapper.Map<ConversationDTO>(model);
            modelDTO.CreatedByUserId = user.Id;
            int conversationId = await _conversationService.CreateConversationAsync(modelDTO, initMessage);
            ConversationDTO? conversationDTO = await _conversationService.GetConversationForUserAsync(user.Id, conversationId, "project,discussionusers,discussionusers.users,messages, messages.users", true);

            // For email alerts
            try
            {
                await _notificationService.AddNotificationsAsync(conversationDTO.Users.Where(cu => cu.StatusId != (int)UserStatus.Deleted && cu.Id != user.Id).Select(cu => cu.Id).ToList(),
                    NotificationType.MessagesMe,
                    new MessageNotificationDetails
                    {
                        ConversationId = conversationDTO.Id,
                        MessageId = conversationDTO.LastMessage.Id
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorAddingNotification);

            }
            // remove LastMessage
            conversationDTO.LastMessage = null;
            return _mapper.Map<ConversationResponse>(conversationDTO);
        }

        public async Task<ConversationMessageResponse> CreateConversationMessageAsync(UserModel user, int conversationId, ConversationMessageRequest model)
        {
            ConversationMessageDTO messageDTO = _mapper.Map<ConversationMessageDTO>(model);
            messageDTO.ConversationId = conversationId;
            messageDTO.UserId = user.Id;
            int messageId = await _conversationService.CreateConversationMessageAsync(messageDTO);

            ConversationMessageDTO? message = await _conversationService.GetConversationMessageAsync(messageId, "user,attachments");
            // For email alerts
            try
            {
                ConversationDTO conversationDTO = await _conversationService.GetConversationForUserAsync(user.Id, conversationId, "discussionusers.users", true);
                List<int> conversationUserIds = conversationDTO.Users
                    .Where(cu => cu.StatusId != (int)UserStatus.Deleted && cu.Id != user.Id)
                    .Select(cu => cu.Id)
                    .ToList();

                await _notificationService.AddNotificationsAsync(conversationUserIds,
                    NotificationType.MessagesMe,
                    new MessageNotificationDetails
                    {
                        ConversationId = conversationId,
                        MessageId = messageId
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorAddingNotification);
            }
            return _mapper.Map<ConversationMessageResponse>(message);
        }

        public async Task<ConversationResponse?> GetConversationAsync(UserModel user, int id, string? expand = null)
        {
            bool hasPermissionToViewAllConversations = user.PermissionIds.Contains((int)PermissionType.MessagesAll);
            ConversationDTO? conversation = await _conversationService.GetConversationForUserAsync(user.Id, id, expand, hasPermissionToViewAllConversations);

            return _mapper.Map<ConversationResponse>(conversation);
        }

        public async Task<WrapperModel<ConversationMessageResponse>> GetConversationMessagesAsync(UserModel user, int conversationId, ConversationMessagesFilterDTO filter)
        {
            bool hasPermissionToViewAllConversations = user.PermissionIds.Contains((int)PermissionType.MessagesAll);
            WrapperModel<ConversationMessageDTO> dtoModel = hasPermissionToViewAllConversations
                ? await _conversationService.GetConversationMessagesAsync(conversationId, filter)
                : await _conversationService.GetConversationMessagesForUserAsync(conversationId, filter, user.Id);

            return new WrapperModel<ConversationMessageResponse>
            {
                Count = dtoModel.Count,
                DataList = dtoModel.DataList.Select(_mapper.Map<ConversationMessageResponse>)
            };
        }

        public async Task<WrapperModel<ConversationResponse>> GetConversationsAsync(UserModel user, ConversationsFilter filter)
        {
            bool isSPAdmin = user.RoleIds.Contains((int)RoleType.SPAdmin);
            WrapperModel<ConversationDTO> dtoModel = filter.IncludeAll
                ? isSPAdmin ? await _conversationService.GetConversationsAsync(filter, user.CompanyId) : await _conversationService.GetConversationsAsync(filter, null)
                : await _conversationService.GetConversationsForUserAsync(user.Id, filter);

            return new WrapperModel<ConversationResponse>
            {
                Count = dtoModel.Count,
                DataList = dtoModel.DataList.Select(_mapper.Map<ConversationResponse>)
            };
        }

        public async Task<int> GetUnreadUserMessagesCountAsync(int userId)
        {
            return await _conversationService.GetUnreadUserMessagesCountAsync(userId);
        }

        public async Task MarkUserMessagesAsReadAsync(UserModel user, int conversationId)
        {
            await _conversationService.MarkUserMessagesAsReadAsync(user.Id, conversationId);
            //For email alert
            try
            {
                await _notificationService.MarkMessageNotificationsAsReadAsync(user.Id, conversationId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error marking message notification as read for user {user.Username}, conversation {conversationId}.", ex);
            }
        }

        public async Task<ConversationResponse> UpdateConversationAsync(UserModel user, int id, ConversationRequest model)
        {
            ConversationDTO conversationDTO = _mapper.Map<ConversationDTO>(model);
            conversationDTO.Id = id;
            conversationDTO.CreatedByUserId = user.Id;
            int conversationId = await _conversationService.UpdateConversationAsync(conversationDTO);

            ConversationDTO? conversation = await _conversationService.GetConversationForUserAsync(user.Id, conversationId, "project,discussionusers.users");
            return _mapper.Map<ConversationResponse>(conversation);
        }

        public async Task<ConversationResponse?> CreateContactUsConversationAsync(UserModel user, ConversationContactUsRequest model)
        {
            List<ConversationUserDTO>? conversationUsers = await _conversationService.GetConversationUserAdminsAsync(user.Id);
            if (conversationUsers != null && conversationUsers.Any())
            {
                ConversationDTO conversationDTO = new ConversationDTO();
                conversationDTO.SourceTypeId = (int)DiscussionSourceType.General;
                conversationDTO.Subject = model.Subject;
                conversationDTO.Users = conversationUsers;
                conversationDTO.CreatedByUserId = user.Id;
                ConversationMessageDTO initMessage = new ConversationMessageDTO() { Text = model.Message };
                initMessage.UserId = user.Id;

                int conversationId = await _conversationService.CreateConversationAsync(conversationDTO, initMessage);

                ConversationDTO? conversation = await _conversationService.GetConversationForUserAsync(user.Id, conversationId, "project,discussionusers,discussionusers.users,messages", true);


                await _notificationService.AddNotificationsAsync(conversationUsers.Select(k => k.Id).ToList(), NotificationType.ContactZeigoNetwork,//
                    new ContactZeigoNetworkMessageNotificationDetails
                    {
                        ConversationId = conversationId,
                        MessageId = conversation?.LastMessage?.Id,
                        UserName = user.LastName + ", " + user.FirstName
                    });


                return _mapper.Map<ConversationResponse>(conversation);
            }
            return null;
        }

        public async Task<bool?> DeleteMessageAsync(int messageId, UserModel currentUser)
        {
            var conversation = await _conversationService.GetConversationMessageAsync(messageId, "user");
            if (conversation?.User?.Id == currentUser.Id)
            {
                return await _conversationService.DeleteMessageAsync(messageId);
            }
            return null;
        }

        public async Task<bool?> UpdateMessageAsync(int messageId, UserModel currentUser, ConversationMessageRequest model)
        {
            var conversation = await _conversationService.GetConversationMessageAsync(messageId, "user");
            if (conversation?.User?.Id == currentUser.Id)
            {
                return await _conversationService.UpdateMessageAsync(messageId, model.Text);
            }
            return null;
        }

        public async Task<int?> GetContactProviderConversationAsync(int userId, int projectId)
        {
            return await _conversationService.GetContactProviderConversationAsync(userId, projectId);
        }
    }
}