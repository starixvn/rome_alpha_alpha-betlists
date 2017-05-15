namespace Alpha.BetLists.Builders.Tests.PTComm
{
    using Builders.PTComm;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Models;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    public class PTComm500Tests
    {
        [Theory]
        [InlineData(2, -2)]
        [InlineData(3, 3 - 2)]
        [InlineData(4, 4 - 3)]
        public void CommissionPercent_ByUserLevel_ComStatusEqual1_ReturnValue(byte userLevelId, decimal expectedComissionPercent)
        {
            // Arrange
            var ticket = new Ticket();
            ticket.AgentDiscount = 2;
            ticket.MasterDiscount = 3;
            ticket.SuperDiscount = 4;
            ticket.CommStatus = "1";

            // Act
            var actualPTComm = new PTComm500(ticket, Enumeration.FromValue<UserLevel>(userLevelId));
            actualPTComm.InitPtCommData();

            // Assert
            Assert.Equal($"2x {ConvertByBetType.Comm_Percentage(expectedComissionPercent)}%", actualPTComm.CommissionPercent);
        }

        [Theory]
        [InlineData(2, -2)]
        [InlineData(3, 3 - 2)]
        [InlineData(4, 4 - 3)]
        public void CommissionPercent_ByUserLevel_ComStatusNotEqual1_ReturnValue(byte userLevelId, decimal expectedComissionPercent)
        {
            // Arrange
            var ticket = new Ticket();
            ticket.AgentDiscount = 2;
            ticket.MasterDiscount = 3;
            ticket.SuperDiscount = 4;
            ticket.CommStatus = "0";

            // Act
            var actualPTComm = new PTComm500(ticket, Enumeration.FromValue<UserLevel>(userLevelId));
            actualPTComm.InitPtCommData();

            // Assert
            Assert.Equal($"{ConvertByBetType.Comm_Percentage(expectedComissionPercent)}%", actualPTComm.CommissionPercent);
        }
    }
}