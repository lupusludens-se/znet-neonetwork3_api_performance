using SE.Neo.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Event")]
    public class Event : BaseIdEntity
    {
        [Column("Event_Id")]
        public override int Id { get; set; }

        [Column("Subject")]
        public string Subject { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("Highlights")]
        public string? Highlights { get; set; }

        [Column("Is_Highlighted")]
        public bool IsHighlighted { get; set; }

        [Column("Location")]
        public string Location { get; set; }

        [Column("Location_Type")]
        public EventLocationType LocationType { get; set; }

        [Column("User_Registration")]
        public string? UserRegistration { get; set; }

        [Column("Time_Zone_Id")]
        [ForeignKey("TimeZone")]
        public int TimeZoneId { get; set; }

        [Column("Status_Id")]
        public EventStatus StatusId { get; set; }

        [Column("Event_Type")]
        public EventType EventType { get; set; }

        [Column("Show_In_Public_Site")]
        public bool? ShowInPublicSite { get; set; }
        public TimeZone TimeZone { get; set; }

        public ICollection<EventLink> EventLinks { get; set; }

        public ICollection<EventModerator> EventModerators { get; set; }

        public ICollection<EventCategory> EventCategories { get; set; }

        public ICollection<EventOccurrence> EventOccurrences { get; set; }

        public ICollection<EventInvitedRole> EventInvitedRoles { get; set; }

        public ICollection<EventInvitedCategory> EventInvitedCategories { get; set; }

        public ICollection<EventInvitedRegion> EventInvitedRegions { get; set; }

        public ICollection<EventAttendee> EventAttendees { get; set; }

        public ICollection<EventInvitedUser> EventInvitedUsers { get; set; }
    }
}