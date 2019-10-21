namespace Alpha.BetLists.Builders.Tests.PTComm
{
    using Builders.PTComm;
    using Fanex.BetList.Core.Entities;
    using Models;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    public class PTComm501Tests
    {
        [Theory]
        [InlineData(BetStatus.Running)]
        [InlineData(BetStatus.Waiting)]
        public void VisibleCommision_IsNotIMBCMatch_BetStatusIsRunningOrWaiting_ReturnFalse(string status)
        {
            // Arrange
            var ticket = new Ticket();
            ticket.SuperDiscount = 1;
            ticket.MasterDiscount = 2;
            ticket.AgentDiscount = 3;
            ticket.Status = status;

            // Act
            var actualPTComm = new PTComm501(ticket, UserLevel.Agent);

            // Assert
            Assert.False(actualPTComm.VisibleCommision);
        }

        [Theory]
        [InlineData(BetStatus.Won)]
        [InlineData(BetStatus.Lose)]
        [InlineData(BetStatus.Draw)]
        [InlineData(BetStatus.Reject)]
        [InlineData(BetStatus.Refund)]
        [InlineData(BetStatus.Void)]
        public void VisibleCommision_IsNotIMBCMatch_BetStatusIsNotRunningOrWaiting_ReturnTrue(string status)
        {
            // Arrange
            var ticket = new Ticket();
            ticket.SuperDiscount = 1;
            ticket.MasterDiscount = 2;
            ticket.AgentDiscount = 3;
            ticket.Status = status;

            // Act
            var actualPTComm = new PTComm501(ticket, UserLevel.Agent);

            // Assert
            Assert.True(actualPTComm.VisibleCommision);
        }

        [Fact]
        public void VisibileCommisionPercent_IsNotIMBCMatch_ReturnBaseValue()
        {
            // Arrange
            var ticket = new Ticket();
            ticket.SuperDiscount = 1;
            ticket.MasterDiscount = 2;
            ticket.AgentDiscount = 3;

            // Act
            var actualPTComm = new PTComm501(ticket, UserLevel.Agent);

            // Assert
            Assert.True(actualPTComm.VisibileCommisionPercent);
        }
    }
}