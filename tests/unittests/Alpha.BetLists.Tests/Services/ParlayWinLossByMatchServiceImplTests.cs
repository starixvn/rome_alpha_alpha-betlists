namespace Alpha.BetLists.Tests.Services
{
    using System;
    using System.Linq;
    using BetLists.Criteria.ParlayWinLossByMatch;
    using BetLists.Models;
    using BetLists.Params;
    using BetLists.Services;
    using Fixtures;
    using NSubstitute;
    using TestData;
    using Xunit;

    [Trait("Category", "ParlayWinLossByMatch")]
    [Collection("FanexDataCollection")]
    public class ParlayWinLossByMatchServiceImplTests
    {
        private readonly ParlayWinLossByMatchServiceImpl winLossService;

        public ParlayWinLossByMatchServiceImplTests(FanexDataFixture dataFixture)
        {
            dataFixture.DynamicRepository
                .FetchAsync<TicketDto>(Arg.Any<GetParlayWinLossByMatchCriteria>())
                .Returns(BetListDataTest.Tickets);

            var reportDateService = Substitute.For<ReportDateService>();
            reportDateService.GetMoveBetsDay().Returns(DateTime.Today);

            winLossService = new ParlayWinLossByMatchServiceImpl(reportDateService, dataFixture.DynamicRepository);
        }

        [Fact]
        public void GetBetList_Always_ReturnCorrectBetList()
        {
            // Arrange
            var param = new ParlayWinLossByMatchParam(1, 2, string.Empty, DateTime.Now, DateTime.Now, UserLevel.Agent);

            // Act
            var actualBetListData = winLossService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(actualBetListData));
        }
    }
}
