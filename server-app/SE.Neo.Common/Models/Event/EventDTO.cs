using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Common.Models.Event
{
    public class EventDTO
    {
        public int Id { get; set; }

        [PropertyComparation("Event Name")]
        public string Subject { get; set; }

        [PropertyComparation("Description of Event")]
        public string Description { get; set; }

        [PropertyComparation("Event Highlights")]
        public string? Highlights { get; set; }

        [PropertyComparation("Highlight Event")]
        public bool IsHighlighted { get; set; }

        [PropertyComparation("Location")]
        public string Location { get; set; }

        [PropertyComparation("Location")]
        public EventLocationType LocationType { get; set; }

        [PropertyComparation("User Registration")]
        public string? UserRegistration { get; set; }

        public TimeZoneDTO TimeZone { get; set; }

        // [PropertyComparation("Time Zone")]
        // don't need to track time zone change because while changing timezone Event Date(s) is changed also (automatically)
        public int TimeZoneId { get; set; }

        public bool? IsAttending { get; set; }

        public EventStatus Status { get; set; }

        public IEnumerable<EventUserDTO> Attendees { get; set; }

        [PropertyComparation("Content Links")]
        public IEnumerable<EventLinkDTO> Links { get; set; }

        [PropertyComparation("Moderator(s)")]
        public IEnumerable<EventModeratorDTO> Moderators { get; set; }

        [PropertyComparation("Tag(s)")]
        public IEnumerable<CategoryDTO> Categories { get; set; }

        [PropertyComparation("Event Date(s)")]
        public IEnumerable<EventOccurrenceDTO> Occurrences { get; set; }

        public IEnumerable<RoleDTO> InvitedRoles { get; set; }

        public IEnumerable<CategoryDTO> InvitedCategories { get; set; }

        public IEnumerable<RegionDTO> InvitedRegions { get; set; }

        public IEnumerable<EventUserDTO> InvitedUsers { get; set; }

        public EventInviteType InviteType { get; set; }

        public EventType EventType { get; set; }

        public bool? ShowInPublicSite { get; set; }
    }
}
