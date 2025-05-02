using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDetails
{
    [Table("Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided")]
    [Index(nameof(ProjectOffsitePowerPurchaseAgreementDetailsId), nameof(ValueProvidedId), IsUnique = true)]
    public class ProjectOffsitePPADetailsValueProvided : BaseIdEntity
    {
        [Column("Project_Offsite_Power_Purchase_Agreement_Details_Value_Provided_Id")]
        public override int Id { get; set; }

        [Column("Project_Offsite_Power_Purchase_Agreement_Details_Id")]
        [ForeignKey("ProjectOffsitePowerPurchaseAgreementDetails")]
        public int ProjectOffsitePowerPurchaseAgreementDetailsId { get; set; }

        [Column("Value_Provided_Id")]
        [ForeignKey("ValueProvided")]
        public Enums.ValueProvidedType ValueProvidedId { get; set; }

        public ProjectOffsitePowerPurchaseAgreementDetails ProjectOffsitePowerPurchaseAgreementDetails { get; set; }
        public ValueProvided ValueProvided { get; set; }
    }
}