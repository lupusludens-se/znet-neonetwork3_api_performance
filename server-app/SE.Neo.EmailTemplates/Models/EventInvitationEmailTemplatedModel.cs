namespace SE.Neo.EmailTemplates.Models
{
    public class EventInvitationEmailTemplatedModel : NotificationTemplateEmailModel
    {
        public override string TemplateName => "EventInvitationEmailTemplate";

        public string FirstName { get; set; }

        public string EventName { get; set; }

        public string EventType { get; set; }

        public List<EventDateInfo> EventDates { get; set; }

        public string EventDateLogoUrl { get; set; }

        public string EventTimeLogoUrl { get; set; }

        public string EventInfo { get; set; }

        public List<string>? EventHighlights { get; set; }

        public string Link { get; set; }
    }

    public class EventDateInfo
    {
        public DateTime EventDate { get; set; }

        public string EventTimeZone { get; set; }

        public string EventStart { get; set; }

        public string EventEnd { get; set; }

        public string TimeZoneAbbreviation { get; set; }
    }
}
