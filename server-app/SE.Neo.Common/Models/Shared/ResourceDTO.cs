using SE.Neo.Common.Models.CMS;

namespace SE.Neo.Common.Models.Shared
{
    public class ResourceDTO
    {
        public int Id { get; set; }

        public string ContentTitle { get; set; }

        public string ReferenceUrl { get; set; }

        public int TypeId { get; set; }

        public string Type { get; set; }

        public int? ArticleId { get; set; }

        public int? ToolId { get; set; }

        public IEnumerable<CategoryDTO> Categories { get; set; }

        public IEnumerable<TechnologyDTO> Technologies { get; set; }
    }
}