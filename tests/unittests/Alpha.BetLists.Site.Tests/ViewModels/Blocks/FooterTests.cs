namespace Alpha.BetLists.Site.Tests.ViewModels.Blocks
{
    using System.Linq;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Site.ViewModels.Blocks;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    public class FooterTests
    {
        [Fact]
        public void SubTotalColSpan_NotShowMemberName_ShowDefaultValue()
        {
            // Act
            var footer = new Footer(BetListDataTest.BetListData, UserLevel.Agent, showMemberName: false);

            // Assert
            Assert.Equal(5, footer.SubTotalColSpan);
        }

        [Fact]
        public void SubTotalColSpan_ShowMemberName_ShowValue()
        {
            // Act
            var footer = new Footer(BetListDataTest.BetListData, UserLevel.Agent, showMemberName: true);

            // Assert
            Assert.Equal(5 + 1, footer.SubTotalColSpan);
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(4, 4)]
        public void StatusColSpan_ByUserLevel_ShowValue(byte userLevelId, int expectedStatusColSpan)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(userLevelId);

            // Act
            var footer = new Footer(BetListDataTest.BetListData, userLevel, false);

            // Assert
            Assert.Equal(expectedStatusColSpan, footer.StatusColSpan);
        }

        [Fact]
        public void TotalWinLoss_Always_CalculateInCtor()
        {
            // Arrange
            var expectedTotalWinLoss = BetListDataTest.Tickets.Where(ticket => ticket.BetTypeId != 500).Sum(ticket => ticket.Winlost);

            // Act
            var footer = new Footer(BetListDataTest.BetListData, UserLevel.Agent, false);

            // Assert
            Assert.Equal(expectedTotalWinLoss, footer.TotalWinLoss);
        }

        [Fact]
        public void TotalCommission_Always_CalculateInCtor()
        {
            // Arrange
            var expectedTotalCommission = BetListDataTest.Tickets.Sum(ticket => ticket.PlayerComm);

            // Act
            var footer = new Footer(BetListDataTest.BetListData, UserLevel.Agent, false);

            // Assert
            Assert.Equal(expectedTotalCommission, footer.TotalCommission);
        }

        [Fact]
        public void SubTotalWinLoss_ExcludingStatusHasValue_CalculateInCtor()
        {
            // Arrange
            BetListDataTest.Tickets.First().BetTypeId = 500;
            BetListDataTest.Tickets.Last().Status = BetStatus.Running;
            var totalWinLoss =
                BetListDataTest.Tickets.Sum(ticket => ticket.Winlost) -
                BetListDataTest.Tickets.First().Winlost -
                BetListDataTest.Tickets.Last().Winlost;

            var expectedSubTotalWinLoss = ConvertByBetType.Winloss(totalWinLoss);
            var excludingStatus = new string[] { BetStatus.Running };
            // Act
            var footer = new Footer(BetListDataTest.BetListData, UserLevel.Agent, false, excludingStatus);

            // Assert
            Assert.Equal(expectedSubTotalWinLoss, footer.SubTotalWinLoss);
        }

        [Fact]
        public void SubTotalWinLoss_ExcludingStatusIsNull_CalculateInCtor()
        {
            // Arrange
            var totalWinLoss = BetListDataTest.Tickets.Sum(ticket => ticket.Winlost);
            var expectedSubTotalWinLoss = ConvertByBetType.Winloss(totalWinLoss);

            // Act
            var footer = new Footer(BetListDataTest.BetListData, UserLevel.Agent, false);

            // Assert
            Assert.Equal(expectedSubTotalWinLoss, footer.SubTotalWinLoss);
        }

        [Fact]
        public void SubTotalCommission_ExcludingStatusHasValue_CalculateInCtor()
        {
            // Arrange
            BetListDataTest.Tickets.Last().Status = BetStatus.Running;
            var totalCommission = BetListDataTest.Tickets.Sum(ticket => ticket.PlayerComm)
                - BetListDataTest.Tickets.Last().PlayerComm;
            var expectedSubTotalCommission = ConvertByBetType.Comm(totalCommission);
            var excludingStatus = new string[] { BetStatus.Running };

            // Act
            var footer = new Footer(BetListDataTest.BetListData, UserLevel.Agent, false, excludingStatus);

            // Assert
            Assert.Equal(expectedSubTotalCommission, footer.SubTotalCommission);
        }

        [Fact]
        public void SubTotalCommission_ExcludingStatusIsNull_CalculateInCtor()
        {
            // Arrange
            var totalCommission = BetListDataTest.Tickets.Sum(ticket => ticket.PlayerComm);
            var expectedSubTotalCommission = ConvertByBetType.Comm(totalCommission);

            // Act
            var footer = new Footer(BetListDataTest.BetListData, UserLevel.Agent, false);

            // Assert
            Assert.Equal(expectedSubTotalCommission, footer.SubTotalCommission);
        }

        [Fact]
        public void Total_Always_CalculateInCtor()
        {
            // Arrange
            var totalWinLoss = BetListDataTest.Tickets.Where(ticket => ticket.BetTypeId != 500).Sum(ticket => ticket.Winlost);
            var totalCommission = BetListDataTest.Tickets.Sum(ticket => ticket.PlayerComm);
            var expectedTotal = ConvertByBetType.Winloss(totalWinLoss + totalCommission);

            // Act
            var footer = new Footer(BetListDataTest.BetListData, UserLevel.Agent, false);

            // Assert
            Assert.Equal(expectedTotal, footer.Total);
        }
    }
}