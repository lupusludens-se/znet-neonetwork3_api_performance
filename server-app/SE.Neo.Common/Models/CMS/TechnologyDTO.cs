using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Common.Models.CMS
{
    public class TechnologyDTO : BaseTaxonomyDTO
    {
        public IEnumerable<CategoryDTO> Categories { get; set; }

        public IEnumerable<ResourceDTO> Resources { get; set; }
    }
}