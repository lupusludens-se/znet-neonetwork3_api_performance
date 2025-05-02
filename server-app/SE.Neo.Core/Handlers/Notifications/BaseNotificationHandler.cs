using AutoMapper;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models.Notifications;
using SE.Neo.Common.Models.Notifications.Details.Base;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using System.Text.Json;


namespace SE.Neo.Core.Handlers.Notifications
{
    public abstract class BaseNotificationHandler
    {
        protected readonly ApplicationContext _context;
        protected readonly IMapper _mapper;
        protected readonly NotificationType _type;

        public BaseNotificationHandler(ApplicationContext context, IMapper mapper, NotificationType type)
        {
            _type = type;
            _context = context;
            _mapper = mapper;
        }

        public abstract Task<int> AddAsync(int userId, ISingleNotificationDetails details);

        protected async Task<int> InsertOrUpdateNotification(NotificationDTO notificationDto,
            Func<INotificationDetails, NotificationType, INotificationDetails?>? handleGroupNotificationDetails = null,
            IEnumerable<UserNotification>? unreadUserNotifications = null)
        {
            UserNotification? userNotification = unreadUserNotifications?.FirstOrDefault(n => n.SupportGroupping);

            if (userNotification == null)
            {
                userNotification = _mapper.Map<UserNotification>(notificationDto);
                userNotification.DetailsModifiedTime = DateTime.UtcNow;
                _context.UserNotifications.Add(userNotification);
            }
            else
            {
                INotificationDetails? currentDetails = userNotification.Details.ToNotificationDetailsObject();
                if (currentDetails != null)
                {
                    INotificationDetails? newDetails = handleGroupNotificationDetails?.Invoke(currentDetails, userNotification.Type);
                    if (newDetails != null)
                    {
                        userNotification.Details = JsonSerializer.Serialize((object)newDetails);
                        userNotification.IsSeen = false;
                        userNotification.DetailsModifiedTime = DateTime.UtcNow;
                    }
                    else
                    {
                        ThrowSaveNotificationTypeError(notificationDto.Type);
                    }
                }
                else
                {
                    ThrowSaveNotificationTypeError(notificationDto.Type);
                }
            }
            await _context.SaveChangesAsync();
            return userNotification.Id;
        }

        protected async Task<int> InsertNotificationRange(IEnumerable<NotificationDTO> notificationDTOs, NotificationType type)
        {
            try
            {
                IEnumerable<UserNotification> userNotifications = notificationDTOs.Select(item => _mapper.Map<UserNotification>(item));
                await _context.UserNotifications.AddRangeAsync(userNotifications);
                return await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                ThrowSaveNotificationTypeError(type);
                return -1;
            }

        }

        protected IEnumerable<UserNotification> GetUnreadUserNotificationsByType(int userId, NotificationType type)
        {
            return _context.UserNotifications.Where(n => !n.IsRead && n.UserId == userId && n.Type == type).AsEnumerable();
        }

        protected int IncrementCount(INotificationDetails details)
        {
            return details is IGrouppedNotificationDetails ? (details as IGrouppedNotificationDetails)!.Count + 1 : 2;
        }

        protected void ThrowNotificationDetailsAreNotValid(ISingleNotificationDetails details)
        {
            throw new CustomException(string.Format(CoreErrorMessages.NotificationDetailsAreNotValid, JsonSerializer.Serialize(details), _type));
        }

        private void ThrowSaveNotificationTypeError(NotificationType type)
        {
            throw new CustomException(string.Format(CoreErrorMessages.SaveNotificationTypeError, type));
        }
    }
}
