namespace Alpha.BetLists.Tests.Services
{
    using System.Linq;
    using BetLists.Criteria.ProgressiveJackpot;
    using BetLists.Models;
    using BetLists.Params;
    using BetLists.Services;
    using Fixtures;
    using NSubstitute;
    using TestData;
    using Xunit;

    [Trait("Category", "JackpotDetails")]
    [Collection("FanexDataCollection")]
    public class JackpotDetailsServiceImplTests
    {
        private readonly JackpotDetailsServiceImpl jackpotDetailsService;

        public JackpotDetailsServiceImplTests(FanexDataFixture dataFixture)
        {
            dataFixture.DynamicRepository
                .FetchAsync<TicketDto>(Arg.Any<GetJackpotDetailsCriteria>())
                .Returns(BetListDataTest.Tickets);

            jackpotDetailsService = new JackpotDetailsServiceImpl(dataFixture.DynamicRepository, dataFixture.ReportDateService);
        }

        [Fact]
        public void GetJackpotDetails_Always_ReturnsCorrectBetList()
        {
            // Arrange
            var param = new JackpotDetailsParam(1, 1, 1);

            // Act
            var actualTickets = jackpotDetailsService.GetJackpotDetails(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(actualTickets));
        }
    }
}