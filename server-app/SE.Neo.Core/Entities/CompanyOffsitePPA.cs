using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Company_Offsite_PPA")]
    public class CompanyOffsitePPA : BaseIdEntity
    {
        [Column("Company_Offsite_PPA_Id")]
        public override int Id { get; set; }

        [Column("Company_Id")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        [Column("Offsite_PPA_Id")]
        [ForeignKey("OffsitePPA")]
        public Enums.OffsitePPAs OffsitePPAId { get; set; }

        public Company Company { get; set; }
        public OffsitePPA OffsitePPA { get; set; }
    }
}