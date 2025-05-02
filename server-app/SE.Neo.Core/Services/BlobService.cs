using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Models.Media;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services
{
    public class BlobService : IBlobService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public BlobService(ApplicationContext context, ILogger<UserService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<string> CreateBlobAsync(BlobBaseDTO modelDTO)
        {
            try
            {
                EntityEntry<Blob> blob = await _context.Blobs.AddAsync(_mapper.Map<Blob>(modelDTO));
                await _context.SaveChangesAsync();

                return blob.Entity.Name;
            }
            catch (Exception ex)
            {
                throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
            }
        }

        public async Task DeleteBlobAsync(BlobBaseDTO modelDTO)
        {
            try
            {
                _context.Blobs.Remove(_mapper.Map<Blob>(modelDTO));
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CustomException(CoreErrorMessages.ErrorOnRemoving, ex);
            }
        }

        public bool IsBlobExist(string name)
        {
            return _context.Blobs.Any(x => x.Name.ToLower() == name.ToLower());
        }
    }
}
