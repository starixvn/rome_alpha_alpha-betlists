namespace Alpha.BetLists.Tests.Services
{
    using System;
    using System.Linq;
    using BetLists.Criteria.DailyStatement;
    using BetLists.Models;
    using BetLists.Params;
    using BetLists.Services;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using NSubstitute;
    using TestData;
    using Xunit;

    [Trait("Category", "DailyStatement")]
    [Collection("FanexDataCollection")]
    public class DailyStatementServiceImplTests
    {
        private readonly FanexDataFixture dataFixture;
        private readonly DailyStatementService dailyStatementService;
        private readonly ReportDateService reportDateService;

        public DailyStatementServiceImplTests(FanexDataFixture dataFixture)
        {
            this.dataFixture = dataFixture;
            reportDateService = Substitute.For<ReportDateService>();
            reportDateService.GetBalanceUpDay().Returns(DateTime.Today);

            dataFixture.DynamicRepository
                .FetchMultipleAsync<TicketDto, TicketData>(Arg.Any<GetAllBetListByDayCriteria>())
                .Returns(BetListDataTest.BetListDataTuple);

            dataFixture.DynamicRepository
                .FetchMultipleAsync<TicketDto, TicketData>(Arg.Any<GetAllBetListByDayOldCriteria>())
                .Returns(BetListDataTest.OldBetListDataTuple);

            dailyStatementService = new DailyStatementServiceImpl(reportDateService, dataFixture.DynamicRepository, dataFixture.PartialUMService);
        }

        [Fact]
        public void GetBetList_FromDateIsBeforeBUDay_CallsGetAllBetListByDayOldCriteria()
        {
            // Arrange
            var param = new DailyStatementParam(0, DateTime.Today.AddDays(-1), 1, "en");

            // Act
            dailyStatementService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            dataFixture.DynamicRepository
                .Received()
                .FetchMultipleAsync<TicketDto, TicketData>(Arg.Is<GetAllBetListByDayOldCriteria>(
                    c =>
                    c.CustId == param.CustId
                    && c.WinLossDate == param.WinLossDate
                    && c.TransType == param.TransType
                    && c.Language == param.Language));
        }

        [Fact]
        public void GetBetList_FromDateIsBeforeBUDay_ReturnsBetListByDayOld()
        {
            // Arrange
            var param = new DailyStatementParam(0, DateTime.Today.AddDays(-1), 1, "en");

            // Act
            var actualBetListData = dailyStatementService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.OldTickets.SequenceEqual(actualBetListData.Tickets) &&
                BetListDataTest.OldTicketData.SequenceEqual(actualBetListData.TicketData));
        }

        [Fact]
        public void GetBetList_FromDateIsEqualBUDay_ReturnsBetListByDay()
        {
            // Arrange
            var param = new DailyStatementParam(0, DateTime.Today, 1, "en");

            // Act
            dailyStatementService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            dataFixture.DynamicRepository
                .Received()
                .FetchMultipleAsync<TicketDto, TicketData>(Arg.Is<GetAllBetListByDayCriteria>(
                    c =>
                    c.CustId == param.CustId
                    && c.WinLossDate == param.WinLossDate
                    && c.TransType == param.TransType));
        }

        [Fact]
        public void GetBetList_FromDateIsEqualBUDay_CallsGetAllBetListByDayCriteria()
        {
            // Arrange
            var param = new DailyStatementParam(0, DateTime.Today, 1, "en");

            // Act
            var actualBetListData = dailyStatementService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(actualBetListData.Tickets) &&
                BetListDataTest.TicketData.SequenceEqual(actualBetListData.TicketData));
        }

        [Fact]
        public void GetBetList_FromDateIsAfterBUDay_CallsGetAllBetListByDayCriteria()
        {
            // Arrange
            var param = new DailyStatementParam(0, DateTime.Today.AddDays(1), 1, "en");

            // Act
            dailyStatementService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            dataFixture.DynamicRepository
                .Received()
                .FetchMultipleAsync<TicketDto, TicketData>(Arg.Is<GetAllBetListByDayCriteria>(
                    c =>
                    c.CustId == param.CustId
                    && c.WinLossDate == param.WinLossDate
                    && c.TransType == param.TransType));
        }

        [Fact]
        public void GetBetList_FromDateIsAfterBUDay_ReturnsBetListByDay()
        {
            // Arrange
            var param = new DailyStatementParam(0, DateTime.Today.AddDays(1), 1, "en");

            // Act
            var actualBetListData = dailyStatementService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(actualBetListData.Tickets) &&
                BetListDataTest.TicketData.SequenceEqual(actualBetListData.TicketData));
        }
    }
}
