using AutoMapper;
using SE.Neo.Common.Models.Media;
using SE.Neo.Infrastructure.Services.Interfaces;
using SE.Neo.WebAPI.Models.Media;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Services
{
    public class BlobApiService : IBlobApiService
    {
        private readonly IBlobServicesFacade _blobServicesFacade;
        private readonly ILogger<BlobApiService> _logger;
        private readonly IMapper _mapper;

        public BlobApiService(
            IBlobServicesFacade blobServicesFacade,
            ILogger<BlobApiService> logger,
            IMapper mapper)
        {
            _blobServicesFacade = blobServicesFacade;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BlobResponse?> CreateBlobAsync(IFormFile formFile, BlobRequest blobRequest)
        {
            BlobBaseDTO blobBaseDTO = _mapper.Map<BlobBaseDTO>(blobRequest);
            blobBaseDTO.Name = String.IsNullOrWhiteSpace(blobRequest.NewFileName) ? (blobRequest.Overwrite ? blobRequest.BlobName : formFile.FileName) : blobRequest.NewFileName;

            BlobDTO resultBlobDTO = null;
            using (Stream stream = formFile.OpenReadStream())
                resultBlobDTO = await _blobServicesFacade.CreateBlobAsync(stream, blobBaseDTO, blobRequest.Overwrite);

            return _mapper.Map<BlobResponse>(resultBlobDTO);
        }

        public async Task<Stream> DownloadBlobAsync(BlobRequest blobRequest)
        {
            BlobBaseDTO blobBaseDTO = _mapper.Map<BlobBaseDTO>(blobRequest);
            return await _blobServicesFacade.DownloadBlobAsync(blobBaseDTO);
        }
    }
}
