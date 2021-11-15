namespace Alpha.BetLists.Tests.Fixtures
{
    using Alpha.BetLists.Services;
    using Alpha.Web.Helpers;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using NSubstitute;

    public class FanexDataFixture
    {
        public FanexDataFixture()
        {
            DynamicRepository = Substitute.For<IDynamicRepository>();
            CacheService = Substitute.For<ICacheService>();
            PartialUMService = Substitute.For<PartialUMService>();
            ReportDateService = Substitute.For<ReportDateService>();
        }

        public ReportDateService ReportDateService;

        public IDynamicRepository DynamicRepository { get; }

        public ICacheService CacheService { get; }

        public PartialUMService PartialUMService { get; }
    }
}