namespace Alpha.BetLists.Tests.Services
{
    using System;
    using Alpha.BetLists.Criteria;
    using BetLists.Models;
    using BetLists.Params;
    using BetLists.Services;
    using Fixtures;
    using NSubstitute;
    using TestData;
    using Xunit;

    [Trait("Category", "CashOutHistory")]
    [Collection("FanexDataCollection")]
    public class CashOutHistoryServiceTests
    {
        private readonly CashOutHistoryServiceImpl service;

        public CashOutHistoryServiceTests(FanexDataFixture dataFixture)
        {
            dataFixture.DynamicRepository
                .GetAsync<CashOutHistoryModel>(Arg.Any<GetCashOutHistoryCriteria>())
                .Returns(BetListDataTest.CashOutHistory);

            service = new CashOutHistoryServiceImpl(dataFixture.ReportDateService, dataFixture.DynamicRepository);
        }

        [Fact]
        public void GetHistory_Always_ReturnsCorrectModel()
        {
            // Arrange
            var param = new CashOutHistoryParam(123, 321, DateTime.Now);

            // Act
            var actualTickets = service.GetHistory(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(actualTickets.GetType() == typeof(CashOutHistoryModel));
        }
    }
}