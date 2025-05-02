using System.Text.Json.Serialization;

namespace SE.Neo.WebAPI.Models.CMS
{
    public class RegionResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("parent")]
        public int ParentId { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}