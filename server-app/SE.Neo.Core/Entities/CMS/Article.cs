using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.CMS
{
    [Table("CMS_Article")]
    public class Article : BaseIdEntity
    {
        [Column("CMS_Article_Id")]
        public override int Id { get; set; }

        [Column("Article_Slug")]
        [MaxLength(250)]
        public string Slug { get; set; }

        [Column("Is_Deleted")]
        public bool IsDeleted { get; set; }

        [Column("Article_Date")]
        public DateTime Date { get; set; }

        [Column("Modified_Date")]
        public DateTime Modified { get; set; }

        [Column("Title")]
        public string Title { get; set; }

        [Column("Content")]
        public string Content { get; set; }

        [Column("Image_Url")]
        public string ImageUrl { get; set; }

        [Column("Video_Url")]
        public string VideoUrl { get; set; }

        [Column("Pdf_Url")]
        public string PdfUrl { get; set; }

        [Column("Is_Public_Article")]
        public bool IsPublicArticle { get; set; }

        [Column("Type_Id")]
        [ForeignKey("Type")]
        public Enums.ArticleType TypeId { get; set; }

        public ArticleType Type { get; set; }

        public ICollection<ArticleCategory> ArticleCategories { get; set; }

        public ICollection<ArticleRegion> ArticleRegions { get; set; }

        public ICollection<ArticleSolution> ArticleSolutions { get; set; }

        public ICollection<ArticleTechnology> ArticleTechnologies { get; set; }

        public ICollection<ArticleRole> ArticleRoles { get; set; }

        public ICollection<ArticleSaved> ArticleSaved { get; set; }

        public ICollection<ArticleContentTag> ArticleContentTags { get; set; }
    }
}