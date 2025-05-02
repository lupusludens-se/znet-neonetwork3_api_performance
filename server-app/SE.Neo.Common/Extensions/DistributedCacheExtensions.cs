using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SE.Neo.Common.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
            string recordId,
            T data,
            DistributedCacheEntryOptions? options = null)
        {
            var jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles });
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);

            if (jsonData is null)
                return default(T);

            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
