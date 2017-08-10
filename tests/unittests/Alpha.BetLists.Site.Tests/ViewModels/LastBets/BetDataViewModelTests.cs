using System.Collections.Generic;
using Alpha.BetLists.Models;
using Alpha.BetLists.Site.Requests.LastBets;
using Alpha.BetLists.Site.Tests.Fixtures;
using Alpha.BetLists.Site.ViewModels.LastBets;
using Fanex.BetList.Core.Entities;
using KellermanSoftware.CompareNetObjects;
using NSubstitute;
using Xunit;

namespace Alpha.BetLists.Site.Tests.ViewModels.LastBets
{
    [Trait("Category", "LastBets")]
    [Collection("BetListCollection")]
    public class BetDataViewModelTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly IUser user;
        private readonly CompareLogic compareLogic;

        public BetDataViewModelTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            user = Substitute.For<IUser>();
            user.Id.Returns(2);
            user.LevelId.Returns(UserLevel.Agent.Value);

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void TotalColumn_Always_ReturnsCorrectValue()
        {
            // Arrange
            var expectedValue = 7;
            var query = new GetLastBetsQuery(user);
            var viewModel = new BetDataViewModel(0, query, null, betlistFixture.HtmlBuilder, betlistFixture.TicketHelper, null);

            // Act
            var actualValue = viewModel.TotalColumn;

            // Assert
            Assert.Equal(expectedValue, expectedValue);
        }

        [Theory]
        [InlineData(4, false)]
        [InlineData(2, true)]
        public void CanViewScoreMap_Always_ReturnsCorrectValue(int betType, bool expectedValue)
        {
            // Arrange
            var query = new GetLastBetsQuery(user);
            var supportScoreMapBettypeIds = new List<int> { 1, 2, 3 };
            var viewModel = new BetDataViewModel(0, query, null, betlistFixture.HtmlBuilder, betlistFixture.TicketHelper, supportScoreMapBettypeIds);
            var ticket = new Ticket
            {
                BetTypeId = betType
            };

            // Act
            var actualValue = viewModel.CanViewScoreMap(ticket);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LastTransId_NoTicket_Returns0()
        {
            // Arrange
            var query = new GetLastBetsQuery(user);
            var betListData = new BetListData(null, null);
            var viewModel = new BetDataViewModel(0, query, betListData, betlistFixture.HtmlBuilder, betlistFixture.TicketHelper, null);
            var expectedLastTransId = 0;

            // Act
            var actualLastTransId = viewModel.LastTransId;

            // Assert
            Assert.Equal(expectedLastTransId, actualLastTransId);
        }

        [Fact]
        public void LastTransId_HasTickets_ReturnsCorrectLastTransId()
        {
            // Arrange
            var query = new GetLastBetsQuery(user);
            var tickets = new List<TicketDto>
            {
                new TicketDto
                {
                    TransId = 1
                }
            };

            var betListData = new BetListData(tickets, null);
            var viewModel = new BetDataViewModel(0, query, betListData, betlistFixture.HtmlBuilder, betlistFixture.TicketHelper, null);
            var expectedLastTransId = 1;

            // Act
            var actualLastTransId = viewModel.LastTransId;

            // Assert
            Assert.Equal(expectedLastTransId, actualLastTransId);
        }
    }
}
