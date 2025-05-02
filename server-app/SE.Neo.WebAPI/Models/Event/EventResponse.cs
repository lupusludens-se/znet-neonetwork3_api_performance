using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Models.CMS;
using SE.Neo.WebAPI.Models.Role;
using SE.Neo.WebAPI.Models.Shared;

namespace SE.Neo.WebAPI.Models.Event
{
    public class EventResponse
    {
        public int Id { get; set; }

        /// <summary>
        /// The event's topic.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Description of the event.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Main highlights of the event.
        /// </summary>
        public string? Highlights { get; set; }

        /// <summary>
        /// Defines is event highlighted. This value defaults to false.
        /// </summary>
        public bool IsHighlighted { get; set; }

        /// <summary>
        /// Location of the event.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Type of location of the event.
        /// </summary>
        public EventLocationType LocationType { get; set; }

        /// <summary>
        /// User registration information. Free format - url, text etc.
        /// </summary>
        public string? UserRegistration { get; set; }

        /// <summary>
        /// Unique identifier of the timezone.
        /// </summary>
        public int TimeZoneId { get; set; }

        /// <summary>
        /// Define timezone of an event.
        /// </summary>
        public TimeZoneResponse TimeZone { get; set; }

        /// <summary>
        /// Defines is current user attending event.
        /// </summary>
        public bool? IsAttending { get; set; }

        /// <summary>
        /// Type of the event.
        /// </summary>
        public EventType EventType { get; set; }

        /// <summary>
        /// It will indicate whether the event has to be shown in public dashboard or not
        /// </summary>
        public bool? ShowInPublicSite { get; set; }

        /// <summary>
        /// List of users that attending event.
        /// </summary>
        public List<EventUserResponse> Attendees { get; set; }

        /// <summary>
        /// List of the recording urls that will be available after end of the event.
        /// </summary>
        public List<EventRecordingResponse> Recordings { get; set; }

        /// <summary>
        /// List of the urls with information about the given event.
        /// </summary>
        public List<EventLinkResponse> Links { get; set; }

        /// <summary>
        /// List of the categories that event relates to.
        /// </summary>
        public List<CategoryResponse> Categories { get; set; }

        /// <summary>
        /// List of the occurrences for the given event.
        /// </summary>
        public List<EventOccurrenceResponse> Occurrences { get; set; }

        /// <summary>
        /// List of the moderators for the given event.
        /// </summary>
        public List<EventModeratorResponse> Moderators { get; set; }

        /// <summary>
        /// List of the roles that event will be visible to.
        /// </summary>
        public List<RoleResponse> InvitedRoles { get; set; }

        /// <summary>
        /// List of the regions where event will be visible.
        /// </summary>
        public List<RegionResponse> InvitedRegions { get; set; }

        /// <summary>
        /// List of the categories where event will be visible.
        /// </summary>
        public List<CategoryResponse> InvitedCategories { get; set; }

        /// <summary>
        /// List of the Users invited for the given event.
        /// </summary>
        public List<EventUserResponse> InvitedUsers { get; set; }
    }
}
