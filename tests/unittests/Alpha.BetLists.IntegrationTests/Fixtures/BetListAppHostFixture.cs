using Alpha.IntegrationTests.Fixtures;

namespace Alpha.BetLists.IntegrationTests.Fixtures
{
    public class BetListAppHostFixture : AppHostFixture
    {
        public override string SetWebsiteDirectory() => "src\\Alpha.BetLists.Site";
    }
}
