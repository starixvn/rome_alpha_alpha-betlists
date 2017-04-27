namespace Alpha.BetLists.Tests.Services
{
    using System;
    using System.Linq;
    using BetLists.Criteria.WinLossByDate;
    using BetLists.Services;
    using Extensions;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using NSubstitute;
    using Params;
    using TestData;
    using Xunit;

    [Trait("Category", "winLoss")]
    [Collection("FanexDataCollection")]
    public class WinLossServiceTests
    {
        private readonly FanexDataFixture dataFixture;
        private readonly ReportDateService reportDateService;
        private readonly WinLossService winLossService;

        public WinLossServiceTests(FanexDataFixture dataFixture)
        {
            this.dataFixture = dataFixture;
            reportDateService = Substitute.For<ReportDateService>();
            reportDateService.GetBalanceUpDay().Returns(DateTime.Today);
            reportDateService.GetMoveBetsDay().Returns(DateTime.Today);

            dataFixture.DynamicRepository
                .FetchMultipleAsync<Ticket, TicketData>(Arg.Any<GetBetListWinLossByDateCriteria>())
                .Returns(BetListDataTest.BetListDataTuple);

            dataFixture.DynamicRepository
                .FetchMultipleAsync<Ticket, TicketData>(Arg.Any<GetBetListWinLossByDateOldCriteria>())
                .Returns(BetListDataTest.OldBetListDataTuple);

            winLossService = new WinLossServiceImpl(reportDateService, this.dataFixture.DynamicRepository);
        }

        [Fact]
        public void GetBetList_FromDateIsAfterBUDay_ReturnBetListWinLossByDate()
        {
            // Arrange
            var param = new WinLossParam(
                0,
                BetListDataTest.ProductIds,
                DateTime.Today.AddDays(1),
                DateTime.Today.AddDays(2),
                "en");

            // Act
            var actualBetListData = winLossService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(actualBetListData.Tickets) &&
                BetListDataTest.TicketData.SequenceEqual(actualBetListData.TicketData));
        }

        [Fact]
        public void GetBetList_FromDateIsAfterBUDay_CallGetBetListWinLossByDateCriteria()
        {
            // Arrange
            var param = new WinLossParam(
                0,
                BetListDataTest.ProductIds,
                DateTime.Today.AddDays(1),
                DateTime.Today.AddDays(2),
                "en");

            // Act
            winLossService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            dataFixture.DynamicRepository
                .Received()
                .FetchMultipleAsync<Ticket, TicketData>(Arg.Is<GetBetListWinLossByDateCriteria>(c =>
                    c.CustId == param.CustId
                    && c.ProductIds == param.ProductIds.JoinWithComma()
                    && c.FromDate == param.FromDate
                    && c.ToDate == param.ToDate
                    && c.MoveBetsDate == reportDateService.GetMoveBetsDay()
                    && c.Language == param.Language));
        }

        [Fact]
        public void GetBetList_FromDateIsBeforeBUDay_ToDateIsBeforePreviousBUDay_ReturnBetListWinLossByDateOld()
        {
            var param = new WinLossParam(
                0,
                BetListDataTest.ProductIds,
                DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(-2),
                "en");

            // Act
            var actualBetListData = winLossService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.OldTickets.SequenceEqual(actualBetListData.Tickets) &&
                BetListDataTest.OldTicketData.SequenceEqual(actualBetListData.TicketData));
        }

        [Fact]
        public void GetBetList_FromDateIsBeforeBUDay_ToDateIsBeforePreviousBUDay_CallGetBetListWinLossByDateOldCriteria()
        {
            // Arrange
            var param = new WinLossParam(
                0,
                BetListDataTest.ProductIds,
                DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(-2),
                "en");

            // Act
            winLossService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            dataFixture.DynamicRepository
                .Received()
                .FetchMultipleAsync<Ticket, TicketData>(Arg.Is<GetBetListWinLossByDateOldCriteria>(c =>
                    c.CustId == param.CustId
                    && c.ProductIds == param.ProductIds.JoinWithComma()
                    && c.FromDate == param.FromDate
                    && c.ToDate == param.ToDate
                    && c.Language == param.Language));
        }

        [Fact]
        public void GetBetList_FromDateIsBeforeBUDay_ToDateIsAfterPreviousBUDay_ReturnBetListWinLossByDateNewAndOld()
        {
            var param = new WinLossParam(
                0,
                BetListDataTest.ProductIds,
                DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(2),
                "en");

            // Act
            var actualBetListData = winLossService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.Concat(BetListDataTest.OldTickets).SequenceEqual(actualBetListData.Tickets) &&
                BetListDataTest.TicketData.Concat(BetListDataTest.OldTicketData).SequenceEqual(actualBetListData.TicketData));
        }

        [Fact]
        public void GetBetList_FromDateIsBeforeBUDay_ToDateIsAfterPreviousBUDay_CallGetBetListWinLossByDateNewAndOldCriteria()
        {
            // Arrange
            var param = new WinLossParam(
                0,
                BetListDataTest.ProductIds,
                DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(2),
                "en");

            // Act
            winLossService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            dataFixture.DynamicRepository
                .Received()
                .FetchMultipleAsync<Ticket, TicketData>(Arg.Is<GetBetListWinLossByDateOldCriteria>(c =>
                    c.CustId == param.CustId
                    && c.ProductIds == param.ProductIds.JoinWithComma()
                    && c.FromDate == param.FromDate
                    && c.ToDate == reportDateService.GetBalanceUpDay().AddDays(-1)
                    && c.Language == param.Language));

            dataFixture.DynamicRepository
                .Received()
                .FetchMultipleAsync<Ticket, TicketData>(Arg.Is<GetBetListWinLossByDateCriteria>(c =>
                    c.CustId == param.CustId
                    && c.ProductIds == param.ProductIds.JoinWithComma()
                    && c.FromDate == reportDateService.GetBalanceUpDay().AddDays(-1)
                    && c.ToDate == param.ToDate
                    && c.MoveBetsDate == reportDateService.GetMoveBetsDay()
                    && c.Language == param.Language));
        }
    }
}