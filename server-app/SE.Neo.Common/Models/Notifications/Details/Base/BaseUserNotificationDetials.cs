using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.Neo.Common.Models.Notifications.Details.Base
{
    public class BaseUserNotificationDetials : BaseNotificationDetails
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
    }
}
