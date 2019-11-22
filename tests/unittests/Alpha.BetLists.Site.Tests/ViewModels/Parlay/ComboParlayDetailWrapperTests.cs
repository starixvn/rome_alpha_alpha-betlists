namespace Alpha.BetLists.Site.Tests.ViewModels.Parlay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using BetLists.Models.Parlay;
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using NSubstitute;
    using Site.ViewModels.Parlay;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    public class ComboParlayDetailWrapperTests
    {
        private readonly ITicketHelper ticketHelper;

        public ComboParlayDetailWrapperTests()
        {
            ticketHelper = Substitute.For<ITicketHelper>();
        }

        [Fact]
        public void ComboParlayDetail_Always_SetInCtor()
        {
            // Act
            var viewModel = new ComboParlayDetailWrapper(BetListDataTest.ComboParlayDetails.First(), BetListDataTest.MatchDetails, ticketHelper);

            // Assert
            Assert.Equal(BetListDataTest.ComboParlayDetails.First(), viewModel.ComboParlayDetail);
        }

        [Fact]
        public void MatchDetails_Always_SetInCtor()
        {
            // Act
            var viewModel = new ComboParlayDetailWrapper(BetListDataTest.ComboParlayDetails.First(), BetListDataTest.MatchDetails, ticketHelper);

            // Assert
            Assert.Equal(BetListDataTest.MatchDetails, viewModel.MatchDetails);
        }

        [Fact]
        public void WinLostDate_Always_SetInCtor()
        {
            // Arrange
            var detail = BetListDataTest.ComboParlayDetails.First();
            var expectedWinLostDate = Formatter.ShortDateFormat(detail.WinLostDate);

            // Act
            var viewModel = new ComboParlayDetailWrapper(detail, BetListDataTest.MatchDetails, ticketHelper);

            // Assert
            Assert.Equal(expectedWinLostDate, viewModel.WinLostDate);
        }

        [Fact]
        public void WinLost_Always_SetInCtor()
        {
            // Arrange
            var detail = BetListDataTest.ComboParlayDetails.First();
            var expectedWinLost = ConvertByBetType.Winloss(detail.WinLost);

            // Act
            var viewModel = new ComboParlayDetailWrapper(detail, BetListDataTest.MatchDetails, ticketHelper);

            // Assert
            Assert.Equal(expectedWinLost, viewModel.WinLost);
        }

        [Fact]
        public void Comm_Always_SetInCtor()
        {
            // Arrange
            var detail = BetListDataTest.ComboParlayDetails.First();
            var expectedComm = ConvertByBetType.Comm(detail.PlayerComm);

            // Act
            var viewModel = new ComboParlayDetailWrapper(detail, BetListDataTest.MatchDetails, ticketHelper);

            // Assert
            Assert.Equal(expectedComm, viewModel.Comm);
        }

        [Fact]
        public void Stake_Always_SetInCtor()
        {
            // Arrange
            var detail = BetListDataTest.ComboParlayDetails.First();
            var expectedStake = ConvertByBetType.Stake(detail.Stake);

            // Act
            var viewModel = new ComboParlayDetailWrapper(detail, BetListDataTest.MatchDetails, ticketHelper);

            // Assert
            Assert.Equal(expectedStake, viewModel.Stake);
        }

        [Theory]
        [InlineData(BetStatus.Running, "Running")]
        [InlineData(BetStatus.Reject, "Reject")]
        [InlineData(BetStatus.Won, "Won")]
        [InlineData(BetStatus.Lose, "Lose")]
        [InlineData(BetStatus.Draw, "Draw")]
        [InlineData(BetStatus.Waiting, "Waiting")]
        [InlineData(BetStatus.Void, "Void")]
        [InlineData(BetStatus.Refund, "Refund")]
        [InlineData("435345242", "")]
        public void Status_Always_SetInCtor(string status, string expectedStatus)
        {
            // Arrange
            var detail = new ComboParlayDetail(DateTime.Today, 1, 1, 1, status, 10, 10, 10, 10);

            // Act
            var viewModel = new ComboParlayDetailWrapper(detail, BetListDataTest.MatchDetails, ticketHelper);

            // Assert
            Assert.Equal(expectedStatus, viewModel.Status);
        }

        [Fact]
        public void Events_StatusIsNotLose_SetInCtor()
        {
            // Arrange
            var detail = new ComboParlayDetail(DateTime.Today, 1, 100, 1, "running", 10, 10, 10, 10);
            var matchDetails = new List<MatchDetail>
            {
                new MatchDetail(100, 1, 2)
            };

            ticketHelper.GetTeamNameById(1).Returns("HomeTeamName");
            ticketHelper.GetTeamNameById(2).Returns("AwayTeamName");

            var expectedEvents = new List<string>
            {
                "HomeTeamName - vs - AwayTeamName"
            };

            // Act
            var viewModel = new ComboParlayDetailWrapper(detail, matchDetails, ticketHelper);

            // Assert
            Assert.Equal(expectedEvents, viewModel.Events);
        }

        [Fact]
        public void Events_StatusIsLose_SetInCtor()
        {
            // Arrange
            var detail = new ComboParlayDetail(DateTime.Today, 1, 100, 1, "lose", 10, 10, 10, 10);
            var expectedEvents = new List<string>();

            // Act
            var viewModel = new ComboParlayDetailWrapper(detail, BetListDataTest.MatchDetails, ticketHelper);

            // Assert
            Assert.Equal(expectedEvents, viewModel.Events);
        }

        [Fact]
        public void Odds_StatusIsNotLose_ShowOdds()
        {
            // Arrange
            var detail = new ComboParlayDetail(DateTime.Today, 1, 100, 1, "running", 0, 10, 10, 10);
            var expectedOdds = ConvertByBetType.CommValue(detail.Odds);

            // Act
            var viewModel = new ComboParlayDetailWrapper(detail, BetListDataTest.MatchDetails, ticketHelper);

            // Assert
            Assert.Equal(expectedOdds, viewModel.Odds);
        }

        [Fact]
        public void Odds_StatusIsLose_ShowHyphen()
        {
            // Arrange
            var detail = new ComboParlayDetail(DateTime.Today, 1, 100, 1, "lose", 0, 10, 10, 10);
            var expectedOdds = "0.0000";

            // Act
            var viewModel = new ComboParlayDetailWrapper(detail, BetListDataTest.MatchDetails, ticketHelper);

            // Assert
            Assert.Equal(expectedOdds, viewModel.Odds);
        }
    }
}