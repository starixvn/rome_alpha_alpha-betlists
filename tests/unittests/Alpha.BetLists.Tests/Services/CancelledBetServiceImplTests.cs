namespace Alpha.BetLists.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using Alpha.BetLists.Criteria.CancelledBet;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Tests.Fixtures;
    using Alpha.BetLists.Tests.TestData;
    using BetLists.Params;
    using Fanex.BetList.Core.Entities;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "CancelledBetService")]
    [Collection("FanexDataCollection")]
    public class CancelledBetServiceImplTests
    {
        private readonly FanexDataFixture dataFixture;
        private readonly CancelledBetServiceImpl service;

        public CancelledBetServiceImplTests(FanexDataFixture dataFixture)
        {
            dataFixture.DynamicRepository
                .FetchMultipleAsync<TicketDto, TicketData>(Arg.Any<GetCancelledBetCriteria>()).Returns(BetListDataTest.BetListDataTuple);

            service = new CancelledBetServiceImpl(dataFixture.DynamicRepository);
            this.dataFixture = dataFixture;
        }

        [Fact]
        public void GetCancelledBet_Always_CallDynamicRepository()
        {
            //// Arrange
            var parameter = new CancelledBetParam(1, DateTime.Now, DateTime.Now, DateTime.Now, 2, new List<int> { 1, 2, 3 }, "1,2,3", "EN", 50, 1);

            //// Act
            service.GetCancelledBet(parameter).GetAwaiter().GetResult();

            //// Assert
            dataFixture.DynamicRepository.Received().FetchMultipleAsync<TicketDto, TicketData>(Arg.Any<GetCancelledBetCriteria>());
        }
    }
}
