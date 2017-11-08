namespace Alpha.BetLists.IntegrationTests.Controllers
{
    using Fixtures;
    using MvcIntegrationTestFramework.Hosting;
    using Xunit;

    [Collection("BetListAppHostCollection")]
    public class BaseBetListsIntegrationTest
    {
        protected AppHost AppHost;

        public BaseBetListsIntegrationTest(BetListAppHostFixture appHostFixture)
        {
            AppHost = appHostFixture.AppHost;
        }
    }
}
