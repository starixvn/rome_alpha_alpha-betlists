namespace Alpha.BetLists.Site.Tests.ViewModels.Blocks
{
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Site.ViewModels.Blocks;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    public class WinLostBlockViewModelTests
    {
        [Theory]
        [InlineData(BetStatus.Running)]
        [InlineData(BetStatus.Waiting)]
        public void WinLost_StatusIsRunningOrWaiting_ShowHyphen(string status)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.Status = status;

            // Act
            var viewModel = new WinLostBlockViewModel(ticket);

            // Assert
            Assert.Equal("-", viewModel.WinLost);
        }

        [Theory]
        [InlineData(500, "(NW) 0.00")]
        [InlineData(1, "")]
        public void WinLost_StatusIsVoid_ShowValue(int betTypeId, string expectedWinLost)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.BetTypeId = betTypeId;
            ticket.Status = BetStatus.Void;

            // Act
            var viewModel = new WinLostBlockViewModel(ticket);

            // Assert
            Assert.Equal(expectedWinLost, viewModel.WinLost);
        }

        [Fact]
        public void WinLost_StatusIsReject_ShowValue()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.Status = BetStatus.Reject;

            // Act
            var viewModel = new WinLostBlockViewModel(ticket);

            // Assert
            Assert.Equal("0.00", viewModel.WinLost);
        }

        [Theory]
        [InlineData(2198)]
        [InlineData(2398)]
        [InlineData(2998)]
        public void WinLostAndComm_BetTypeIsJackpotContribution_ShowValue(int betType)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.BetTypeId = betType;

            // Act
            var viewModel = new WinLostBlockViewModel(ticket);

            // Assert
            Assert.True(viewModel.WinLost.Equals("-") && viewModel.Comm.Equals(string.Empty));
        }

        [Theory]
        [InlineData(BetStatus.Won)]
        [InlineData(BetStatus.Lose)]
        [InlineData(BetStatus.Draw)]
        [InlineData(BetStatus.Refund)]
        public void WinLost_Default_BetTypeIs500_ShowValue(string status)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.ActualStake = 100;
            ticket.BetTypeId = 500;
            ticket.Status = status;

            // Act
            var viewModel = new WinLostBlockViewModel(ticket);

            // Assert
            Assert.Equal($"(NW) {ConvertByBetType.Winloss(ticket.ActualStake)}", viewModel.WinLost);
        }

        [Theory]
        [InlineData(BetStatus.Won)]
        [InlineData(BetStatus.Lose)]
        [InlineData(BetStatus.Draw)]
        [InlineData(BetStatus.Refund)]
        public void WinLost_Default_BetTypeIsNot500_ShowValue(string status)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.Winlost = 101;
            ticket.BetTypeId = 1;
            ticket.Status = status;

            // Act
            var viewModel = new WinLostBlockViewModel(ticket);

            // Assert
            Assert.Equal(ConvertByBetType.Winloss(ticket.Winlost), viewModel.WinLost);
        }

        [Theory]
        [InlineData(BetStatus.Running)]
        [InlineData(BetStatus.Waiting)]
        [InlineData(BetStatus.Reject)]
        public void Comm_StatusIsRunningOrWaitingOrReject_ShowEmpty(string status)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.Status = status;

            // Act
            var viewModel = new WinLostBlockViewModel(ticket);

            // Assert
            Assert.Equal(string.Empty, viewModel.Comm);
        }

        [Fact]
        public void Comm_StatusIsVoid_ShowEmpty()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.Status = BetStatus.Void;

            // Act
            var viewModel = new WinLostBlockViewModel(ticket);

            // Assert
            Assert.Equal("0.00", viewModel.Comm);
        }

        [Theory]
        [InlineData(BetStatus.Won)]
        [InlineData(BetStatus.Lose)]
        [InlineData(BetStatus.Draw)]
        [InlineData(BetStatus.Refund)]
        public void Comm_Default_BetTypeIs500_ShowValue(string status)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.BetTypeId = 500;
            ticket.PlayerComm = 500;
            ticket.Status = status;

            // Act
            var viewModel = new WinLostBlockViewModel(ticket);

            // Assert
            Assert.Equal(ConvertByBetType.Winloss(ticket.PlayerComm), viewModel.Comm);
        }

        [Theory]
        [InlineData(BetStatus.Won)]
        [InlineData(BetStatus.Lose)]
        [InlineData(BetStatus.Draw)]
        [InlineData(BetStatus.Refund)]
        public void Comm_Default_BetTypeIsNot500_ShowValue(string status)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.BetTypeId = 1;
            ticket.PlayerComm = 500;
            ticket.Status = status;

            // Act
            var viewModel = new WinLostBlockViewModel(ticket);

            // Assert
            Assert.Equal(ConvertByBetType.Comm(ticket.PlayerComm), viewModel.Comm);
        }

        [Fact]
        public void CommVisibility_BetTypeIdIs501_IsNotIMBCMatch_ShowValue()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.SuperDiscount = 1;
            ticket.MasterDiscount = 1;
            ticket.AgentDiscount = 1;
            ticket.BetTypeId = 501;

            // Act
            var viewModel = new WinLostBlockViewModel(ticket);

            // Assert
            Assert.True(!viewModel.CommVisibility);
        }

        [Theory]
        [InlineData(2198)]
        [InlineData(2398)]
        [InlineData(2998)]
        public void CommVisibility_BetTypeIdIsJackpotContribution_IsNotIMBCMatch_ShowValue(int betType)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.SuperDiscount = 1;
            ticket.MasterDiscount = 1;
            ticket.AgentDiscount = 1;
            ticket.BetTypeId = betType;

            // Act
            var viewModel = new WinLostBlockViewModel(ticket);

            // Assert
            Assert.True(!viewModel.CommVisibility);
        }

        [Fact]
        public void CommVisibility_BetTypeIdIsNot501_ShowTrue()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.BetTypeId = 1;

            // Act
            var viewModel = new WinLostBlockViewModel(ticket);

            // Assert
            Assert.True(viewModel.CommVisibility);
        }
    }
}