using SE.Neo.Core.Constants;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectBatteryStorageRequest : BaseProjectDetailedRequest<ProjectBatteryStorageDetailsRequest>
    {
        [Required]
        [AllowedProjectCategories(CategoriesSlugs.BatteryStorage)]
        public override ProjectRequest Project { get; set; }

        [Required]
        public override ProjectBatteryStorageDetailsRequest ProjectDetails { get; set; }
    }
}
