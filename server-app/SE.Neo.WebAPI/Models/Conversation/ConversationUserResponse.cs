using SE.Neo.WebAPI.Models.Media;

namespace SE.Neo.WebAPI.Models.Conversation
{
    public class ConversationUserResponse
    {
        /// <summary>
        /// Unique identifier of the user.
        /// </summary>
        public int Id { get; set; }

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
        /// User image definition.
        /// </summary>
        public BlobResponse? Image { get; set; }

        /// <summary>
        /// User Solution Provider definition.
        /// </summary>
        public bool? IsSolutionProvider { get; set; }
    }
}
