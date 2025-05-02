using SE.Neo.WebAPI.Models.CMS;

namespace SE.Neo.WebAPI.Models.Article
{
    public class ArticleResponse
    {
        /// <summary>
        /// Unique identifier of the article.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date and time of creation.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The article's slug.
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Date and time of last modification.
        /// </summary>
        public DateTime Modified { get; set; }

        /// <summary>
        /// The article's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Content of the article.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Hyperlink for main image of the article.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Hyperlink for main video of the article.
        /// </summary>
        public string VideoUrl { get; set; }

        /// <summary>
        /// Hyperlink for main pdf of the article.
        /// </summary>
        public string PdfUrl { get; set; }

        /// <summary>
        /// Type of the article.
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// Is article saved for current user.
        /// </summary>
        public bool IsSaved { get; set; }

        /// <summary>
        /// Is article public.
        /// </summary>
        public bool IsPublicArticle { get; set; }

        /// <summary>
        /// List of the categories that article relates to.
        /// </summary>
        public IEnumerable<CategoryResponse> Categories { get; set; }

        /// <summary>
        /// List of the regions that article relates to.
        /// </summary>
        public IEnumerable<RegionResponse> Regions { get; set; }

        /// <summary>
        /// List of the technologies that article relates to.
        /// </summary>
        public IEnumerable<TechnologyResponse> Technologies { get; set; }

        /// <summary>
        /// List of the solutions that article relates to.
        /// </summary>
        public IEnumerable<SolutionResponse> Solutions { get; set; }

        /// <summary>
        ///  List of the content tags that article relates to.
        /// </summary>
        public IEnumerable<ContentTagResponse> ContentTags { get; set; }
    }
}
