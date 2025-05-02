using SE.Neo.Common.Enums;
using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class ValidateUserRoleAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //RoleType inputAsEnum = (RoleType)Enum.TryParse(typeof(RoleType), Convert.ToString(value));
            var isEnumStringParsed = Enum.TryParse(Convert.ToString(value), true, out RoleType role);

            if (!isEnumStringParsed)
            {
                return new ValidationResult($"Please select a valid {validationContext.DisplayName}");
            }

            //var ids = value as IEnumerable<int>;
            if (role <= 0)
            {
                return new ValidationResult($"Please select a valid {validationContext.DisplayName}");
            }

            ICommonService service = (ICommonService)validationContext.GetService(typeof(ICommonService));

            var exist = service.IsRoleIdExistAsync(Convert.ToInt32(value)).Result;
            if (exist)
            {
                if (role == RoleType.Corporation || role == RoleType.SolutionProvider || role == RoleType.SPAdmin || role == RoleType.Internal)
                {
                    return ValidationResult.Success;

                }
            }
            return new ValidationResult($"Please select a valid {validationContext.DisplayName}.");

        }
    }
}