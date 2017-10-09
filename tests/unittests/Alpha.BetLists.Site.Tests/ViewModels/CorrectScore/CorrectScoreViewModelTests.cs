namespace Alpha.BetLists.Site.Tests.ViewModels.CorrectScore
{
    using BetLists.Constants;
    using BetLists.Tests.TestData;
    using Fixtures;
    using NSubstitute;
    using RemotingRef;
    using Requests.CorrectScore;
    using Site.ViewModels.CorrectScore;
    using Xunit;

    [Trait("Category", "CorrectScore")]
    [Collection("BetListCollection")]
    public class CorrectScoreViewModelTests
    {
        private readonly BetListFixture betListFixture;

        public CorrectScoreViewModelTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;
        }

        [Fact]
        public void Constructor_Always_ReturnsCorrectTitle()
        {
            // Arrange
            var cacheRef = Substitute.For<CacheRefService>();
            cacheRef.GetBetTypeName(Arg.Is(BetTypeIdConstants.CorrectScore)).Returns("Correct Score");
            CacheRefFactory.CacheRef = cacheRef;

            var message = new CorrectScoreQuery(betListFixture.User);

            // Act
            var viewModel = new CorrectScoreViewModel(
                message,
                BetListDataTest.BetListData,
                betListFixture.HtmlBuilder,
                betListFixture.TicketHelper);

            // Assert
            Assert.Equal("CORRECT SCORE", viewModel.Title);
        }
    }
}
