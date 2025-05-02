using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class TimeZoneIdExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var id = (value as int?) ?? 0;

            ICommonService service = (ICommonService)validationContext.GetService(typeof(ICommonService));

            var exist = service.IsTimeZoneIdExistAsync(id).Result;
            return exist ? ValidationResult.Success : new ValidationResult($"{validationContext.DisplayName} must be a valid timezone id.");
        }
    }
}