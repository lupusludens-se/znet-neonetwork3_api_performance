using System.Text.Json.Serialization;

namespace SE.Neo.Common.Models.CMS
{
    public class ArticleCMSDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("modified")]
        public DateTime Modified { get; set; }

        [JsonPropertyName("title")]
        public ArticleTitle Title { get; set; }

        [JsonPropertyName("content")]
        public ArticleContent Content { get; set; }

        [JsonPropertyName("categories")]
        public int[] Categories { get; set; }

        [JsonPropertyName("solutions")]
        public int[] Solutions { get; set; }

        [JsonPropertyName("technologies")]
        public int[] Technologies { get; set; }

        [JsonPropertyName("regions")]
        public int[] Regions { get; set; }

        [JsonPropertyName("neo_post_types")]
        public int[] TypeId { get; set; }

        [JsonPropertyName("neo_user_types")]
        public int[] Roles { get; set; }

        [JsonPropertyName("_neo_image")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("_neo_video")]
        public string VideoUrl { get; set; }

        [JsonPropertyName("_neo_pdf")]
        public string PdfUrl { get; set; }

        [JsonPropertyName("contenttags")]
        public int[]? ContentTags { get; set; }

        [JsonPropertyName("acf")]
        public PublicField CustomFields { get; set; }

    }

    public class ArticleTitle
    {
        [JsonPropertyName("rendered")]
        public string Name { get; set; }
    }

    public class ArticleContent
    {
        [JsonPropertyName("rendered")]
        public string Name { get; set; }
    }

    public class PublicField
    {
        [JsonPropertyName("_neo_public_site")]
        public bool IsPublicField { get; set; }
    }
}