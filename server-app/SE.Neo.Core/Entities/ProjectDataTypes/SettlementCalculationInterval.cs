using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDataTypes
{
    [Table("Settlement_Calculation_Interval")]
    public class SettlementCalculationInterval : BaseEntity
    {
        [Column("Settlement_Calculation_Interval_Id")]
        public Enums.SettlementCalculationIntervalType Id { get; set; }

        [Column("Settlement_Calculation_Interval_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
