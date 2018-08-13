namespace Alpha.BetLists.Site.Tests.ViewModels.CombinationMixParlay
{
    using BetLists.Constants;
    using BetLists.Helpers;
    using BetLists.Tests.TestData;
    using Fanex.RemotingRef;
    using Fixtures;
    using NSubstitute;
    using Requests.CombinationMixParlay;
    using Site.ViewModels.CombinationMixParlay;
    using Xunit;

    [Trait("Category", "CombinationMixParlay")]
    [Collection("BetListCollection")]
    public class CombinationMixParlayViewModelTests
    {
        private readonly BetListFixture betListFixture;

        public CombinationMixParlayViewModelTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;
        }

        [Fact]
        public void Constructor_Always_ReturnsCorrectTitle()
        {
            // Arrange
            var cacheRef = Substitute.For<IRemotingRefService>();
            cacheRef.GetBetTypeName(BetTypeIdConstants.CombinationMixParlay.ToString(), Arg.Any<string>()).Returns("Correct Score");
            CacheRefServiceHelper.Instance = cacheRef;

            var message = new CombinationMixParlayQuery(betListFixture.User);

            // Act
            var viewModel = new CombinationMixParlayViewModel(
                message,
                BetListDataTest.BetListData,
                betListFixture.HtmlBuilder,
                betListFixture.TicketHelper);

            // Assert
            Assert.Equal("PARLAY", viewModel.Title);
        }
    }
}
