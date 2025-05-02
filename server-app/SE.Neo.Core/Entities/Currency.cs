using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Currency")]
    public class Currency : BaseEntity
    {
        [Column("Currency_Id")]
        public Enums.Currency Id { get; set; }

        [Column("Currency_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
