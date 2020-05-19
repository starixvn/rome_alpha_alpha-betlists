namespace Alpha.BetLists.Site.Tests.ViewModels.CancelledBet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.BetLists.Constants;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Params;
    using Alpha.BetLists.Site.Requests.CancelledBet;
    using Alpha.BetLists.Site.Tests.Fixtures;
    using Alpha.BetLists.Site.ViewModels.CancelledBet;
    using Alpha.BetLists.Site.ViewModels.Components;
    using Alpha.BetLists.Tests.TestData;
    using Constants;
    using NSubstitute;
    using Ploeh.AutoFixture;
    using Services;
    using Xunit;

    [Trait("Category", "CancelledBetViewModel")]
    [Collection("BetListCollection")]
    public class CancelledBetViewModelTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly IUser user;
        private CancelledBetQuery query;
        private CancelledBetParam param;
        private PagedReportData<BetListData> reportData;
        private static IFixture fixture = new Fixture();

        public CancelledBetViewModelTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            user = betlistFixture.User;
            query = new CancelledBetQuery(user);

            reportData = new PagedReportData<BetListData>(new List<BetListData> { BetListDataTest.BetListData }, 1, 50, 50);

            var reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(DefaultVariablesTest.BalanceUpDate);
        }

        [Fact]
        public void StatusMultiSelectionViewModel_SelectAllStatus_Return3Items()
        {
            //// Arrange
            var productSelectItem = fixture.CreateMany<GroupProductViewModel>();
            param = new CancelledBetParam(user.Id, DateTime.Now, DateTime.Now, DateTime.Now, user.LevelId, BetListDataTest.ProductIds, "101,102,103", user.Language, 50, 1);
            var viewModel = new CancelledBetViewModel(query, param, reportData, productSelectItem, betlistFixture.HtmlBuilder, betlistFixture.TicketHelper);

            //// Act
            var act = viewModel.StatusMultiSelectionViewModel;

            //// Assert
            Assert.Equal(act.StatusListItem.Count(i => i.Selected), 3);

        }

        [Fact]
        public void StatusMultiSelectionViewModel_NoSelectStatus_Return3Items()
        {
            //// Arrange
            var productSelectItem = fixture.CreateMany<GroupProductViewModel>();
            param = new CancelledBetParam(user.Id, DateTime.Now, DateTime.Now, DateTime.Now, user.LevelId, BetListDataTest.ProductIds, "", user.Language, 50, 1);
            var viewModel = new CancelledBetViewModel(query, param, reportData, productSelectItem, betlistFixture.HtmlBuilder, betlistFixture.TicketHelper);

            //// Act
            var act = viewModel.StatusMultiSelectionViewModel;

            //// Assert
            Assert.Equal(act.StatusListItem.Count(i => i.Selected), 3);

        }

        [Fact]
        public void StatusMultiSelectionViewModel_SelectedVoid_VoidItemIsSelected()
        {
            //// Arrange
            var voidSelected = $"{TicketStatusIdConstants.Void}";
            var productSelectItem = fixture.CreateMany<GroupProductViewModel>();
            param = new CancelledBetParam(user.Id, DateTime.Now, DateTime.Now, DateTime.Now, user.LevelId, BetListDataTest.ProductIds, voidSelected, user.Language, 50, 1);
            var viewModel = new CancelledBetViewModel(query, param, reportData, productSelectItem, betlistFixture.HtmlBuilder, betlistFixture.TicketHelper);

            //// Act
            var act = viewModel.StatusMultiSelectionViewModel;

            //// Assert
            Assert.Equal(act.StatusListItem.Count(i => i.Value == voidSelected && i.Selected == true), 1);

        }
    }
}
