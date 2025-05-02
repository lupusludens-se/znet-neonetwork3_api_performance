using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Common.Models.CMS
{
    public class CategoryDTO : BaseTaxonomyDTO
    {
        public int? SolutionId { get; set; }

        public IEnumerable<TechnologyDTO> Technologies { get; set; }

        public IEnumerable<ResourceDTO> Resources { get; set; }
    }
}