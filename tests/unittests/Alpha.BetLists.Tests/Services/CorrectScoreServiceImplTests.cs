namespace Alpha.BetLists.Tests.Services
{
    using System.Linq;
    using BetLists.Criteria.CorrectScore;
    using BetLists.Models;
    using BetLists.Params;
    using BetLists.Services;
    using Fixtures;
    using NSubstitute;
    using TestData;
    using Xunit;

    [Trait("Category", "CorrectScore")]
    [Collection("FanexDataCollection")]
    public class CorrectScoreServiceImplTests
    {
        private readonly CorrectScoreServiceImpl correctScoreService;

        public CorrectScoreServiceImplTests(FanexDataFixture dataFixture)
        {
            dataFixture.DynamicRepository
                .FetchAsync<TicketDto>(Arg.Any<GetRunningCorrectScoreCriteria>())
                .Returns(BetListDataTest.BetListDataTuple.Item1);

            correctScoreService = new CorrectScoreServiceImpl(dataFixture.DynamicRepository);
        }

        [Fact]
        public void GetBetList_Always_ReturnsCorrectBetList()
        {
            // Arrange
            var param = new CorrectScoreParam(1, string.Empty, UserLevel.Agent);

            // Act
            var actualTickets = correctScoreService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(actualTickets));
        }
    }
}
