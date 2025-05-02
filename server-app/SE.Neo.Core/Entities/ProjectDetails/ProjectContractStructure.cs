using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDetails
{
    [Table("Project_Contract_Structure")]
    [Index(nameof(ProjectId), nameof(ContractStructureId), IsUnique = true)]
    public class ProjectContractStructure : BaseIdEntity
    {
        [Column("Project_Contract_Structure_Id")]
        public override int Id { get; set; }

        [Column("Project_Id")]
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        [Column("Contract_Structure_Id")]
        [ForeignKey("ContractStructure")]
        public Enums.ContractStructureType ContractStructureId { get; set; }

        public Project Project { get; set; }
        public ContractStructure ContractStructure { get; set; }
    }
}