namespace SE.Neo.Common.Models.CMS
{
    public class TaxonomyDTO
    {
        public IEnumerable<CategoryDTO> categories { get; set; }
        public IEnumerable<SolutionDTO> solutions { get; set; }
        public IEnumerable<TechnologyDTO> technologies { get; set; }
        public IEnumerable<RegionDTO> regions { get; set; }

        public IEnumerable<ContentTagDTO> contenttags { get; set; }
    }
}