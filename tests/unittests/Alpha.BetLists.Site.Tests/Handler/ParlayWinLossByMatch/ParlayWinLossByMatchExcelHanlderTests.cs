using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Alpha.BetLists.Models;
using Alpha.BetLists.Params;
using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Handlers.ParlayWinLossByMatch;
using Alpha.BetLists.Site.Handlers.WinLossByMatch;
using Alpha.BetLists.Site.Requests.WinLossByMatch;
using Alpha.BetLists.Site.Tests.Constants;
using Alpha.BetLists.Site.Tests.Fixtures;
using Alpha.BetLists.Tests.TestData;
using Alpha.RemotingRef;
using Fanex.BetList.Core.Entities;
using NSubstitute;
using ONEbook.SpreadSheets;
using Xunit;

namespace Alpha.BetLists.Site.Tests.Handler.ParlayWinLossByMatch
{
    [Trait("Category", "ParlayWinLossByMatch")]
    [Collection("BetListCollection")]
    public class ParlayWinLossByMatchExcelHanlderTests
    {
        private readonly ParlayWinLossByMatchExcelHanlder winLossHandler;
        private readonly IUser user;
        private readonly Workbook<TicketDto> workbook;
        private readonly DateTime today;

        public ParlayWinLossByMatchExcelHanlderTests(BetListFixture betlistFixture)
        {
            var winLossService = Substitute.For<ParlayWinLossByMatchService>();
            winLossService.GetBetList(Arg.Any<ParlayWinLossByMatchParam>()).Returns(BetListDataTest.Tickets);

            user = Substitute.For<IUser>();
            user.Id.Returns(2);
            user.LevelId.Returns(UserLevel.Agent.Value);

            workbook = Substitute.For<Workbook<TicketDto>>();
            workbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<TicketDto>>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);
            workbook.Footer(Arg.Any<TicketDto>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);
            workbook.Sheet(Arg.Any<string>()).Returns(workbook);

            var winLossByMatchExcelHandler = Substitute.ForPartsOf<WinLossByMatchExcelHandler>(
                null,
                betlistFixture.MoniterService,
                betlistFixture.HtmlBuilder,
                betlistFixture.ExcelBuilder,
                betlistFixture.TicketHelper,
                betlistFixture.IpService,
                workbook);
            winLossByMatchExcelHandler.MapPath = (path) => path;
            winLossByMatchExcelHandler.IsFileExist = (path) => true;

            winLossHandler = Substitute.ForPartsOf<ParlayWinLossByMatchExcelHanlder>(
                winLossService,
                betlistFixture.MoniterService,
                betlistFixture.HtmlBuilder,
                betlistFixture.ExcelBuilder,
                betlistFixture.TicketHelper,
                betlistFixture.IpService,
                workbook,
                winLossByMatchExcelHandler);

            var cacheRef = Substitute.For<CacheRefService>();
            cacheRef.GetBetTypeName(Arg.Any<object>()).Returns(callInfo => $"Bet Type {callInfo.Args()[0]}");
            CacheRefFactory.CacheRef = cacheRef;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            today = new DateTime(2017, 4, 27);
        }

        [Fact]
        public void HandleExportParlayWinLossByMatchExcelQuery_Always_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = CreateWinLossByMatchExcelQuery(custLevelId: 4);

            // Act
            winLossHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(
                tp => tp.NoInfomationText == "No information is available" &&
                tp.BodyStartIndex == 2 &&
                tp.BodyEndIndex == 3 &&
                tp.FooterEndIndex == 4));
        }

        [Fact]
        public void HandleExportWinLossByMatchExcelQuery_Always_ExportsCorrectTitle()
        {
            // Arrange
            var message = CreateWinLossByMatchExcelQuery(custLevelId: 4);

            // Act
            winLossHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Title(Arg.Is("Bet Type 9 4/27/2017 --> 4/27/2017"));
        }

        [Fact]
        public void HandleExportWinLossByMatchExcelQuery_Always_ExportsCorrectHeader()
        {
            // Arrange
            var message = CreateWinLossByMatchExcelQuery(custLevelId: 4);

            // Act
            winLossHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandleExportWinLossByMatchExcelQuery_Always_ExportsCorrectSheetName()
        {
            // Arrange
            var message = CreateWinLossByMatchExcelQuery(custLevelId: 4);

            // Act
            winLossHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Sheet(Arg.Is("Bet List"));
        }

        private ParlayWinLossByMatchExcelQuery CreateWinLossByMatchExcelQuery(byte custLevelId, IUser fakeUser = null)
            => new ParlayWinLossByMatchExcelQuery(fakeUser ?? user)
            {
                CustId = 1,
                ProductIds = "1,2",
                FromDate = today,
                ToDate = today,
                Language = "en",

                BetType = 9,
                CustName = "CustName",
                CustLevelId = custLevelId
            };
    }
}
