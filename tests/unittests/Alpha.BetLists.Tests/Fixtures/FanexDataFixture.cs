namespace Alpha.BetLists.Tests.Fixtures
{
    using Fanex.Data.Repository;
    using NSubstitute;

    public class FanexDataFixture
    {
        public FanexDataFixture()
        {
            DynamicRepository = Substitute.For<IDynamicRepository>();
        }

        public IDynamicRepository DynamicRepository { get; set; }
    }
}