using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class ToolTitleUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;
            var title = value as string;
            if (string.IsNullOrEmpty(title))
                return new ValidationResult("Title field is required.");
            IToolService service = (IToolService)validationContext.GetService(typeof(IToolService));
            var request = ((IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor))).HttpContext.Request;
            if (request.Method == HttpMethod.Post.Method)
            {
                var unique = service.IsToolTitleUnique(title);
                return unique ? ValidationResult.Success : new ValidationResult($"{validationContext.DisplayName} must be unique.");
            }
            else
            {
                int toolId = int.Parse(request.RouteValues["id"].ToString());
                var unique = service.IsToolTitleUnique(title, toolId);
                return unique ? ValidationResult.Success : new ValidationResult($"{validationContext.DisplayName} must be unique.");

            }
        }
    }
}
