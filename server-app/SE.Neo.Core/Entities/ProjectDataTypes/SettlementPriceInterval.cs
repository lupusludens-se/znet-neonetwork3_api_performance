using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDataTypes
{
    [Table("Settlement_Price_Interval")]
    public class SettlementPriceInterval : BaseEntity
    {
        [Column("Settlement_Price_Interval_Id")]
        public Enums.SettlementPriceIntervalType Id { get; set; }

        [Column("Settlement_Price_Interval_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
