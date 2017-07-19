using Alpha.BetLists.IntegrationTests.Fixtures;
using Xunit;

namespace Alpha.BetLists.IntegrationTests.Collections
{
    [CollectionDefinition("AppHostCollection")]
    public class AppHostCollection : ICollectionFixture<AppHostFixture>
    {
    }
}
