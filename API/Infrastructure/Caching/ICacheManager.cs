namespace API.Infrastructure.Caching
{
    public interface ICacheManager
    {
        Task<T?> TryGetAsync<T>(string key);
        Task<bool> TrySetAsync<T>(string key, T entry);
    }
}