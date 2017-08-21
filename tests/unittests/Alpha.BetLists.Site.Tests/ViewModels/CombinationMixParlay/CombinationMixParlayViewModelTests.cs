namespace Alpha.BetLists.Site.Tests.ViewModels.CombinationMixParlay
{
    using BetLists.Tests.TestData;
    using Fixtures;
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
            var message = new CombinationMixParlayQuery(betListFixture.User);

            // Act
            var viewModel = new CombinationMixParlayViewModel(
                message,
                BetListDataTest.BetListData,
                betListFixture.HtmlBuilder,
                betListFixture.TicketHelper);

            // Assert
            Assert.Equal("SYSTEM PARLAY", viewModel.Title);
        }
    }
}
