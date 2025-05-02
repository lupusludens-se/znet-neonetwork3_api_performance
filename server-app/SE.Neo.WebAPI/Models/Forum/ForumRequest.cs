using SE.Neo.WebAPI.Models.CMS;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Forum
{
    public class ForumRequest
    {
        /// <summary>
        /// The forum's topic.
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string Subject { get; set; }

        /// <summary>
        /// Defines is forum private or not. This value defaults to false.
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Defines is forum pinned. This value defaults to false.
        /// </summary>
        public bool IsPinned { get; set; }

        /// <summary>
        /// First message of the forum, that actually is a forum description.
        /// </summary>
        [Required]
        public ForumFirstMessageRequest FirstMessage { get; set; }

        /// <summary>
        /// List of the users in the private forum.
        /// </summary>
        public List<ForumUserRequest>? Users { get; set; }

        /// <summary>
        /// List of the regions that forum relates to.
        /// </summary>
        public IEnumerable<RegionRequest>? Regions { get; set; }

        /// <summary>
        /// List of the categories that forum relates to.
        /// </summary>
        public IEnumerable<CategoryRequest>? Categories { get; set; }

        /// <summary>
        /// Unique identifier of the forum
        /// </summary>
        public int Id { get; set; }
    }
}
