namespace Alpha.BetLists.Tests.Collections
{
    using Fixtures;
    using Xunit;

    [CollectionDefinition("FanexLibsCollection")]
    public class FanexLibsCollection : ICollectionFixture<FanexDataFixture>
    {
    }
}