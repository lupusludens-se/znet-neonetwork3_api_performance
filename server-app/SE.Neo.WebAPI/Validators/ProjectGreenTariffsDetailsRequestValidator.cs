using FluentValidation;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Models.Project;

namespace SE.Neo.WebAPI.Validators
{
    public class ProjectGreenTariffsDetailsRequestValidator : AbstractValidator<ProjectGreenTariffsDetailsRequest>
    {
        public ProjectGreenTariffsDetailsRequestValidator()
        {
            RuleFor(x => x.ValuesProvided)
                .NotEmpty()
                .When((x, d) => x.StatusId != ProjectStatus.Draft);
        }
    }
}
