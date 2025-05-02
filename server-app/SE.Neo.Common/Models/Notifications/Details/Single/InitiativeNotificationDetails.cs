using SE.Neo.Common.Models.Notifications.Details.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.Neo.Common.Models.Notifications.Details.Single
{
    public class InitiativeNotificationDetails : BaseUserNotificationDetials, ISingleNotificationDetails, INotificationDetails
    {
        public int InitiativeId { get; set; }
        public string InitiativeTitle { get; set; }
    }
}
