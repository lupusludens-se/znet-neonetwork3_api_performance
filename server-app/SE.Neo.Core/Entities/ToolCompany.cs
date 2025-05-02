using System.ComponentModel.DataAnnotations.Schema;
namespace SE.Neo.Core.Entities
{
    [Table("Tool_Company")]
    public class ToolCompany : BaseIdEntity
    {
        [Column("Tool_Company_Id")]
        public override int Id { get; set; }

        [Column("Tool_Id")]
        [ForeignKey("Tool")]
        public int ToolId { get; set; }

        [Column("Company_Id")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        public Tool Tool { get; set; }

        public Company Company { get; set; }

    }
}
