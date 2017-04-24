namespace Alpha.BetLists.Tests.Services
{
    using System;
    using System.Linq;
    using BetLists.Services;
    using Criteria.Parlay;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Models.Parlay;
    using NSubstitute;
    using TestData;
    using Xunit;

    [Trait("Category", "BaseBetListService")]
    [Collection("FanexDataCollection")]
    public class ParlayServiceImplTests
    {
        private readonly FanexDataFixture dataFixture;
        private readonly ReportDateService reportDateService;
        private readonly ParlayService parlayService;

        public ParlayServiceImplTests(FanexDataFixture dataFixture)
        {
            reportDateService = Substitute.For<ReportDateService>();
            reportDateService.GetBalanceUpDay().Returns(DateTime.Today);
            reportDateService.GetMoveBetsDay().Returns(DateTime.Today);

            this.dataFixture = dataFixture;
            parlayService = new ParlayServiceImpl(reportDateService, this.dataFixture.DynamicRepository);
        }

        [Fact]
        public void GetParlayBetList_Always_ReturnParlayTickets()
        {
            // Arrange
            dataFixture.DynamicRepository
                .FetchAsync<Ticket>(Arg.Is<GetParlayBetListCriteria>(c => c.TransId == 123 && c.Language == "en"))
                .Returns(BetListDataTest.Tickets);

            // Act
            var actualTickets = parlayService.GetParlayBetList(123, "en").GetAwaiter().GetResult();

            // Assert
            Assert.Equal(BetListDataTest.Tickets, actualTickets);
        }

        [Fact]
        public void GetComboParlayDetail_Always_ReturnComboParlayDetailData()
        {
            // Arrange
            dataFixture.DynamicRepository.FetchMultipleAsync<ComboParlayDetail, MatchDetail>(
                Arg.Is<GetComboParlayDetailCriteria>(c => c.TransId == 123
                && c.RefNo == 0
                && c.BetId == 1
                && c.CustId == 1
                && c.BalanceUpDay == reportDateService.GetBalanceUpDay()
                && c.MoveBetDay == reportDateService.GetMoveBetsDay()
                && c.Language == "en")).Returns(BetListDataTest.ComboParlayDetailTuple);

            // Act
            var actualData = parlayService.GetComboParlayDetail(123, 0, 1, 1, "en").GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.ComboParlayDetails.SequenceEqual(actualData.ComboParlayDetails)
                    && BetListDataTest.MatchDetails.SequenceEqual(actualData.MatchDetails));
        }
    }
}