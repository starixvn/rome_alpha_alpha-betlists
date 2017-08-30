using System.Linq;
using Alpha.BetLists.Criteria.RacingByMatch;
using Alpha.BetLists.Models;
using Alpha.BetLists.Params;
using Alpha.BetLists.Services;
using Alpha.BetLists.Tests.Fixtures;
using Alpha.BetLists.Tests.TestData;
using NSubstitute;
using Xunit;

namespace Alpha.BetLists.Tests.Services
{
    [Trait("Category", "RacingByMatch")]
    [Collection("FanexDataCollection")]
    public class RacingByMatchServiceImplTests
    {
        private readonly RacingByMatchServiceImpl service;

        public RacingByMatchServiceImplTests(FanexDataFixture dataFixture)
        {
            dataFixture.DynamicRepository
                .FetchAsync<TicketDto>(Arg.Any<GetBetListRacingByMatchCriteria>())
                .Returns(BetListDataTest.Tickets);

            service = new RacingByMatchServiceImpl(dataFixture.DynamicRepository);
        }

        [Fact]
        public void GetBetList_Always_ReturnsCorrectBetList()
        {
            // Arrange
            var param = new RacingByMatchParam(1, 1, 1, string.Empty, true, UserLevel.Agent);

            // Act
            var atualTickets = service.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(atualTickets));
        }
    }
}
