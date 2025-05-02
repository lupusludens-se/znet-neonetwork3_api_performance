using SE.Neo.Core.Entities.CMS;
using SE.Neo.Core.Entities.ProjectDetails;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Project")]
    public class Project : BaseIdEntity
    {
        [Column("Project_Id")]
        public override int Id { get; set; }

        [Column("Title")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Column("Tags")]
        [MaxLength(600)]
        public string? Tags { get; set; }

        [Column("SubTitle")]
        [MaxLength(200)]
        public string SubTitle { get; set; }

        [Column("Description")]
        [StringLength(8000)]
        public string Description { get; set; }

        [Column("Opportunity")]
        [StringLength(8000)]
        public string Opportunity { get; set; }

        [Column("Status_Id")]
        [ForeignKey("Status")]
        public Enums.ProjectStatus StatusId { get; set; }

        public virtual ProjectStatus Status { get; set; }

        [Column("Changed_On")]
        public DateTime ChangedOn { get; set; }

        [Column("Is_Pinned")]
        public bool IsPinned { get; set; }

        [Column("Category_Id")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<ProjectTechnology> Technologies { get; set; }

        [Column("Company_Id")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        [Column("Owner_Id")]
        [ForeignKey("Owner")]
        public int OwnerId { get; set; }

        public User Owner { get; set; }
        public DateTime? PublishedOn { get; set; }

        [Column("First_Time_Published_On")]
        public DateTime? FirstTimePublishedOn { get; set; }

        public ICollection<ProjectRegion> Regions { get; set; }

        public ICollection<ProjectSaved> ProjectSaved { get; set; }

        public ICollection<ProjectContractStructure> ContractStructures { get; set; }

        public ICollection<ProjectValueProvided> ValuesProvided { get; set; }

        //Project details navigation properties
        public ProjectBatteryStorageDetails ProjectBatteryStorageDetails { get; set; }
        public ProjectCarbonOffsetsDetails ProjectCarbonOffsetsDetails { get; set; }
        public ProjectCommunitySolarDetails ProjectCommunitySolarDetails { get; set; }
        public ProjectEACDetails ProjectEACDetails { get; set; }
        public ProjectEfficiencyAuditsAndConsultingDetails ProjectEfficiencyAuditsAndConsultingDetails { get; set; }
        public ProjectEfficiencyEquipmentMeasuresDetails ProjectEfficiencyEquipmentMeasuresDetails { get; set; }
        public ProjectEmergingTechnologyDetails ProjectEmergingTechnologyDetails { get; set; }
        public ProjectEVChargingDetails ProjectEVChargingDetails { get; set; }
        public ProjectFuelCellsDetails ProjectFuelCellsDetails { get; set; }
        public ProjectGreenTariffsDetails ProjectGreenTariffsDetails { get; set; }
        public ProjectOffsitePowerPurchaseAgreementDetails ProjectOffsitePowerPurchaseAgreementDetails { get; set; }
        public ProjectOnsiteSolarDetails ProjectOnsiteSolarDetails { get; set; }
        public ProjectRenewableRetailDetails ProjectRenewableRetailDetails { get; set; }
    }
}