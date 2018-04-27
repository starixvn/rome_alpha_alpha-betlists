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
        private readonly FanexDataFixture dataFixture;

        public ParlayWinLossByMatchServiceImplTests(FanexDataFixture dataFixture)
        {
            this.dataFixture = dataFixture;
            this.dataFixture.DynamicRepository
                .FetchAsync<TicketDto>(Arg.Any<GetParlayWinLossByMatchCriteria>())
                .Returns(BetListDataTest.Tickets);

            var reportDateService = Substitute.For<ReportDateService>();
            reportDateService.GetMoveBetsDay().Returns(DateTime.Today);

            winLossService = new ParlayWinLossByMatchServiceImpl(reportDateService, this.dataFixture.DynamicRepository);
        }

        [Fact]
        public void GetBetList_Always_ReturnCorrectBetList()
        {
            // Arrange
            var param = new ParlayWinLossByMatchParam(1, 2, string.Empty, DateTime.Now, DateTime.Now, DateTime.MinValue, UserLevel.Agent);

            // Act
            var actualBetListData = winLossService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(actualBetListData));
        }

        [Fact]
        public void GetBetList_UserIsInLimitedDateRange_UseLimitedDate()
        {
            // Arrange
            var limitedDate = DateTime.Today.AddDays(1);
            var param = new ParlayWinLossByMatchParam(1, 2, string.Empty, DateTime.Now, DateTime.Now, limitedDate, UserLevel.Agent);

            // Act
            var actualBetListData = winLossService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            dataFixture.DynamicRepository.Received().FetchAsync<TicketDto>(Arg.Is<GetParlayWinLossByMatchCriteria>(i => i.FromDate == limitedDate));
        }
    }
}
