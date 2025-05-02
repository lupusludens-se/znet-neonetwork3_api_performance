using SE.Neo.WebAPI.Models.Media;

namespace SE.Neo.WebAPI.Models.Forum
{
    public class ForumUserResponse
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
        /// User job title.
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// Defines is user followed by user that makes request.
        /// </summary>
        public bool IsFollowed { get; set; }

        /// <summary>
        /// User image definition.
        /// </summary>
        public BlobResponse? Image { get; set; }
    }
}