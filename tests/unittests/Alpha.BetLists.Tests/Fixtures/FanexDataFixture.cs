namespace Alpha.BetLists.Tests.Fixtures
{
    using Alpha.BetLists.DataAccesses;
    using Alpha.BetLists.Services;
    using AlphaPlus.PartialUM.Services;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using NSubstitute;

    public class FanexDataFixture
    {
        public FanexDataFixture()
        {
            DynamicRepository = Substitute.For<IDynamicRepository>();
            CacheService = Substitute.For<ICacheService>();
            PartialUMService = Substitute.For<IPartialUMService>();
            ReportDateService = Substitute.For<ReportDateService>();
            BetListRepository = Substitute.For<BetListRepository>();
        }

        public ReportDateService ReportDateService;

        public IDynamicRepository DynamicRepository { get; }

        public ICacheService CacheService { get; }

        public IPartialUMService PartialUMService { get; }

        public IBetListRepository BetListRepository { get; }
    }
}