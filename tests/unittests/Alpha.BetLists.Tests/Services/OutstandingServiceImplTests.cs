namespace Alpha.BetLists.Tests.Services
{
    using BetLists.Criteria.Outstanding;
    using BetLists.Params;
    using BetLists.Services;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using NSubstitute;
    using System;
    using System.Linq;
    using TestData;
    using Xunit;

    [Trait("Category", "Outstanding")]
    [Collection("FanexDataCollection")]
    public class OutstandingServiceImplTests
    {
        private readonly OutstandingService outstandingService;

        public OutstandingServiceImplTests(FanexDataFixture dataFixture)
        {
            dataFixture.DynamicRepository
                .FetchMultipleAsync<Ticket, TicketData>(Arg.Any<GetBetListRunningByDateCriteria>())
                .Returns(BetListDataTest.BetListDataTuple);

            outstandingService = new OutstandingServiceImpl(dataFixture.DynamicRepository);
        }

        [Fact]
        public void GetBetList_Always_ReturnCorrectBetList()
        {
            // Arrange
            var param = new OutstandingParam(1, Enumerable.Empty<int>(), DateTime.Now, DateTime.Now, true, string.Empty, string.Empty);

            // Act
            var actualBetListData = outstandingService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(actualBetListData.Tickets) &&
                BetListDataTest.TicketData.SequenceEqual(actualBetListData.TicketData));
        }
    }
}