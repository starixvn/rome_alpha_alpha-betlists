namespace Alpha.BetLists.Site.Tests.ViewModels.DailyStatement
{
    using System.Collections.Generic;
    using BetLists.Models;
    using BetLists.Tests.Constants;
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Requests.DailyStatement;
    using Site.ViewModels.DailyStatement;
    using Xunit;

    [Trait("Category", "DailyStatement")]
    [Collection("BetListCollection")]
    public class DailyStatementViewModelTests
    {
        private readonly BetListFixture betlistFixture;
        private DailyStatementQuery query;

        public DailyStatementViewModelTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            query = new DailyStatementQuery(betlistFixture.User)
            {
                CustId = 1,
                WinLossDate = DefaultVariables.CurrentDate,
                CustName = "CustName",
                TransType = 1,
                Language = "en"
            };
        }

        [Fact]
        public void Title_Always_ReturnsCorrectValue()
        {
            // Arrange
            var viewModel = new DailyStatementViewModel(
                query,
                BetListDataTest.BetListData,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);
            var expectedTitle = "Daily Statement 5/25/2017 - CustName";

            // Act
            var actualTitle = viewModel.Title;

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void CanViewScoreMap_NotSupportScoreMapBettypeIs_ReturnsFalse()
        {
            // Arrange
            var ticket = new TicketDto
            {
                BetTypeId = 1,
                Status = BetStatus.Running
            };

            var viewModel = new DailyStatementViewModel(
                    query,
                    BetListDataTest.BetListData,
                    betlistFixture.HtmlBuilder,
                    betlistFixture.TicketHelper);

            // Act
            var actualCanViewScoreMap = viewModel.CanViewScoreMap(ticket);

            // Assert
            Assert.False(actualCanViewScoreMap);
        }

        [Theory]
        [InlineData(BetStatus.Running, 1)]
        [InlineData(BetStatus.Refund, 1)]
        [InlineData(BetStatus.Running, 4)]
        [InlineData(BetStatus.Waiting, 4)]
        public void CanViewScoreMap_SupportScoreMapBettypeIds_ReturnsCorrectValue(string ticketStatus, int bettypeId)
        {
            // Arrange
            var ticket = new TicketDto
            {
                BetTypeId = bettypeId,
                Status = ticketStatus
            };

            var supportScoreMapBettypeIds = new List<int> { 1, 2, 3 };
            var expectedCanViewScoreMap = ticketStatus.Equals(BetStatus.Running) && supportScoreMapBettypeIds.Contains(bettypeId);

            var viewModel = new DailyStatementViewModel(
                    query,
                    BetListDataTest.BetListData,
                    betlistFixture.HtmlBuilder,
                    betlistFixture.TicketHelper,
                    supportScoreMapBettypeIds);

            // Act
            var actualCanViewScoreMap = viewModel.CanViewScoreMap(ticket);

            // Assert
            Assert.Equal(expectedCanViewScoreMap, actualCanViewScoreMap);
        }
    }
}
