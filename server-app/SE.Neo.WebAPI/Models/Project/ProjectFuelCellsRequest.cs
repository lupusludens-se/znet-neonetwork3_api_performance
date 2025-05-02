using SE.Neo.Core.Constants;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectFuelCellsRequest : BaseProjectDetailedRequest<ProjectFuelCellsDetailsRequest>
    {
        [Required]
        [AllowedProjectCategories(CategoriesSlugs.FuelCells)]
        public override ProjectRequest Project { get; set; }

        [Required]
        public override ProjectFuelCellsDetailsRequest ProjectDetails { get; set; }
    }
}
