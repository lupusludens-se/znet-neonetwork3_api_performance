using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Company_Category")]
    public class CompanyCategory : BaseIdEntity
    {
        [Column("Company_Category_Id")]
        public override int Id { get; set; }

        [Column("Company_Id")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        [Column("Category_Id")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Company Company { get; set; }
        public Category Category { get; set; }
    }
}