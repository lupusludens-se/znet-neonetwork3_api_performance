namespace SE.Neo.WebAPI.Models.CMS
{
    public class TaxonomyRequest
    {
        public IEnumerable<CategoryResponse> categories { get; set; }
        public IEnumerable<SolutionResponse> solutions { get; set; }
        public IEnumerable<TechnologyResponse> technologies { get; set; }
        public IEnumerable<RegionResponse> regions { get; set; }
        public IEnumerable<ContentTagResponse> contenttags { get; set; }

    }
}
