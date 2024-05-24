namespace Alpha.BetLists.Site.Tests.Handler.CancelledBet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Requests.CancelledBet;
    using Alpha.BetLists.Site.Tests.Fixtures;
    using BetLists.Tests.TestData;
    using Constants;
    using Handlers.CancelledBet;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Xunit;

    [Trait("Category", "CancelledBet")]
    [Collection("ParlayBetListCollection")]
    public class CancelledBetExcelHandlerTests
    {
        private readonly BetListFixture betListFixture;
        private readonly Workbook<TicketDto> workbook;
        private readonly CancelledBetService service;
        private readonly ProductService productService;
        private readonly CancelledBetExcelHandler handler;
        private readonly CancelledBetExcelQuery query;
        private readonly ReportDateService reportDateService;

        public CancelledBetExcelHandlerTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;
            service = Substitute.For<CancelledBetService>();
            productService = Substitute.For<ProductService>();
            reportDateService = Substitute.For<ReportDateService>();
            query = new CancelledBetExcelQuery(betListFixture.User);
            query.RtfHelper = this.betListFixture.RtfHelper;

            workbook = Substitute.For<Workbook<TicketDto>>();
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<TicketDto>>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);
            workbook.Footer(Arg.Any<TicketDto>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);

            handler = new CancelledBetExcelHandler(
                service,
                productService,
                reportDateService,
                betListFixture.MonitorService,
                betListFixture.HtmlBuilder,
                betListFixture.ExcelBuilder,
                betListFixture.TicketHelper,
                betListFixture.IpService,
                workbook,
                betListFixture.IpInfoService);

            handler.IsFileExist = path => true;

            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(DefaultVariablesTest.BalanceUpDate);
        }

        [Fact]
        public void Handle_Always_CallGetCancelledBetFromService()
        {
            //// Arrange
            var productIds = new List<int> { 1, 2, 3, 4, 5, 6 };
            var betlistData = new PagedReportData<BetListData>(new List<BetListData> { BetListDataTest.BetListData }, 1, 50, 100);
            service.GetCancelledBet(Arg.Any<CancelledBetParam>()).Returns(betlistData);
            productService.FilterProductIds(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<IEnumerable<int>>()).Returns(productIds);
            productService.GetAllOfferedProducts(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<DateTime>()).Returns(Enumerable.Empty<Product>());
            reportDateService.GetMoveBetsDay().Returns(DateTime.Now);

            //// Act
            handler.Handle(query).GetAwaiter().GetResult();

            //// Assert
            service.Received().GetCancelledBet(Arg.Any<CancelledBetParam>());
            productService.Received().FilterProductIds(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<IEnumerable<int>>());
            productService.Received().GetAllOfferedProducts(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<DateTime>());
        }

        [Fact]
        public void Handle_Always_CallUseTemplate()
        {
            //// Arrange
            var productIds = new List<int> { 1, 2, 3, 4, 5, 6 };
            var betlistData = new PagedReportData<BetListData>(new List<BetListData> { BetListDataTest.BetListData }, 1, 50, 100);
            service.GetCancelledBet(Arg.Any<CancelledBetParam>()).Returns(betlistData);
            productService.FilterProductIds(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<DateTime>(), productIds).Returns(productIds);
            productService.GetAllOfferedProducts(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<DateTime>()).Returns(Enumerable.Empty<Product>());
            reportDateService.GetMoveBetsDay().Returns(DateTime.Now);

            //// Act
            handler.Handle(query).GetAwaiter().GetResult();

            //// Assert
            workbook.Received().UseTemplate(Arg.Any<TemplateInfo>());
        }

        [Fact]
        public void Handle_Always_CallHeaderOfWorkbook()
        {
            //// Arrange
            var productIds = new List<int> { 1, 2, 3, 4, 5, 6 };
            var betlistData = new PagedReportData<BetListData>(new List<BetListData> { BetListDataTest.BetListData }, 1, 50, 100);
            service.GetCancelledBet(Arg.Any<CancelledBetParam>()).Returns(betlistData);
            productService.FilterProductIds(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<DateTime>(), productIds).Returns(productIds);
            productService.GetAllOfferedProducts(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<DateTime>()).Returns(Enumerable.Empty<Product>());

            //// Act
            handler.Handle(query).GetAwaiter().GetResult();

            //// Assert
            workbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void Handle_Always_CallTitleOfWorkbook()
        {
            //// Arrange
            var productIds = new List<int> { 1, 2, 3, 4, 5, 6 };
            var betlistData = new PagedReportData<BetListData>(new List<BetListData> { BetListDataTest.BetListData }, 1, 50, 100);
            service.GetCancelledBet(Arg.Any<CancelledBetParam>()).Returns(betlistData);
            productService.FilterProductIds(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<DateTime>(), productIds).Returns(productIds);
            productService.GetAllOfferedProducts(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<DateTime>()).Returns(Enumerable.Empty<Product>());
            reportDateService.GetMoveBetsDay().Returns(DateTime.Now);

            //// Act
            handler.Handle(query).GetAwaiter().GetResult();

            //// Assert
            workbook.Received().Title(Arg.Any<string>());
        }

        [Fact]
        public void Handle_Always_CallExportToBytesOfWorkbook()
        {
            //// Arrange
            var productIds = new List<int> { 1, 2, 3, 4, 5, 6 };
            var betlistData = new PagedReportData<BetListData>(new List<BetListData> { BetListDataTest.BetListData }, 1, 50, 100);
            service.GetCancelledBet(Arg.Any<CancelledBetParam>()).Returns(betlistData);
            productService.FilterProductIds(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<DateTime>(), productIds).Returns(productIds);
            productService.GetAllOfferedProducts(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<DateTime>()).Returns(Enumerable.Empty<Product>());
            reportDateService.GetMoveBetsDay().Returns(DateTime.Now);

            //// Act
            handler.Handle(query).GetAwaiter().GetResult();

            //// Assert
            workbook.Received().ExportToBytes();
        }
    }
}
