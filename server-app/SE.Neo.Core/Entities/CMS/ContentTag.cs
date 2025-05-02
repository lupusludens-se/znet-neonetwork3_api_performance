using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.CMS
{
    [Table("CMS_Content_Tag")]
    public class ContentTag : BaseCMSEntity
    {
        [Column("CMS_Content_Tag_Id")]
        public override int Id { get; set; }

        [Column("Content_Tag_Name")]
        [MaxLength(250)]
        public override string Name { get; set; }

        [Column("Content_Tag_Slug")]
        [MaxLength(250)]
        public override string Slug { get; set; }

        [Column("Is_Deleted")]
        public override bool IsDeleted { get; set; }

        [Column("Description")]
        public override string Description { get; set; }
    }
}
