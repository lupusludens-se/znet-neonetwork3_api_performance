using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.CMS
{
    [Table("CMS_Category_Technology")]
    public class CategoryTechnology : BaseEntity
    {
        [Column("CMS_Category_Technology_Id")]
        public int Id { get; set; }

        [Column("CMS_Category_Id")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [Column("CMS_Technology_Id")]
        [ForeignKey("Technology")]
        public int TechnologyId { get; set; }

        public Category Category { get; set; }
        public Technology Technology { get; set; }
    }
}