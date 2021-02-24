namespace Alpha.BetLists.Tests.Fixtures
{
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using NSubstitute;

    public class FanexDataFixture
    {
        public FanexDataFixture()
        {
            DynamicRepository = Substitute.For<IDynamicRepository>();
            CacheService = Substitute.For<ICacheService>();
        }

        public IDynamicRepository DynamicRepository { get; }

        public ICacheService CacheService { get; }
    }
}