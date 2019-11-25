namespace Alpha.BetLists.Tests.DataAccesses
{
    using Alpha.BetLists.DataAccesses;
    using Alpha.BetLists.Tests.Fixtures;
    using BetLists.Criteria.BetBuilder;
    using Fanex.BetDetails.BetBuilder.Criteria;
    using Fanex.BetDetails.BetBuilder.Model;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "BetBuilder")]
    [Collection("FanexDataCollection")]
    public class BetBuilderDataAccessTests
    {
        private readonly FanexDataFixture dataFixture;
        private readonly BetBuilderDataAccess dataAccess;

        public BetBuilderDataAccessTests(FanexDataFixture dataFixture)
        {
            this.dataFixture = dataFixture;
            dataAccess = new BetBuilderDataAccess(dataFixture.DynamicRepository);
        }

        [Fact]
        public void GetCasinoDetails_Always_DynamicRepositoryCallFetch()
        {
            // Arrange
            var criteria = new GetBetBuilderDetailParameter();

            // Act
            dataAccess.GetBetBuilderDetail(criteria);

            // Assert
            dataFixture.DynamicRepository.Received().Fetch<BetBuilderDetailModel>(Arg.Any<GetBetBuilderDetailCriteria>());
        }
    }
}
