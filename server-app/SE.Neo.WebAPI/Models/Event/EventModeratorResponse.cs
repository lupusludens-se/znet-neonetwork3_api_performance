namespace SE.Neo.WebAPI.Models.Event
{
    public class EventModeratorResponse
    {
        /// <summary>
        /// Name of the moderator that not exist in the system.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Company name of the moderator that not exist in the system.
        /// </summary>
        public string? Company { get; set; }

        /// <summary>
        /// Unique identifier of the existing user.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// User object.
        /// </summary>
        public EventUserResponse? User { get; set; }
    }
}
