using SE.Neo.WebAPI.Models.Shared;
using System.Text.Json.Serialization;

namespace SE.Neo.WebAPI.Models.CMS
{
    public class CategoryResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        public int? SolutionId { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        public IEnumerable<TechnologyResponse> Technologies { get; set; }

        public IEnumerable<ResourceResponse> Resources { get; set; }
    }
}