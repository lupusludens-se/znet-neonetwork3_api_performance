using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    /// <summary>
    /// Only for userId field on UserProfile model
    /// </summary>
    public class UserIdExistAndUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var userId = (int)value;
            if (userId <= 0)
            {
                return new ValidationResult($"{validationContext.DisplayName} must be a valid user id.");
            }

            IUserService service = (IUserService)validationContext.GetService(typeof(IUserService));
            var request = ((IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor))).HttpContext.Request;

            var exist = service.IsUserIdExist(userId);

            var userProfileExist = service.IsUserIdInProfileExist(userId);

            if (request.Method == HttpMethod.Put.Method)
            {
                return (exist && userProfileExist) ? ValidationResult.Success : new ValidationResult($"{validationContext.DisplayName} must be a valid user id.");
            }
            else
            {
                return (exist && !userProfileExist) ? ValidationResult.Success : new ValidationResult($"{validationContext.DisplayName} must be a valid user id.");
            }
        }
    }
}