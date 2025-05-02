using SE.Neo.WebAPI.Models.CMS;

namespace SE.Neo.WebAPI.Models.Forum
{
    public class ForumResponse
    {
        /// <summary>
        /// Unique identifier of the forum.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The forum's topic.
        /// </summary>
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
        /// Count of the replies for the given forum.
        /// </summary>
        public int ResponsesCount { get; set; }

        /// <summary>
        /// Defines is forum followed by user that makes request.
        /// </summary>
        public bool IsFollowed { get; set; }

        /// <summary>
        /// Defines is forum saved by user that makes request.
        /// </summary>
        public bool IsSaved { get; set; }

        /// <summary>
        /// First message of the forum, that actually is a forum description.
        /// </summary>
        public ForumMessageResponse FirstMessage { get; set; }

        /// <summary>
        /// List of the users in the private forum.
        /// </summary>
        public IEnumerable<ForumUserResponse> Users { get; set; }

        /// <summary>
        /// List of the categories that forum relates to.
        /// </summary>
        public IEnumerable<CategoryResponse> Categories { get; set; }

        /// <summary>
        /// List of the regions that forum relates to.
        /// </summary>
        public IEnumerable<RegionResponse> Regions { get; set; }

        /// <summary>
        /// Last Modified User ID
        /// </summary>
        public int? UpdatedByUserId { get; set; }

        /// <summary>
        /// Time of Modification.
        /// </summary>
        public DateTime? ModifiedOn { get; set; }
    }
}
