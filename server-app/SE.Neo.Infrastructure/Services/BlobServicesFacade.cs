using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Models.Media;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Services
{
    public class BlobServicesFacade : IBlobServicesFacade
    {
        private readonly IBlobService _blobService;
        private readonly IAzureStorageBlobService _azureStorageBlobService;
        private readonly ILogger<BlobServicesFacade> _logger;

        public BlobServicesFacade(
            IBlobService blobService,
            IAzureStorageBlobService azureStorageBlobService,
            ILogger<BlobServicesFacade> logger)
        {
            _blobService = blobService;
            _azureStorageBlobService = azureStorageBlobService;
            _logger = logger;
        }

        public async Task PopulateWithBlobAsync<T>(T DTO, Func<T, BlobDTO?> blobFieldGetter, Action<T, BlobDTO> blobFieldSetter)
        {
            BlobDTO? blobField = blobFieldGetter(DTO);
            if (blobField != null)
                blobFieldSetter(DTO, await _azureStorageBlobService.GetBlobAsync(blobField!));
        }

        public async Task PopulateWithBlobAsync<T>(IList<T> DTOs, Func<T, BlobDTO?> blobFieldGetter, Action<T, BlobDTO> blobFieldSetter)
        {
            Task<BlobDTO>[] DTOsBlobTasks = new Task<BlobDTO>[DTOs.Count];

            for (int index = 0; index < DTOs.Count; ++index)
                DTOsBlobTasks[index] = blobFieldGetter(DTOs[index]) != null
                    ? _azureStorageBlobService.GetBlobAsync(blobFieldGetter(DTOs[index])!)
                    : Task.FromResult<BlobDTO>(null);

            await Task.WhenAll(DTOsBlobTasks);

            for (int index = 0; index < DTOs.Count; ++index)
                blobFieldSetter(DTOs[index], DTOsBlobTasks[index].Result);
        }

        public async Task<BlobDTO> CreateBlobAsync(Stream stream, BlobBaseDTO modelDTO, bool overwrite = false)
        {
            BlobDTO? blobDTO = await _azureStorageBlobService.UploadBlobAsync(stream, modelDTO, overwrite);
            if (blobDTO == null)
            {
                _logger.LogInformation($"Not able to upload a blob {modelDTO.Name} in the {modelDTO.ContainerName} container.");
                throw new ArgumentException(CoreErrorMessages.ErrorOnSaving);
            }

            if (!overwrite)
            {
                _logger.LogInformation($"A blob {modelDTO.Name} uploaded in the {blobDTO.ContainerName} container under {blobDTO.Name} name.");
                try
                {
                    await _blobService.CreateBlobAsync(blobDTO);
                }
                catch
                {
                    _logger.LogError($"Failed to create an entry for the blob {modelDTO.Name} in the {blobDTO.ContainerName} container under {blobDTO.Name} name. Removing it from the account storage.");
                    await _azureStorageBlobService.DeleteBlobAsync(blobDTO);
                    throw;
                }
            }

            _logger.LogInformation($"A blob {modelDTO.Name} successfully uploaded in the {blobDTO.ContainerName} container under {blobDTO.Name} name and entry saved to the database.");

            return blobDTO;
        }

        public async Task DeleteBlobAsync(BlobBaseDTO modelDTO)
        {
            if (!_azureStorageBlobService.TryInitializeBlobClient(
                modelDTO.Name,
                modelDTO.ContainerName.ToString(),
                out BlobClient blobClient))
            {
                _logger.LogWarning($"Not able to initialize blob client in the container: {modelDTO.ContainerName}.");
                throw new CustomException(CoreErrorMessages.ErrorOnRemoving);
            }

            await _azureStorageBlobService.DeleteBlobAsync(blobClient);
            _logger.LogInformation($"A blob {modelDTO.Name} deleted from the {blobClient.BlobContainerName} container under {blobClient.Name} name.");
            try
            {
                await _blobService.DeleteBlobAsync(modelDTO);
            }
            catch
            {
                _logger.LogError($"Failed to delete an entry for the blob {modelDTO.Name} in the {modelDTO.ContainerName} container. Undeleting it from the account storage.");
                await blobClient.UndeleteAsync();
                throw;
            }

            _logger.LogInformation($"A blob {blobClient.Name} in the {blobClient.BlobContainerName} container successfully deleted and entry erased from the database.");
        }

        public async Task<Stream> DownloadBlobAsync(BlobBaseDTO modelDTO)
        {
            if (!_azureStorageBlobService.TryInitializeBlobClient(
                modelDTO.Name,
                modelDTO.ContainerName.ToString(),
                out BlobClient blobClient))
            {
                _logger.LogWarning($"Not able to initialize blob client in the container: {modelDTO.ContainerName}.");
                throw new CustomException(CoreErrorMessages.ErrorOnRemoving);
            }

            var blobStream = await _azureStorageBlobService.GetBlobStream(modelDTO);
            if (blobStream == null)
            {
                _logger.LogInformation($"Not able to read a blob {modelDTO.Name} in the {modelDTO.ContainerName} container.");
                throw new CustomException(CoreErrorMessages.ErrorOnReading);
            }
            return blobStream;
        }
    }
}
