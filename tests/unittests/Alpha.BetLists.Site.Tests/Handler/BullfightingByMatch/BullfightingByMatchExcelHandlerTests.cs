namespace Alpha.BetLists.Site.Tests.Handler.BullfightingByMatch
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using BetLists.Helpers;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Constants;
    using Fanex.RemotingRef;
    using Fixtures;
    using Handlers.BullfightingByMatch;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Requests.BullfightingByMatch;
    using Services;
    using Site.ViewModels;
    using Site.ViewModels.BullfightingByMatch;
    using Xunit;

    [Trait("Category", "BullfightingByMatch")]
    [Collection("BetListCollection")]
    public class BullfightingByMatchExcelHandlerTests
    {
        private readonly BetListFixture betListFixture;
        private readonly Workbook<TicketDto> workbook;
        private readonly BullfightingByMatchExcelQuery message;
        private readonly BullfightingByMatchService service;
        private readonly BullfightingByMatchExcelHandler handler;

        public BullfightingByMatchExcelHandlerTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;

            workbook = Substitute.For<Workbook<TicketDto>>();
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<TicketDto>>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);
            workbook.Footer(Arg.Any<TicketDto>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);

            message = new BullfightingByMatchExcelQuery(betListFixture.User);
            message.RtfHelper = betListFixture.RtfHelper;

            service = Substitute.For<BullfightingByMatchService>();
            service.GetBetList(Arg.Any<BullfightingByMatchParam>()).Returns(BetListDataTest.Tickets);

            handler = new BullfightingByMatchExcelHandler(
                service,
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

            var cacheRef = Substitute.For<IRemotingRefService>();
            cacheRef.GetBetTypeName(Arg.Any<string>(), Arg.Any<string>()).Returns("abc");
            CacheRefServiceHelper.Instance = cacheRef;
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
            workbook.Received().Title(Arg.Is("BULLFIGHTING"));
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

            var query = new BullfightingByMatchExcelQuery(fakeUser);
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
            var bullfightingByMatchViewModel = new BullfightingByMatchViewModel(message, new BetListData(BetListDataTest.Tickets, null), null, betListFixture.TicketHelper);
            workbook.ExportToBytes().Returns(BetListDataTest.ExcelFileContent);
            var expectedViewModel = new ExcelFileViewModel(bullfightingByMatchViewModel.Title, BetListDataTest.ExcelFileContent);
            var compareLogic = new CompareLogic();

            // Act
            var actualViewModel = handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(actualViewModel, expectedViewModel).AreEqual);
        }
    }
}
