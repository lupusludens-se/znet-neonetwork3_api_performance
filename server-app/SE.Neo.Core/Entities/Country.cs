using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Country")]
    public class Country : BaseIdEntity
    {
        [Column("Country_Id")]
        public override int Id { get; set; }

        [Column("Country_Name")]
        [MaxLength(250)]
        public string Name { get; set; }

        [Column("Country_Code")]
        [MaxLength(50)]
        public string Code { get; set; }

        [Column("Country_Code3")]
        [MaxLength(50)]
        public string Code3 { get; set; }

        public ICollection<State> States { get; set; }
    }
}