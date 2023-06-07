namespace Alpha.BetLists.Site.Tests.Handler.DailyStatement
{
    using System;
    using System.Collections.Generic;
    using BetLists.Models;
    using BetLists.Tests.Constants;
    using BetLists.Tests.TestData;
    using Fixtures;
    using Handlers.DailyStatement;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Requests.DailyStatement;
    using Services;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "DailyStatement")]
    [Collection("BetListCollection")]
    public class DailyStatementExcelHandlerTests
    {
        private readonly IUser user;
        private readonly DailyStatementExcelQuery message;
        private readonly Workbook<TicketDto> workbook;
        private readonly DailyStatementExcelHandler dailyStatementExcelHandler;
        private readonly CompareLogic comparer;

        public DailyStatementExcelHandlerTests(BetListFixture betlistFixture)
        {
            user = betlistFixture.User;
            message = CreateDailyStatementExcelQuery();
            message.RtfHelper = betlistFixture.RtfHelper;

            workbook = Substitute.For<Workbook<TicketDto>>();
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<TicketDto>>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);
            workbook.Footer(Arg.Any<TicketDto>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);
            workbook.ExportToBytes().Returns(BetListDataTest.ExcelFileContent);

            var dailyStatementService = Substitute.For<DailyStatementService>();
            dailyStatementService.GetBetList(Arg.Any<DailyStatementParam>()).Returns(BetListDataTest.BetListData);
            dailyStatementExcelHandler = new DailyStatementExcelHandler(
                dailyStatementService,
                betlistFixture.HtmlBuilder,
                betlistFixture.ExcelBuilder,
                betlistFixture.TicketHelper,
                betlistFixture.MonitorService,
                betlistFixture.IpService,
                workbook,
                betlistFixture.IpInfoService)
            {
                MapPath = path => path,
                IsFileExist = path => true
            };

            comparer = new CompareLogic();
        }

        [Fact]
        public void HandlerExportDailyStatementExcelQuery_Always_ReturnsExcelFileViewModel()
        {
            // Arrange
            var expectedViewModel = new ExcelFileViewModel("BetList", BetListDataTest.ExcelFileContent);

            // Act
            var actualViewModel = dailyStatementExcelHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            Assert.True(comparer.Compare(expectedViewModel, actualViewModel).AreEqual);
        }

        [Fact]
        public void GetTemplateInfo_Always_UsesCorrectTemplateInfo()
        {
            // Arrange
            var templateInfo = new TemplateInfo(
                  "~/App_Data/ExcelTemplates/DailyStatement/Member.xls",
                  "No information is available",
                  bodyStartIndex: 3,
                  bodyEndIndex: 4,
                  footerEndIndex: 5);

            // Act
            dailyStatementExcelHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(t => comparer.Compare(t, templateInfo).AreEqual));
        }

        [Fact]
        public void HandlerExportDailyStatementExcelQuery_Always_ExportsCorrectTitle()
        {
            // Act
            dailyStatementExcelHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Title(Arg.Is("Daily Statement 5/25/2017 - CustName"));
        }

        [Fact]
        public void HandlerExportDailyStatementExcelQuery_Always_ExportsCorrectHeader()
        {
            // Act
            dailyStatementExcelHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        private DailyStatementExcelQuery CreateDailyStatementExcelQuery()
            => new DailyStatementExcelQuery(user)
            {
                CustId = 1,
                CustName = "CustName",
                WinLossDate = DefaultVariables.CurrentDate,
                TransType = 1,
                Language = "en"
            };
    }
}
