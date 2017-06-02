namespace Alpha.BetLists.Builders.Tests.PTComm
{
    using System.Collections.Generic;
    using BetLists.Tests.TestData;
    using Builders.PTComm;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Models;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    public class PTComm33Tests
    {
        private ITicket ticket;
        private List<ITicketData> ticketData;

        public PTComm33Tests()
        {
            ticketData = new List<ITicketData>
            {
                new TicketData { BetTypeId = 31, RefNo = "1111", SuperPT = 4, MasterPT = 3, AgentPT = 2 },
                new TicketData { BetTypeId = 32, RefNo = "1111", SuperPT = 4, MasterPT = 3, AgentPT = 2 },
                new TicketData { BetTypeId = 1231, RefNo = "1111", SuperPT = 4, MasterPT = 3, AgentPT = 2 },
                new TicketData { BetTypeId = 1232, RefNo = "1111", SuperPT = 4, MasterPT = 3, AgentPT = 2 },
            };

            ticket = BetListDataTest.Ticket;
            ticket.TransId = 1111;
            ticket.RefNo = "1111";
        }

        [Theory]
        [InlineData(33, 31)]
        [InlineData(1233, 1231)]
        public void WinBetId_Always_ReturnValue(int betTypeId, int expectedWinBetId)
        {
            // Arrange
            ticket.BetTypeId = betTypeId;

            // Act
            var actualPTComm = new PTComm33(ticket, ticketData, UserLevel.Agent);

            // Assert
            Assert.Equal(expectedWinBetId, actualPTComm.WinBetId);
        }

        [Theory]
        [InlineData(33, 32)]
        [InlineData(1233, 1232)]
        public void PlaceBetId_Always_ReturnValue(int betTypeId, int expectedPlaceBetId)
        {
            // Arrange
            ticket.BetTypeId = betTypeId;

            // Act
            var actualPTComm = new PTComm33(ticket, ticketData, UserLevel.Agent);

            // Assert
            Assert.Equal(expectedPlaceBetId, actualPTComm.PlaceBetId);
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(4, 4)]
        public void PositionTakingWin_ByUserLevel_IsNotVirtualRacing_ReturnValue(byte userLevelId, decimal expectedPositionTaking)
        {
            // Arrange
            ticket.BetTypeId = 33;

            // Act
            var actualPTComm = new PTComm33(ticket, ticketData, Enumeration.FromValue<UserLevel>(userLevelId));

            // Assert
            Assert.Equal(Formatter.FormatNumber2(expectedPositionTaking, null) + "%", actualPTComm.PositionTakingWin);
        }

        [Theory]
        [InlineData(2, 0)]
        [InlineData(3, 0)]
        [InlineData(4, 0)]
        public void PositionTakingWin_ByUserLevel_IsNotVirtualRacing_NullData_ReturnValue(byte userLevelId, decimal expectedPositionTaking)
        {
            // Arrange
            ticket.BetTypeId = 33;

            // Act
            var actualPTComm = new PTComm33(ticket, new List<ITicketData>(), Enumeration.FromValue<UserLevel>(userLevelId));

            // Assert
            Assert.Equal(Formatter.FormatNumber2(expectedPositionTaking, null) + "%", actualPTComm.PositionTakingWin);
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(4, 4)]
        public void PositionTakingWin_ByUserLevel_IsVirtualRacing_ReturnValue(byte userLevelId, decimal expectedPositionTaking)
        {
            // Arrange
            ticket.BetTypeId = 1233;

            // Act
            var actualPTComm = new PTComm33(ticket, ticketData, Enumeration.FromValue<UserLevel>(userLevelId));

            // Assert
            Assert.Equal(Formatter.FormatNumber2(expectedPositionTaking, null) + "%", actualPTComm.PositionTakingWin);
        }

        [Theory]
        [InlineData(2, 0)]
        [InlineData(3, 0)]
        [InlineData(4, 0)]
        public void PositionTakingWin_ByUserLevel_IsVirtualRacing_NullData_ReturnValue(byte userLevelId, decimal expectedPositionTaking)
        {
            // Arrange
            ticket.BetTypeId = 1233;

            // Act
            var actualPTComm = new PTComm33(ticket, new List<ITicketData>(), Enumeration.FromValue<UserLevel>(userLevelId));

            // Assert
            Assert.Equal(Formatter.FormatNumber2(expectedPositionTaking, null) + "%", actualPTComm.PositionTakingWin);
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(4, 4)]
        public void PositionTakingPlace_ByUserLevel_IsNotVirtualRacing_ReturnValue(byte userLevelId, decimal expectedPositionTaking)
        {
            // Arrange
            ticket.BetTypeId = 33;

            // Act
            var actualPTComm = new PTComm33(ticket, ticketData, Enumeration.FromValue<UserLevel>(userLevelId));

            // Assert
            Assert.Equal(Formatter.FormatNumber2(expectedPositionTaking, null) + "%", actualPTComm.PositionTakingPlace);
        }

        [Theory]
        [InlineData(2, 0)]
        [InlineData(3, 0)]
        [InlineData(4, 0)]
        public void PositionTakingPlace_ByUserLevel_IsNotVirtualRacing_NullData_ReturnValue(byte userLevelId, decimal expectedPositionTaking)
        {
            // Arrange
            ticket.BetTypeId = 33;

            // Act
            var actualPTComm = new PTComm33(ticket, new List<ITicketData>(), Enumeration.FromValue<UserLevel>(userLevelId));

            // Assert
            Assert.Equal(Formatter.FormatNumber2(expectedPositionTaking, null) + "%", actualPTComm.PositionTakingPlace);
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(4, 4)]
        public void PositionTakingPlace_ByUserLevel_IsVirtualRacing_ReturnValue(byte userLevelId, decimal expectedPositionTaking)
        {
            // Arrange
            ticket.BetTypeId = 1233;

            // Act
            var actualPTComm = new PTComm33(ticket, ticketData, Enumeration.FromValue<UserLevel>(userLevelId));

            // Assert
            Assert.Equal(Formatter.FormatNumber2(expectedPositionTaking, null) + "%", actualPTComm.PositionTakingPlace);
        }

        [Theory]
        [InlineData(2, 0)]
        [InlineData(3, 0)]
        [InlineData(4, 0)]
        public void PositionTakingPlace_ByUserLevel_IsVirtualRacing_NullData_ReturnValue(byte userLevelId, decimal expectedPositionTaking)
        {
            // Arrange
            ticket.BetTypeId = 1233;

            // Act
            var actualPTComm = new PTComm33(ticket, new List<ITicketData>(), Enumeration.FromValue<UserLevel>(userLevelId));

            // Assert
            Assert.Equal(Formatter.FormatNumber2(expectedPositionTaking, null) + "%", actualPTComm.PositionTakingPlace);
        }
    }
}