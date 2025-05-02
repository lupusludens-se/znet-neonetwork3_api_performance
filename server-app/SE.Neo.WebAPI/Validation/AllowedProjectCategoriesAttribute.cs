using SE.Neo.Common.Models.CMS;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Models.Project;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class AllowedProjectCategoriesAttribute : ValidationAttribute
    {
        private readonly string[] _categorySlugs;

        public AllowedProjectCategoriesAttribute(params string[] categorySlugs)
        {
            _categorySlugs = categorySlugs;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var projectRequest = value as ProjectRequest;
            if (projectRequest == null)
                return new ValidationResult("Invalid value provided.");

            ICommonService service = (ICommonService)validationContext.GetService(typeof(ICommonService))!;
            IEnumerable<CategoryDTO> categoryDTOs = service.GetCategoriesAsync().Result;

            List<CategoryDTO> specifiedCategoryDTOs = new List<CategoryDTO>();
            foreach (var categorySlug in _categorySlugs)
            {
                CategoryDTO? categoryDTO = categoryDTOs.SingleOrDefault(c => c.Slug == categorySlug);
                if (categoryDTO == null)
                    throw new ArgumentException(categorySlug);

                specifiedCategoryDTOs.Add(categoryDTO);
            }

            bool isValid = specifiedCategoryDTOs.Any(sc => sc.Id == projectRequest.CategoryId);
            return isValid
                ? ValidationResult.Success
                : new ValidationResult($"The {validationContext.DisplayName} must be one of {string.Join(", ", specifiedCategoryDTOs.Select(c => c.Name))} categories.");
        }
    }
}
