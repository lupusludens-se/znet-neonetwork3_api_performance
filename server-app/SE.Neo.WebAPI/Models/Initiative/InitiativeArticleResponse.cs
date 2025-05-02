using SE.Neo.WebAPI.Models.CMS;

namespace SE.Neo.WebAPI.Models.Initiative
{
    public class InitiativeArticleResponse
    {
        /// <summary>
        /// Unique identifier of the article.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The article's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Hyperlink for main image of the article.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Type of the article.
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// Count of all the tags present for an article including technology, solutions, categories, regions, content tags
        /// </summary>
        public int TagsTotalCount { get; set; }

        /// <summary>
        /// List of the categories that article relates to.
        /// </summary>
        public IEnumerable<CategoryResponse> Categories { get; set; }

        /// <summary>
        /// Is article new
        /// </summary>
        public bool? IsNew { get; set; }
    }
}
