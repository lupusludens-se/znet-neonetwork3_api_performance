using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Resource")]
    public class Resource : BaseIdEntity
    {
        [Column("Resource_Id")]
        public override int Id { get; set; }

        [Column("Content_Title")]
        [MaxLength(250)]
        public string ContentTitle { get; set; }

        [Column("Reference_Url")]
        [MaxLength(2048)]
        public string ReferenceUrl { get; set; }

        [Column("Type_Id")]
        [ForeignKey("Type")]
        public Enums.ResourceType TypeId { get; set; }

        public ResourceType Type { get; set; }

        [Column("Article_Id")]
        [ForeignKey("Article")]
        public int? ArticleId { get; set; }

        [Column("Tool_Id")]
        [ForeignKey("Tool")]
        public int? ToolId { get; set; }

        public Article? Article { get; set; }

        public Tool? Tool { get; set; }

        public ICollection<ResourceCategory> ResourceCategories { get; set; }

        public ICollection<ResourceTechnology> ResourceTechnologies { get; set; }
    }
}