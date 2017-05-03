namespace Alpha.BetLists.Site.Tests.Collection
{
    using Fixtures;
    using Xunit;

    [CollectionDefinition("ParlayBetListCollection")]
    public class ParlayBetListCollection : ICollectionFixture<BetListFixture>, ICollectionFixture<ParlayServiceFixture>
    {
    }
}