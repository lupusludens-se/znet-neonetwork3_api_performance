using SE.Neo.Common.Models.Media;

namespace SE.Neo.Common.Models.Forum
{
    public class ForumUserDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int StatusId { get; set; }

        public string Company { get; set; }

        public string JobTitle { get; set; }

        public bool IsFollowed { get; set; }

        public BlobDTO? Image { get; set; }
    }
}
