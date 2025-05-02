using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDetails
{
    [Table("Project_Value_Provided")]
    [Index(nameof(ProjectId), nameof(ValueProvidedId), IsUnique = true)]
    public class ProjectValueProvided : BaseIdEntity
    {
        [Column("Project_Value_Provided_Id")]
        public override int Id { get; set; }

        [Column("Project_Id")]
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        [Column("Value_Provided_Id")]
        [ForeignKey("ValueProvided")]
        public Enums.ValueProvidedType ValueProvidedId { get; set; }

        public Project Project { get; set; }
        public ValueProvided ValueProvided { get; set; }
    }
}