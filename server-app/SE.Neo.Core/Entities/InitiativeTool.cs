using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Initiative_Tool")]
    public class InitiativeTool : BaseIdEntity
    {
        [Column("Initiative_Tool_Id")]
        public override int Id { get; set; }

        [Column("Initiative_Id")]
        [ForeignKey("Initiative")]
        public int InitiativeId { get; set; }

        public Initiative Initiative { get; set; }

        [Column("Tool_Id")]
        [ForeignKey("Tool")]
        public int ToolId { get; set; }

        public Tool Tool { get; set; }
    }
}
