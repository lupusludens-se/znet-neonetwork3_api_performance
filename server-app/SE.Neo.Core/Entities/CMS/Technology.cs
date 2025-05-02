using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.CMS
{
    [Table("CMS_Technology")]
    public class Technology : BaseCMSEntity
    {
        [Column("CMS_Technology_Id")]
        public override int Id { get; set; }

        [Column("Technology_Name")]
        [MaxLength(250)]
        public override string Name { get; set; }

        [Column("Technology_Slug")]
        [MaxLength(250)]
        public override string Slug { get; set; }

        [Column("Is_Deleted")]
        public override bool IsDeleted { get; set; }

        [Column("Description")]
        public override string Description { get; set; }

        public ICollection<ResourceTechnology> TechnologyResources { get; set; }

        public ICollection<CategoryTechnology> Categories { get; set; }
    }
}