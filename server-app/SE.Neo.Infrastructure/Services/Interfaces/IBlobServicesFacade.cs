using SE.Neo.Common.Models.Media;

namespace SE.Neo.Infrastructure.Services.Interfaces
{
    public interface IBlobServicesFacade
    {
        Task PopulateWithBlobAsync<T>(T DTO, Func<T, BlobDTO?> blobFieldGetter, Action<T, BlobDTO> blobFieldSetter);
        Task PopulateWithBlobAsync<T>(IList<T> DTOs, Func<T, BlobDTO?> blobFieldGetter, Action<T, BlobDTO> blobFieldSetter);
        Task<BlobDTO> CreateBlobAsync(Stream stream, BlobBaseDTO modelDTO, bool overwrite = false);
        Task DeleteBlobAsync(BlobBaseDTO modelDTO);
        Task<Stream> DownloadBlobAsync(BlobBaseDTO modelDTO);
    }
}
