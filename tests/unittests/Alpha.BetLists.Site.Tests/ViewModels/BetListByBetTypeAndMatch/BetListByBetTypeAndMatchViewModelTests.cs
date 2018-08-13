namespace Alpha.BetLists.Site.Tests.ViewModels.BetListByBetTypeAndMatch
{
    using BetLists.Helpers;
    using BetLists.Tests.TestData;
    using Fanex.RemotingRef;
    using Fixtures;
    using NSubstitute;
    using Requests.BetListByBetTypeAndMatch;
    using Site.ViewModels.BetListByBetTypeAndMatch;
    using Xunit;

    [Trait("Category", "BetListByBetTypeAndMatch")]
    [Collection("BetListCollection")]
    public class BetListByBetTypeAndMatchViewModelTests
    {
        private readonly BetListFixture betListFixture;
        private readonly BetListByBetTypeAndMatchQuery query;

        public BetListByBetTypeAndMatchViewModelTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;
            query = new BetListByBetTypeAndMatchQuery(betListFixture.User);
        }

        [Fact]
        public void Constructor_BetTypeIsOutright_ReturnsOutrightTitle()
        {
            // Arrange
            var cacheRef = Substitute.For<IRemotingRefService>();
            cacheRef.GetBetTypeName("10", Arg.Any<string>()).Returns("Outright");
            CacheRefServiceHelper.Instance = cacheRef;

            query.BetType = 10;
            var viewModel = new BetListByBetTypeAndMatchViewModel(
                query,
                BetListDataTest.BetListData,
                betListFixture.HtmlBuilder,
                betListFixture.TicketHelper);

            // Act
            var actualTitle = viewModel.Title;

            // Assert
            Assert.Equal("OUTRIGHT", actualTitle);
        }

        [Fact]
        public void Constructor_BetTypeIsNotOutright_ReturnsCorrectTitle()
        {
            // Arrange
            var cacheRef = Substitute.For<IRemotingRefService>();
            cacheRef.GetBetTypeName("1", Arg.Any<string>()).Returns("Handicap");
            cacheRef.GetLeagueName(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns("LeaugeName");
            CacheRefServiceHelper.Instance = cacheRef;

            query.BetType = 1;
            var viewModel = new BetListByBetTypeAndMatchViewModel(
                query,
                BetListDataTest.BetListData,
                betListFixture.HtmlBuilder,
                betListFixture.TicketHelper);

            // Act
            var actualTitle = viewModel.Title;

            // Assert
            Assert.Equal("LEAUGENAME HANDICAP", actualTitle);
        }
    }
}
