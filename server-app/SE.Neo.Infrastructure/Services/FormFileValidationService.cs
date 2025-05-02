using FileTypeChecker;
using FileTypeChecker.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SE.Neo.Core.Constants;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Services
{
    public class FormFileValidationService : IFormFileValidationService
    {
        private readonly ILogger<FormFileValidationService> _logger;

        public FormFileValidationService(ILogger<FormFileValidationService> logger)
        {
            _logger = logger;
        }

        public IFileType GetFileType(IFormFile formFile)
        {
            using (Stream stream = formFile.OpenReadStream())
            {
                if (!FileTypeValidator.IsTypeRecognizable(stream))
                {
                    _logger.LogWarning($"File {formFile.FileName} type is not recognizable.");
                    throw new InvalidDataException(CoreErrorMessages.InvalidFileSignature);
                }

                return FileTypeValidator.GetFileType(stream);
            }
        }

        public bool IsValid(IFormFile formFile, IEnumerable<FileTypeLimitation> fileTypeLimitations, out string? errorMsg)
        {
            IFileType fileType = GetFileType(formFile);

            FileTypeLimitation? fileTypeLimitation = fileTypeLimitations
                .FirstOrDefault(ftl => ftl.Extensions.Any(e => e.Contains(fileType.Extension)));

            if (fileTypeLimitation == null)
            {
                errorMsg = $"Invalid file type. Valid file types have the extensions {String.Join(',', fileTypeLimitations.SelectMany(ftl => ftl.Extensions))}";
                _logger.LogWarning($"File {formFile.FileName} of .{fileType.Extension} ({fileType.Name}) type not supported.");
                return false;
            }

            if (formFile.Length > fileTypeLimitation.Size)
            {
                errorMsg = $"File is too large, maximum file size is {fileTypeLimitation.Size}";
                _logger.LogWarning($"File {formFile.FileName} exceeds size limit.");
                return false;
            }
            errorMsg = null;
            return true;
        }
    }
}
