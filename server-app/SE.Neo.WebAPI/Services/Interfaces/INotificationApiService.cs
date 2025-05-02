using SE.Neo.Common.Models;
using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Models.Notification;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface INotificationApiService
    {
        Task<WrapperModel<NotificationResponse>> GetUserNotificationsAsync(int userId, PaginationModel filter);
        Task<int> GetUnreadUserNotificationsCountAsync(int userId);
        Task<bool> MarkNotificationsAsReadAsync(int userId, int? notificationId);
        Task<int> GetUnseenUserNotificationsCountAsync(int userId);
        Task<bool> MarkNotificationsAsSeenAsync(int userId);
    }
}
