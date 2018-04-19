namespace Alpha.BetLists.Builders.Tests.PTComm
{
    using BetLists.Tests.TestData;
    using Builders.PTComm;
    using Fanex.BetList.Core.Utils;
    using Models;
    using Xunit;

    [Trait("Category", "ProgressiveJackpot")]
    public class PTComm2198Tests
    {
        [Fact]
        public void PositionTaking_ValueIsLessThanZero_ReturnCorrectValue()
        {
            // Arrange
            var ptValue = -1;
            var ticket = BetListDataTest.Ticket;
            ticket.AgentWinlost = ptValue;
            var expectedValue = string.Format(ConvertByBetType.Minus, string.Format(Formatter.DECIMALFORMAT40, ptValue));

            // Act
            var actualPTComm = new PTComm2198(ticket, UserLevel.Agent);
            actualPTComm.InitPtCommData();

            // Assert
            Assert.Equal(expectedValue, actualPTComm.PositionTaking);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void PositionTaking_ValueIsGreaterThanOrEqualZero_ReturnCorrectValue(decimal ptValue)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.AgentWinlost = ptValue;
            var expectedValue = string.Format(Formatter.DECIMALFORMAT40, ptValue);

            // Act
            var actualPTComm = new PTComm2198(ticket, UserLevel.Agent);
            actualPTComm.InitPtCommData();

            // Assert
            Assert.Equal(expectedValue, actualPTComm.PositionTaking);
        }
    }
}