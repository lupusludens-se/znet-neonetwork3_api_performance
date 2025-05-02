using Microsoft.EntityFrameworkCore;
using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Project_Technology")]
    [Index(nameof(ProjectId), nameof(TechnologyId), IsUnique = true)]
    public class ProjectTechnology : BaseIdEntity
    {
        [Column("Project_Technology_Id")]
        public override int Id { get; set; }

        [Column("Project_Id")]
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        [Column("Technology_Id")]
        [ForeignKey("Technology")]
        public int TechnologyId { get; set; }

        public Project Project { get; set; }
        public Technology Technology { get; set; }
    }
}