namespace Alpha.BetLists.Site.Tests.ViewModels.BetListByBetTypeAndMatch
{
    using BetLists.Tests.TestData;
    using Fixtures;
    using NSubstitute;
    using RemotingRef;
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
            var cacheRef = Substitute.For<CacheRefService>();
            cacheRef.GetBetTypeName(Arg.Is(10)).Returns("Outright");
            CacheRefFactory.CacheRef = cacheRef;

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
            var cacheRef = Substitute.For<CacheRefService>();
            cacheRef.GetBetTypeName(Arg.Is(1)).Returns("Handicap");
            cacheRef.GetLeagueName(Arg.Any<int>()).Returns("LeaugeName");
            CacheRefFactory.CacheRef = cacheRef;

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
