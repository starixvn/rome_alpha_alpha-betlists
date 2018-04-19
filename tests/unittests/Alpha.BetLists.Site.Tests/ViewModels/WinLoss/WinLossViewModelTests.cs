namespace Alpha.BetLists.Site.Tests.ViewModels.WinLoss
{
    using BetLists.Tests.TestData;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Requests.WinLoss;
    using Site.ViewModels.Blocks;
    using Site.ViewModels.WinLoss;
    using Xunit;

    [Trait("Category", "WinLossBetList")]
    [Collection("BetListCollection")]
    public class WinLossViewModelTests
    {
        private readonly BetListFixture betListFixture;
        private readonly CompareLogic comparer;

        public WinLossViewModelTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;
            comparer = new CompareLogic();
        }

        [Fact]
        public void AllowToViewSuperPT_ViewerLevelIsSuper_ReturnTrue()
        {
            // Arrange
            betListFixture.User.LevelId.Returns((byte)4);
            var query = new WinLossQuery(betListFixture.User);

            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, false);

            // Assert
            Assert.True(viewModel.AllowToViewSuperPT);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public void AllowToViewMasterPT_ViewerLevelIsMasterOrSuper_ReturnTrue(byte viewerLevelId)
        {
            // Arrange
            betListFixture.User.LevelId.Returns(viewerLevelId);
            var query = new WinLossQuery(betListFixture.User);

            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, false);

            // Assert
            Assert.True(viewModel.AllowToViewMasterPT);
        }

        [Fact]
        public void Footer_Always_SetInCtor()
        {
            // Arrange
            var query = new WinLossQuery(betListFixture.User);
            var expectedFooter = new Footer(BetListDataTest.BetListData, query.ViewerLevel, false);

            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, false);

            // Assert
            Assert.True(comparer.Compare(expectedFooter, viewModel.Footer).AreEqual);
        }
    }
}