using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDataTypes
{
    [Table("Settlement_Type")]
    public class SettlementType : BaseEntity
    {
        [Column("Settlement_Type_Id")]
        public Enums.SettlementType Id { get; set; }

        [Column("Settlement_Type_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
