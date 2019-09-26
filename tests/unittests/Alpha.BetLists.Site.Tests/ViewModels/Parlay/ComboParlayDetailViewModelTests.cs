namespace Alpha.BetLists.Site.Tests.ViewModels.Parlay
{
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using BetLists.Models;
    using BetLists.Models.Parlay;
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using Ploeh.AutoFixture;
    using Requests.Parlay;
    using Site.ViewModels.Parlay;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    [Collection("BetListCollection")]
    public class ComboParlayDetailViewModelTests
    {
        private readonly IUser user;
        private readonly ITicketHelper ticketHelper;
        private readonly CompareLogic comparer;
        private ComboParlayDetailQuery query;

        public ComboParlayDetailViewModelTests(BetListFixture betListFixture)
        {
            user = betListFixture.User;
            ticketHelper = betListFixture.TicketHelper;
            query = new ComboParlayDetailQuery(user);
            query.BetTeam = "0";
            comparer = new CompareLogic();
        }

        [Fact]
        public void Query_Always_SetInCtor()
        {
            // Act
            var viewModel = new ComboParlayDetailViewModel(query, BetListDataTest.ComboParlayDetailData, ticketHelper, false);

            // Assert
            Assert.Equal(query, viewModel.Query);
        }

        [Fact]
        public void ComboParlayDetailWrappers_Always_SetInCtor()
        {
            // Arrange
            var comboParlayDetailWrappers = new List<ComboParlayDetailWrapper>(BetListDataTest.ComboParlayDetails
                    .Select(d => new ComboParlayDetailWrapper(d, BetListDataTest.MatchDetails, ticketHelper)));

            // Act
            var viewModel = new ComboParlayDetailViewModel(query, BetListDataTest.ComboParlayDetailData, ticketHelper, false);

            // Assert
            Assert.True(comparer.Compare(comboParlayDetailWrappers, viewModel.ComboParlayDetailWrappers).AreEqual);
        }

        [Fact]
        public void ComboParlayDetailWrapperGroups_Always_SetInCtor()
        {
            // Arrange
            var comboParlayDetailWrappers = new List<ComboParlayDetailWrapper>(BetListDataTest.ComboParlayDetails
                    .Select(d => new ComboParlayDetailWrapper(d, BetListDataTest.MatchDetails, ticketHelper)));
            var comboParlayDetailWrapperGroups = comboParlayDetailWrappers.GroupBy(w => w.WinLostDate);

            // Act
            var viewModel = new ComboParlayDetailViewModel(query, BetListDataTest.ComboParlayDetailData, ticketHelper, false);

            // Assert
            Assert.True(comparer.Compare(comboParlayDetailWrapperGroups, viewModel.ComboParlayDetailWrapperGroups).AreEqual);
        }

        [Fact]
        public void TotalWinLost_Always_SetInCtor()
        {
            // Arrange
            var expectedTotalWinLost = ConvertByBetType.Winloss(BetListDataTest.ComboParlayDetails.Sum(d => d.WinLost + d.PlayerComm));

            // Act
            var viewModel = new ComboParlayDetailViewModel(query, BetListDataTest.ComboParlayDetailData, ticketHelper, false);

            // Assert
            Assert.Equal(expectedTotalWinLost, viewModel.TotalWinLost);
        }

        [Fact]
        public void OutStanding_Always_SetInCtor()
        {
            // Arrange
            var expectedOutStanding = ConvertByBetType.Stake(BetListDataTest.ComboParlayDetailData.Outstanding);

            // Act
            var viewModel = new ComboParlayDetailViewModel(query, BetListDataTest.ComboParlayDetailData, ticketHelper, false);

            // Assert
            Assert.Equal(expectedOutStanding, viewModel.OutStanding);
        }

        [Fact]
        public void Title_BetTeamIsNOTEqualOne_TitleIsSystemParlay()
        {
            // Arrange
            query.BetTeam = "0";
            var expectedTitle = Resources.lblSystemParlay;

            // Act
            var viewModel = new ComboParlayDetailViewModel(query, BetListDataTest.ComboParlayDetailData, ticketHelper, false);

            // Assert
            Assert.Equal(expectedTitle, viewModel.Title);
        }

        [Fact]
        public void Title_BetTeamIsEqualOne_TitleIsMixParlay()
        {
            // Arrange
            query.BetTeam = "1";
            var expectedTitle = Resources.lblMixParlay;

            // Act
            var viewModel = new ComboParlayDetailViewModel(query, BetListDataTest.ComboParlayDetailData, ticketHelper, false);

            // Assert
            Assert.Equal(expectedTitle, viewModel.Title);
        }

        [Fact]
        public void IsDataTooHuge_True_WhenComboParlayDetailsCountIsGreaterThan5000()
        {
            // Arrange
            var comboParlayDetails = new Fixture().CreateMany<ComboParlayDetail>(5100);
            var compoParlayDetailData = new ComboParlayDetailData(comboParlayDetails, BetListDataTest.MatchDetails, 10);

            // Act
            var viewModel = new ComboParlayDetailViewModel(query, compoParlayDetailData, ticketHelper, false);

            // Assert
            Assert.True(viewModel.IsDataTooHuge);
        }

        [Fact]
        public void IsDataTooHuge_False_WhenComboParlayDetailsCountIsLessThan5000()
        {
            // Arrange
            var comboParlayDetails = new Fixture().CreateMany<ComboParlayDetail>(4999);
            var compoParlayDetailData = new ComboParlayDetailData(comboParlayDetails, BetListDataTest.MatchDetails, 10);

            // Act
            var viewModel = new ComboParlayDetailViewModel(query, compoParlayDetailData, ticketHelper, false);

            // Assert
            Assert.False(viewModel.IsDataTooHuge);
        }

        [Fact]
        public void SubTotalWinLost_Always_CalculateFromGroup()
        {
            // Arrange
            var viewModel = new ComboParlayDetailViewModel(query, BetListDataTest.ComboParlayDetailData, ticketHelper, false);
            var expectedSubtotalWinLost
                = ConvertByBetType.Winloss(viewModel.ComboParlayDetailWrappers.Sum(item => item.ComboParlayDetail.WinLost + item.ComboParlayDetail.PlayerComm));

            // Act
            var subtotalWinLost = viewModel.SubTotalWinLost(viewModel.ComboParlayDetailWrappers);

            // Assert
            Assert.Equal(expectedSubtotalWinLost, subtotalWinLost);
        }

        [Fact]
        public void IsRunningTicket_WhenHasNoData_True()
        {
            // Arrange
            var comboParlayDetailsData = new ComboParlayDetailData(new List<ComboParlayDetail>(), new List<MatchDetail>(), 0);
            var viewModel = new ComboParlayDetailViewModel(query, comboParlayDetailsData, ticketHelper, false);

            // Act
            var isRunningTicket = viewModel.IsRunningTicket;

            // Assert
            Assert.True(isRunningTicket);
        }

        [Fact]
        public void ComboParlayDetailViewModelCtor_IsDataNotHuge_InitData()
        {
            // Arrange
            var comboParlayDetailsData = BetListDataTest.ComboParlayDetailData;

            // Act
            var viewModel = new ComboParlayDetailViewModel(query, comboParlayDetailsData, ticketHelper, false);

            // Assert
            Assert.True(
                viewModel.ComboParlayDetailWrapperGroups.Any() &&
                !string.IsNullOrEmpty(viewModel.OutStanding) &&
                !string.IsNullOrEmpty(viewModel.TotalWinLost));
        }

        [Fact]
        public void ComboParlayDetailViewModelCtor_IsExportingExcel_InitData()
        {
            // Arrange
            var comboParlayDetailsData = new ComboParlayDetailData(BetListDataTest.ComboParlayDetailsHuge, BetListDataTest.MatchDetails, 10);

            // Act
            var viewModel = new ComboParlayDetailViewModel(query, comboParlayDetailsData, ticketHelper, false, isExportingExcel: true);

            // Assert
            Assert.True(
                viewModel.ComboParlayDetailWrapperGroups.Any() &&
                !string.IsNullOrEmpty(viewModel.OutStanding) &&
                !string.IsNullOrEmpty(viewModel.TotalWinLost));
        }
    }
}
