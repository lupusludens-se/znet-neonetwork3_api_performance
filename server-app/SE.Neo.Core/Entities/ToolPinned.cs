using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Tool_Pinned")]
    public class ToolPinned : BaseIdEntity
    {
        [Column("Tool_Pinned_Id")]
        public override int Id { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Column("Tool_Id")]
        [ForeignKey("Tool")]
        public int ToolId { get; set; }

        public User User { get; set; }

        public Tool Tool { get; set; }
    }
}