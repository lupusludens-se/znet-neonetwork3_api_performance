using System.Text.Json.Serialization;

namespace SE.Neo.WebAPI.Models.CMS
{
    public class TaxonomyTypeModel
    {
        [JsonPropertyName("regions")]
        public RegionTypeModel Regions { get; set; }
    }
    public class RegionTypeModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("child_counter")]
        public int Count { get; set; }
    }
}
