namespace Alpha.BetLists.Tests.Services
{
    using System;
    using System.Linq;
    using BetLists.Criteria.MemberBetList;
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
    public class MemberBetListServiceImplTests
    {
        private readonly MemberBetListServiceImpl memberBetListService;
        private readonly FanexDataFixture dataFixture;

        public MemberBetListServiceImplTests(FanexDataFixture dataFixture)
        {
            this.dataFixture = dataFixture;
            this.dataFixture.DynamicRepository
                .FetchMultipleAsync<TicketDto, TicketData>(Arg.Any<GetBetListAllByDateCriteria>())
                .Returns(BetListDataTest.BetListDataTuple);

            memberBetListService = new MemberBetListServiceImpl(this.dataFixture.DynamicRepository, dataFixture.PartialUMService);
        }

        [Fact]
        public void GetBetList_Always_ReturnCorrectBetList()
        {
            // Arrange
            var param = new MemberBetListParam(1, Enumerable.Empty<int>(), DateTime.Now, DateTime.Now, DateTime.MinValue, string.Empty, string.Empty);

            // Act
            var actualBetListData = memberBetListService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(BetListDataTest.Tickets.SequenceEqual(actualBetListData.Tickets) &&
                BetListDataTest.TicketData.SequenceEqual(actualBetListData.TicketData));
        }

        [Fact]
        public void GetBetList_UserIsInLimitedDateRange_UseLimitedDate()
        {
            // Arrange
            var limitedDate = DateTime.Today.AddDays(1);
            var param = new MemberBetListParam(1, Enumerable.Empty<int>(), DateTime.Now, DateTime.Now, limitedDate, string.Empty, string.Empty);

            // Act
            var actualBetListData = memberBetListService.GetBetList(param).GetAwaiter().GetResult();

            // Assert
            dataFixture.DynamicRepository.Received().FetchMultipleAsync<TicketDto, TicketData>(Arg.Is<GetBetListAllByDateCriteria>(i => i.FromDate == limitedDate));
        }
    }
}
