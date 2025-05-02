using FluentValidation;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Models.Project;

namespace SE.Neo.WebAPI.Validators
{
    public class ProjectEmergingTechnologyDetailsRequestValidator : AbstractValidator<ProjectEmergingTechnologyDetailsRequest>
    {
        public ProjectEmergingTechnologyDetailsRequestValidator()
        {
            RuleFor(x => x.ValuesProvided)
                .NotEmpty()
                .When((x, d) => x.StatusId != ProjectStatus.Draft);

            RuleFor(x => x.ContractStructures)
                .NotEmpty()
                .When((x, d) => x.StatusId != ProjectStatus.Draft);
        }
    }
}
