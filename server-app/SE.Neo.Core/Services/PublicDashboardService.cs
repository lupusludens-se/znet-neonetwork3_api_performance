using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models.Public;
using SE.Neo.Core.Configs;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PublicDashboardService : BaseService, IPublicDashboardService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<PublicDashboardService> _logger;
        private readonly IMapper _mapper;
        protected readonly IDistributedCache _cache;
        private readonly MemoryCacheTimeStamp _memoryCacheTimeStamp;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="memoryCacheTimeStamp"></param>
        /// <param name="cache"></param>
        /// <param name="logger"></param>
        public PublicDashboardService(ApplicationContext context, IMapper mapper,
             IOptions<MemoryCacheTimeStamp> memoryCacheTimeStamp, IDistributedCache cache, ILogger<PublicDashboardService> logger) : base(cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
            _memoryCacheTimeStamp = memoryCacheTimeStamp.Value;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IList<DiscoverabilityProjectsDataDTO>> GetProjectsDataForDiscoverability()
        {
            var discoverabilityProjects = await _cache.GetRecordAsync<List<DiscoverabilityProjectsData>>(CoreCacheKeys.DiscoverabilityProjectsDataContext);
            if (discoverabilityProjects is null)
            {
                discoverabilityProjects = await _context.DiscoverabilityProjectsData.AsNoTracking().ToListAsync();
                await _cache.SetRecordAsync(CoreCacheKeys.DiscoverabilityProjectsDataContext, discoverabilityProjects, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_memoryCacheTimeStamp.Long)
                });
            }
            return discoverabilityProjects.Select(_mapper.Map<DiscoverabilityProjectsDataDTO>).ToList();
        }
    }
}