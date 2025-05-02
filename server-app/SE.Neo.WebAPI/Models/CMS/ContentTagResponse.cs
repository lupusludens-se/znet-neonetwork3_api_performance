using System.Text.Json.Serialization;

namespace SE.Neo.WebAPI.Models.CMS
{
    public class ContentTagResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("isdeleted")]
        public bool IsDeleted { get; set; }
    }
}
