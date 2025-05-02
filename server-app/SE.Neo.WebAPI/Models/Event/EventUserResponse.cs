using SE.Neo.WebAPI.Models.Media;

namespace SE.Neo.WebAPI.Models.Event
{
    public class EventUserResponse
    {
        /// <summary>
        /// Unique identifier of the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// First name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Status of the user.
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// User company name.
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Defines if user followed by current user.
        /// </summary>
        public bool IsFollowed { get; set; }

        /// <summary>
        /// User image definition.
        /// </summary>
        public BlobResponse? Image { get; set; }
    }
}
