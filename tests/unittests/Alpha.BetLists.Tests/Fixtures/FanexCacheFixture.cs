namespace Alpha.BetLists.Tests.Fixtures
{
    using Fanex.Caching;
    using NSubstitute;

    public class FanexCacheFixture
    {
        public FanexCacheFixture()
        {
            CacheStorage = Substitute.For<Cache>();
#pragma warning disable CS0618 // Type or member is obsolete
            ConfiguredCacheService.SwitchCacheStorage(CacheStorage);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public Cache CacheStorage { get; }
    }
}