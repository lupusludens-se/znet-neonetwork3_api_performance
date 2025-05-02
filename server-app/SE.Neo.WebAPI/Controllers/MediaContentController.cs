using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SE.Neo.Common;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Models.Media;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Controllers
{
    [ApiController]
    [Route("api/media")]
    public class MediaContentController : ControllerBase
    {
        private readonly IBlobApiService _blobApiService;
        private readonly ILogger<MediaContentController> _logger;

        public MediaContentController(
            IBlobApiService blobApiService,
            ILogger<MediaContentController> logger)
        {
            _blobApiService = blobApiService;
            _logger = logger;
        }

        //TODO: Uploaded files supposed to be quarantined and then scanned by an antivirus service.
        /// <summary>
        /// Uploads file to blob.
        /// </summary>
        [HttpPost]
        [Route("blob")]
        [Authorize, Active]
        public async Task<IActionResult> Upload([FromQuery] BlobRequest blobRequest)
        {
            _logger.LogInformation($"Attempt to upload {blobRequest.File.Length} bytes of blob {blobRequest.File.FileName} into the container {blobRequest.BlobType}.");

            if(blobRequest.BlobType == BlobType.Companies && blobRequest.IsLogoOnlyAllowed && !blobRequest.File.ContentType.ToLower().Contains(ZnConstants.Image.ToLower()))
            {
                return BadRequest(CoreErrorMessages.OnlyLogoAccepted);
            }

            BlobResponse? blobResponse = await _blobApiService.CreateBlobAsync(blobRequest.File, blobRequest);

            return Ok(blobResponse);
        }

        /// <summary>
        /// Downloads a blob from the specified container.
        /// </summary>
        /// <param name="blobType">The type of the blob container.</param>
        /// <param name="blobName">The name of the blob to download.</param>
        /// <returns>The file stream of the downloaded blob.</returns>
        [HttpPost]
        [Route("blob/download/{blobType}/{blobName}")]
        [Authorize, Active]
        public async Task<IActionResult> Download(BlobType blobType, string blobName)
        {
            BlobRequest blobRequest = new BlobRequest()
            {
                BlobType = blobType,
                BlobName = blobName
            };
            var contentType = ZnConstants.GenericContentType;
            var fileStream = await _blobApiService.DownloadBlobAsync(blobRequest);
            return File(fileStream, contentType, blobName);
        }
    }
}
