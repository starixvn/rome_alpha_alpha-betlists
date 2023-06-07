namespace Alpha.BetLists.Site.Tests.Handler.CasinoDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Builders.ExcelBuilders;
    using Fanex.BetDetails.Casino.Models;
    using Fanex.BetDetails.Casino.Params;
    using Fanex.BetDetails.Casino.Services;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Handlers.CasinoDetails;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Requests.CasinoDetails;
    using Site.ViewModels;
    using Site.ViewModels.CasinoDetails;
    using Xunit;
    using static ONEbook.SpreadSheets.Column;

    [Trait("Category", "CasinoDetails")]
    [Collection("BetListCollection")]
    public class CasinoDetailsHandlerExcelTests
    {
        private readonly CompareLogic comparer;
        private readonly BetListFixture betlistFixture;
        private readonly IUser user;
        private readonly CasinoDetailsExcelQuery message;
        private readonly Workbook<TicketDto> workbook;
        private readonly PagedData<ITicket> detailData;
        private readonly ICasinoDetailsService casinoDetailsService;
        private readonly CasinoDetailsExcelHandler casinoDetailsExcelHandler;

        public CasinoDetailsHandlerExcelTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            comparer = new CompareLogic();
            user = betlistFixture.User;
            message = new CasinoDetailsExcelQuery(user)
            {
                RefNo = 1,
                TransType = 1,
                SportName = "SportName",
                ProductId = 1,
                PageIndex = 0,
                PageSize = 0
            };
            message.RtfHelper = betlistFixture.RtfHelper;

            workbook = Substitute.For<Workbook<TicketDto>>();
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<TicketDto>>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);
            workbook.Footer(Arg.Any<TicketDto>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);
            workbook.ExportToBytes().Returns(BetListDataTest.ExcelFileContent);

            detailData = new PagedData<ITicket>(BetListDataTest.Tickets, 0, 0, 0);
            casinoDetailsService = Substitute.For<ICasinoDetailsService>();
            casinoDetailsService.GetCasinoDetails(Arg.Any<CasinoDetailsParam>()).Returns(detailData);

            casinoDetailsExcelHandler = new CasinoDetailsExcelHandler(
                casinoDetailsService,
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
        }

        [Fact]
        public void HandlerExportCasinoDetailsExcelQuery_Always_ReturnsExcelFileViewModel()
        {
            // Arrange
            var param = CasinoDetailsHandler.CreateCasinoDetailsParam(message);
            var casinoDetailsViewModel = new CasinoDetailsViewModel(
                message,
                param,
                detailData,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            workbook.ExportToBytes().Returns(BetListDataTest.ExcelFileContent);
            var expectedViewModel = new ExcelFileViewModel(casinoDetailsViewModel.Title, BetListDataTest.ExcelFileContent);

            // Act
            var actualViewModel = casinoDetailsExcelHandler.Handle(message);

            // Assert
            Assert.True(comparer.Compare(expectedViewModel, actualViewModel).AreEqual);
        }

        [Fact]
        public void HandlerExportCasinoDetailsExcelQuery_Always_CallsGetCasinoDetailsFromService()
        {
            // Arrange
            var param = CasinoDetailsHandler.CreateCasinoDetailsParam(message);
            var casinoDetailsViewModel = new CasinoDetailsViewModel(
                message,
                param,
                detailData,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            workbook.ExportToBytes().Returns(BetListDataTest.ExcelFileContent);
            var expectedViewModel = new ExcelFileViewModel(casinoDetailsViewModel.Title, BetListDataTest.ExcelFileContent);

            // Act
            casinoDetailsExcelHandler.Handle(message);

            // Assert
            casinoDetailsService.Received().GetCasinoDetails(Arg.Is<CasinoDetailsParam>(p => comparer.Compare(param, p).AreEqual));
        }

        [Fact]
        public void HandlerExportCasinoDetailsExcelQuery_Always_UsesCorrectTemplateInfo()
        {
            // Act
            casinoDetailsExcelHandler.Handle(message);

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(
                tp => tp.NoInfomationText == "No information is available" &&
                tp.BodyStartIndex == 2 &&
                tp.BodyEndIndex == 3 &&
                tp.FooterEndIndex == 4));
        }

        [Fact]
        public void HandlerExportCasinoDetailsExcelQuery_Always_ExportsCorrectHeader()
        {
            // Act
            casinoDetailsExcelHandler.Handle(message);

            // Assert
            workbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandlerExportCasinoDetailsExcelQuery_Always_ExportsCorrectTitle()
        {
            // Act
            var expectedTitle = "SportName Details";

            // Act
            casinoDetailsExcelHandler.Handle(message);

            // Assert
            workbook.Received().Title(Arg.Is(expectedTitle));
        }

        [Fact]
        public void HandlerExportCasinoDetailsExcelQuery_Always_ExportsExcelToBytes()
        {
            // Act
            casinoDetailsExcelHandler.Handle(message);

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void GetBodyRowMapper_Choice_MapBuildChoice()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            betlistFixture.ExcelBuilder.CreateChoiceBuilder(ticket.BetTypeId)
               .RenderRTF(ticket, betlistFixture.TicketHelper, null, false, betlistFixture.RtfHelper)
               .Returns(BetListDataTest.ChoiceRtf);

            // Act
            var mapper = casinoDetailsExcelHandler.GetBodyRowMapper(UserLevel.Agent, message.DisplayVNIP);

            // Assert
            Assert.Equal(BetListDataTest.ChoiceRtf.FirstOrDefault(), mapper.Mappings[C].Invoke(ticket));
        }

        [Fact]
        public void GetBodyRowMapper_Odds_MapBuildOdds()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            betlistFixture.ExcelBuilder.CreateOddsBuilder(ticket.BetTypeId)
                   .RenderRTF(ticket, null, casinoDetailsExcelHandler.GetOddsTypeName, betlistFixture.RtfHelper)
                   .Returns(BetListDataTest.OddsRtf);

            // Act
            var mapper = casinoDetailsExcelHandler.GetBodyRowMapper(UserLevel.Agent, message.DisplayVNIP);

            // Assert
            Assert.Equal(BetListDataTest.OddsRtf, mapper.Mappings[D].Invoke(ticket));
        }

        [Fact]
        public void GetBodyRowMapper_Status_MapBuildStatus()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;

            betlistFixture.ExcelBuilder.CreateStatusBuilder(ticket.BetTypeId)
                 .RenderRTF(ticket, betlistFixture.TicketHelper, null, message.DisplayVNIP, betlistFixture.RtfHelper)
                 .Returns(BetListDataTest.StatusRtf);

            // Act
            var mapper = casinoDetailsExcelHandler.GetBodyRowMapper(UserLevel.Agent, message.DisplayVNIP);

            // Assert
            Assert.Equal(BetListDataTest.StatusRtf, mapper.Mappings[G].Invoke(ticket));
        }

        [Fact]
        public void GetBodyRowMapper_PTComm_AgentLevel_MapBuildPTComm()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            var positionTakingCommRtf = BetListExcelBuilderExt.BuildPtCommmission(ticket, null, UserLevel.Agent, betlistFixture.RtfHelper);

            // Act
            var mapper = casinoDetailsExcelHandler.GetBodyRowMapper(UserLevel.Agent, message.DisplayVNIP);

            // Assert
            Assert.Equal(positionTakingCommRtf, mapper.Mappings[H].Invoke(ticket));
        }

        [Fact]
        public void GetBodyRowMapper_PTComm_MasterLevel_MapBuildPTComm()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            var positionTakingCommMasterRtf
                = BetListExcelBuilderExt.BuildPtCommmission(ticket, null, UserLevel.Master, betlistFixture.RtfHelper);
            var positionTakingCommAgentRtf
                = BetListExcelBuilderExt.BuildPtCommmission(ticket, null, UserLevel.Agent, betlistFixture.RtfHelper);

            // Act
            var mapper = casinoDetailsExcelHandler.GetBodyRowMapper(UserLevel.Master, message.DisplayVNIP);

            // Assert
            Assert.True(mapper.Mappings[H].Invoke(ticket).Equals(positionTakingCommMasterRtf)
                && mapper.Mappings[I].Invoke(ticket).Equals(positionTakingCommAgentRtf));
        }

        [Fact]
        public void GetBodyRowMapper_PTComm_SuperLevel_MapBuildPTComm()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            var positionTakingCommSuperRtf
                = BetListExcelBuilderExt.BuildPtCommmission(ticket, null, UserLevel.Super, betlistFixture.RtfHelper);
            var positionTakingCommMasterRtf
                = BetListExcelBuilderExt.BuildPtCommmission(ticket, null, UserLevel.Master, betlistFixture.RtfHelper);
            var positionTakingCommAgentRtf
                = BetListExcelBuilderExt.BuildPtCommmission(ticket, null, UserLevel.Agent, betlistFixture.RtfHelper);

            // Act
            var mapper = casinoDetailsExcelHandler.GetBodyRowMapper(UserLevel.Super, message.DisplayVNIP);

            // Assert
            Assert.True(mapper.Mappings[H].Invoke(ticket).Equals(positionTakingCommSuperRtf)
                && mapper.Mappings[I].Invoke(ticket).Equals(positionTakingCommMasterRtf)
                && mapper.Mappings[J].Invoke(ticket).Equals(positionTakingCommAgentRtf));
        }
    }
}
