using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Heard_Via")]
    public class HeardVia : BaseEntity
    {
        [Column("Heard_Via_Id")]
        public Enums.HeardVia Id { get; set; }

        [Column("Heard_Via_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
