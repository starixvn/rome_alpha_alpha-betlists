namespace Alpha.BetLists.Tests.Services
{
    using System.Linq;
    using BetLists.Criteria.RunningBetListByBetType;
    using BetLists.Models;
    using BetLists.Params;
    using BetLists.Services;
    using Fixtures;
    using NSubstitute;
    using TestData;
    using Xunit;

    [Trait("Category", "RunningBetListByBetType")]
    [Collection("FanexDataCollection")]
    public class RunningBetListByBetTypeServiceImplTests
    {
        private readonly RunningBetListByBetTypeServiceImpl runningBetListByBetTypeService;

        public RunningBetListByBetTypeServiceImplTests(FanexDataFixture dataFixture)
        {
            dataFixture.DynamicRepository
                .FetchAsync<TicketDto>(Arg.Any<GetBetListRunningByBetTypeCriteria>())
                .Returns(BetListDataTest.BetListDataTuple.Item1);

            runningBetListByBetTypeService = new RunningBetListByBetTypeServiceImpl(dataFixture.DynamicRepository);
        }

        [Fact]
        public void GetBetList_Always_ReturnsCorrectBetList()
        {
            // Arrange
            var param = new RunningBetListByBetTypeParam(1, 1, string.Empty, UserLevel.Agent);

            // Act
            var atualTickets = runningBetListByBetTypeService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(atualTickets));
        }
    }
}
