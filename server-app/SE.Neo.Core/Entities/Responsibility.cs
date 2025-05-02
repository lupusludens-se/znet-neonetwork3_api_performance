using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    public class Responsibility : BaseEntity
    {
        [Column("Responsibility_Id")]
        public Enums.Responsibility Id { get; set; }

        [Column("Responsibility_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}