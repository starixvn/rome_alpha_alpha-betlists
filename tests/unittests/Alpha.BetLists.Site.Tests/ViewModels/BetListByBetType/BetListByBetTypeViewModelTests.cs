namespace Alpha.BetLists.Site.Tests.ViewModels.BetListByBetType
{
    using System.Collections.Generic;
    using BetLists.Helpers;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fanex.RemotingRef;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Requests.BetListByBetType;
    using Site.ViewModels.BetListByBetType;
    using Xunit;

    [Trait("Category", "RunningBetListByBetType")]
    [Collection("BetListCollection")]
    public class BetListByBetTypeViewModelTests
    {
        private readonly CompareLogic compareLogic;
        private readonly BetListFixture betListFixture;

        public BetListByBetTypeViewModelTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;
            compareLogic = new CompareLogic();

            var cacheRef = Substitute.For<IRemotingRefService>();
            cacheRef.GetBetTypeName(Arg.Any<string>(), Arg.Any<string>()).Returns("abc");
            CacheRefServiceHelper.Instance = cacheRef;
        }

        [Fact]
        public void AllowToViewSuperPT_ViewerLevelIsSuper_ReturnTrue()
        {
            // Arrange
            betListFixture.User.LevelId.Returns((byte)4);
            var query = new BetListByBetTypeQuery(betListFixture.User);

            // Act
            var viewModel = new BetListByBetTypeViewModel(
                query,
                new BetListData(BetListDataTest.Tickets, null),
                betListFixture.HtmlBuilder,
                betListFixture.TicketHelper);

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
            var query = new BetListByBetTypeQuery(betListFixture.User);

            // Act
            var viewModel = new BetListByBetTypeViewModel(
                query,
                new BetListData(BetListDataTest.Tickets, null),
                betListFixture.HtmlBuilder,
                betListFixture.TicketHelper);

            // Assert
            Assert.True(viewModel.AllowToViewMasterPT);
        }

        [Fact]
        public void CanViewScoreMap_SupportScoreMapBetTypeIdsIsNull_ReturnsFalse()
        {
            // Arrange
            var ticket = new TicketDto
            {
                BetTypeId = 1
            };

            var query = new BetListByBetTypeQuery(betListFixture.User);
            var viewModel = new BetListByBetTypeViewModel(query,
                new BetListData(BetListDataTest.Tickets, null),
                betListFixture.HtmlBuilder,
                betListFixture.TicketHelper);

            // Act
            var actualCanViewScoreMap = viewModel.CanViewScoreMap(ticket);

            // Assert
            Assert.False(actualCanViewScoreMap);
        }

        [Fact]
        public void CanViewScoreMap_SupportScoreMapBetTypeIdsContainsBetTypeId_ReturnsTrue()
        {
            // Arrange
            var ticket = new TicketDto
            {
                BetTypeId = 1
            };

            var supportScoreMapBetTypeIds = new List<int> { 1, 2, 3 };
            var query = new BetListByBetTypeQuery(betListFixture.User);
            var viewModel = new BetListByBetTypeViewModel(
                query,
                new BetListData(BetListDataTest.Tickets, null),
                betListFixture.HtmlBuilder,
                betListFixture.TicketHelper,
                supportScoreMapBetTypeIds);

            // Act
            var actualCanViewScoreMap = viewModel.CanViewScoreMap(ticket);

            // Assert
            Assert.True(actualCanViewScoreMap);
        }

        [Fact]
        public void CanViewScoreMap_SupportScoreMapBetTypeIdsNotContainsBetTypeId_ReturnsFalse()
        {
            // Arrange
            var ticket = new TicketDto
            {
                BetTypeId = 4
            };

            var supportScoreMapBetTypeIds = new List<int> { 1, 2, 3 };
            var query = new BetListByBetTypeQuery(betListFixture.User);
            var viewModel = new BetListByBetTypeViewModel(
                query,
                new BetListData(BetListDataTest.Tickets, null),
                betListFixture.HtmlBuilder,
                betListFixture.TicketHelper,
                supportScoreMapBetTypeIds);

            // Act
            var actualCanViewScoreMap = viewModel.CanViewScoreMap(ticket);

            // Assert
            Assert.False(actualCanViewScoreMap);
        }
    }
}
