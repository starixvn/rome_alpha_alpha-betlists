namespace Alpha.BetLists.Services
{
    using Fanex.Caching;

    public interface RedisCacheService : ICacheService
    {
    }

    public class RedisCacheServiceImpl : CacheService, RedisCacheService
    {
        public RedisCacheServiceImpl(ICache cacheStorage) : base(cacheStorage)
        {
        }
    }
}
