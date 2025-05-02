using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("CompanyDomain")]
    public class CompanyDomain : BaseIdEntity
    {
        [Column("Company_Id")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        [Column("Domain_Name")]
        [MaxLength(256)]
        public string DomainName { get; set; }

        [Column("Is_Active")]
        public bool IsActive { get; set; }
    }
}
