using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDataTypes
{
    [Table("Purchase_Option")]
    public class PurchaseOption : BaseEntity
    {
        [Column("Purchase_Option_Id")]
        public Enums.PurchaseOptionType Id { get; set; }

        [Column("Purchase_Option_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
