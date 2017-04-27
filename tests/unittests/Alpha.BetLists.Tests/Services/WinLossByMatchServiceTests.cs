using System;
using System.Linq;
using Alpha.BetLists.Constants;
using Alpha.BetLists.Criteria.WinLossByMatch;
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
    [Trait("Category", "WinLossByMatch")]
    [Collection("FanexDataCollection")]
    public class WinLossByMatchServiceTests
    {
        private readonly WinLossByMatchServiceImpl winLossService;

        public WinLossByMatchServiceTests(FanexDataFixture dataFixture)
        {
            dataFixture.DynamicRepository
                .FetchMultipleAsync<Ticket, TicketData>(Arg.Any<GetWinLossOutrightCriteria>())
                .Returns(BetListDataTest.BetListDataTuple);

            dataFixture.DynamicRepository
                .FetchMultipleAsync<Ticket, TicketData>(Arg.Any<GetWinLossRacingCriteria>())
                .Returns(BetListDataTest.BetListDataTuple);

            dataFixture.DynamicRepository
                .FetchMultipleAsync<Ticket, TicketData>(Arg.Any<GetWinLossOthersCriteria>())
                .Returns(BetListDataTest.BetListDataTuple);

            var reportDateService = Substitute.For<ReportDateService>();
            reportDateService.GetMoveBetsDay().Returns(DateTime.Today);

            winLossService = new WinLossByMatchServiceImpl(reportDateService, dataFixture.DynamicRepository);
        }

        [Fact]
        public void GetBetList_OutRight_ReturnOutRightBetList()
        {
            // Arrange
            var param = CreateWinLossByMatchParam(BetTypeIdConstants.Outright);

            // Act
            var actualBetListData = winLossService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(actualBetListData.Tickets) &&
                BetListDataTest.TicketData.SequenceEqual(actualBetListData.TicketData));
        }

        [Theory]
        [InlineData(BetTypeIdConstants.HorseRacing_Win)]
        [InlineData(BetTypeIdConstants.HorseRacing_Place)]
        [InlineData(BetTypeIdConstants.HorseRacing_WinPlace)]
        [InlineData(BetTypeIdConstants.HorseRacingTote_Win)]
        [InlineData(BetTypeIdConstants.HorseRacingTote_Place)]
        [InlineData(BetTypeIdConstants.HorseRacingTote_WinPlace)]
        [InlineData(BetTypeIdConstants.VirtualHorseRacing_Win)]
        [InlineData(BetTypeIdConstants.VirtualHorseRacing_Place)]
        [InlineData(BetTypeIdConstants.VirtualHorseRacing_WinPlace)]
        public void GetBetList_Racing_ReturnRacingBetList(int betType)
        {
            // Arrange
            var param = CreateWinLossByMatchParam(betType);

            // Act
            var actualBetListData = winLossService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(actualBetListData.Tickets) &&
                BetListDataTest.TicketData.SequenceEqual(actualBetListData.TicketData));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(30)]
        [InlineData(40)]
        [InlineData(44)]
        [InlineData(1230)]
        [InlineData(1234)]
        [InlineData(int.MaxValue)]
        public void GetBetList_OtherBetType_ReturnOtherBetTypeBetList(int betType)
        {
            // Arrange
            var param = CreateWinLossByMatchParam(betType);

            // Act
            var actualBetListData = winLossService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(actualBetListData.Tickets) &&
                BetListDataTest.TicketData.SequenceEqual(actualBetListData.TicketData));
        }

        private WinLossByMatchParam CreateWinLossByMatchParam(int betType)
            => new WinLossByMatchParam(
                0,
                1,
                2,
                betType,
                "Race 3",
                0,
                "en",
                "member01",
                DateTime.Today.AddDays(1),
                DateTime.Today.AddDays(2),
                UserLevel.Agent);
    }
}
