using Alpha.BetLists.Models;
using Alpha.BetLists.Models.CancelledBet;
using Alpha.BetLists.Params;
using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Handlers.CancelledBet;
using Alpha.BetLists.Site.Requests.CancelledBet;
using Alpha.BetLists.Site.Tests.Fixtures;
using Alpha.BetLists.Tests.TestData;
using NSubstitute;
using ONEbook.SpreadSheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Alpha.BetLists.Site.Tests.Handler.CancelledBet
{
    [Trait("Category", "CancelledBet")]
    [Collection("ParlayBetListCollection")]
    public class CancelledBetExcelHandlerTests
    {
        private readonly BetListFixture betListFixture;
        private readonly Workbook<TicketDto> workbook;
        private readonly CancelledBetService service;
        private readonly CancelledBetExcelHandler handler;
        private readonly CancelledBetExcelQuery query;

        public CancelledBetExcelHandlerTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;
            service = Substitute.For<CancelledBetService>();
            query = new CancelledBetExcelQuery(betListFixture.User);
            query.RtfHelper = this.betListFixture.RtfHelper;

            workbook = Substitute.For<Workbook<TicketDto>>();
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<TicketDto>>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);
            workbook.Footer(Arg.Any<TicketDto>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);
            workbook.Sheet(Arg.Any<string>()).Returns(workbook);

            handler = new CancelledBetExcelHandler(
                service,
                betListFixture.MoniterService,
                betListFixture.HtmlBuilder,
                betListFixture.ExcelBuilder,
                betListFixture.TicketHelper,
                betListFixture.IpService,
                workbook);

            handler.IsFileExist = path => true;
        }

        [Fact]
        public void ExcelTemplatePath_UserIsAgent_ReturnCorrectExcelPath()
        {
            //// Arrange
            var expectedPath = "App_Data/ExcelTemplates/CancelledBet/Agent.xls";
            handler.MapPath = path => expectedPath;

            //// Act
            var act = handler.ExcelTemplatePath(UserLevel.Agent);

            //// Assert
            Assert.Equal(expectedPath, act);
        }

        [Fact]
        public void Handle_Always_CallGetCancelledBetFromService()
        {
            //// Arrange
            var model = new CancelledBetModel(new FilterCancelledBetModel(), BetListDataTest.BetListData);
            service.GetCancelledBet(Arg.Any<CancelledBetParam>()).Returns(model);

            //// Act
            handler.Handle(query).GetAwaiter().GetResult();

            //// Assert
            service.Received().GetCancelledBet(Arg.Any<CancelledBetParam>());
        }

        [Fact]
        public void Handle_Always_CallUseTemplate()
        {
            //// Arrange
            var model = new CancelledBetModel(new FilterCancelledBetModel(), BetListDataTest.BetListData);
            service.GetCancelledBet(Arg.Any<CancelledBetParam>()).Returns(model);

            //// Act
            handler.Handle(query).GetAwaiter().GetResult();

            //// Assert
            workbook.Received().UseTemplate(Arg.Any<TemplateInfo>());
        }

        [Fact]
        public void Handle_Always_CallHeaderOfWorkbook()
        {
            //// Arrange
            var model = new CancelledBetModel(new FilterCancelledBetModel(), BetListDataTest.BetListData);
            service.GetCancelledBet(Arg.Any<CancelledBetParam>()).Returns(model);

            //// Act
            handler.Handle(query).GetAwaiter().GetResult();

            //// Assert
            workbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void Handle_Always_CallTitleOfWorkbook()
        {
            //// Arrange
            var model = new CancelledBetModel(new FilterCancelledBetModel(), BetListDataTest.BetListData);
            service.GetCancelledBet(Arg.Any<CancelledBetParam>()).Returns(model);

            //// Act
            handler.Handle(query).GetAwaiter().GetResult();

            //// Assert
            workbook.Received().Title(Arg.Any<string>());
        }

        [Fact]
        public void Handle_Always_CallSheetOfWorkbook()
        {
            //// Arrange
            var model = new CancelledBetModel(new FilterCancelledBetModel(), BetListDataTest.BetListData);
            service.GetCancelledBet(Arg.Any<CancelledBetParam>()).Returns(model);

            //// Act
            handler.Handle(query).GetAwaiter().GetResult();

            //// Assert
            workbook.Received().Sheet(Arg.Any<string>());
        }

        [Fact]
        public void Handle_Always_CallExportToBytesOfWorkbook()
        {
            //// Arrange
            var model = new CancelledBetModel(new FilterCancelledBetModel(), BetListDataTest.BetListData);
            service.GetCancelledBet(Arg.Any<CancelledBetParam>()).Returns(model);

            //// Act
            handler.Handle(query).GetAwaiter().GetResult();

            //// Assert
            workbook.Received().ExportToBytes();
        }
    }
}
