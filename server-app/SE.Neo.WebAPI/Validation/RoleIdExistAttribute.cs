using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class RoleIdExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var id = (int)value;
            if (id <= 0)
            {
                return new ValidationResult($"{validationContext.DisplayName} must be a valid role id.");
            }

            ICommonService service = (ICommonService)validationContext.GetService(typeof(ICommonService));

            var exist = service.IsRoleIdExistAsync(id).Result;
            return exist ? ValidationResult.Success : new ValidationResult($"{validationContext.DisplayName} must be a valid role id.");
        }
    }
}