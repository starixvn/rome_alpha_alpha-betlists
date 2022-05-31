namespace Alpha.BetLists.Site.Tests.ViewModels.MoveStatementHistory
{
    using Alpha.BetLists.Site.Requests.MoveStatementHistory;
    using Alpha.BetLists.Site.Tests.Fixtures;
    using Alpha.BetLists.Site.ViewModels.MoveStatementHistory;
    using Alpha.BetLists.Tests.TestData;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "MoveStatementHistory")]
    [Collection("BetListCollection")]
    public class MoveStatementHistoryViewModelTests
    {
        private readonly BetListFixture betListFixture;
        private readonly CompareLogic comparer;

        public MoveStatementHistoryViewModelTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;
            comparer = new CompareLogic();
        }

        [Fact]
        public void AllowToViewSuperPT_ViewerLevelIsSuper_ReturnTrue()
        {
            // Arrange
            betListFixture.User.LevelId.Returns((byte)4);
            var query = new MoveStatementHistoryQuery(betListFixture.User);

            // Act
            var viewModel = new MoveStatementHistoryViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

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
            var query = new MoveStatementHistoryQuery(betListFixture.User);

            // Act
            var viewModel = new MoveStatementHistoryViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            // Assert
            Assert.True(viewModel.AllowToViewMasterPT);
        }
    }
}
