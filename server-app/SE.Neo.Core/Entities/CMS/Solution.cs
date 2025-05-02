using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.CMS
{
    [Table("CMS_Solution")]
    public class Solution : BaseCMSEntity
    {
        [Column("CMS_Solution_Id")]
        public override int Id { get; set; }

        [Column("Solution_Name")]
        [MaxLength(250)]
        public override string Name { get; set; }

        [Column("Solution_Slug")]
        [MaxLength(250)]
        public override string Slug { get; set; }

        [Column("Is_Deleted")]
        public override bool IsDeleted { get; set; }

        [Column("Description")]
        public override string Description { get; set; }

        [Column("Scope")]
        [MaxLength(100)]
        public string? Scope { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}