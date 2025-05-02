using SE.Neo.Core.Constants;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectEfficiencyEquipmentMeasuresRequest : BaseProjectDetailedRequest<ProjectEfficiencyEquipmentMeasuresDetailsRequest>
    {
        [Required]
        [AllowedProjectCategories(CategoriesSlugs.EfficiencyEquipmentMeasures)]
        public override ProjectRequest Project { get; set; }

        [Required]
        public override ProjectEfficiencyEquipmentMeasuresDetailsRequest ProjectDetails { get; set; }
    }
}
