using SE.Neo.WebAPI.Models.Media;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface IBlobApiService
    {
        public Task<BlobResponse?> CreateBlobAsync(IFormFile formFile, BlobRequest model);

        public Task<Stream> DownloadBlobAsync(BlobRequest model);
    }
}
