using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Initiative_Project")]
    public class InitiativeProject : BaseIdEntity
    {
        [Column("Initiative_Project_Id")]
        public override int Id { get; set; }

        [Column("Initiative_Id")]
        [ForeignKey("Initiative")]
        public int InitiativeId { get; set; }

        public Initiative Initiative { get; set; }

        [Column("Project_Id")]
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        public Project Project { get; set; }
    }
}
