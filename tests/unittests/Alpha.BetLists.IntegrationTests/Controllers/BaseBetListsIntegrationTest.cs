using Alpha.BetLists.IntegrationTests.Fixtures;
using MvcIntegrationTestFramework.Hosting;
using Xunit;

namespace Alpha.BetLists.IntegrationTests.Controllers
{
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
