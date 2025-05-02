using SE.Neo.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDataTypes
{
    [Table("EAC")]
    public class EAC : BaseEntity
    {
        [Column("EAC_Id")]
        public EACType Id { get; set; }

        [Column("EAC_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
