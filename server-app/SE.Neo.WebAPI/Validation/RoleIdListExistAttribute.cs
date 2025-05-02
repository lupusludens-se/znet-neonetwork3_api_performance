using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class RoleIdListExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;
            var ids = value as IEnumerable<int>;
            if (ids == null)
                return new ValidationResult($"{validationContext.DisplayName} must contain valid role ids.");
            if (ids.Any(id => id <= 0))
            {
                return new ValidationResult($"{validationContext.DisplayName} must contain valid role ids.");
            }

            ICommonService service = (ICommonService)validationContext.GetService(typeof(ICommonService));

            var exist = ids.All(id => service.IsRoleIdExistAsync(id).Result);
            return exist ? ValidationResult.Success : new ValidationResult($"{validationContext.DisplayName} must contain valid role ids.");
        }
    }
}