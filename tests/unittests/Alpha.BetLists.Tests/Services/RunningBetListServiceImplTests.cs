namespace Alpha.BetLists.Tests.Services
{
    using System;
    using System.Linq;
    using BetLists.Criteria.RunningBetList;
    using BetLists.Models;
    using BetLists.Params;
    using BetLists.Services;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using NSubstitute;
    using TestData;
    using Xunit;

    [Trait("Category", "RunningBetList")]
    [Collection("FanexDataCollection")]
    public class RunningBetListServiceImplTests
    {
        private readonly RunningBetListServiceImpl runningBetListService;

        public RunningBetListServiceImplTests(FanexDataFixture dataFixture)
        {
            dataFixture.DynamicRepository
                .FetchMultipleAsync<TicketDto, TicketData>(Arg.Any<GetBetListRunningByDateCriteria>())
                .Returns(BetListDataTest.BetListDataTuple);

            runningBetListService = new RunningBetListServiceImpl(dataFixture.DynamicRepository);
        }

        [Fact]
        public void GetBetList_Always_ReturnCorrectBetList()
        {
            // Arrange
            var param = new RunningBetListParam(1, Enumerable.Empty<int>(), DateTime.Now, DateTime.Now, true, string.Empty, string.Empty);

            // Act
            var actualBetListData = runningBetListService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(actualBetListData.Tickets) &&
                BetListDataTest.TicketData.SequenceEqual(actualBetListData.TicketData));
        }
    }
}
