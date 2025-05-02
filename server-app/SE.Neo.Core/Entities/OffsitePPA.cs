using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Offsite_PPA")]
    public class OffsitePPA : BaseEntity
    {
        [Column("Offsite_PPA_Id")]
        public Enums.OffsitePPAs Id { get; set; }

        [Column("Offsite_PPA_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}