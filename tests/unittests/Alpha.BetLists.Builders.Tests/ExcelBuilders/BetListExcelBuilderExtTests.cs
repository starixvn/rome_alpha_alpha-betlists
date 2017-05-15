namespace Alpha.BetLists.Builders.Tests.ExcelBuilders
{
    using System.Linq;
    using AppResources;
    using BetLists.Tests.TestData;
    using Builders.ExcelBuilders;
    using Builders.PTComm;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using KellermanSoftware.CompareNetObjects;
    using Models;
    using NPOI.HSSF.UserModel;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    public class BetListExcelBuilderExtTests
    {
        private readonly RTFHelper rtfHelper;
        private readonly CompareLogic comparer;

        public BetListExcelBuilderExtTests()
        {
            rtfHelper = CreateRtfHelper();
            comparer = new CompareLogic();
        }

        private RTFHelper CreateRtfHelper()
        {
            var workbook = new HSSFWorkbook();

            return new RTFHelper()
            {
                NegFont = workbook.CreateFont(),
                NormalFont = workbook.CreateFont(),
                PosFont = workbook.CreateFont(),
                NegFontCrossed = workbook.CreateFont(),
                NormalFontCrossed = workbook.CreateFont(),
                PosFontCrossed = workbook.CreateFont(),
                RTFRenderer = new RtfTextRender()
            };
        }

        [Fact]
        public void BuildFooterHeader_Always_RenderExpectedRtfText()
        {
            // Arrange
            var expectedFooterHeaderRtf = new HSSFRichTextString($"{Resources.Subtotal_wl}:\n{Resources.Subtotal_com}:\n{Resources.total}:");

            // Act
            var actualRtfText = BetListExcelBuilderExt.BuildFooterHeader(rtfHelper);

            // Assert
            Assert.Equal(expectedFooterHeaderRtf, actualRtfText);
        }

        [Fact]
        public void BuildFooter_Always_RenderExpectedRtfText()
        {
            // Arrange
            var totalWinLoss = "10";
            var totalCommision = "11";
            var total = "12";
            var rtfNumber = new RTFNumber(rtfHelper.RTFRenderer, rtfHelper.PosFont, rtfHelper.NegFont);
            rtfNumber.Render($"{totalWinLoss}\n{totalCommision}\n{total}");
            var expectedFooterRtf = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            // Act
            var actualRtfText = BetListExcelBuilderExt.BuildFooter(totalWinLoss, totalCommision, total, rtfHelper);

            // Assert
            Assert.Equal(expectedFooterRtf.String, actualRtfText.String);
        }

        [Theory]
        [InlineData("10", "11", "10\n11")]
        [InlineData("", "11", "11")]
        [InlineData("10", "", "10\n")]
        public void BuildWinLoss_WinLossIsNullOrEmpty_NotRenderWinLoss(string winLoss, string comm, string expectedText)
        {
            // Arrange
            var expectedRtfText = new HSSFRichTextString(expectedText);

            // Act
            var actualRtfText = BetListExcelBuilderExt.BuildWinLoss(winLoss, comm, rtfHelper);

            // Assert
            Assert.Equal(expectedRtfText.String, actualRtfText.String);
        }

        [Theory]
        [InlineData(33)]
        [InlineData(1233)]
        public void BuildPTCommmission_RacingTicket_ShowRacingPTValue(int betTypeId)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            var ticketData = BetListDataTest.TicketData.ToList<ITicketData>();
            ticket.BetTypeId = betTypeId;
            var positionTakingComm = PTCommFactory.CreateInstance(ticket, ticketData, UserLevel.Agent) as PTComm33;
            var expectedRtfText
                = new HSSFRichTextString(
                    $"{Resources.Win}: {positionTakingComm.PositionTakingWin}" +
                    $"\n{Resources.place}: {positionTakingComm.PositionTakingPlace}\n" +
                    $"{positionTakingComm.PositionTaking}\n" +
                    $"{positionTakingComm.CommissionPercent}\n" +
                    $"{positionTakingComm.Commission}\n");

            // Act
            var actualRtfText = BetListExcelBuilderExt.BuildPtCommmission(ticket, ticketData, UserLevel.Agent, rtfHelper);

            // Assert
            Assert.Equal(expectedRtfText.String, actualRtfText.String);
        }

        [Fact]
        public void BuildPTCommmission_PTCommPercentIsNullOrNotVisible_HidePTCommPercent()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            var ticketData = BetListDataTest.TicketData.ToList<ITicketData>();
            ticket.BetTypeId = 71;
            var positionTakingComm = PTCommFactory.CreateInstance(ticket, ticketData, UserLevel.Agent);
            var expectedRtfText
                = new HSSFRichTextString(
                    $"{positionTakingComm.PositionTaking}\n" +
                    $"{positionTakingComm.Commission}\n");

            // Act
            var actualRtfText = BetListExcelBuilderExt.BuildPtCommmission(ticket, ticketData, UserLevel.Agent, rtfHelper);

            // Assert
            Assert.Equal(expectedRtfText.String, actualRtfText.String);
        }

        [Fact]
        public void BuildPTCommmission_PTCommValueIsNullOrNotVisible_HidePTCommValue()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            var ticketData = BetListDataTest.TicketData.ToList<ITicketData>();
            ticket.BetTypeId = 1;
            ticket.Status = BetStatus.Running;
            var positionTakingComm = PTCommFactory.CreateInstance(ticket, ticketData, UserLevel.Agent);
            var expectedRtfText
                = new HSSFRichTextString(
                    $"{positionTakingComm.PositionTakingPercent}\n" +
                    $"{positionTakingComm.CommissionPercent}\n");

            // Act
            var actualRtfText = BetListExcelBuilderExt.BuildPtCommmission(ticket, ticketData, UserLevel.Agent, rtfHelper);

            // Assert
            Assert.Equal(expectedRtfText.String, actualRtfText.String);
        }
    }
}