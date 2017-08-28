namespace Alpha.BetLists.Site.Tests.Handler.CombinationMixParlay
{
    using System;
    using System.Collections.Generic;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fixtures;
    using Handlers.CombinationMixParlay;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Requests.CombinationMixParlay;
    using Services;
    using Site.ViewModels;
    using Site.ViewModels.CombinationMixParlay;
    using Xunit;

    [Trait("Category", "CombinationMixParlay")]
    [Collection("BetListCollection")]
    public class CombinationMixParlayExcelHandlerTests
    {
        private readonly BetListFixture betListFixture;
        private readonly Workbook<TicketDto> workbook;
        private readonly CombinationMixParlayExcelQuery message;
        private readonly CombinationMixParlayExcelHandler combinationMixParlayExcelHandler;
        private readonly RunningBetListByBetTypeService runningBetListByBetTypeService;

        public CombinationMixParlayExcelHandlerTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;
            runningBetListByBetTypeService = Substitute.For<RunningBetListByBetTypeService>();
            runningBetListByBetTypeService.GetBetList(Arg.Any<RunningBetListByBetTypeParam>()).Returns(BetListDataTest.Tickets);

            message = new CombinationMixParlayExcelQuery(betListFixture.User);
            message.RtfHelper = betListFixture.RtfHelper;

            workbook = Substitute.For<Workbook<TicketDto>>();
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<TicketDto>>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);

            combinationMixParlayExcelHandler = new CombinationMixParlayExcelHandler(
                runningBetListByBetTypeService,
                betListFixture.HtmlBuilder,
                betListFixture.ExcelBuilder,
                betListFixture.TicketHelper,
                betListFixture.MoniterService,
                betListFixture.IpService,
                workbook);
            combinationMixParlayExcelHandler.MapPath = (path) => path;
            combinationMixParlayExcelHandler.IsFileExist = (path) => true;
        }

        [Fact]
        public void Handle_Always_CallGetBetListFromService()
        {
            // Arrange
            var compareLogic = new CompareLogic();
            var param = new RunningBetListByBetTypeParam(
                message.ViewerId,
                message.BetType,
                message.Language,
                message.ViewerLevel);

            // Act
            combinationMixParlayExcelHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            runningBetListByBetTypeService.Received().GetBetList(Arg.Is<RunningBetListByBetTypeParam>(p => compareLogic.Compare(param, p).AreEqual));
        }

        [Fact]
        public void Handle_Always_UsesCorrectTemplateInfo()
        {
            // Act
            combinationMixParlayExcelHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(
                tp => tp.NoInfomationText == "No information is available" &&
                tp.BodyStartIndex == 2 &&
                tp.BodyEndIndex == 3 &&
                tp.FooterEndIndex == 4));
        }

        [Fact]
        public void Handle_Always_ExportsCorrectTitle()
        {
            // Act
            combinationMixParlayExcelHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Title(Arg.Is("SYSTEM PARLAY"));
        }

        [Fact]
        public void Handle_Always_ExportsCorrectHeader()
        {
            // Act
            combinationMixParlayExcelHandler.Handle(message).GetAwaiter().GetResult();

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
            var query = new CombinationMixParlayExcelQuery(betListFixture.User);
            query.RtfHelper = betListFixture.RtfHelper;

            // Act
            combinationMixParlayExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void Handle_Always_ReturnExcelViewModel()
        {
            // Arrange
            var combinationMixParlayViewModel = new CombinationMixParlayViewModel(message, BetListDataTest.BetListData, null, betListFixture.TicketHelper);
            workbook.ExportToBytes().Returns(BetListDataTest.ExcelFileContent);
            var expectedViewModel = new ExcelFileViewModel(combinationMixParlayViewModel.Title, BetListDataTest.ExcelFileContent);
            var compareLogic = new CompareLogic();

            // Act
            var actualViewModel = combinationMixParlayExcelHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(actualViewModel, expectedViewModel).AreEqual);
        }
    }
}
