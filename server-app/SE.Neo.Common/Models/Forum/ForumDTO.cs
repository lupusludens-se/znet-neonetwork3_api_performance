using SE.Neo.Common.Models.CMS;

namespace SE.Neo.Common.Models.Forum
{
    public class ForumDTO
    {
        public int Id { get; set; }

        public int CreatedByUserId { get; set; }

        public string Subject { get; set; }

        public bool IsPrivate { get; set; }

        public bool IsPinned { get; set; }

        public int ResponsesCount { get; set; }

        public bool IsFollowed { get; set; }

        public bool IsSaved { get; set; }

        public ForumMessageDTO FirstMessage { get; set; }

        public IEnumerable<ForumUserDTO> Users { get; set; }

        public IEnumerable<CategoryDTO> Categories { get; set; }

        public IEnumerable<RegionDTO> Regions { get; set; }
        public int? UpdatedByUserId { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
