namespace Alpha.BetLists.Tests.Services
{
    using System;
    using System.Linq;
    using Alpha.BetLists.AppResources;
    using BetLists.Criteria.FindTickets;
    using BetLists.Extensions;
    using BetLists.Models;
    using BetLists.Params;
    using BetLists.Services;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using NSubstitute;
    using TestData;
    using Xunit;

    [Trait("Category", "FindTickets")]
    [Collection("FanexDataCollection")]
    public class FindTicketsServiceTests
    {
        private readonly FindTicketsService findTicketsService;

        public FindTicketsServiceTests(FanexDataFixture dataFixture)
        {
            var reportDateService = Substitute.For<ReportDateService>();
            reportDateService.GetBalanceUpDay().Returns(DateTime.Today);
            reportDateService.GetMoveBetsDay().Returns(DateTime.Today);

            dataFixture.DynamicRepository
                .FetchMultipleAsync<TicketDto, TicketData>(Arg.Any<FindTicketsCriteria>())
                .Returns(BetListDataTest.BetListDataTuple);

            findTicketsService = new FindTicketsServiceImpl(reportDateService, dataFixture.DynamicRepository, dataFixture.PartialUMService);
        }

        [Fact]
        public void GetBetList_Always_ReturnBetListData()
        {
            // Arrange
            var param = new FindTicketsParam(
                1,
                2,
                UserLevel.Agent);

            // Act
            var actualBetListData = findTicketsService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(actualBetListData.Tickets) &&
                BetListDataTest.TicketData.SequenceEqual(actualBetListData.TicketData));
        }

    }
}