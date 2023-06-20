namespace Alpha.BetLists.Site.Tests.Handler.JackpotDetails
{
    using System;
    using System.Collections.Generic;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fixtures;
    using Handlers.JackpotDetails;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Requests.JackpotDetails;
    using Services;
    using Site.ViewModels;
    using Site.ViewModels.JackpotDetails;
    using Xunit;

    [Trait("Category", "JackpotDetails")]
    [Collection("BetListCollection")]
    public class JackpotDetailsHandlerExcelTests
    {
        private readonly BetListFixture betListFixture;
        private readonly Workbook<TicketDto> workbook;
        private readonly JackpotDetailsExcelQuery message;
        private readonly JackpotDetailsService jackpotDetailsService;
        private readonly JackpotDetailsExcelHandler handler;

        public JackpotDetailsHandlerExcelTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;

            workbook = Substitute.For<Workbook<TicketDto>>();
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<TicketDto>>(), Arg.Any<Mapper<TicketDto>>()).Returns(workbook);

            message = new JackpotDetailsExcelQuery(betListFixture.User);
            message.RtfHelper = betListFixture.RtfHelper;

            jackpotDetailsService = Substitute.For<JackpotDetailsService>();
            jackpotDetailsService.GetJackpotDetails(Arg.Any<JackpotDetailsParam>()).Returns(BetListDataTest.Tickets);

            handler = new JackpotDetailsExcelHandler(
                jackpotDetailsService,
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
        public void Handle_Always_CallGetJackpotDetailsFromService()
        {
            // Arrange
            var compareLogic = new CompareLogic();
            message.BetType = 13;
            var param = new JackpotDetailsParam(message.TransId, message.CustId, message.BetType);

            // Act
            handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            jackpotDetailsService.Received().GetJackpotDetails(Arg.Is<JackpotDetailsParam>(p => compareLogic.Compare(param, p).AreEqual));
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
        public void Handle_Always_ExportsCorrectTitle()
        {
            // Act
            handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Title(Arg.Is("Progressive Jackpot Won Details"));
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
            var query = new JackpotDetailsExcelQuery(betListFixture.User);
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
            var jackpotDetailsViewModel = new JackpotDetailsViewModel(message, new BetListData(BetListDataTest.Tickets, null), null, betListFixture.TicketHelper);
            workbook.ExportToBytes().Returns(BetListDataTest.ExcelFileContent);
            var expectedViewModel = new ExcelFileViewModel(jackpotDetailsViewModel.Title, BetListDataTest.ExcelFileContent);
            var compareLogic = new CompareLogic();

            // Act
            var actualViewModel = handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(actualViewModel, expectedViewModel).AreEqual);
        }
    }
}