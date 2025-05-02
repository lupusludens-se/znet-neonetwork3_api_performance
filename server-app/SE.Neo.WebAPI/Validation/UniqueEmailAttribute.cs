using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var email = (string?)value;
            if (string.IsNullOrEmpty(email))
            {
                return new ValidationResult($"{validationContext.DisplayName} must be a valid email address.");
            }

            var userPendingservice = (validationContext.GetService(typeof(IUserPendingService)) as IUserPendingService)!;
            var userService = (validationContext.GetService(typeof(IUserService)) as IUserService)!;
            var request = ((IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor))).HttpContext.Request;

            bool userPendingEmailExists = userPendingservice.IsUserPendingExist(email, request.Method == HttpMethod.Put.Method ? int.Parse(request.RouteValues["id"].ToString()) : null);
            bool userNameOrEmailExists = userService.IsUserNameOrEmailExists(email, request.Method == HttpMethod.Put.Method ? int.Parse(request.RouteValues["id"].ToString()) : null);


            return userPendingEmailExists || userNameOrEmailExists ? new ValidationResult($"Such {validationContext.DisplayName} already exists.") : ValidationResult.Success;

        }
    }
}