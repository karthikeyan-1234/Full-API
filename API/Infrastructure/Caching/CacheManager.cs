using API.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;


namespace API.Infrastructure.Caching
{
    public class CacheManager : ICacheManager
    {
        IDistributedCache cache;
        ISessionService sessionService;
        DistributedCacheEntryOptions options;
        ILogger<CacheManager> logger;

        public CacheManager(IDistributedCache cache, ISessionService sessionService, ILogger<CacheManager> logger)
        {
            this.cache = cache;
            options = new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20) };
            this.logger = logger;
            this.sessionService = sessionService;
        }

        public async Task<bool> TrySetAsync<T>(string key, T entry)
        {
            try
            {
                var user = sessionService?.GetString("user");
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
                var user = sessionService?.GetString("user");
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
