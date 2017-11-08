namespace Alpha.BetLists.IntegrationTests.Collections
{
    using Fixtures;
    using Xunit;

    [CollectionDefinition("BetListAppHostCollection")]
    public class BetListAppHostCollection : ICollectionFixture<BetListAppHostFixture>
    {
    }
}
