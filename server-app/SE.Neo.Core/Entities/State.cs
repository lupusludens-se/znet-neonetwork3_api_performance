using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("State")]
    public class State : BaseIdEntity
    {
        [Column("State_Id")]
        public override int Id { get; set; }

        [Column("State_Name")]
        [MaxLength(250)]
        public string Name { get; set; }

        [Column("State_Abbr")]
        [MaxLength(50)]
        public string Abbr { get; set; }

        [Column("Country_Id")]
        [ForeignKey("Country")]
        public int CountryId { get; set; }

        public Country Country { get; set; }
    }
}