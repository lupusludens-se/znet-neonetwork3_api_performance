namespace SE.Neo.Infrastructure.Models.AzureSearch
{
    public class SearchOutput
    {
        public long? Count { get; set; }

        public List<SearchEntity> Results { get; set; }

        public Dictionary<string, IList<FacetValue>> Facets { get; set; }
    }
}
