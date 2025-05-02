using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class MessageAttachmentTypeAttribute : ValidationAttribute
    {
        private readonly AttachmentType[] _allowedTypes;

        public MessageAttachmentTypeAttribute(params AttachmentType[] allowedTypes)
        {
            _allowedTypes = allowedTypes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IEnumerable<MessageAttachmentRequest>? attachment = value as IEnumerable<MessageAttachmentRequest>;
            if (attachment == null)
            {
                return ValidationResult.Success!;
            }

            if (!attachment.All(a => _allowedTypes.Contains(a.Type)))
            {
                return new ValidationResult($"{validationContext.DisplayName} must contain valid attachment types. Allowed types: {string.Join(',', _allowedTypes)}.");
            }

            return ValidationResult.Success!;
        }
    }
}
