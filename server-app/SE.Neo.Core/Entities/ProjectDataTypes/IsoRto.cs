using SE.Neo.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDataTypes
{
    [Table("ISO_RTO")]
    public class IsoRto : BaseEntity
    {
        [Column("ISO_RTO_Id")]
        public IsoRtoType Id { get; set; }

        [Column("ISO_RTO_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
