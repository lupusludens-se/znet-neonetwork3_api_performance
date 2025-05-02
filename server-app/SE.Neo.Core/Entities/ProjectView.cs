using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Project_View")]
    public class ProjectView : BaseEntity
    {
        [Column("Project_View_Id")]
        public int Id { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Column("Project_Id")]
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        public Project Project { get; set; }

        public User User { get; set; }
    }
}