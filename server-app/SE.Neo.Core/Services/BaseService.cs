using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SE.Neo.Common.Extensions;
using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Services
{
    public abstract class BaseService : BaseFilterService
    {
        protected readonly IDistributedCache _cache;

        public BaseService(IDistributedCache cache)
        {
            _cache = cache;
        }

        protected async Task<bool> IsIdExistAsync<T>(int id, DbSet<T> dbSet, string cacheName, int cacheTime) where T : BaseIdEntity
        {
            var items = await _cache.GetRecordAsync<List<T>>(cacheName);
            if (items == null)
            {
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTime));

                items = dbSet.ToList();
                await _cache.SetRecordAsync(cacheName, items, options);
            }
            return items.Any(p => p.Id.Equals(id));
        }
    }
}