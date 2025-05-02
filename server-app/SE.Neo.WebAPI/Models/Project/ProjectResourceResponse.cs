using SE.Neo.WebAPI.Models.CMS;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectResourceResponse
    {
        public int CategoryId { get; set; }
        public CategoryResponse Category { get; set; }
        public IEnumerable<TechnologyResponse> Technologies { get; set; }
    }
}
