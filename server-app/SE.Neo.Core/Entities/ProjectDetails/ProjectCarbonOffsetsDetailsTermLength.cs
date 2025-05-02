using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDetails
{
    [Table("Project_Carbon_Offsets_Details_Term_Length")]
    [Index(nameof(ProjectCarbonOffsetsDetailsId), nameof(TermLengthId), IsUnique = true)]
    public class ProjectCarbonOffsetsDetailsTermLength : BaseIdEntity
    {
        [Column("Project_Carbon_Offsets_Details_Term_Length_Id")]
        public override int Id { get; set; }

        [Column("Project_Carbon_Offsets_Details_Id")]
        [ForeignKey("ProjectCarbonOffsetsDetails")]
        public int ProjectCarbonOffsetsDetailsId { get; set; }

        [Column("Term_Length_Id")]
        [ForeignKey("TermLength")]
        public Enums.TermLengthType TermLengthId { get; set; }

        public ProjectCarbonOffsetsDetails ProjectCarbonOffsetsDetails { get; set; }
        public TermLength TermLength { get; set; }
    }
}