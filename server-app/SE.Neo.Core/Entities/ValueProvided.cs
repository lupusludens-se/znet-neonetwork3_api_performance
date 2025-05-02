using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Value_Provided")]
    public class ValueProvided : BaseEntity
    {
        [Column("Value_Provided_Id")]
        public Enums.ValueProvidedType Id { get; set; }

        [Column("Value_Provided_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
