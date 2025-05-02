using SE.Neo.Common.Models.CMS;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Models.CMS;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SE.Neo.WebAPI.Validation
{
    public class TechnologyRelateToCategoryAttribute : ValidationAttribute
    {
        private readonly string _categoryIdPropertyName;

        public TechnologyRelateToCategoryAttribute(string categoryIdPropertyName)
        {
            _categoryIdPropertyName = categoryIdPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo? categoryIdPropertyInfo = validationContext.ObjectType.GetProperty(_categoryIdPropertyName);
            if (categoryIdPropertyInfo == null)
                return new ValidationResult($"Unknown property {_categoryIdPropertyName}.");

            object? categoryIdPropertyValue = categoryIdPropertyInfo.GetValue(validationContext.ObjectInstance, null);
            if (categoryIdPropertyValue is not int)
                return new ValidationResult($"Property {_categoryIdPropertyName} value is not of {nameof(Int32)} type.");

            int categoryId = (int)categoryIdPropertyValue;

            var technologiesRequest = value as IEnumerable<TechnologyRequest>;
            if (value != null && technologiesRequest == null)
                return new ValidationResult("Invalid value provided.");

            ICommonService service = (ICommonService)validationContext.GetService(typeof(ICommonService))!;
            CategoryDTO? categoryDTO = service.GetCategoriesAsync().Result.FirstOrDefault(c => c.Id == categoryId && !c.IsDeleted);

            if (!service.IsCategoryIdExistAsync(categoryId).Result || categoryDTO == null)
                return new ValidationResult("Non existent category.");

            bool isRelate;
            if (technologiesRequest != null)
                isRelate = !technologiesRequest.Any(tr => !categoryDTO.Technologies.Any(t => tr.Id == t.Id!));
            else
                isRelate = !categoryDTO.Technologies.Any();

            return isRelate ? ValidationResult.Success : new ValidationResult($"{validationContext.DisplayName} must relate to a provided {categoryDTO.Name} category.");
        }
    }
}
