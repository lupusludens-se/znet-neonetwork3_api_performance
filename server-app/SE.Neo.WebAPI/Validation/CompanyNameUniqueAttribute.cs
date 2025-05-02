using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class CompanyNameUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var companyName = value as string;
            bool includeActiveStatusOnly = true;
            if (string.IsNullOrEmpty(companyName))
            {
                return new ValidationResult($"{validationContext.DisplayName} can't be blank.");
            }
            ICompanyService service = (ICompanyService)validationContext.GetService(typeof(ICompanyService));
            var request = ((IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor))).HttpContext.Request;
            bool unique;
            if (request.Method == HttpMethod.Post.Method)
            {
                unique = service.IsCompanyNameUnique(companyName, includeActiveStatusOnly);
            }
            else
            {
                int companyId = int.Parse(request.RouteValues["id"].ToString());
                unique = service.IsCompanyNameUnique(companyName, includeActiveStatusOnly, companyId);
            }
            return unique ? ValidationResult.Success : new ValidationResult($"{validationContext.DisplayName} must be a unique company name.");
        }
    }
}