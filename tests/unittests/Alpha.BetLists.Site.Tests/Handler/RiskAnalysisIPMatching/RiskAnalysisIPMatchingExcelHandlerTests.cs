namespace Alpha.BetLists.Site.Tests.Handler.WinLoss
{
    using Alpha.BetLists.Site.Handlers.RiskAnalysisIPMatching;
    using Alpha.BetLists.Site.Requests.RiskAnalysisIPMatching;
    using Alpha.BetLists.Site.ViewModels.RiskAnalysisIPMatching;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Site.ViewModels;
    using System;
    using System.Collections.Generic;
    using Xunit;

    [Trait("Category", "RiskAnalysisIPMatching")]
    [Collection("BetListCollection")]
    public class RiskAnalysisIPMatchingExcelHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly RiskAnalysisIPMatchingExcelHandler riskAnalysisIPMatchingExcelHandler;
        private readonly RiskAnalysisIPMatchingHelper riskAnalysisIPMatchingHelper;
        private readonly RiskAnalysisIPMatchingExcelQuery query;
        private readonly Workbook<TicketDto> workbook;
        private readonly CompareLogic compareLogic;

        public RiskAnalysisIPMatchingExcelHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            riskAnalysisIPMatchingHelper = Substitute.For<RiskAnalysisIPMatchingHelper>();
            riskAnalysisIPMatchingHelper
                .BuildRiskAnalysisIPMatchingViewModel(Arg.Any<RiskAnalysisIPMatchingQuery>())
                .ReturnsForAnyArgs(BetListDataTest.BetListData);

            query = new RiskAnalysisIPMatchingExcelQuery(this.betlistFixture.User);
            InitQuery();

            workbook = Substitute.For<Workbook<TicketDto>>();
            InitWorkbook();

            riskAnalysisIPMatchingExcelHandler = new RiskAnalysisIPMatchingExcelHandler(
                this.betlistFixture.HtmlBuilder,
                this.betlistFixture.ExcelBuilder,
                this.betlistFixture.TicketHelper,
                this.betlistFixture.MonitorService,
                this.betlistFixture.IpService,
                riskAnalysisIPMatchingHelper,
                workbook);

            riskAnalysisIPMatchingExcelHandler.MapPath = (path) => path;
            riskAnalysisIPMatchingExcelHandler.IsFileExist = (path) => true;

            compareLogic = new CompareLogic(new ComparisonConfig
            {
                MembersToIgnore = new List<string> { "ResultsPath", "OldMainPath" }
            });
        }

        private void InitWorkbook()
        {
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<TicketDto>>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);
            workbook.Footer(Arg.Any<TicketDto>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);
        }

        private void InitQuery()
        {
            query.CustId = 1;
            query.Date = DateTime.Today;
            query.Language = "en";
            query.IP = "192.168.1.2";
            query.RtfHelper = betlistFixture.RtfHelper;
        }

        [Fact]
        public void Handle_Always_CallGetBetListFromService()
        {
            // Act
            riskAnalysisIPMatchingExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            riskAnalysisIPMatchingHelper.Received().BuildRiskAnalysisIPMatchingViewModel(Arg.Is<RiskAnalysisIPMatchingQuery>(
                p => p.CustId == query.CustId
                && p.Date == query.Date
                && p.Language == query.Language));
        }

        [Fact]
        public void Handle_Workbook_Body()
        {
            // Act
            riskAnalysisIPMatchingExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Body(Arg.Is(BetListDataTest.Tickets), Arg.Any<Mapper<TicketDto>>());
        }

        [Fact]
        public void Handle_Workbook_Footer()
        {
            // Act
            riskAnalysisIPMatchingExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Footer(Arg.Any<TicketDto>(), Arg.Any<Mapper<TicketDto>>());
        }

        [Fact]
        public void Handle_Workbook_CallExportToBytes()
        {
            // Act
            riskAnalysisIPMatchingExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void Handle_ValidViewModel_ReturnExcelViewModel()
        {
            // Arrange
            var viewModel = new RiskAnalysisIPMatchingViewModel(query, BetListDataTest.BetListData, null, betlistFixture.TicketHelper);
            workbook.ExportToBytes().Returns(BetListDataTest.ExcelFileContent);
            var expectedViewModel = new ExcelFileViewModel(viewModel.Title, BetListDataTest.ExcelFileContent);

            // Act
            var actualViewModel = riskAnalysisIPMatchingExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(actualViewModel, expectedViewModel).AreEqual);
        }

        [Theory]
        [InlineData(null)]
        public void Handle_NullViewModel_ReturnNull(BetListData viewModelData)
        {
            // Arrange
            riskAnalysisIPMatchingHelper.BuildRiskAnalysisIPMatchingViewModel(Arg.Any<RiskAnalysisIPMatchingQuery>()).Returns(viewModelData);

            // Act
            var actualViewModel = riskAnalysisIPMatchingExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            Assert.Null(actualViewModel);
        }
    }
}