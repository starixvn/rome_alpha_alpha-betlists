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
        private readonly FanexDataFixture dataFixture;
        private readonly ParlayService parlayService;

        public RunningBetListServiceImplTests(FanexDataFixture dataFixture)
        {
            parlayService = Substitute.For<ParlayService>();
            this.dataFixture = dataFixture;
            this.dataFixture.DynamicRepository
                .FetchMultipleAsync<TicketDto, TicketData>(Arg.Any<GetBetListRunningByDateCriteria>())
                .Returns(BetListDataTest.BetListDataTuple);

            runningBetListService = new RunningBetListServiceImpl(this.dataFixture.DynamicRepository, dataFixture.PartialUMService, parlayService);
        }

        [Fact]
        public void GetBetList_Always_ReturnCorrectBetList()
        {
            // Arrange
            var param = new RunningBetListParam(1, Enumerable.Empty<int>(), DateTime.Now, DateTime.Now, DateTime.MinValue, true, string.Empty, string.Empty);

            // Act
            var actualBetListData = runningBetListService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(actualBetListData.Tickets) &&
                BetListDataTest.TicketData.SequenceEqual(actualBetListData.TicketData));
        }

        [Fact]
        public void GetBetList_UserIsInLimitedDateRange_UseLimitedDate()
        {
            // Arrange
            var limitedDate = DateTime.Today.AddDays(1);
            var param = new RunningBetListParam(1, Enumerable.Empty<int>(), DateTime.Now, DateTime.Now, limitedDate, true, string.Empty, string.Empty);

            // Act
            var actualBetListData = runningBetListService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            dataFixture.DynamicRepository.Received().FetchMultipleAsync<TicketDto, TicketData>(Arg.Is<GetBetListRunningByDateCriteria>(i => i.FromDate == limitedDate));
        }
    }
}
