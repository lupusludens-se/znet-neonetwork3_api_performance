using Microsoft.EntityFrameworkCore;
using SE.Neo.Core.Entities.ProjectDataTypes;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDetails
{
    [Table("Project_Renewable_Retail_Details_Purchase_Option")]
    [Index(nameof(ProjectRenewableRetailDetailsId), nameof(PurchaseOptionId), IsUnique = true)]
    public class ProjectRenewableRetailDetailsPurchaseOption : BaseIdEntity
    {
        [Column("Project_Renewable_Retail_Details_Purchase_Option_Id")]
        public override int Id { get; set; }

        [Column("Project_Renewable_Retail_Details_Id")]
        [ForeignKey("ProjectRenewableRetailDetails")]
        public int ProjectRenewableRetailDetailsId { get; set; }

        [Column("Purchase_Option_Id")]
        [ForeignKey("PurchaseOption")]
        public Enums.PurchaseOptionType PurchaseOptionId { get; set; }

        public ProjectRenewableRetailDetails ProjectRenewableRetailDetails { get; set; }
        public PurchaseOption PurchaseOption { get; set; }
    }
}