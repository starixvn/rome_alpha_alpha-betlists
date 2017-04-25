namespace Alpha.BetLists.Site.Tests.Collection
{
    using Fixtures;
    using Xunit;

    [CollectionDefinition("BetListCollection")]
    public class BetListCollection : ICollectionFixture<BetListFixture>
    {
    }
}