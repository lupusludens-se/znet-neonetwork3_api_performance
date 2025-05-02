using FluentValidation;
using SE.Neo.WebAPI.Models.Project;

namespace SE.Neo.WebAPI.Validators
{
    public class BaseProjectDetailedRequestValidator<T> : AbstractValidator<BaseProjectDetailedRequest<T>> where T : BaseProjectDetailsRequest
    {
        public BaseProjectDetailedRequestValidator(IValidator<ProjectRequest> projectValidator, IValidator<T> projectDetailsValidator)
        {
            RuleFor(x => x.Project.StatusId)
                .Equal(x => x.ProjectDetails.StatusId);

            RuleFor(x => x.Project)
                .SetValidator(projectValidator);

            RuleFor(x => x.ProjectDetails)
                .SetValidator(projectDetailsValidator);
        }
    }
}
