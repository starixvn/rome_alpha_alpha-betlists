namespace Alpha.BetLists.IntegrationTests.Fixtures
{
    using Alpha.IntegrationTests.Fixtures;

    public class BetListAppHostFixture : AppHostFixture
    {
        public override string SetWebsiteDirectory() => "src\\Alpha.BetLists.Site";
    }
}
