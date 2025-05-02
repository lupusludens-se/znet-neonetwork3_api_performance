using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System.ComponentModel.DataAnnotations;
namespace SE.Neo.WebAPI.Validation
{
    public class PatchWhiteListAttribute : ValidationAttribute
    {
        private readonly string[] _whiteList;

        public PatchWhiteListAttribute(string[] whiteList)
        {
            _whiteList = whiteList.Select(str => str.ToLower()).ToArray();
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("No patch sent.");
            var patchDoc = value as JsonPatchDocument;
            if (patchDoc == null)
                return new ValidationResult("Invalid patch document.");
            Operation currentOp;
            try
            {
                currentOp = patchDoc.Operations.SingleOrDefault();
                if (currentOp == null)
                {
                    return new ValidationResult("No operation sent.");
                }
                if (!_whiteList.Contains(currentOp.path.ToLower()))
                {
                    return new ValidationResult($"{currentOp.path} is not allowed.");
                }
            }
            catch (InvalidOperationException)
            {
                return new ValidationResult("Only one patch operation at a time.");
            }
            return ValidationResult.Success;
        }
    }
}
