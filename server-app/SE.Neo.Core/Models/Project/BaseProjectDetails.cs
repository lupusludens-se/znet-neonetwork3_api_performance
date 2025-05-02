using SE.Neo.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Models.Project
{
    public abstract class BaseProjectDetails : BaseIdEntity
    {
        [Column("Project_Id")]
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        public Entities.Project Project { get; set; }
    }
}
