using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using SE.Neo.Common.Models.Media;

namespace SE.Neo.Infrastructure.Services.Interfaces
{
    public interface IAzureStorageBlobService
    {
        bool TryInitializeBlobClient(string blobName, string? blobContainerName, out BlobClient blobClient);

        Task<BlobDTO> GetBlobAsync(BlobBaseDTO blobBaseDTO, BlobSasPermissions blobContainerSasPermissions = BlobSasPermissions.Read);
        Task<BlobDTO?> GetBlobAsync(BlobClient blobClient, BlobSasPermissions blobContainerSasPermissions = BlobSasPermissions.Read);
        Task<BlobDTO?> UploadBlobAsync(Stream stream, BlobBaseDTO blobBaseDTO, bool overwrite = false);
        Task<BlobDTO?> UploadBlobAsync(Stream stream, BlobClient blobClient, bool overwrite = false);
        Task<bool> DeleteBlobAsync(BlobBaseDTO blobBaseDTO);
        Task<bool> DeleteBlobAsync(BlobClient blobClient);
        Task<Stream> GetBlobStream(BlobBaseDTO blobBaseDTO);
    }
}
