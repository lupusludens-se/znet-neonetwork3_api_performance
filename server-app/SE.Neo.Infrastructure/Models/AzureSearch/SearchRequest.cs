namespace SE.Neo.Infrastructure.Models.AzureSearch
{
    public class SearchRequest
    {
        public string SearchText { get; set; }

        public int? Size { get; set; }

        public int? Skip { get; set; }

        public bool IncludeCount { get; set; }

        public List<string>? Facets { get; set; } = null;

        public List<SearchFilter>? Filters { get; set; } = null;
        public string OrderBy { get; set; } // Add this property
    }
}
