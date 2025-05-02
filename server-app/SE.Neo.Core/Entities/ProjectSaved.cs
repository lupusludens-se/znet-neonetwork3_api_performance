using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Project_Saved")]
    public class ProjectSaved : BaseIdEntity
    {
        [Column("Project_Saved_Id")]
        public override int Id { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Column("Project_Id")]
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        public User User { get; set; }

        public Project Project { get; set; }
    }
}