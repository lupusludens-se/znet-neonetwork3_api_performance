namespace SE.Neo.Infrastructure.Models.AzureSearch
{
    public class SearchEntity
    {
        public string? Id { get; set; }

        public int? OriginalId { get; set; }

        public string? Subject { get; set; }

        public string? Description { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? EntityType { get; set; }

        public bool? IsDeleted { get; set; }

        public List<SearchEntityCategory>? Categories { get; set; }

        public List<SearchEntitySolution>? Solutions { get; set; }

        public List<SearchEntityTechnology>? Technologies { get; set; }

        public List<SearchEntityRegion>? Regions { get; set; }

        public List<SearchEntityContentTag>? ContentTags { get; set; }
    }
}
