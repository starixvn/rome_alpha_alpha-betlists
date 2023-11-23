namespace Alpha.BetLists.Tests.Services
{
    using Alpha.BetLists.Criteria.MoveStatementHistory;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Params;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Tests.Fixtures;
    using Alpha.BetLists.Tests.TestData;
    using NSubstitute;
    using System.Linq;
    using Xunit;

    [Trait("Category", "MoveStatementHistory")]
    [Collection("FanexDataCollection")]
    public class MoveStatementHistoryServiceImplTests
    {
        private readonly MoveStatementHistoryServiceImpl moveStatementHistoryService;

        public MoveStatementHistoryServiceImplTests(FanexDataFixture dataFixture)
        {
            dataFixture.DynamicRepository
                .FetchAsync<TicketDto>(Arg.Any<GetBetListMoveStatementHistoryCriteria>())
                .Returns(BetListDataTest.Tickets);

            moveStatementHistoryService = new MoveStatementHistoryServiceImpl(dataFixture.DynamicRepository, dataFixture.PartialUMService);
        }

        [Fact]
        public void GetBetList_Always_ReturnsCorrectBetList()
        {
            // Arrange
            var param = new MoveStatementHistoryParam(10, 123456);

            // Act
            var atualTickets = moveStatementHistoryService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(atualTickets));
        }
    }
}
