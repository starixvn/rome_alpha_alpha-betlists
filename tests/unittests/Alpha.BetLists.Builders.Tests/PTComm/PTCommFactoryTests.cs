namespace Alpha.BetLists.Builders.Tests.PTComm
{
    using System;
    using System.Linq;
    using BetLists.Tests.TestData;
    using Builders.PTComm;
    using Fanex.BetList.Core.Entities;
    using Models;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    public class PTCommFactoryTests
    {
        [Theory]
        [InlineData(33, typeof(PTComm33))]
        [InlineData(1233, typeof(PTComm33))]
        [InlineData(500, typeof(PTComm500))]
        [InlineData(501, typeof(PTComm501))]
        [InlineData(71, typeof(PTComm71))]
        [InlineData(73, typeof(PTComm71))]
        [InlineData(1, typeof(PTComm))]
        [InlineData(2198, typeof(PTComm2198))]
        [InlineData(2398, typeof(PTComm2198))]
        [InlineData(2998, typeof(PTComm2198))]
        public void CreateInstance_ByBetType_ReturnPTComm(int betTypeId, Type expectedType)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.BetTypeId = betTypeId;

            // Act
            var actualPTCommIns = PTCommFactory.CreateInstance(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

            // Assert
            Assert.IsType(expectedType, actualPTCommIns);
        }
    }
}