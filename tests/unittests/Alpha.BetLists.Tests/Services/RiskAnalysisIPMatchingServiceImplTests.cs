namespace Alpha.BetLists.Tests.Services
{
    using System;
    using System.Linq;
    using Alpha.BetLists.Criteria.RiskAnalysisIPMatching;
    using BetLists.Models;
    using BetLists.Params;
    using BetLists.Services;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using NSubstitute;
    using TestData;
    using Xunit;

    [Trait("Category", "RiskAnalysisIPMatching")]
    [Collection("FanexDataCollection")]
    public class RiskAnalysisIPMatchingServiceImplTests
    {
        private readonly RiskAnalysisIPMatchingServiceImpl service;
        private readonly FanexDataFixture dataFixture;

        public RiskAnalysisIPMatchingServiceImplTests(FanexDataFixture dataFixture)
        {
            this.dataFixture = dataFixture;
            this.dataFixture.DynamicRepository
                .FetchMultipleAsync<TicketDto, TicketData>(Arg.Any<GetRiskAnalysisIPMatchingCriteria>())
                .Returns(BetListDataTest.BetListDataTuple);

            service = new RiskAnalysisIPMatchingServiceImpl(this.dataFixture.DynamicRepository, this.dataFixture.PartialUMService, dataFixture.BetListRepository);
        }

        [Fact]
        public void GetBetList_Always_ReturnCorrectBetList()
        {
            // Arrange
            var param = new RiskAnalysisIPMatchingParam(1, DateTime.Now, DateTime.Now, "192.168.1.1");

            // Act
            var actualBetListData = service.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.TicketData.SequenceEqual(actualBetListData.TicketData) &&
                BetListDataTest.Tickets.OrderByDescending(ticket => ticket.TransDate).SequenceEqual(actualBetListData.Tickets));
        }
    }
}