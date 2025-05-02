using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models;
using SE.Neo.Common.Models.Notifications;
using SE.Neo.Common.Models.Notifications.Details.Base;
using SE.Neo.Common.Models.Notifications.Details.Single;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Handlers.Notifications;
using SE.Neo.Core.Services.Interfaces;
using System.Text.Json;

namespace SE.Neo.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<NotificationService> _logger;
        private readonly IMapper _mapper;

        public NotificationService(ApplicationContext context, ILogger<NotificationService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<WrapperModel<NotificationDTO>> GetUserNotificationsAsync(int userId, PaginationModel filter)
        {
            var notificationsQueryable = _context.UserNotifications
                .Where(n => n.UserId == userId && n.Type != NotificationType.MessagesMe)
                .OrderByDescending(n => n.DetailsModifiedTime)
                .AsNoTracking();

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await notificationsQueryable.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<NotificationDTO> { Count = count, DataList = new List<NotificationDTO>() };
                }
            }

            if (filter.Skip.HasValue)
            {
                notificationsQueryable = notificationsQueryable.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                notificationsQueryable = notificationsQueryable.Take(filter.Take.Value);
            }

            IEnumerable<UserNotification> notifications = await notificationsQueryable.ToListAsync();
            IEnumerable<NotificationDTO> notificationDTOs = notifications.Select(_mapper.Map<NotificationDTO>);
            return new WrapperModel<NotificationDTO> { Count = count, DataList = notificationDTOs };
        }

        public async Task<int> GetUnreadUserNotificationsCountAsync(int userId)
        {
            return await _context.UserNotifications.Where(n => n.UserId == userId && !n.IsRead && n.Type != NotificationType.MessagesMe).AsNoTracking().CountAsync();
        }

        public async Task<int> GetUnseenUserNotificationsCountAsync(int userId)
        {
            return await _context.UserNotifications.Where(n => n.UserId == userId && !n.IsSeen && n.Type != NotificationType.MessagesMe).AsNoTracking().CountAsync();
        }

        public async Task<int> AddNotificationAsync<T>(int userId, NotificationType type, T details) where T : ISingleNotificationDetails
        {
            _logger.LogInformation($"New notification with type {type} for user {userId} and details {JsonSerializer.Serialize(details)} is adding...");

            BaseNotificationHandler notificationHandler = GetNotificationHandler(type);

            return await notificationHandler.AddAsync(userId, details);
        }

        public async Task<List<int>> AddNotificationsAsync<T>(List<int> userIds, NotificationType type, T details) where T : ISingleNotificationDetails
        {
            _logger.LogInformation($"New notification with type {type} for users and details {JsonSerializer.Serialize(details)} is adding...");

            BaseNotificationHandler notificationHandler = GetNotificationHandler(type);

            List<int> ids = new List<int>();
            foreach (int userId in userIds)
            {
                ids.Add(await notificationHandler.AddAsync(userId, details));
            }
            return ids;
        }

        public async Task<int> AddNotificationRangeAsync<T>(List<int> userIds, NotificationType type, T details) where T : ISingleNotificationDetails
        {
            _logger.LogInformation($"New notification with type {type} for users and details {JsonSerializer.Serialize(details)} is adding...");

            //TODO: Implement this for other handlers
            NewForumNotificationHandler newForumNotificationHandler = new NewForumNotificationHandler(_context, _mapper, type);

            return await newForumNotificationHandler.AddRangeAsync(userIds, details);
        }



        public async Task<bool> MarkNotificationsAsReadAsync(int userId, int? notificationId)
        {
            var unreadNotifications = notificationId == null
                ? _context.UserNotifications.Where(n => n.UserId == userId && !n.IsRead)
                : _context.UserNotifications.Where(n => n.Id == notificationId && !n.IsRead);

            await unreadNotifications.ForEachAsync(notification => notification.IsRead = true);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkNotificationsAsSeenAsync(int userId)
        {
            var unseenNotifications = _context.UserNotifications.Where(n => n.UserId == userId && !n.IsSeen && n.Type != NotificationType.MessagesMe);

            await unseenNotifications.ForEachAsync(notification => notification.IsSeen = true);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkMessageNotificationsAsReadAsync(int userId, int conversationId)
        {
            var unreadMessageNotifications = await _context.UserNotifications
                .Where(un => un.UserId == userId)
                .Where(un => un.Type == NotificationType.MessagesMe)
                .Where(un => un.IsRead == false)
                .ToListAsync();
            unreadMessageNotifications
                .Where(un => JsonSerializer.Deserialize<MessageNotificationDetails>(un.Details).ConversationId == conversationId)
                .ToList()
                .ForEach(un => un.IsRead = true);
            await _context.SaveChangesAsync();
            return true;
        }

        private BaseNotificationHandler GetNotificationHandler(NotificationType type)
        {
            Type notificationHandlerType = NotificationHandlerTypeFactory.GetHandlerType(type);
            return (BaseNotificationHandler)Activator.CreateInstance(notificationHandlerType, _context, _mapper, type)!;
        }
    }
}
