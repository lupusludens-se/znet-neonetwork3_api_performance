using Microsoft.EntityFrameworkCore;
using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Project_Region")]
    [Index(nameof(ProjectId), nameof(RegionId), IsUnique = true)]
    public class ProjectRegion : BaseIdEntity
    {
        [Column("Project_Region_Id")]
        public override int Id { get; set; }

        [Column("Project_Id")]
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        [Column("Region_Id")]
        [ForeignKey("Region")]
        public int RegionId { get; set; }

        public Project Project { get; set; }
        public Region Region { get; set; }
    }
}