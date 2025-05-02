using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    /// <summary>
    /// Only for userEmail field on User model
    /// </summary>
    public class UserEmailUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var email = value as string;
            if (string.IsNullOrEmpty(email))
            {
                return new ValidationResult($"{validationContext.DisplayName} can't be blank.");
            }
            IUserService service = (IUserService)validationContext.GetService(typeof(IUserService));
            var request = ((IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor))).HttpContext.Request;
            if (request.Method == HttpMethod.Put.Method)
            {
                int userId = int.Parse(request.RouteValues["id"].ToString());
                var correct = service.IsUserEmailExist(userId, email);
                return correct ? ValidationResult.Success : new ValidationResult($"{validationContext.DisplayName} must match current user data.");
            }
            return ValidationResult.Success;
        }
    }
}
