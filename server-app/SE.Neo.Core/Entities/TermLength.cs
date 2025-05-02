using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Term_Length")]
    public class TermLength : BaseEntity
    {
        [Column("Term_Length_Id")]
        public Enums.TermLengthType Id { get; set; }

        [Column("Term_Length_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
