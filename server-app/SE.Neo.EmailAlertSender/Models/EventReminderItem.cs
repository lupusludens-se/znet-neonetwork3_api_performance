using SE.Neo.Common.Enums;
using TimeZone = SE.Neo.Core.Entities.TimeZone;

namespace SE.Neo.EmailAlertSender.Models
{
    public class EventReminderItem
    {
        public int EventId { get; set; }
        public IEnumerable<EventAlertDateInfo> EventDates { get; set; }
        public EventLocationType EventType { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string EventHighlights { get; set; }
        public IEnumerable<EventUserInfo> Users { get; set; }
        public TimeZone EventTimeZone { get; set; }
    }

    public class EventUserInfo
    {
        public string UserFirstName { get; set; }
        public string UserEmailAddress { get; set; }
        public int UserId { get; set; }
        public TimeZone UserTimeZone { get; set; }
    }
}
