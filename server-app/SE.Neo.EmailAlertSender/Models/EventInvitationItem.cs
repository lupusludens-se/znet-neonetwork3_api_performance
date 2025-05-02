using SE.Neo.Common.Enums;
using TimeZone = SE.Neo.Core.Entities.TimeZone;

namespace SE.Neo.EmailAlertSender.Models
{
    public class EventInvitationItem
    {
        public int EventId { get; set; }
        public string UserFirstName { get; set; }
        public string UserEmailAddress { get; set; }
        public List<EventAlertDateInfo> EventDates { get; set; }
        public EventLocationType EventType { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string? EventHighlights { get; set; }
        public TimeZone LocalTimeZone { get; set; }
        public TimeZone EventTimeZone { get; set; }

        public DateTime EventCreatedDate { get; set; }
        public DateTime EventModifiedDate { get; set; }
        public bool? IsFirstTimeEmail { get; set; }
    }

    public class EventAlertDateInfo
    {
        public DateTime EventDateStart { get; set; }
        public DateTime EventDateEnd { get; set; }
    }
}