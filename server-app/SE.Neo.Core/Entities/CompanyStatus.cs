using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Company_Status")]
    public class CompanyStatus : BaseEntity
    {
        [Column("Company_Status_Id")]
        public Enums.CompanyStatus Id { get; set; }

        [Column("Company_Status_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
