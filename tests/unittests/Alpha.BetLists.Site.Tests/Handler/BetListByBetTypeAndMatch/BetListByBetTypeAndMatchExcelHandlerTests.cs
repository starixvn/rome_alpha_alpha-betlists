namespace Alpha.BetLists.Site.Tests.Handler.BetListByBetTypeAndMatch
{
    using System;
    using System.Collections.Generic;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fixtures;
    using Handlers.BetListByBetTypeAndMatch;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Requests.BetListByBetTypeAndMatch;
    using Services;
    using Site.ViewModels;
    using Site.ViewModels.BetListByBetTypeAndMatch;
    using Xunit;

    [Trait("Category", "BetListByBetTypeAndMatch")]
    [Collection("BetListCollection")]
    public class BetListByBetTypeAndMatchExcelHandlerTests
    {
        private readonly BetListFixture betListFixture;
        private readonly Workbook<TicketDto> workbook;
        private readonly BetListByBetTypeAndMatchExcelQuery message;
        private readonly RunningBetListByBetTypeAndMatchService runningBetListByBetTypeAndMatchService;
        private readonly BetListByBetTypeAndMatchExcelHandler handler;

        public BetListByBetTypeAndMatchExcelHandlerTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;

            workbook = Substitute.For<Workbook<TicketDto>>();
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<TicketDto>>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);

            message = new BetListByBetTypeAndMatchExcelQuery(betListFixture.User);
            message.RtfHelper = betListFixture.RtfHelper;

            runningBetListByBetTypeAndMatchService = Substitute.For<RunningBetListByBetTypeAndMatchService>();
            runningBetListByBetTypeAndMatchService
                .GetBetList(Arg.Any<RunningBetListByBetTypeAndMatchParam>())
                .Returns(BetListDataTest.BetListData);

            handler = new BetListByBetTypeAndMatchExcelHandler(
                runningBetListByBetTypeAndMatchService,
                betListFixture.HtmlBuilder,
                betListFixture.ExcelBuilder,
                betListFixture.TicketHelper,
                betListFixture.MonitorService,
                betListFixture.IpService,
                workbook,
                betListFixture.IpInfoService);
            handler.MapPath = (path) => path;
            handler.IsFileExist = (path) => true;
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

            var query = new BetListByBetTypeAndMatchExcelQuery(fakeUser);
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
            var betListByBetTypeAndMatchViewModel = new BetListByBetTypeAndMatchViewModel(message, BetListDataTest.BetListData, null, betListFixture.TicketHelper);
            workbook.ExportToBytes().Returns(BetListDataTest.ExcelFileContent);
            var expectedViewModel = new ExcelFileViewModel(betListByBetTypeAndMatchViewModel.Title, BetListDataTest.ExcelFileContent);
            var compareLogic = new CompareLogic();

            // Act
            var actualViewModel = handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(actualViewModel, expectedViewModel).AreEqual);
        }
    }
}
