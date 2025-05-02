using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Saved
{
    public class ArticleSavedRequest
    {
        /// <summary>
        /// Unique identifier of the article.
        /// </summary>
        [Required]
        public int ArticleId { get; set; }
    }
}
