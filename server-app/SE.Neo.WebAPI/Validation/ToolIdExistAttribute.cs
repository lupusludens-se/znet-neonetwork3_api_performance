using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class ToolIdExistAttribute : ValidationAttribute
    {
        private readonly bool _includeInactive;

        public ToolIdExistAttribute(bool includeInactive = false)
        {
            _includeInactive = includeInactive;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int? id = (int?)value;

            if (!id.HasValue)
            {
                return ValidationResult.Success!;
            }

            if (id.Value <= 0)
            {
                return new ValidationResult($"{validationContext.DisplayName} must be a valid tool id.");
            }

            IToolService service = (IToolService)validationContext.GetService(typeof(IToolService));

            var exist = service.IsToolExist(id.Value, _includeInactive);
            return exist ? ValidationResult.Success! : new ValidationResult($"{validationContext.DisplayName} must be a valid tool id.");
        }
    }
}