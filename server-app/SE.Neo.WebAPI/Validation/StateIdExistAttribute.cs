using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class StateIdExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var id = value as int?;
            if (id == null)
                return ValidationResult.Success;

            if (id <= 0)
                return new ValidationResult($"{validationContext.DisplayName} must be a valid state id.");

            ICommonService service = (ICommonService)validationContext.GetService(typeof(ICommonService));

            var exist = service.IsStateIdExistAsync(id ?? 0).Result;
            return exist ? ValidationResult.Success : new ValidationResult($"{validationContext.DisplayName} must be a valid state id.");
        }
    }
}