using FileTypeChecker.Abstracts;
using Microsoft.AspNetCore.Http;
using SE.Neo.Infrastructure.Configs;

namespace SE.Neo.Infrastructure.Services.Interfaces
{
    public interface IFormFileValidationService
    {
        public IFileType GetFileType(IFormFile formFile);

        public bool IsValid(IFormFile formFile, IEnumerable<FileTypeLimitation> fileTypeLimitations, out string? errorMsg);
    }
}
