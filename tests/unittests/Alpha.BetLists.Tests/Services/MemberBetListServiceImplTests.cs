namespace Alpha.BetLists.Tests.Services
{
    using BetLists.Criteria.MemberBetList;
    using BetLists.Params;
    using BetLists.Services;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using NSubstitute;
    using System;
    using System.Linq;
    using TestData;
    using Xunit;

    [Trait("Category", "RunningBetList")]
    [Collection("FanexDataCollection")]
    public class MemberBetListServiceImplTests
    {
        private readonly MemberBetListService memberBetListService;

        public MemberBetListServiceImplTests(FanexDataFixture dataFixture)
        {
            dataFixture.DynamicRepository
                .FetchMultipleAsync<Ticket, TicketData>(Arg.Any<GetBetListAllByDateCriteria>())
                .Returns(BetListDataTest.BetListDataTuple);

            memberBetListService = new MemberBetListServiceImpl(dataFixture.DynamicRepository);
        }

        [Fact]
        public void GetBetList_Always_ReturnCorrectBetList()
        {
            // Arrange
            var param = new MemberBetListParam(1, Enumerable.Empty<int>(), DateTime.Now, DateTime.Now, string.Empty, string.Empty);

            // Act
            var actualBetListData = memberBetListService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(actualBetListData.Tickets) &&
                BetListDataTest.TicketData.SequenceEqual(actualBetListData.TicketData));
        }
    }
}