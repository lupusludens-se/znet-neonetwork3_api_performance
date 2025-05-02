using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Initiative_Status")]
    public class InitiativeStatus : BaseEntity
    {
        [Column("Status_Id")]
        public Enums.InitiativeStatus Id { get; set; }

        [Column("Status_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
