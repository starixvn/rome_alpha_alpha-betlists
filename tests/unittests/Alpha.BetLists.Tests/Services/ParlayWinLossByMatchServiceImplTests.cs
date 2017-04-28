using System;
using System.Linq;
using Alpha.BetLists.Criteria.ParlayWinLossByMatch;
using Alpha.BetLists.Models;
using Alpha.BetLists.Params;
using Alpha.BetLists.Services;
using Alpha.BetLists.Tests.Fixtures;
using Alpha.BetLists.Tests.TestData;
using Fanex.BetList.Core.Entities;
using NSubstitute;
using Xunit;

namespace Alpha.BetLists.Tests.Services
{
    [Trait("Category", "ParlayWinLossByMatch")]
    [Collection("FanexDataCollection")]
    public class ParlayWinLossByMatchServiceImplTests
    {
        private readonly ParlayWinLossByMatchServiceImpl winLossService;

        public ParlayWinLossByMatchServiceImplTests(FanexDataFixture dataFixture)
        {
            dataFixture.DynamicRepository
                .FetchAsync<Ticket>(Arg.Any<GetParlayWinLossByMatchCriteria>())
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
