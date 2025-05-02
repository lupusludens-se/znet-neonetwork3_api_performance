using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SE.Neo.Common.Models.Media;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Services
{
    public class AzureStorageBlobService : IAzureStorageBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly AzureStorageConfig _azureStorageConfig;
        private readonly ILogger<AzureStorageBlobService> _logger;

        public AzureStorageBlobService(IOptions<AzureStorageConfig> azureStorageConfig, ILogger<AzureStorageBlobService> logger)
        {
            _azureStorageConfig = azureStorageConfig.Value;
            _blobServiceClient = new BlobServiceClient(_azureStorageConfig.ConnectionString);
            _logger = logger;
        }

        private async Task<bool> IsBlobExists(BlobClient blobClient)
        {
            bool exists = await blobClient.ExistsAsync();

            if (!exists)
            {
                _logger.LogWarning($"A blob {blobClient.Name} is not found in the {blobClient.BlobContainerName} container.");
                return exists;
            }

            return exists;
        }

        public bool TryInitializeBlobClient(string blobName, string? blobContainerName, out BlobClient blobClient)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName?.ToLower() ?? _azureStorageConfig.DefaultBlobContainer);
            Response<bool> response = blobContainerClient.Exists();

            if (!response.Value)
            {
                _logger.LogWarning($"A specified container does not exist: {blobContainerName}");

                blobClient = null;
                return response.Value;
            }

            // Get a reference to a blob
            blobClient = blobContainerClient.GetBlobClient(blobName);
            return response.Value;
        }

        public async Task<BlobDTO> GetBlobAsync(BlobBaseDTO blobBaseDTO, BlobSasPermissions blobContainerSasPermissions = BlobSasPermissions.Read)
        {
            if (!TryInitializeBlobClient(blobBaseDTO.Name, blobBaseDTO.ContainerName, out BlobClient blobClient))
            {
                _logger.LogWarning($"Not able to initialize blob client in the container: {blobBaseDTO.ContainerName}.");
                return null;
            }

            return await GetBlobAsync(blobClient, blobContainerSasPermissions);
        }

        public async Task<BlobDTO?> GetBlobAsync(BlobClient blobClient, BlobSasPermissions blobContainerSasPermissions = BlobSasPermissions.Read)
        {

            if (!await IsBlobExists(blobClient))
                return null;

            Uri generatedSasUri = blobClient.GenerateSasUri(blobContainerSasPermissions, DateTime.UtcNow.AddDays(1));

            return new BlobDTO()
            {
                Name = blobClient.Name,
                ContainerName = blobClient.BlobContainerName,
                Uri = new Uri(_azureStorageConfig.CdnEndpoint, generatedSasUri.PathAndQuery)
            };
        }

        public async Task<BlobDTO?> UploadBlobAsync(Stream stream, BlobBaseDTO blobBaseDTO, bool overwrite = false)
        {
            _logger.LogInformation($"Attempt to upload {stream.Length} bytes of {blobBaseDTO.Name ?? "unnamed"} blob into the {blobBaseDTO.ContainerName} container. Overwrite flag set: {overwrite}.");

            if (!overwrite)
            {
                string newBlobName = Guid.NewGuid().ToString() + Path.GetExtension(blobBaseDTO.Name);
                _logger.LogInformation($"A {(string.IsNullOrEmpty(blobBaseDTO.Name) ? "unnamed" : blobBaseDTO.Name)} blob renamed as {newBlobName} and excpected in the {blobBaseDTO.ContainerName} container.");
                blobBaseDTO.Name = newBlobName;
            }

            if (!TryInitializeBlobClient(blobBaseDTO.Name, blobBaseDTO.ContainerName, out BlobClient blobClient))
                return null;

            //True flag for overwrite is to avoid initializing blob client again.
            return await UploadBlobAsync(stream, blobClient, true);
        }

        public async Task<BlobDTO?> UploadBlobAsync(Stream stream, BlobClient blobClient, bool overwrite = false)
        {
            _logger.LogInformation($"Attempt to upload {stream.Length} bytes of blob {blobClient.Name} into the {blobClient.BlobContainerName} container. Overwrite flag set: {overwrite}.");

            if (!overwrite)
            {
                string newBlobName = Guid.NewGuid().ToString() + Path.GetExtension(blobClient.Name);

                if (!TryInitializeBlobClient(newBlobName, blobClient.BlobContainerName, out BlobClient newBlobClient))
                    return null;

                _logger.LogInformation($"A blob {blobClient.Name} renamed as {newBlobClient.Name} in the {blobClient.BlobContainerName} container.");

                blobClient = newBlobClient;
            }

            await blobClient.UploadAsync(stream, overwrite);

            _logger.LogInformation($"A blob {blobClient.Name} successfully uploaded into the {blobClient.BlobContainerName} container.");

            return await GetBlobAsync(blobClient);
        }

        public async Task<bool> DeleteBlobAsync(BlobBaseDTO blobBaseDTO)
        {
            if (!TryInitializeBlobClient(blobBaseDTO.Name, blobBaseDTO.ContainerName, out BlobClient blobClient))
            {
                _logger.LogWarning($"Not able to initialize blob client in the container: {blobBaseDTO.ContainerName}.");
                return false;
            }

            return await DeleteBlobAsync(blobClient);
        }

        public async Task<bool> DeleteBlobAsync(BlobClient blobClient)
        {
            Response<bool> response = await blobClient.DeleteIfExistsAsync();

            _logger.LogInformation($"Attempt to delete a blob {blobClient.Name} in the {blobClient.BlobContainerName} container was successful: {response.Value}.");

            return response.Value;
        }

        public async Task<Stream> GetBlobStream(BlobBaseDTO blobBaseDTO)
        {
            if (!TryInitializeBlobClient(blobBaseDTO.Name, blobBaseDTO.ContainerName, out BlobClient blobClient))
            {
                _logger.LogWarning($"Not able to initialize blob client in the container: {blobBaseDTO.ContainerName}.");
                return null;
            }
            return await blobClient.OpenReadAsync();
        }
    }
}
