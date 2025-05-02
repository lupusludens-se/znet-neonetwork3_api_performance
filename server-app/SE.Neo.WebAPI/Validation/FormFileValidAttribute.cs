using SE.Neo.Core.Enums;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Services.Interfaces;
using SE.Neo.WebAPI.Constants;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SE.Neo.WebAPI.Validation
{
    public class FormFileValidAttribute : ValidationAttribute
    {
        public FormFileValidAttribute(string blobTypeProperty)
        {
            if (blobTypeProperty == null)
            {
                throw new ArgumentNullException("blobTypeField");
            }
            BlobTypeProperty = blobTypeProperty;
        }

        public string BlobTypeProperty { get; private set; }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            PropertyInfo? blobTypePropertyInfo = validationContext.ObjectType.GetProperty(BlobTypeProperty);
            if (blobTypePropertyInfo == null)
                return new ValidationResult(ErrorMessages.ModelStateInvalid);

            var formFile = value as IFormFile;
            object? blobTypePropertyValue = blobTypePropertyInfo.GetValue(validationContext.ObjectInstance, null);

            if (formFile == null || blobTypePropertyValue is not BlobType)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            IFormFileValidationService formFileValidationService = (IFormFileValidationService)validationContext.GetService(typeof(IFormFileValidationService))!;
            BlobTypesFilesLimitationsConfig blobTypesFilesLimitationsConfig = (BlobTypesFilesLimitationsConfig)validationContext.GetService(typeof(BlobTypesFilesLimitationsConfig))!;

            string? errorMsg;
            bool isValid = formFileValidationService.IsValid(formFile, blobTypesFilesLimitationsConfig[(BlobType)blobTypePropertyValue], out errorMsg);
            return isValid ? ValidationResult.Success : new ValidationResult(errorMsg);
        }
    }
}
