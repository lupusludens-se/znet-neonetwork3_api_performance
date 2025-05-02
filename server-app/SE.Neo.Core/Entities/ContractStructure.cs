using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Contract_Structure")]
    public class ContractStructure : BaseEntity
    {
        [Column("Contract_Structure_Id")]
        public Enums.ContractStructureType Id { get; set; }

        [Column("Contract_Structure_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
