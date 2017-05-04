namespace Alpha.BetLists.Builders.Tests.PTComm
{
    using BetLists.Tests.TestData;
    using Builders.PTComm;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Models;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    public class PTCommTests
    {
        [Theory]
        [InlineData(BetStatus.Running)]
        [InlineData(BetStatus.Waiting)]
        public void VisiblePositionTaking_BetStatusIsRunningOrWaiting_ReturnFalse(string status)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.Status = status;

            // Act
            var actualPTComm = new PTComm(ticket, UserLevel.Agent);

            // Assert
            Assert.False(actualPTComm.VisiblePositionTaking);
        }

        [Theory]
        [InlineData(BetStatus.Won)]
        [InlineData(BetStatus.Lose)]
        [InlineData(BetStatus.Draw)]
        [InlineData(BetStatus.Reject)]
        [InlineData(BetStatus.Refund)]
        public void VisiblePositionTaking_BetStatusIsNotRunningOrWaiting_ReturnTrue(string status)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.Status = status;

            // Act
            var actualPTComm = new PTComm(ticket, UserLevel.Agent);

            // Assert
            Assert.True(actualPTComm.VisiblePositionTaking);
        }

        [Theory]
        [InlineData(BetStatus.Running)]
        [InlineData(BetStatus.Waiting)]
        public void VisibleCommision_BetStatusIsRunningOrWaiting_ReturnFalse(string status)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.Status = status;

            // Act
            var actualPTComm = new PTComm(ticket, UserLevel.Agent);

            // Assert
            Assert.False(actualPTComm.VisibleCommision);
        }

        [Theory]
        [InlineData(BetStatus.Won)]
        [InlineData(BetStatus.Lose)]
        [InlineData(BetStatus.Draw)]
        [InlineData(BetStatus.Reject)]
        [InlineData(BetStatus.Refund)]
        public void VisibleCommision_BetStatusIsNotRunningOrWaiting_ReturnTrue(string status)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.Status = status;

            // Act
            var actualPTComm = new PTComm(ticket, UserLevel.Agent);

            // Assert
            Assert.True(actualPTComm.VisibleCommision);
        }

        [Fact]
        public void VisibilePositionTakingPercent_Always_ReturnTrue()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;

            // Act
            var actualPTComm = new PTComm(ticket, UserLevel.Agent);

            // Assert
            Assert.True(actualPTComm.VisibilePositionTakingPercent);
        }

        [Fact]
        public void VisibileCommisionPercent_Always_ReturnTrue()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;

            // Act
            var actualPTComm = new PTComm(ticket, UserLevel.Agent);

            // Assert
            Assert.True(actualPTComm.VisibileCommisionPercent);
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(4, 4)]
        public void PositionTaking_ByUserLevel_ReturnValue(byte userLevelId, decimal expectedPTValue)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.AgentWinlost = 2;
            ticket.MasterWinlost = 3;
            ticket.SuperWinlost = 4;

            // Act
            var actualPTComm = new PTComm(ticket, Enumeration.FromValue<UserLevel>(userLevelId));
            actualPTComm.InitPTCommData();

            // Assert
            Assert.Equal(ConvertByBetType.PtValue(expectedPTValue), actualPTComm.PositionTaking);
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(4, 4)]
        public void PositionTakingPercent_ByUserLevel_ReturnValue(byte userLevelId, decimal expectedPTPercent)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.AgentPositionTaking = 2;
            ticket.MasterPositionTaking = 3;
            ticket.SuperPositionTaking = 4;

            // Act
            var actualPTComm = new PTComm(ticket, Enumeration.FromValue<UserLevel>(userLevelId));
            actualPTComm.InitPTCommData();

            // Assert
            Assert.Equal(ConvertByBetType.Pt(expectedPTPercent) + "%", actualPTComm.PositionTakingPercent);
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(4, 4)]
        public void Commission_ByUserLevel_ReturnValue(byte userLevelId, decimal expectedComission)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.Comm = 2;
            ticket.AgentComm = 3;
            ticket.SuperComm = 4;

            // Act
            var actualPTComm = new PTComm(ticket, Enumeration.FromValue<UserLevel>(userLevelId));
            actualPTComm.InitPTCommData();

            // Assert
            Assert.Equal(ConvertByBetType.Comm(expectedComission), actualPTComm.Commission);
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(4, 4)]
        public void CommissionPercent_ByUserLevel_ComStatusEqual1_ReturnValue(byte userLevelId, decimal expectedComissionPercent)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.AgentDiscount = 2;
            ticket.MasterDiscount = 3;
            ticket.SuperDiscount = 4;
            ticket.CommStatus = "1";

            // Act
            var actualPTComm = new PTComm(ticket, Enumeration.FromValue<UserLevel>(userLevelId));
            actualPTComm.InitPTCommData();

            // Assert
            Assert.Equal($"2x {ConvertByBetType.Comm_Percentage(expectedComissionPercent)}%", actualPTComm.CommissionPercent);
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(4, 4)]
        public void CommissionPercent_ByUserLevel_ComStatusNotEqual1_ReturnValue(byte userLevelId, decimal expectedComissionPercent)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.AgentDiscount = 2;
            ticket.MasterDiscount = 3;
            ticket.SuperDiscount = 4;
            ticket.CommStatus = "0";

            // Act
            var actualPTComm = new PTComm(ticket, Enumeration.FromValue<UserLevel>(userLevelId));
            actualPTComm.InitPTCommData();

            // Assert
            Assert.Equal($"{ConvertByBetType.Comm_Percentage(expectedComissionPercent)}%", actualPTComm.CommissionPercent);
        }
    }
}