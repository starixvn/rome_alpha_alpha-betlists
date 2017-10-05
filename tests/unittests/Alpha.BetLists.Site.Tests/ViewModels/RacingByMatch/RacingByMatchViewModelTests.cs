namespace Alpha.BetLists.Site.Tests.ViewModels.RacingByMatch
{
    using System.Collections.Generic;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fixtures;
    using Requests.RacingByMatch;
    using Site.ViewModels.RacingByMatch;
    using Xunit;

    [Trait("Category", "RacingByMatch")]
    [Collection("BetListCollection")]
    public class RacingByMatchViewModelTests
    {
        private readonly BetListFixture betListFixture;
        private readonly RacingByMatchQuery query;

        public RacingByMatchViewModelTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;
            query = new RacingByMatchQuery(betListFixture.User);
        }


        [Fact]
        public void Constructor_Always_ReturnsCorrectTitle()
        {
            // Arrange
            var viewModel = new RacingByMatchViewModel(
                query,
                new BetListData(BetListDataTest.Tickets, null),
                betListFixture.HtmlBuilder,
                betListFixture.TicketHelper);

            // Act
            var actualTitle = viewModel.Title;

            // Assert
            Assert.Equal("RACING", actualTitle);
        }

        [Fact]
        public void CanViewScoreMap_SupportScoreMapBetTypeIdsContainsBetTypeId_ReturnsTrue()
        {
            // Arrange
            var ticket = new TicketDto
            {
                BetTypeId = 1,
                Status = "running"
            };

            var supportScoreMapBetTypeIds = new List<int> { 1, 2, 3 };
            var viewModel = new RacingByMatchViewModel(
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
                BetTypeId = 4,
                Status = "running"
            };

            var supportScoreMapBetTypeIds = new List<int> { 1, 2, 3 };
            var viewModel = new RacingByMatchViewModel(
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

        [Fact]
        public void CanViewScoreMap_TicketStatusIsNotRunning_ReturnsFalse()
        {
            // Arrange
            var ticket = new TicketDto
            {
                BetTypeId = 4,
                Status = "void"
            };

            var supportScoreMapBetTypeIds = new List<int> { 1, 2, 3 };
            var viewModel = new RacingByMatchViewModel(
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
