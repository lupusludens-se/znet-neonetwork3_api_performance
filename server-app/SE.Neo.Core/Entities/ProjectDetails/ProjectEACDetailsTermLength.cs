using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDetails
{
    [Table("Project_EAC_Details_Term_Length")]
    [Index(nameof(ProjectEACDetailsId), nameof(TermLengthId), IsUnique = true)]
    public class ProjectEACDetailsTermLength : BaseIdEntity
    {
        [Column("Project_EAC_Details_Term_Length_Id")]
        public override int Id { get; set; }

        [Column("Project_EAC_Details_Id")]
        [ForeignKey("ProjectEACDetails")]
        public int ProjectEACDetailsId { get; set; }

        [Column("Term_Length_Id")]
        [ForeignKey("TermLength")]
        public Enums.TermLengthType TermLengthId { get; set; }

        public ProjectEACDetails ProjectEACDetails { get; set; }
        public TermLength TermLength { get; set; }
    }
}