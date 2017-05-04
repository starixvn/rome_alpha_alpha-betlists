namespace Alpha.BetLists.Site.Tests.ViewModels
{
    using System;
    using AppResources;
    using BetLists.Tests.TestData;
    using Fixtures;
    using NSubstitute;
    using Requests;
    using Requests.WinLoss;
    using Site.ViewModels.WinLoss;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    [Collection("BetListCollection")]
    public class BetListBaseViewModelTests
    {
        private readonly BetListFixture betListFixture;
        private BaseQuery query;

        public BetListBaseViewModelTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;
            query = new WinLossQuery(this.betListFixture.User);
        }

        [Fact]
        public void CustName_Always_SetFromQueryInCtor()
        {
            // Arrange
            query.CustName = "harrison";

            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, true);

            // Assert
            Assert.Equal(query.CustName, viewModel.CustName);
        }

        [Fact]
        public void CustId_Always_SetFromQueryInCtor()
        {
            // Arrange
            query.CustId = 1;

            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, true);

            // Assert
            Assert.Equal(query.CustId, viewModel.CustId);
        }

        [Fact]
        public void FromDate_Always_SetFromQueryInCtor()
        {
            // Arrange
            query.FromDate = DateTime.Today;

            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, true);

            // Assert
            Assert.Equal(query.FromDate, viewModel.FromDate);
        }

        [Fact]
        public void ToDate_Always_SetFromQueryInCtor()
        {
            // Arrange
            query.ToDate = DateTime.Today;

            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, true);

            // Assert
            Assert.Equal(query.ToDate, viewModel.ToDate);
        }

        [Fact]
        public void BetListData_Always_SetInCtor()
        {
            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, true);

            // Assert
            Assert.Equal(BetListDataTest.BetListData, viewModel.BetListData);
        }

        [Fact]
        public void Builder_Always_SetInCtor()
        {
            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, true);

            // Assert
            Assert.Equal(betListFixture.HtmlBuilder, viewModel.Builder);
        }

        [Fact]
        public void TicketHelper_Always_SetInCtor()
        {
            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, true);

            // Assert
            Assert.Equal(betListFixture.HtmlBuilder, viewModel.Builder);
        }

        [Fact]
        public void CanViewScoreMap_Always_GetDefault()
        {
            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, true);

            // Assert
            Assert.Equal(false, viewModel.CanViewScoreMap);
        }

        [Fact]
        public void DisplayVNIP_Always_SetFromQueryInCtor()
        {
            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, true);

            // Assert
            Assert.Equal(query.DisplayVNIP, viewModel.DisplayVNIP);
        }

        [Fact]
        public void ProductsId_Always_SetFromQueryInCtor()
        {
            // Arrange
            query.ProductIds = "1,2";

            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, true);

            // Assert
            Assert.Equal(query.ProductIds, viewModel.ProductsId);
        }

        [Fact]
        public void BetListType_Always_SetFromQueryInCtor()
        {
            // Arrange
            query.Type = "1,2";

            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, true);

            // Assert
            Assert.Equal(query.Type, viewModel.BetListType);
        }

        [Fact]
        public void IsHistoricReport_Always_SetFromQueryInCtor()
        {
            // Arrange
            query.IsHistoricReport = false;

            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, true);

            // Assert
            Assert.Equal(query.IsHistoricReport, viewModel.IsHistoricReport);
        }

        [Fact]
        public void ViewerIdOrViewerAdminId_Always_SetFromQueryInCtor()
        {
            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, true);

            // Assert
            Assert.Equal(query.ViewerIdOrViewerAdminId, viewModel.ViewerIdOrViewerAdminId);
        }

        [Fact]
        public void ViewerLevelId_Always_SetFromQueryInCtor()
        {
            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, true);

            // Assert
            Assert.Equal(query.ViewerLevelId, viewModel.ViewerLevelId);
        }

        [Fact]
        public void Title_HasDateParam_ShowTitleWithDate()
        {
            // Arrange
            query.FromDate = DateTime.Today;
            query.ToDate = DateTime.Today;
            var expectedTitle = $"{Resources.memberBetList} {query.FromDate.ToShortDateString()} --> {query.ToDate.ToShortDateString()} - {query.CustName}";

            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, true);

            // Assert
            Assert.Equal(expectedTitle.ToUpperInvariant(), viewModel.Title);
        }

        [Fact]
        public void Title_HasNotDateParam_ShowTitleWithDate()
        {
            // Arrange
            query.FromDate = DateTime.MinValue;
            query.ToDate = DateTime.MinValue;
            var expectedTitle = Resources.memberBetList;

            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, true);

            // Assert
            Assert.Equal(expectedTitle.ToUpperInvariant(), viewModel.Title);
        }

        [Theory]
        [InlineData(2, 8)]
        [InlineData(3, 9)]
        [InlineData(4, 10)]
        public void TotalColumn_NotShowMemberName_GetTotalColumnByViewerLevelId(byte userLevelId, int expectedTotalColumn)
        {
            // Arrange
            betListFixture.User.LevelId.Returns(userLevelId);
            query = new WinLossQuery(betListFixture.User);

            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, false);

            // Assert
            Assert.Equal(expectedTotalColumn, viewModel.TotalColumn);
        }

        [Theory]
        [InlineData(2, 8)]
        [InlineData(3, 9)]
        [InlineData(4, 10)]
        public void TotalColumn_ShowMemberName_GetTotalColumnByViewerLevelId(byte userLevelId, int expectedTotalColumn)
        {
            // Arrange
            betListFixture.User.LevelId.Returns(userLevelId);
            query = new WinLossQuery(betListFixture.User);

            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, true);

            // Assert
            Assert.Equal(expectedTotalColumn + 1, viewModel.TotalColumn);
        }

        [Fact]
        public void ShowMemberName_WhenNotHaveParamValue_GetDefaultValue()
        {
            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            // Assert
            Assert.Equal(false, viewModel.ShowMemberName);
        }

        [Fact]
        public void ShowMemberName_WhenHaveParamValue_SetInCtor()
        {
            // Act
            var viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betListFixture.HtmlBuilder, betListFixture.TicketHelper, true);

            // Assert
            Assert.Equal(true, viewModel.ShowMemberName);
        }
    }
}