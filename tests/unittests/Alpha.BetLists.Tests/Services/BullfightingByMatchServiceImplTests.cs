namespace Alpha.BetLists.Tests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Params;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Tests.Fixtures;
    using Alpha.BetLists.Tests.TestData;
    using Alpha.SmartPunter.Services;
    using BetLists.Criteria.BullfightingByMatch;
    using Fanex.BetList.Core.Entities;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "BullfightingByMatch")]
    [Collection("FanexDataCollection")]
    public class BullfightingByMatchServiceImplTests
    {
        private readonly BullfightingByMatchServiceImpl service;
        private readonly ISmartPunterBetListMonitor monitorService;

        public BullfightingByMatchServiceImplTests(FanexDataFixture dataFixture)
        {
            dataFixture.DynamicRepository
                .FetchAsync<TicketDto>(Arg.Any<GetBetListBullfightingByMatchCriteria>())
                .Returns(BetListDataTest.Tickets);
            monitorService = Substitute.For<ISmartPunterBetListMonitor>();
            service = new BullfightingByMatchServiceImpl(dataFixture.DynamicRepository, monitorService);
        }

        [Fact]
        public void GetBetList_Always_ReturnsCorrectBetList()
        {
            // Arrange
            var param = new BullfightingByMatchParam(1, 1, UserLevel.Agent);

            // Act
            var atualTickets = service.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(atualTickets));
        }

        [Fact]
        public void GetBetList_Always_WriteLogFromSmartPunter()
        {
            // Arrange
            var param = new BullfightingByMatchParam(1, 1, UserLevel.Agent);

            // Act
            var atualTickets = service.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            monitorService.Received().WriteLog(1, Arg.Any<IList<ITicket>>());
        }
    }
}
