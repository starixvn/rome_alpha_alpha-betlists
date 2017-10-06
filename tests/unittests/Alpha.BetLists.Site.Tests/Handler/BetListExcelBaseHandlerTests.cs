namespace Alpha.BetLists.Site.Tests.Handler
{
    using System;
    using System.IO;
    using System.Linq;
    using AppResources;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Builders.ExcelBuilders;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Fixtures;
    using Handlers;
    using KellermanSoftware.CompareNetObjects;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using Requests.WinLoss;
    using Site.ViewModels.Blocks;
    using Site.ViewModels.WinLoss;
    using Xunit;
    using static ONEbook.SpreadSheets.Column;

    [Trait("Category", "BaseBetList")]
    [Collection("BetListCollection")]
    public class BetListExcelBaseHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly BetListExcelBaseHandler excelBaseHandler;
        private readonly WinLossQuery query;
        private readonly WinLossViewModel viewModel;
        private readonly CompareLogic compareLogic;

        public BetListExcelBaseHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;

            excelBaseHandler = new BetListExcelBaseHandler(
                this.betlistFixture.HtmlBuilder,
                this.betlistFixture.ExcelBuilder,
                this.betlistFixture.TicketHelper,
                this.betlistFixture.MoniterService,
                this.betlistFixture.IpService);

            query = new WinLossQuery(betlistFixture.User);
            viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betlistFixture.HtmlBuilder, betlistFixture.TicketHelper);

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void ExcelBuilder_Always_SetValueFromCtor()
        {
            // Act & Assert
            Assert.Equal(excelBaseHandler.ExcelBuilder, betlistFixture.ExcelBuilder);
        }

        [Fact]
        public void RtfHelper_Always_SetValueFromExcelBuilder()
        {
            // Act & Assert
            Assert.Equal(excelBaseHandler.RtfHelper, betlistFixture.ExcelBuilder.RTFHelper);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void GetExcelTemplatePath_TemplateIsNullOrWhiteSpace_ThrowArgumentNullException(string fileName)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => excelBaseHandler.GetExcelTemplatePath(fileName));
        }

        [Fact]
        public void GetExcelTemplatePath_FileDoesNotExist_ThrowFileNotFoundException()
        {
            // Arrange
            excelBaseHandler.MapPath = path => "path";
            excelBaseHandler.IsFileExist = path => false;

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => excelBaseHandler.GetExcelTemplatePath("test"));
        }

        [Fact]
        public void GetExcelTemplatePath_FileExists_ReturnPath()
        {
            // Arrange
            excelBaseHandler.MapPath = relativePath => "path/" + relativePath;
            excelBaseHandler.IsFileExist = absolutePath => true;

            // Act
            var path = excelBaseHandler.GetExcelTemplatePath("test");

            // Assert
            Assert.Equal("path/~/App_Data/ExcelTemplates/Handlers/test.xls", path);
        }

        [Fact]
        public void GetTemplateInfo_Always_ReturnTemplateInfo()
        {
            // Arrange
            excelBaseHandler.IsFileExist = path => true;
            excelBaseHandler.MapPath = path => path;
            var expectedTemplateInfo = new ONEbook.SpreadSheets.TemplateInfo(
                $"~/App_Data/ExcelTemplates/Handlers/{UserLevel.Agent.DisplayName}.xls",
                noInfomationText: Resources.NoInformationIsAvailable,
                bodyStartIndex: 3,
                bodyEndIndex: 4,
                footerEndIndex: 5);

            // Act
            var actualTemplateInfo = excelBaseHandler.GetTemplateInfo(UserLevel.Agent);

            // Assert
            Assert.True(compareLogic.Compare(expectedTemplateInfo, actualTemplateInfo).AreEqual);
        }

        [Fact]
        public void GetBodyRowMapper_Trans_MapBuildTrans()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            betlistFixture.ExcelBuilder.CreateTransBuilder(ticket.BetTypeId)
                .RenderRTF(ticket, betlistFixture.TicketHelper)
                .Returns(BetListDataTest.TransRtf);

            // Act
            var mapper = excelBaseHandler.GetBodyRowMapper(UserLevel.Agent, viewModel);

            // Assert
            Assert.Equal(BetListDataTest.TransRtf, mapper.Mappings[B].Invoke(ticket));
        }

        [Fact]
        public void GetBodyRowMapper_Choice_MapBuildChoice()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            betlistFixture.ExcelBuilder.CreateChoiceBuilder(ticket.BetTypeId)
               .RenderRTF(ticket, betlistFixture.TicketHelper, viewModel.BetListData.TicketData, viewModel.CanViewScoreMap(ticket), betlistFixture.RtfHelper)
               .Returns(BetListDataTest.ChoiceRtf);

            // Act
            var mapper = excelBaseHandler.GetBodyRowMapper(UserLevel.Agent, viewModel);

            // Assert
            Assert.Equal(BetListDataTest.ChoiceRtf.FirstOrDefault(), mapper.Mappings[C].Invoke(ticket));
        }

        [Fact]
        public void GetBodyRowMapper_Odds_MapBuildOdds()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            betlistFixture.ExcelBuilder.CreateOddsBuilder(ticket.BetTypeId)
                   .RenderRTF(ticket, viewModel.BetListData.TicketData, excelBaseHandler.GetOddsTypeName, betlistFixture.RtfHelper)
                   .Returns(BetListDataTest.OddsRtf);

            // Act
            var mapper = excelBaseHandler.GetBodyRowMapper(UserLevel.Agent, viewModel);

            // Assert
            Assert.Equal(BetListDataTest.OddsRtf, mapper.Mappings[D].Invoke(ticket));
        }

        [Fact]
        public void GetBodyRowMapper_Stake_MapBuildStake()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            betlistFixture.ExcelBuilder.CreateStakeBuilder(ticket.BetTypeId)
                    .RenderRTF(ticket, betlistFixture.RtfHelper)
                    .Returns(BetListDataTest.StakeRtf);

            // Act
            var mapper = excelBaseHandler.GetBodyRowMapper(UserLevel.Agent, viewModel);

            // Assert
            Assert.Equal(BetListDataTest.StakeRtf, mapper.Mappings[E].Invoke(ticket));
        }

        [Fact]
        public void GetBodyRowMapper_Status_MapBuildStatus()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;

            betlistFixture.ExcelBuilder.CreateStatusBuilder(ticket.BetTypeId)
                 .RenderRTF(ticket, betlistFixture.TicketHelper, viewModel.BetListData.TicketData, true, betlistFixture.RtfHelper)
                 .Returns(BetListDataTest.StatusRtf);

            // Act
            var mapper = excelBaseHandler.GetBodyRowMapper(UserLevel.Agent, viewModel);

            // Assert
            Assert.Equal(BetListDataTest.StatusRtf, mapper.Mappings[G].Invoke(ticket));
        }

        [Fact]
        public void GetBodyRowMapper_WinLoss_DisplayComm_MapBuildWinLoss()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.BetTypeId = 1;
            var winLossBlockViewModel = new WinLostBlockViewModel(ticket);
            var winLossRtf = BetListExcelBuilderExt.BuildWinLoss(
                winLossBlockViewModel.WinLost,
                winLossBlockViewModel.Comm,
                betlistFixture.RtfHelper);

            // Act
            var mapper = excelBaseHandler.GetBodyRowMapper(UserLevel.Agent, viewModel);

            // Assert
            Assert.Equal(winLossRtf, mapper.Mappings[F].Invoke(ticket));
        }

        [Fact]
        public void GetBodyRowMapper_WinLoss_HideComm_MapBuildWinLoss()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.BetTypeId = 501;
            ticket.SuperDiscount = -1;
            var winLossBlockViewModel = new WinLostBlockViewModel(ticket);
            var winLossRtf = BetListExcelBuilderExt.BuildWinLoss(
                winLossBlockViewModel.WinLost,
                string.Empty,
                betlistFixture.RtfHelper);

            // Act
            var mapper = excelBaseHandler.GetBodyRowMapper(UserLevel.Agent, viewModel);

            // Assert
            Assert.Equal(winLossRtf, mapper.Mappings[F].Invoke(ticket));
        }

        [Fact]
        public void GetBodyRowMapper_PTComm_AgentLevel_MapBuildPTComm()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            var positionTakingCommRtf = BetListExcelBuilderExt.BuildPtCommmission(ticket, viewModel.BetListData.TicketData, UserLevel.Agent, betlistFixture.RtfHelper);

            // Act
            var mapper = excelBaseHandler.GetBodyRowMapper(UserLevel.Agent, viewModel);

            // Assert
            Assert.Equal(positionTakingCommRtf, mapper.Mappings[H].Invoke(ticket));
        }

        [Fact]
        public void GetBodyRowMapper_PTComm_MasterLevel_MapBuildPTComm()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            var positionTakingCommMasterRtf
                = BetListExcelBuilderExt.BuildPtCommmission(ticket, viewModel.BetListData.TicketData, UserLevel.Master, betlistFixture.RtfHelper);
            var positionTakingCommAgentRtf
                = BetListExcelBuilderExt.BuildPtCommmission(ticket, viewModel.BetListData.TicketData, UserLevel.Agent, betlistFixture.RtfHelper);

            // Act
            var mapper = excelBaseHandler.GetBodyRowMapper(UserLevel.Master, viewModel);

            // Assert
            Assert.True(mapper.Mappings[H].Invoke(ticket).Equals(positionTakingCommMasterRtf)
                && mapper.Mappings[I].Invoke(ticket).Equals(positionTakingCommAgentRtf));
        }

        [Fact]
        public void GetBodyRowMapper_PTComm_SuperLevel_MapBuildPTComm()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            var positionTakingCommSuperRtf
                = BetListExcelBuilderExt.BuildPtCommmission(ticket, viewModel.BetListData.TicketData, UserLevel.Super, betlistFixture.RtfHelper);
            var positionTakingCommMasterRtf
                = BetListExcelBuilderExt.BuildPtCommmission(ticket, viewModel.BetListData.TicketData, UserLevel.Master, betlistFixture.RtfHelper);
            var positionTakingCommAgentRtf
                = BetListExcelBuilderExt.BuildPtCommmission(ticket, viewModel.BetListData.TicketData, UserLevel.Agent, betlistFixture.RtfHelper);

            // Act
            var mapper = excelBaseHandler.GetBodyRowMapper(UserLevel.Super, viewModel);

            // Assert
            Assert.True(mapper.Mappings[H].Invoke(ticket).Equals(positionTakingCommSuperRtf)
                && mapper.Mappings[I].Invoke(ticket).Equals(positionTakingCommMasterRtf)
                && mapper.Mappings[J].Invoke(ticket).Equals(positionTakingCommAgentRtf));
        }

        [Fact]
        public void GetFooterMapper_FooterHeader_MapBuildFooterHeader()
        {
            // Arrange
            var footer = new Footer(BetListDataTest.BetListData, UserLevel.Agent, false);
            var footerHeaderRtf = BetListExcelBuilderExt.BuildFooterHeader(betlistFixture.RtfHelper);

            // Act
            var mapper = excelBaseHandler.GetFooterRowMapper(footer);

            // Assert
            Assert.Equal(footerHeaderRtf, mapper.Mappings[A].Invoke(BetListDataTest.Ticket));
        }

        [Fact]
        public void GetFooterMapper_FooterBody_MapBuildFooter()
        {
            // Arrange
            var footer = new Footer(BetListDataTest.BetListData, UserLevel.Agent, false);
            var footerRtf = BetListExcelBuilderExt.BuildFooter(
                footer.SubTotalWinLoss,
                footer.SubTotalCommission,
                footer.Total,
                betlistFixture.RtfHelper);

            // Act
            var mapper = excelBaseHandler.GetFooterRowMapper(footer);

            // Assert
            Assert.Equal(footerRtf, mapper.Mappings[F].Invoke(BetListDataTest.Ticket));
        }

        [Fact]
        public void InitRTFHelper_Always_CallsInitRTFHelperFonts()
        {
            // Arrange
            IWorkbook workbook = Substitute.For<IWorkbook>();

            var rtfHelper = new RTFHelper
            {
                NegFont = workbook.CreateFont(),
                NormalFont = workbook.CreateFont(),
                PosFont = workbook.CreateFont(),
                NegFontCrossed = workbook.CreateFont(),
                NormalFontCrossed = workbook.CreateFont(),
                PosFontCrossed = workbook.CreateFont(),
                RTFRenderer = new RtfTextRender(),
            };
            rtfHelper.PosFont.Boldweight = (short)FontBoldWeight.Bold;
            rtfHelper.PosFontCrossed.Boldweight = (short)FontBoldWeight.Bold;
            rtfHelper.NegFont.Boldweight = (short)FontBoldWeight.Bold;
            rtfHelper.NegFontCrossed.Boldweight = (short)FontBoldWeight.Bold;
            rtfHelper.NormalFont.Boldweight = (short)FontBoldWeight.Normal;
            rtfHelper.NormalFontCrossed.Boldweight = (short)FontBoldWeight.Normal;
            rtfHelper.NegFont.FontName = "Tahoma";
            rtfHelper.NegFontCrossed.FontName = "Tahoma";
            rtfHelper.NormalFont.FontName = "Tahoma";
            rtfHelper.NormalFontCrossed.FontName = "Tahoma";
            rtfHelper.PosFont.FontName = "Tahoma";
            rtfHelper.PosFontCrossed.FontName = "Tahoma";
            rtfHelper.NegFont.FontHeightInPoints = 9;
            rtfHelper.NegFontCrossed.FontHeightInPoints = 9;
            rtfHelper.NormalFont.FontHeightInPoints = 9;
            rtfHelper.NormalFontCrossed.FontHeightInPoints = 9;
            rtfHelper.PosFont.FontHeightInPoints = 9;
            rtfHelper.PosFontCrossed.FontHeightInPoints = 9;
            rtfHelper.NormalFontCrossed.IsStrikeout = true;
            rtfHelper.PosFontCrossed.IsStrikeout = true;
            rtfHelper.NegFontCrossed.IsStrikeout = true;
            rtfHelper.NegFont.Color = (short)FontColor.Red;
            rtfHelper.NegFontCrossed.Color = (short)FontColor.Red;

            // Act
            var actualRTFHelper = BetListExcelBaseHandler.InitRTFHelper(workbook);

            // Assert
            Assert.True(compareLogic.Compare(rtfHelper, actualRTFHelper).AreEqual);
        }
    }
}
