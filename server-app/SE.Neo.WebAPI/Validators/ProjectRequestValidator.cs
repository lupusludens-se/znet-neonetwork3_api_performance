using FluentValidation;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.User;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Models.Project;

namespace SE.Neo.WebAPI.Validators
{
    public class ProjectRequestValidator : AbstractValidator<ProjectRequest>
    {
        public ProjectRequestValidator(IUserService userService, ICompanyService companyService)
        {
            RuleFor(x => x.OwnerId)
                .Must((x, ownerId) =>
                {
                    if (ownerId is null)
                        return false;

                    UserDTO? userDTO = userService.GetUserAsync((int)ownerId).Result;

                    if (userDTO is null || userDTO.StatusId != (int)UserStatus.Active)
                        return false;

                    CompanyDTO? companyDTO = companyService.GetCompanyAsync(userDTO.CompanyId, expand: "categories").Result;

                    if (companyDTO is null || companyDTO.TypeId != (int)CompanyType.SolutionProvider)
                        return false;

                    return companyDTO.Categories.Any(c => c.Id == x.CategoryId);
                }).WithMessage("PublishedBy User is either inactive or has been deleted or the user's company doesn't have the solution capability for the selected project type");
        }
    }
}