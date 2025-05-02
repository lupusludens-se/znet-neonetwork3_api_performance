namespace SE.Neo.Common.Models.CMS
{
    public class SolutionDTO : BaseTaxonomyDTO
    {
        public string? Scope { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }
    }
}