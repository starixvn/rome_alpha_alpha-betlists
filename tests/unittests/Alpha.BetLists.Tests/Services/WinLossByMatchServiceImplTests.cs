namespace Alpha.BetLists.Tests.Services
{
    using System;
    using System.Linq;
    using BetLists.Constants;
    using BetLists.Criteria.WinLossByMatch;
    using BetLists.Models;
    using BetLists.Params;
    using BetLists.Services;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using NSubstitute;
    using TestData;
    using Xunit;

    [Trait("Category", "WinLossByMatch")]
    [Collection("FanexDataCollection")]
    public class WinLossByMatchServiceImplTests
    {
        private readonly WinLossByMatchServiceImpl winLossService;

        public WinLossByMatchServiceImplTests(FanexDataFixture dataFixture)
        {
            dataFixture.DynamicRepository
                .FetchMultipleAsync<TicketDto, TicketData>(Arg.Any<GetWinLossOutrightCriteria>())
                .Returns(BetListDataTest.BetListDataTuple);

            dataFixture.DynamicRepository
                .FetchMultipleAsync<TicketDto, TicketData>(Arg.Any<GetWinLossRacingCriteria>())
                .Returns(BetListDataTest.BetListDataTuple);

            dataFixture.DynamicRepository
                .FetchMultipleAsync<TicketDto, TicketData>(Arg.Any<GetWinLossByMatchCriteria>())
                .Returns(BetListDataTest.BetListDataTuple);

            dataFixture.DynamicRepository
                .FetchMultipleAsync<TicketDto, TicketData>(Arg.Any<GetWinLossByBetTypeCriteria>())
                .Returns(BetListDataTest.BetListDataTuple);

            var reportDateService = Substitute.For<ReportDateService>();
            reportDateService.GetMoveBetsDay().Returns(DateTime.Today);

            winLossService = new WinLossByMatchServiceImpl(reportDateService, dataFixture.DynamicRepository, dataFixture.PartialUMService);
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

        [Fact]
        public void GetBetList_LeapGaming_ReturnLeapGamingBetList()
        {
            // Arrange
            var param = CreateWinLossByMatchParam(5301);

            // Act
            var actualBetListData = winLossService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(actualBetListData.Tickets) &&
               BetListDataTest.TicketData.SequenceEqual(actualBetListData.TicketData));
        }

        [Fact]
        public void GetBetList_Happy5LinkedGame_ReturnHappy5LinkedGameList()
        {
            // Arrange
            var param = CreateWinLossByMatchParam(3601);

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
                0,
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
