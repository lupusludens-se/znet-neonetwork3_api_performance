using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Common.Models.CMS
{
    public class ArticleDTO
    {
        public int Id { get; set; }

        public string Slug { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime Date { get; set; }

        public DateTime Modified { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public string VideoUrl { get; set; }

        public string PdfUrl { get; set; }

        public int TypeId { get; set; }

        public bool IsSaved { get; set; }

        public bool IsPublicArticle { get; set; }

        public IEnumerable<RoleDTO> Roles { get; set; }

        public IEnumerable<CategoryDTO> Categories { get; set; }

        public IEnumerable<RegionDTO> Regions { get; set; }

        public IEnumerable<TechnologyDTO> Technologies { get; set; }

        public IEnumerable<SolutionDTO> Solutions { get; set; }

        public IEnumerable<ContentTagDTO> ContentTags { get; set; }
    }
}