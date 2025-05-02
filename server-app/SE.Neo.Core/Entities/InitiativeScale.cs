using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Initiative_Scale")]
    public class InitiativeScale : BaseEntity
    {
        [Column("Initiative_Scale_Id")]
        public SE.Neo.Common.Enums.InitiativeScale Id { get; set; }

        [Column("Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
