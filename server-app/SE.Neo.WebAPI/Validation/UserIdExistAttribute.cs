using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class UserIdExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var userId = (int?)value;
            if (!userId.HasValue)
                return ValidationResult.Success;

            if (userId <= 0)
                return new ValidationResult($"{validationContext.DisplayName} must be a valid user id.");

            IUserService service = (IUserService)validationContext.GetService(typeof(IUserService));
            var exist = service.IsUserIdExist(userId.Value);
            return exist ? ValidationResult.Success : new ValidationResult($"{validationContext.DisplayName} must be a valid user id.");

        }
    }
}