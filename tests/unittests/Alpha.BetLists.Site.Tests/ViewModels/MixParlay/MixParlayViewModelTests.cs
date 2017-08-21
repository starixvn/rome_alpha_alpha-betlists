namespace Alpha.BetLists.Site.Tests.ViewModels.MixParlay
{
    using BetLists.Tests.TestData;
    using Fixtures;
    using Requests.MixParlay;
    using Site.ViewModels.MixParlay;
    using Xunit;

    [Trait("Category", "MixParlay")]
    [Collection("BetListCollection")]
    public class MixParlayViewModelTests
    {
        private readonly BetListFixture betListFixture;

        public MixParlayViewModelTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;
        }

        [Fact]
        public void Constructor_Always_ReturnsCorrectTitle()
        {
            // Arrange
            var message = new MixParlayQuery(betListFixture.User);

            // Act
            var viewModel = new MixParlayViewModel(
                message,
                BetListDataTest.BetListData,
                betListFixture.HtmlBuilder,
                betListFixture.TicketHelper);

            // Assert
            Assert.Equal("MIX PARLAY", viewModel.Title);
        }
    }
}
