using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDataTypes
{
    [Table("Settlement_Hub_Or_Load_Zone")]
    public class SettlementHubOrLoadZone : BaseEntity
    {
        [Column("Settlement_Type_Id")]
        public Enums.SettlementHubOrLoadZoneType Id { get; set; }

        [Column("Settlement_Type_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
