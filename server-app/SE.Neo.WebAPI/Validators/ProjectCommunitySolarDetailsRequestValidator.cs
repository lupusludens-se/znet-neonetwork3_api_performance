using FluentValidation;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Models.Project;

namespace SE.Neo.WebAPI.Validators
{
    public class ProjectCommunitySolarDetailsRequestValidator : AbstractValidator<ProjectCommunitySolarDetailsRequest>
    {
        public ProjectCommunitySolarDetailsRequestValidator()
        {
            RuleFor(x => x.ValuesProvided)
                .NotEmpty()
                .When((x, d) => x.StatusId != ProjectStatus.Draft);

            RuleFor(x => x.ContractStructures)
                .NotEmpty()
                .When((x, d) => x.StatusId != ProjectStatus.Draft);

            RuleFor(x => x.ProjectAvailabilityApproximateDate)
                .NotNull()
                .When((x, d) => x.StatusId != ProjectStatus.Draft && x.ProjectAvailable == false);
        }
    }
}
