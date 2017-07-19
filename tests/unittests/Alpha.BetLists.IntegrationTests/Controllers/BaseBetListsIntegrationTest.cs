using Alpha.BetLists.IntegrationTests.Fixtures;
using MvcIntegrationTestFramework.Hosting;
using Xunit;

namespace Alpha.BetLists.IntegrationTests.Controllers
{
    [Collection("AppHostCollection")]
    public class BaseBetListsIntegrationTest
    {
        protected AppHost AppHost;

        public BaseBetListsIntegrationTest(AppHostFixture appHostFixture)
        {
            AppHost = appHostFixture.AppHost;
        }
    }
}
