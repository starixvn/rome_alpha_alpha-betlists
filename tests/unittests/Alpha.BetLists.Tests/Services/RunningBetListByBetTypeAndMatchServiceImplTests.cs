namespace Alpha.BetLists.Tests.Services
{
    using System.Linq;
    using BetLists.Criteria.RunningBetListByBetTypeAndMatch;
    using BetLists.Models;
    using BetLists.Params;
    using BetLists.Services;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using NSubstitute;
    using TestData;
    using Xunit;

    [Trait("Category", "RunningByBetTypeAndMatch")]
    [Collection("FanexDataCollection")]
    public class RunningBetListByBetTypeAndMatchServiceImplTests
    {
        private readonly RunningBetListByBetTypeAndMatchServiceImpl service;
        private readonly ParlayService parlayService;

        public RunningBetListByBetTypeAndMatchServiceImplTests(FanexDataFixture dataFixture)
        {
            parlayService = Substitute.For<ParlayService>();
            dataFixture.DynamicRepository
                .FetchMultipleAsync<TicketDto, TicketData>(Arg.Any<GetBetListRunningByBetTypeAndMatchCriteria>())
                .Returns(BetListDataTest.BetListDataTuple);

            service = new RunningBetListByBetTypeAndMatchServiceImpl(dataFixture.DynamicRepository, parlayService);
        }

        [Fact]
        public void GetBetList_Always_ReturnsCorrectBetList()
        {
            // Arrange
            var param = new RunningBetListByBetTypeAndMatchParam(1, 1, 1, string.Empty, 0, string.Empty, UserLevel.Agent);

            // Act
            var atualBetListData = service.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(atualBetListData.Tickets)
                && BetListDataTest.TicketData.SequenceEqual(atualBetListData.TicketData));
        }
    }
}
