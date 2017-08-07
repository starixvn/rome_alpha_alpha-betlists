using Alpha.BetLists.IntegrationTests.Fixtures;
using Xunit;

namespace Alpha.BetLists.IntegrationTests.Collections
{
    [CollectionDefinition("BetListAppHostCollection")]
    public class BetListAppHostCollection : ICollectionFixture<BetListAppHostFixture>
    {
    }
}
