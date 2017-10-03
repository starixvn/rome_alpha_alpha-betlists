namespace Alpha.BetLists.Site.Tests.Handler.RacingByMatch
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Constants;
    using Fixtures;
    using Handlers.RacingByMatch;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Requests.RacingByMatch;
    using Services;
    using Site.ViewModels;
    using Site.ViewModels.RacingByMatch;
    using Xunit;

    [Trait("Category", "RacingByMatch")]
    [Collection("BetListCollection")]
    public class RacingByMatchExcelHandlerTests
    {
        private readonly BetListFixture betListFixture;
        private readonly Workbook<TicketDto> workbook;
        private readonly RacingByMatchExcelQuery message;
        private readonly RacingByMatchService racingByMatchService;
        private readonly RacingByMatchExcelHandler handler;

        public RacingByMatchExcelHandlerTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;

            workbook = Substitute.For<Workbook<TicketDto>>();
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<TicketDto>>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);
            workbook.Footer(Arg.Any<TicketDto>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);

            message = new RacingByMatchExcelQuery(betListFixture.User);
            message.RtfHelper = betListFixture.RtfHelper;

            racingByMatchService = Substitute.For<RacingByMatchService>();
            racingByMatchService.GetBetList(Arg.Any<RacingByMatchParam>()).Returns(BetListDataTest.Tickets);

            handler = new RacingByMatchExcelHandler(
                racingByMatchService,
                betListFixture.HtmlBuilder,
                betListFixture.ExcelBuilder,
                betListFixture.TicketHelper,
                betListFixture.MonitorService,
                betListFixture.IpService,
                workbook);
            handler.MapPath = (path) => path;
            handler.IsFileExist = (path) => true;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void Handle_Always_UsesCorrectTemplateInfo()
        {
            // Act
            handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(
                tp => tp.NoInfomationText == "No information is available" &&
                tp.BodyStartIndex == 2 &&
                tp.BodyEndIndex == 3 &&
                tp.FooterEndIndex == 4));
        }

        [Fact]
        public void Handle_Always_ExportsCorrectHeader()
        {
            // Act
            handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void Handle_Always_ExportsCorrectTitle()
        {
            // Act
            handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Title(Arg.Is("RACING"));
        }

        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        [InlineData(2)]
        public void Handle_Always_ExportsExcelToBytes(byte custRoleId)
        {
            // Arrange
            var fakeUser = Substitute.For<IUser>();
            fakeUser.Id.Returns(2);
            fakeUser.LevelId.Returns(custRoleId);

            var query = new RacingByMatchExcelQuery(fakeUser);
            query.RtfHelper = betListFixture.RtfHelper;

            // Act
            handler.Handle(query).GetAwaiter().GetResult();

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void Handle_Always_ReturnExcelViewModel()
        {
            // Arrange
            var racingByMatchViewModel = new RacingByMatchViewModel(message, new BetListData(BetListDataTest.Tickets, null), null, betListFixture.TicketHelper);
            workbook.ExportToBytes().Returns(BetListDataTest.ExcelFileContent);
            var expectedViewModel = new ExcelFileViewModel(racingByMatchViewModel.Title, BetListDataTest.ExcelFileContent);
            var compareLogic = new CompareLogic();

            // Act
            var actualViewModel = handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(actualViewModel, expectedViewModel).AreEqual);
        }
    }
}
