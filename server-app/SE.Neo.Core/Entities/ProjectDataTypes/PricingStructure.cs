using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDataTypes
{
    [Table("Contract_Price")]
    public class PricingStructure : BaseEntity
    {
        [Column("Contract_Price_Id")]
        public Enums.PricingStructureType Id { get; set; }

        [Column("Contract_Price_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
