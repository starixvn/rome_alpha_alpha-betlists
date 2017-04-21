namespace Alpha.BetLists.Tests.Fixtures
{
    using Fanex.Caching;
    using NSubstitute;

    public class FanexCacheFixture
    {
        public FanexCacheFixture()
        {
            CacheStorage = Substitute.For<Cache>();
            ConfiguredCacheService.SwitchCacheStorage(CacheStorage);
        }

        public Cache CacheStorage { get; }
    }
}