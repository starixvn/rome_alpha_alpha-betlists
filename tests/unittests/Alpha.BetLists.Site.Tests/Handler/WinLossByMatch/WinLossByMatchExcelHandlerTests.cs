using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Alpha.BetLists.Models;
using Alpha.BetLists.Params;
using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Handlers.WinLossByMatch;
using Alpha.BetLists.Site.Requests.WinLossByMatch;
using Alpha.BetLists.Site.Tests.Constants;
using Alpha.BetLists.Site.Tests.Fixtures;
using Alpha.BetLists.Tests.TestData;
using Fanex.BetList.Core.Entities;
using NSubstitute;
using ONEbook.SpreadSheets;
using Xunit;

namespace Alpha.BetLists.Site.Tests.Handler.WinLossByMatch
{
    [Trait("Category", "WinLossByMatch")]
    [Collection("BetListCollection")]
    public class WinLossByMatchExcelHandlerTests
    {
        private readonly WinLossByMatchExcelHandler winLossHandler;
        private readonly IUser user;
        private readonly Workbook<TicketDto> workbook;
        private readonly DateTime today;

        public WinLossByMatchExcelHandlerTests(BetListFixture betlistFixture)
        {
            var winLossService = Substitute.For<WinLossByMatchService>();
            winLossService.GetBetList(Arg.Any<WinLossByMatchParam>()).Returns(BetListDataTest.BetListData);

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

            winLossHandler = Substitute.ForPartsOf<WinLossByMatchExcelHandler>(
                winLossService,
                betlistFixture.MoniterService,
                betlistFixture.HtmlBuilder,
                betlistFixture.ExcelBuilder,
                betlistFixture.TicketHelper,
                betlistFixture.IpService,
                workbook);
            winLossHandler.MapPath = (path) => path;
            winLossHandler.IsFileExist = (path) => true;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            today = new DateTime(2017, 4, 27);
        }

        [Fact]
        public void HandleExportWinLossByMatchExcelQuery_Always_UsesCorrectTemplateInfo()
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
            workbook.Received().Title(Arg.Is("MEMBER BET LIST 4/27/2017 --> 4/27/2017 - CUSTNAME"));
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

        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        [InlineData(2)]
        public void HandleExportWinLossByMatchExcelQuery_Always_ExportsExcelToBytes(byte custRoleId)
        {
            // Arrange
            var fakeUser = Substitute.For<IUser>();
            fakeUser.Id.Returns(2);
            fakeUser.LevelId.Returns(custRoleId);

            var message = CreateWinLossByMatchExcelQuery(custLevelId: custRoleId, fakeUser: fakeUser);

            // Act
            winLossHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            workbook.Received().ExportToBytes();
        }

        private WinLossByMatchExcelQuery CreateWinLossByMatchExcelQuery(byte custLevelId, IUser fakeUser = null)
            => new WinLossByMatchExcelQuery(fakeUser ?? user)
            {
                CustId = 1,
                ProductIds = "1,2",
                FromDate = today,
                ToDate = today,
                Language = "en",

                LeagueId = 2,
                MatchId = 3,
                BetType = 4,
                Race = "5",
                LiveIndicator = 1,
                CustName = "CustName",
                CustLevelId = custLevelId
            };
    }
}
