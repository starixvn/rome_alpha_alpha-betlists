namespace Alpha.BetLists.Tests.Services
{
    using System;
    using System.Linq;
    using Alpha.BetLists.AppResources;
    using BetLists.Criteria.SearchTickets;
    using BetLists.Extensions;
    using BetLists.Models;
    using BetLists.Params;
    using BetLists.Services;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using NSubstitute;
    using TestData;
    using Xunit;

    [Trait("Category", "SearchTickets")]
    [Collection("FanexDataCollection")]
    public class SearchTicketsServiceTests
    {
        private readonly SearchTicketsService searchTicketsService;

        public SearchTicketsServiceTests(FanexDataFixture dataFixture)
        {
            var reportDateService = Substitute.For<ReportDateService>();
            reportDateService.GetBalanceUpDay().Returns(DateTime.Today);
            reportDateService.GetMoveBetsDay().Returns(DateTime.Today);

            dataFixture.DynamicRepository
                .FetchMultipleAsync<TicketDto, TicketData>(Arg.Any<SearchTicketsCriteria>())
                .Returns(BetListDataTest.BetListDataTuple);

            searchTicketsService = new SearchTicketsServiceImpl(reportDateService, dataFixture.DynamicRepository, dataFixture.PartialUMService, dataFixture.BetListRepository);
        }

        [Fact]
        public void GetBetList_Always_ReturnBetListData()
        {
            // Arrange
            var param = new SearchTicketsParam(
                1,
                2,
                UserLevel.Agent);

            // Act
            var actualBetListData = searchTicketsService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(actualBetListData.Tickets) &&
                BetListDataTest.TicketData.SequenceEqual(actualBetListData.TicketData));
        }

    }
}