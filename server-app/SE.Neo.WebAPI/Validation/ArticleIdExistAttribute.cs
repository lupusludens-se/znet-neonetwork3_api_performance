using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class ArticleIdExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int? id = (int?)value;

            if (!id.HasValue)
            {
                return ValidationResult.Success!;
            }

            if (id.Value <= 0)
            {
                return new ValidationResult($"{validationContext.DisplayName} must be a valid article cms id.");
            }

            IArticleService service = (IArticleService)validationContext.GetService(typeof(IArticleService));

            var exist = service.IsArticleExist(id.Value);
            return exist ? ValidationResult.Success! : new ValidationResult($"{validationContext.DisplayName} must be a valid article cms id.");
        }
    }
}