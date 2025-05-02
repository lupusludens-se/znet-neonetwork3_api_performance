using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Industry")]
    public class Industry : BaseIdEntity
    {
        [Column("Industry_Id")]
        public override int Id { get; set; }

        [Column("Industry_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
