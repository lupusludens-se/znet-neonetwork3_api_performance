using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Models.CMS;
using SE.Neo.WebAPI.Models.Role;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Event
{
    public class EventRequest
    {
        /// <summary>
        /// The event's topic.
        /// </summary>
        [Required]
        public string Subject { get; set; }

        /// <summary>
        /// Description of the event.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Main highlights of the event.
        /// </summary>
        public string? Highlights { get; set; }

        /// <summary>
        /// Defines is event highlighted. This value defaults to false.
        /// </summary>
        [Required]
        public bool IsHighlighted { get; set; }

        /// <summary>
        /// Location of the event.
        /// </summary>
        [Required]
        public string Location { get; set; }

        /// <summary>
        /// Type of location of the event.
        /// </summary>
        [Required]
        public EventLocationType LocationType { get; set; }

        /// <summary>
        /// User registration information. Free format - url, text etc.
        /// </summary>
        public string? UserRegistration { get; set; }

        /// <summary>
        /// Unique identifier of the timezone.
        /// </summary>
        [TimeZoneIdExist]
        public int TimeZoneId { get; set; }

        /// <summary>
        /// List of the recording urls that will be available after end of the event.
        /// </summary>
        public List<EventRecordingRequest>? Recordings { get; set; }

        /// <summary>
        /// List of the urls with information about the given event.
        /// </summary>
        public List<EventLinkRequest>? Links { get; set; }

        /// <summary>
        /// List of the categories that event relates to.
        /// </summary>
        [Required]
        [NonEmptyList]
        public List<CategoryRequest> Categories { get; set; }

        /// <summary>
        /// List of the occurrences for the given event.
        /// </summary>
        [Required]
        [NonEmptyList]
        public List<EventOccurrenceRequest> Occurrences { get; set; }

        /// <summary>
        /// List of the moderators for the given event.
        /// </summary>
        [Required]
        [NonEmptyList]
        public List<EventModeratorRequest> Moderators { get; set; }

        /// <summary>
        /// List of the roles that event will be visible to.
        /// </summary>
        public List<RoleRequest> InvitedRoles { get; set; }

        /// <summary>
        /// List of the regions where event will be visible.
        /// </summary>
        public List<RegionRequest>? InvitedRegions { get; set; }

        /// <summary>
        /// List of the categories where event will be visible.
        /// </summary>
        public List<CategoryRequest>? InvitedCategories { get; set; }

        /// <summary>
        /// List of the Users invited for the given event.
        /// </summary>
        public List<EventUserRequest>? InvitedUsers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EventInviteType? InviteType { get; set; }

        /// <summary>
        /// The type of event whether it is public or private
        /// </summary>
        public EventType? EventType { get; set; }

        /// <summary>
        /// It will indicate whether the event has to be shown in public dashboard or not
        /// </summary>
        public bool? ShowInPublicSite { get; set; }

        public bool IsEventModeratorsUnique()
        {
            return Moderators.Where(x => x.UserId.HasValue).GroupBy(x => x.UserId).All(g => g.Count() == 1);
        }
    }
}
