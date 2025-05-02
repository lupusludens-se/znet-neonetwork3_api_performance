using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Notifications.Details.Base;
using SE.Neo.Common.Models.Notifications.Details.Single;
using SE.Neo.Common.Models.Notifications;
using SE.Neo.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.Neo.Core.Handlers.Notifications
{
    public class InitiativeNotificationHandler : BaseNotificationHandler
    {
        public InitiativeNotificationHandler(ApplicationContext context, IMapper mapper, NotificationType type)
        : base(context, mapper, type)
        {
        }

        public override async Task<int> AddAsync(int userId, ISingleNotificationDetails details)
        {
            if (details is InitiativeNotificationDetails)
            {
                return await InsertOrUpdateNotification(new NotificationDTO(userId, _type, details));
            }

            ThrowNotificationDetailsAreNotValid(details);
            return 0;
        }
    }
}
