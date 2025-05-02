using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Project_Status")]
    public class ProjectStatus : BaseEntity
    {
        [Column("Project_Status_Id")]
        public Enums.ProjectStatus Id { get; set; }

        [Column("Project_Status_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
