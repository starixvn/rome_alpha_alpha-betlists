namespace Alpha.BetLists.Site.Tests.Handler.BetListByBetType
{
    using System;
    using System.Collections.Generic;
    using BetLists.Helpers;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fanex.RemotingRef;
    using Fixtures;
    using Handlers.BetListByBetType;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Requests.BetListByBetType;
    using Services;
    using Site.ViewModels;
    using Site.ViewModels.BetListByBetType;
    using Xunit;

    [Trait("Category", "BetListByBetType")]
    [Collection("BetListCollection")]
    public class BetListByBetTypeExcelHandlerTests
    {
        private readonly BetListFixture betListFixture;
        private readonly Workbook<TicketDto> workbook;
        private readonly BetListByBetTypeExcelQuery message;
        private readonly RunningBetListByBetTypeService runningBetListByBetTypeService;
        private readonly BetListByBetTypeExcelHandler handler;

        public BetListByBetTypeExcelHandlerTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;

            workbook = Substitute.For<Workbook<TicketDto>>();
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<TicketDto>>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);

            message = new BetListByBetTypeExcelQuery(betListFixture.User);
            message.RtfHelper = betListFixture.RtfHelper;

            runningBetListByBetTypeService = Substitute.For<RunningBetListByBetTypeService>();
            runningBetListByBetTypeService.GetBetList(Arg.Any<RunningBetListByBetTypeParam>()).Returns(BetListDataTest.Tickets);

            handler = new BetListByBetTypeExcelHandler(
                runningBetListByBetTypeService,
                betListFixture.HtmlBuilder,
                betListFixture.ExcelBuilder,
                betListFixture.TicketHelper,
                betListFixture.MonitorService,
                betListFixture.IpService,
                workbook,
                betListFixture.IpInfoService);
            handler.MapPath = (path) => path;
            handler.IsFileExist = (path) => true;

            var cacheRef = Substitute.For<IRemotingRefService>();
            cacheRef.GetBetTypeName(Arg.Any<string>(), Arg.Any<string>()).Returns("abc");
            CacheRefServiceHelper.Instance = cacheRef;
        }

        [Fact]
        public void Handle_Always_CallGetBetListFromService()
        {
            // Arrange
            var compareLogic = new CompareLogic();
            message.BetType = 13;
            var param = new RunningBetListByBetTypeParam(
                message.ViewerId,
                message.BetType,
                message.Language,
                message.ViewerLevel);

            // Act
            handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            runningBetListByBetTypeService.Received().GetBetList(Arg.Is<RunningBetListByBetTypeParam>(p => compareLogic.Compare(param, p).AreEqual));
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

        [Theory]
        [InlineData(13, "Clean Sheet")]
        [InlineData(24, "Double Chance")]
        [InlineData(26, "Both/One/Neither Team To Score")]
        [InlineData(27, "To Win To Nil")]
        [InlineData(28, "3-Way Handicap")]
        public void Handle_Always_ExportsCorrectTitle(int betType, string betTypeName)
        {
            // Arrange
            CacheRefServiceHelper
                .Instance
                .GetBetTypeName(betType.ToString(), Arg.Any<string>()).Returns(betTypeName);

            message.BetType = betType;
            var expectedTitle = betTypeName.ToUpperInvariant();

            // Act
            handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Title(Arg.Is(expectedTitle));
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
            betListFixture.User.LevelId.Returns(custRoleId);
            var query = new BetListByBetTypeExcelQuery(betListFixture.User);
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
            var betListByBetTypeViewModel = new BetListByBetTypeViewModel(message, new BetListData(BetListDataTest.Tickets, null), null, betListFixture.TicketHelper);
            workbook.ExportToBytes().Returns(BetListDataTest.ExcelFileContent);
            var expectedViewModel = new ExcelFileViewModel(betListByBetTypeViewModel.Title, BetListDataTest.ExcelFileContent);
            var compareLogic = new CompareLogic();

            // Act
            var actualViewModel = handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(actualViewModel, expectedViewModel).AreEqual);
        }
    }
}
