namespace Alpha.BetLists.Site.Tests.Handler.Parlay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Utils;
    using Fixtures;
    using Handlers;
    using Handlers.Parlay;
    using KellermanSoftware.CompareNetObjects;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Requests.Parlay;
    using Site.ViewModels;
    using Site.ViewModels.Parlay;
    using Xunit;
    using static ONEbook.SpreadSheets.Column;

    [Trait("Category", "BaseBetList")]
    [Collection("ParlayBetListCollection")]
    public class ParlayExcelHandlerTests
    {
        private readonly BetListFixture betListFixture;
        private readonly ParlayServiceFixture parlayServiceFixture;
        private readonly ParlayExcelHandler parlayExcelHandler;
        private readonly ComboParlayDetailWorkbook workbook;
        private readonly Workbook<IEnumerable<ComboParlayDetailWrapper>> onebookWorkbook;
        private readonly ComboParlayDetailExcelQuery query;
        private readonly CompareLogic comparer;

        public ParlayExcelHandlerTests(BetListFixture betListFixture, ParlayServiceFixture parlayServiceFixture)
        {
            this.betListFixture = betListFixture;
            this.parlayServiceFixture = parlayServiceFixture;
            workbook = Substitute.For<ComboParlayDetailWorkbook>();
            onebookWorkbook = Substitute.For<Workbook<IEnumerable<ComboParlayDetailWrapper>>>();
            InitWorkbook();

            query = new ComboParlayDetailExcelQuery(betListFixture.User);

            parlayExcelHandler = new ParlayExcelHandler(
                parlayServiceFixture.ParlayService,
                betListFixture.HtmlBuilder,
                betListFixture.ExcelBuilder,
                betListFixture.TicketHelper,
                betListFixture.MoniterService,
                betListFixture.IpService,
                workbook);

            parlayExcelHandler.MapPath = path => path;
            parlayExcelHandler.IsFileExist = path => true;
            comparer = new CompareLogic(new ComparisonConfig
            {
                MembersToIgnore = { "HugeDataLabel" }
            });
        }

        private void InitWorkbook()
        {
            workbook.SetData(Arg.Any<ComboParlayDetailViewModel>()).Returns(workbook);
            workbook.SetRowMapper(
                Arg.Any<Mapper<IEnumerable<ComboParlayDetailWrapper>>>(),
                Arg.Any<Mapper<IEnumerable<ComboParlayDetailWrapper>>>(),
                Arg.Any<Mapper<IEnumerable<ComboParlayDetailWrapper>>>(),
                Arg.Any<Mapper<IEnumerable<ComboParlayDetailWrapper>>>()).Returns(workbook);
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Title(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(onebookWorkbook);
            onebookWorkbook.ExportToBytes().Returns(BetListDataTest.ExcelFileContent);
        }

        [Fact]
        public void Handle_ComboParlayDetailExcelQuery_CallGetComboParlayDetailService()
        {
            // Act
            parlayExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            parlayServiceFixture.ParlayService.Received().GetComboParlayDetail(
               Arg.Is<long>(t => t == query.TransId),
               Arg.Is<long>(r => r == query.RefNo),
               Arg.Is<long>(b => b == query.BetId),
               Arg.Is<int>(c => c == query.CustId),
               Arg.Is<string>(l => l == query.Language));
        }

        [Fact]
        public void Handle_ComboParlayDetailExcelQuery_Workbook_CallSetData()
        {
            // Arrange
            var expectedViewModel = new ComboParlayDetailViewModel(
                query,
                BetListDataTest.ComboParlayDetailData,
                betListFixture.TicketHelper,
                isExportingExcel: true);

            // Act
            parlayExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            workbook.Received()
                .SetData(Arg.Is<ComboParlayDetailViewModel>(v => comparer.Compare(v, expectedViewModel).AreEqual));
        }

        [Fact]
        public void Handle_ComboParlayDetailExcelQuery_Workbook_CallSetRowMapper()
        {
            // Arrange
            var viewModel = new ComboParlayDetailViewModel(
                query,
                BetListDataTest.ComboParlayDetailData,
                betListFixture.TicketHelper,
                isExportingExcel: true);
            var details = viewModel.ComboParlayDetailWrappers;

            // Act
            parlayExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            workbook.Received().SetRowMapper(
                Arg.Is<Mapper<IEnumerable<ComboParlayDetailWrapper>>>(m =>
                    m.Mappings[A].Invoke(details).Equals(details.First().WinLostDate)
                    && m.Mappings[B].Invoke(details).Equals(RenderEvent(details.First()))
                    && m.Mappings[C].Invoke(details).Equals(details.First().Odds)
                    && m.Mappings[D].Invoke(details).Equals(details.First().Stake)
                    && m.Mappings[E].Invoke(details).Equals(RenderWinLossComm(details.First()))
                    && m.Mappings[F].Invoke(details).Equals(details.First().Status)),
                Arg.Is<Mapper<IEnumerable<ComboParlayDetailWrapper>>>(m =>
                    m.Mappings[A].Invoke(details).Equals(Resources.subTotal + ":")
                    && m.Mappings[E].Invoke(details).Equals(viewModel.SubTotalWinLost(details))),
                Arg.Is<Mapper<IEnumerable<ComboParlayDetailWrapper>>>(m =>
                    m.Mappings[A].Invoke(details).Equals(Resources.outstanding + ":")
                    && m.Mappings[E].Invoke(details).Equals(viewModel.OutStanding)),
                Arg.Is<Mapper<IEnumerable<ComboParlayDetailWrapper>>>(m =>
                    m.Mappings[A].Invoke(details).Equals(Resources.total + ":")
                    && m.Mappings[E].Invoke(details).Equals(viewModel.TotalWinLost)));
        }

        private IRichTextString RenderEvent(ComboParlayDetailWrapper detail)
        {
            for (var i = 0; i < detail.Events.Count(); i++)
            {
                betListFixture.RtfHelper.RTFRenderer.AddText((i > 0 ? "\n" : string.Empty) + detail.Events[i]);
            }

            var eventRtfText = betListFixture.RtfHelper.RTFRenderer.Render();
            betListFixture.RtfHelper.RTFRenderer.Clear();

            return eventRtfText;
        }

        private IRichTextString RenderWinLossComm(ComboParlayDetailWrapper detail)
        {
            var rtfNumber = new RTFNumber(
                betListFixture.RtfHelper.RTFRenderer,
                betListFixture.RtfHelper.PosFont,
                betListFixture.RtfHelper.NegFont);

            rtfNumber.Render(detail.WinLost + "\n");
            rtfNumber.Render(detail.Comm);

            var rtfText = betListFixture.RtfHelper.RTFRenderer.Render();
            betListFixture.RtfHelper.RTFRenderer.Clear();

            return rtfText;
        }

        [Fact]
        public void Handle_ComboParlayDetailExcelQuery_Workbook_CallUseTemplate()
        {
            // Arrange
            var templateInfo = new TemplateInfo(
                  "~/App_Data/ExcelTemplates/Parlay/ComboParlayDetail.xls",
                  noInfomationText: Resources.NoInformationIsAvailable,
                  bodyStartIndex: 3,
                  bodyEndIndex: 5,
                  footerEndIndex: 7);

            // Act
            parlayExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(t => comparer.Compare(t, templateInfo).AreEqual));
        }

        [Fact]
        public void Handle_ComboParlayDetailExcelQuery_Workbook_CallSheet()
        {
            // Arrange
            var viewModel = new ComboParlayDetailViewModel(
                query,
                BetListDataTest.ComboParlayDetailData,
                betListFixture.TicketHelper);

            // Act
            parlayExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            onebookWorkbook.Received().Sheet(Arg.Is(viewModel.Title));
        }

        [Fact]
        public void Handle_ComboParlayDetailExcelQuery_Workbook_CallTitle()
        {
            // Arrange
            var viewModel = new ComboParlayDetailViewModel(
                query,
                BetListDataTest.ComboParlayDetailData,
                betListFixture.TicketHelper);

            // Act
            parlayExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            onebookWorkbook.Received().Title(Arg.Is(viewModel.Title));
        }

        [Fact]
        public void Handle_ComboParlayDetailExcelQuery_Workbook_CallHeader()
        {
            // Act
            parlayExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            onebookWorkbook.Received().Header(Resources.ResourceManager.GetString);
        }

        [Fact]
        public void Handle_ComboParlayDetailExcelQuery_Workbook_CallExportToBytes()
        {
            // Act
            parlayExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            onebookWorkbook.Received().ExportToBytes();
        }

        [Fact]
        public void Handle_ComboParlayDetailExcelQuery_ReturnExcelFileViewModel()
        {
            // Arrange
            var viewModel = new ComboParlayDetailViewModel(
                query,
                BetListDataTest.ComboParlayDetailData,
                betListFixture.TicketHelper);
            var expectedViewModel = new ExcelFileViewModel("SystemParlay_Details", BetListDataTest.ExcelFileContent);

            // Act
            var actualViewModel = parlayExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            Assert.True(comparer.Compare(expectedViewModel, actualViewModel).AreEqual);
        }
    }
}