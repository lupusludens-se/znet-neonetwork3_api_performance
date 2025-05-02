using AutoMapper;
using SE.Neo.Common.Models;
using SE.Neo.Common.Models.Notifications;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Models.Notification;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Services
{
    public class NotificationApiService : INotificationApiService
    {
        private readonly ILogger<SavedContentApiService> _logger;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public NotificationApiService(ILogger<SavedContentApiService> logger, IMapper mapper, INotificationService notificationService)
        {
            _logger = logger;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<int> GetUnreadUserNotificationsCountAsync(int userId)
        {
            return await _notificationService.GetUnreadUserNotificationsCountAsync(userId);
        }

        public async Task<int> GetUnseenUserNotificationsCountAsync(int userId)
        {
            return await _notificationService.GetUnseenUserNotificationsCountAsync(userId);
        }

        public async Task<WrapperModel<NotificationResponse>> GetUserNotificationsAsync(int userId, PaginationModel filter)
        {
            WrapperModel<NotificationDTO> notificationsResult = await _notificationService.GetUserNotificationsAsync(userId, filter);

            var wrapper = new WrapperModel<NotificationResponse>
            {
                Count = notificationsResult.Count,
                DataList = notificationsResult.DataList.Select(_mapper.Map<NotificationResponse>)
            };
            return wrapper;
        }

        public async Task<bool> MarkNotificationsAsReadAsync(int userId, int? notificationId)
        {
            return await _notificationService.MarkNotificationsAsReadAsync(userId, notificationId);
        }

        public async Task<bool> MarkNotificationsAsSeenAsync(int userId)
        {
            return await _notificationService.MarkNotificationsAsSeenAsync(userId);
        }
    }
}
