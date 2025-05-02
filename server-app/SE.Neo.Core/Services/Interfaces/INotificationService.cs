using SE.Neo.Common.Enums;
using SE.Neo.Common.Models;
using SE.Neo.Common.Models.Notifications;
using SE.Neo.Common.Models.Notifications.Details.Base;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface INotificationService
    {
        Task<WrapperModel<NotificationDTO>> GetUserNotificationsAsync(int userId, PaginationModel filter);
        Task<int> GetUnreadUserNotificationsCountAsync(int userId);
        Task<int> GetUnseenUserNotificationsCountAsync(int userId);
        Task<int> AddNotificationAsync<T>(int userId, NotificationType type, T details) where T : ISingleNotificationDetails;
        Task<List<int>> AddNotificationsAsync<T>(List<int> userIds, NotificationType type, T details) where T : ISingleNotificationDetails;
        Task<int> AddNotificationRangeAsync<T>(List<int> userIds, NotificationType type, T details) where T : ISingleNotificationDetails;
        Task<bool> MarkNotificationsAsReadAsync(int userId, int? notificationId);
        Task<bool> MarkNotificationsAsSeenAsync(int userId);
        Task<bool> MarkMessageNotificationsAsReadAsync(int userId, int conversationId);
    }
}