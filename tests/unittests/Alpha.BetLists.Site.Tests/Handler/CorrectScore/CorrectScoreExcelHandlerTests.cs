namespace Alpha.BetLists.Site.Tests.Handler.CorrectScore
{
    using System;
    using System.Collections.Generic;
    using BetLists.Constants;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fixtures;
    using Handlers.CorrectScore;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using RemotingRef;
    using Requests.CorrectScore;
    using Services;
    using Site.ViewModels;
    using Site.ViewModels.CorrectScore;
    using Xunit;

    [Trait("Category", "CorrectScore")]
    [Collection("BetListCollection")]
    public class CorrectScoreExcelHandlerTests
    {
        private readonly BetListFixture betListFixture;
        private readonly Workbook<TicketDto> workbook;
        private readonly CorrectScoreExcelQuery message;
        private readonly CorrectScoreService correctScoreService;
        private readonly CorrectScoreExcelHandler correctScoreExcelHandler;

        public CorrectScoreExcelHandlerTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;
            correctScoreService = Substitute.For<CorrectScoreService>();
            correctScoreService.GetBetList(Arg.Any<CorrectScoreParam>()).Returns(BetListDataTest.Tickets);

            message = new CorrectScoreExcelQuery(betListFixture.User);
            message.RtfHelper = betListFixture.RtfHelper;

            workbook = Substitute.For<Workbook<TicketDto>>();
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<TicketDto>>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);

            correctScoreExcelHandler = new CorrectScoreExcelHandler(
                correctScoreService,
                betListFixture.HtmlBuilder,
                betListFixture.ExcelBuilder,
                betListFixture.TicketHelper,
                betListFixture.MoniterService,
                betListFixture.IpService,
                workbook);
            correctScoreExcelHandler.MapPath = (path) => path;
            correctScoreExcelHandler.IsFileExist = (path) => true;
        }

        [Fact]
        public void Handle_Always_CallGetBetListFromService()
        {
            // Arrange
            var compareLogic = new CompareLogic();
            var correctScoreParam = new CorrectScoreParam(
                message.ViewerId,
                message.Language,
                message.ViewerLevel);

            // Act
            correctScoreExcelHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            correctScoreService.Received().GetBetList(Arg.Is<CorrectScoreParam>(p => compareLogic.Compare(correctScoreParam, p).AreEqual));
        }

        [Fact]
        public void Handle_Always_UsesCorrectTemplateInfo()
        {
            // Act
            correctScoreExcelHandler.Handle(message).GetAwaiter().GetResult();

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
            // Arrange
            var cacheRef = Substitute.For<CacheRefService>();
            cacheRef.GetBetTypeName(Arg.Is(BetTypeIdConstants.CorrectScore)).Returns("Correct Score");
            CacheRefFactory.CacheRef = cacheRef;

            // Act
            correctScoreExcelHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Title(Arg.Is("CORRECT SCORE"));
        }

        [Fact]
        public void Handle_Always_ExportsCorrectHeader()
        {
            // Act
            correctScoreExcelHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        [InlineData(2)]
        public void HandleExportWinLossByMatchExcelQuery_Always_ExportsExcelToBytes(byte custRoleId)
        {
            // Arrange
            betListFixture.User.LevelId.Returns(custRoleId);
            var query = new CorrectScoreExcelQuery(betListFixture.User);
            query.RtfHelper = betListFixture.RtfHelper;

            // Act
            correctScoreExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void Handle_Always_ReturnExcelViewModel()
        {
            // Arrange
            var correctScoreViewModel = new CorrectScoreViewModel(message, BetListDataTest.BetListData, null, betListFixture.TicketHelper);
            workbook.ExportToBytes().Returns(BetListDataTest.ExcelFileContent);
            var expectedViewModel = new ExcelFileViewModel(correctScoreViewModel.Title, BetListDataTest.ExcelFileContent);
            var compareLogic = new CompareLogic();

            // Act
            var actualViewModel = correctScoreExcelHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(actualViewModel, expectedViewModel).AreEqual);
        }
    }
}
