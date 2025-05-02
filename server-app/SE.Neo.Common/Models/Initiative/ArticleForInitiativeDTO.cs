using SE.Neo.Common.Models.CMS;

namespace SE.Neo.Common.Models.Initiative
{
    public class ArticleForInitiativeDTO
    {
        /// <summary>
        /// Id of the article item.
        /// </summary>
        public int Id { get; set; }
        public int InitiativeId { get; set; }
        public string Title { get; set; }
        public int TypeId { get; set; }
        public string ImageUrl { get; set; }
        public int TagsTotalCount { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }
        public bool? IsNew { get; set; } = false;
    }
}
