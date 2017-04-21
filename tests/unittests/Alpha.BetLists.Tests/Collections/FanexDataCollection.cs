namespace Alpha.BetLists.Tests.Collections
{
    using Fixtures;
    using Xunit;

    [CollectionDefinition("FanexDataCollection")]
    public class FanexDataCollection : ICollectionFixture<FanexDataFixture>
    {
    }
}