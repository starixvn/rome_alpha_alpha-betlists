namespace Alpha.BetLists.Site.Tests.Handler.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Extensions;
    using Fixtures;
    using Handlers.WinLoss;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Requests.WinLoss;
    using Services;
    using Site.ViewModels;
    using Site.ViewModels.WinLoss;
    using Xunit;

    [Trait("Category", "WinLossBetList")]
    [Collection("BetListCollection")]
    public class WinLossExcelHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly WinLossExcelHandler winLossExcelHandler;
        private readonly WinLossService winLossService;
        private readonly WinLossExcelQuery query;
        private readonly Workbook<TicketDto> workbook;
        private readonly CompareLogic compareLogic;
        private readonly ProductService productService;

        public WinLossExcelHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            winLossService = Substitute.For<WinLossService>();
            winLossService.GetBetList(Arg.Any<WinLossParam>()).Returns(BetListDataTest.BetListData);

            query = new WinLossExcelQuery(this.betlistFixture.User);
            InitQuery();

            workbook = Substitute.For<Workbook<TicketDto>>();
            InitWorkbook();

            productService = Substitute.For<ProductService>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<IEnumerable<int>>())
                .Returns(callInfo => callInfo.Args()[2]);

            winLossExcelHandler = new WinLossExcelHandler(
                winLossService,
                this.betlistFixture.HtmlBuilder,
                this.betlistFixture.ExcelBuilder,
                this.betlistFixture.TicketHelper,
                this.betlistFixture.MonitorService,
                this.betlistFixture.IpService,
                workbook,
                productService);

            winLossExcelHandler.MapPath = (path) => path;
            winLossExcelHandler.IsFileExist = (path) => true;

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
            query.ProductIds = "1";
            query.FromDate = DateTime.Today;
            query.ToDate = DateTime.Today;
            query.Language = "en";
            query.RtfHelper = betlistFixture.RtfHelper;
        }

        [Fact]
        public void Handle_Always_CallGetBetListFromService()
        {
            // Act
            winLossExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            winLossService.Received().GetBetList(Arg.Is<WinLossParam>(
                p => p.CustId == query.CustId
                && p.ProductIds.SequenceEqual(query.ProductIds.ToListInt())
                && p.FromDate == query.FromDate
                && p.ToDate == query.ToDate
                && p.Language == query.Language));
        }

        [Fact]
        public void Handle_Workbook_UseTemplate()
        {
            // Act
            winLossExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(
                t => t.TemplatePath == $"~/App_Data/ExcelTemplates/WinLoss/{query.ViewerLevel.DisplayName}.xls"
                && t.NoInfomationText == Resources.NoInformationIsAvailable
                && t.BodyStartIndex == 3 - 1
                && t.BodyEndIndex == 4 - 1
                && t.FooterEndIndex == 5 - 1));
        }

        [Fact]
        public void Handle_Workbook_Header()
        {
            // Act
            winLossExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Header(Arg.Is<Func<string, string>>(f => f == Resources.ResourceManager.GetString));
        }

        [Fact]
        public void Handle_Workbook_Title()
        {
            // Arrange
            var title = $"{Resources.memberBetList} {query.FromDate.ToShortDateString()} --> {query.ToDate.ToShortDateString()} - {query.CustName}";

            // Act
            winLossExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Title(Arg.Is(title.ToUpperInvariant()));
        }

        [Fact]
        public void Handle_Workbook_Body()
        {
            // Act
            winLossExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Body(Arg.Is(BetListDataTest.Tickets), Arg.Any<Mapper<TicketDto>>());
        }

        [Fact]
        public void Handle_Workbook_Footer()
        {
            // Act
            winLossExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Footer(Arg.Any<TicketDto>(), Arg.Any<Mapper<TicketDto>>());
        }

        [Fact]
        public void Handle_Workbook_CallExportToBytes()
        {
            // Act
            winLossExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void Handle_Always_ReturnExcelViewModel()
        {
            // Arrange
            var winLossViewModel = new WinLossViewModel(query, BetListDataTest.BetListData, null, betlistFixture.TicketHelper, false);
            workbook.ExportToBytes().Returns(BetListDataTest.ExcelFileContent);
            var expectedViewModel = new ExcelFileViewModel(winLossViewModel.Title, BetListDataTest.ExcelFileContent);

            // Act
            var actualViewModel = winLossExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(actualViewModel, expectedViewModel).AreEqual);
        }
    }
}