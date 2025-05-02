using SE.Neo.WebAPI.Models.CMS;

namespace SE.Neo.WebAPI.Models.Shared
{
    public class ResourceResponse
    {
        public int Id { get; set; }

        public string ContentTitle { get; set; }

        //TODO  Remove after all resource types will be defined
        [Obsolete("This property is obsolete. Use ArticleId and ToolId instead.")]
        public string ReferenceUrl { get; set; }

        public int TypeId { get; set; }

        public string Type { get; set; }

        public int? ArticleId { get; set; }

        public int? ToolId { get; set; }

        public IEnumerable<CategoryResponse> Categories { get; set; }

        public IEnumerable<TechnologyResponse> Technologies { get; set; }
    }
}
