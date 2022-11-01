using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;


namespace API.Infrastructure.Caching
{
    public class CacheManager : ICacheManager
    {
        IDistributedCache cache;
        IHttpContextAccessor accessor;
        DistributedCacheEntryOptions options;
        ILogger<CacheManager> logger;

        public CacheManager(IDistributedCache cache, IHttpContextAccessor accessor, ILogger<CacheManager> logger)
        {
            this.cache = cache;
            options = new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20) };
            this.accessor = accessor;
            this.logger = logger;
        }

        public async Task<bool> TrySetAsync<T>(string key, T entry)
        {
            try
            {
                var session = accessor.HttpContext?.Session;
                var user = session?.GetString("user");
                string json = JsonSerializer.Serialize(entry);
                await cache.SetStringAsync(user + "-" + key,json,options);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Error in getting Redis Cache" + ex.Message);
                return false;
            }
        }

        public async Task<T?> TryGetAsync<T>(string key)
        {
            try
            {
                var session = accessor.HttpContext?.Session;
                var user = session?.GetString("user");
                var json = await cache.GetStringAsync(user + "-" + key);
                if(json != null)
                    return JsonSerializer.Deserialize<T>(json);
                return default(T);
            }
            catch (Exception ex)
            {
                logger.LogError("Error in getting Redis Cache" + ex.Message);
                return default;
            }
        }
    }
}
