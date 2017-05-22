namespace Alpha.BetLists.Site.Tests.Handler
{
    using AppResources;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Builders.ExcelBuilders;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Fixtures;
    using Handlers;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Requests.WinLoss;
    using Site.ViewModels.Blocks;
    using Site.ViewModels.WinLoss;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using Xunit;
    using static ONEbook.SpreadSheets.Column;

    [Trait("Category", "BaseBetList")]
    [Collection("BetListCollection")]
    public class BetListBaseHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly BetListBaseHandler baseHandler;
        private readonly WinLossQuery query;
        private readonly WinLossViewModel viewModel;
        private readonly CompareLogic compareLogic;

        public BetListBaseHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;

            baseHandler = new BetListBaseHandler(
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
        public void Builder_Always_SetValueFromCtor()
        {
            // Act & Assert
            Assert.Equal(baseHandler.Builder, betlistFixture.HtmlBuilder);
        }

        [Fact]
        public void ExcelBuilder_Always_SetValueFromCtor()
        {
            // Act & Assert
            Assert.Equal(baseHandler.ExcelBuilder, betlistFixture.ExcelBuilder);
        }

        [Fact]
        public void TicketHelper_Always_SetValueFromCtor()
        {
            // Act && Assert
            Assert.Equal(baseHandler.TicketHelper, betlistFixture.TicketHelper);
        }

        [Fact]
        public void TicketHelper_Always_SetIsVnIpFromIpService()
        {
            // Act & Assert
            Assert.Equal(baseHandler.TicketHelper.IsVnIp, betlistFixture.IpService.IsVNIP);
        }

        [Fact]
        public void MonitorService_Always_SetValueFromCtor()
        {
            // Act & Assert
            Assert.Equal(baseHandler.MonitorService, betlistFixture.MoniterService);
        }

        [Fact]
        public void IpService_Always_SetValueFromCtor()
        {
            // Act & Assert
            Assert.Equal(baseHandler.IpService, betlistFixture.IpService);
        }

        [Fact]
        public void RtfHelper_Always_SetValueFromExcelBuilder()
        {
            // Act & Assert
            Assert.Equal(baseHandler.RtfHelper, betlistFixture.ExcelBuilder.RTFHelper);
        }

        [Fact]
        public void GetOddsTypeName_Always_ReturnDefaultValue()
        {
            // Act & Assert
            Assert.Equal(CacheRef.GetOddsTypeName, baseHandler.GetOddsTypeName);
        }

        [Fact]
        public void MapPath_WhenMapPathIsNotNull_ReturnMapPathValue()
        {
            // Arrange
            Func<string, string> mapPath = (path) => "path";

            // Act
            baseHandler.MapPath = mapPath;

            // Assert
            Assert.Equal(baseHandler.MapPath, mapPath);
        }

        [Fact]
        public void MapPath_WhenMapPathIsNull_ReturnServerMapPath()
        {
            // Act
            baseHandler.MapPath = null;

            // Assert
            Assert.Equal(baseHandler.MapPath, HostingEnvironment.MapPath);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void GetExcelTemplatePath_TemplateIsNullOrWhiteSpace_ThrowArgumentNullException(string fileName)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => baseHandler.GetExcelTemplatePath(fileName));
        }

        [Fact]
        public void GetExcelTemplatePath_FileDoesNotExist_ThrowFileNotFoundException()
        {
            // Arrange
            baseHandler.MapPath = path => "path";
            baseHandler.IsFileExist = path => false;

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => baseHandler.GetExcelTemplatePath("test"));
        }

        [Fact]
        public void GetExcelTemplatePath_FileExists_ReturnPath()
        {
            // Arrange
            baseHandler.MapPath = relativePath => "path/" + relativePath;
            baseHandler.IsFileExist = absolutePath => true;

            // Act
            var path = baseHandler.GetExcelTemplatePath("test");

            // Assert
            Assert.Equal("path/~/App_Data/ExcelTemplates/test.xls", path);
        }

        [Fact]
        public void WriteLog_Always_CallMonitorServiceWriteLog()
        {
            // Arrange
            var tickets = BetListDataTest.Tickets;
            var custId = 1;

            // Act
            baseHandler.WriteLog(custId, tickets);

            // Assert
            betlistFixture.MoniterService.Received().WriteLog(
                Arg.Is<int>(cid => cid == custId),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(tickets)));
        }

        [Fact]
        public void GetTemplateInfo_Always_ReturnTemplateInfo()
        {
            // Arrange
            baseHandler.IsFileExist = path => true;
            baseHandler.MapPath = path => path;
            var expectedTemplateInfo = new ONEbook.SpreadSheets.TemplateInfo(
                $"~/App_Data/ExcelTemplates/{UserLevel.Agent.DisplayName}.xls",
                noInfomationText: Resources.NoInformationIsAvailable,
                bodyStartIndex: 3,
                bodyEndIndex: 4,
                footerEndIndex: 5);

            // Act
            var actualTemplateInfo = baseHandler.GetTemplateInfo(UserLevel.Agent);

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
            var mapper = baseHandler.GetBodyRowMapper(UserLevel.Agent, viewModel);

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
            var mapper = baseHandler.GetBodyRowMapper(UserLevel.Agent, viewModel);

            // Assert
            Assert.Equal(BetListDataTest.ChoiceRtf.FirstOrDefault(), mapper.Mappings[C].Invoke(ticket));
        }

        [Fact]
        public void GetBodyRowMapper_Odds_MapBuildOdds()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            betlistFixture.ExcelBuilder.CreateOddsBuilder(ticket.BetTypeId)
                   .RenderRTF(ticket, viewModel.BetListData.TicketData, baseHandler.GetOddsTypeName, betlistFixture.RtfHelper)
                   .Returns(BetListDataTest.OddsRtf);

            // Act
            var mapper = baseHandler.GetBodyRowMapper(UserLevel.Agent, viewModel);

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
            var mapper = baseHandler.GetBodyRowMapper(UserLevel.Agent, viewModel);

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
            var mapper = baseHandler.GetBodyRowMapper(UserLevel.Agent, viewModel);

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
            var mapper = baseHandler.GetBodyRowMapper(UserLevel.Agent, viewModel);

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
            var mapper = baseHandler.GetBodyRowMapper(UserLevel.Agent, viewModel);

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
            var mapper = baseHandler.GetBodyRowMapper(UserLevel.Agent, viewModel);

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
            var mapper = baseHandler.GetBodyRowMapper(UserLevel.Master, viewModel);

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
            var mapper = baseHandler.GetBodyRowMapper(UserLevel.Super, viewModel);

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
            var mapper = baseHandler.GetFooterRowMapper(footer);

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
            var mapper = baseHandler.GetFooterRowMapper(footer);

            // Assert
            Assert.Equal(footerRtf, mapper.Mappings[F].Invoke(BetListDataTest.Ticket));
        }

        [Fact]
        public void BuildProductSelectListItems_NullProducts_ReturnsEmptySelectListItem()
        {
            // Arrange
            IEnumerable<Product> products = null;
            var expectedSelectListItems = Enumerable.Empty<SelectListItem>();

            // Act
            var actualSelectListItems = baseHandler.BuildProductSelectListItems(Enumerable.Empty<int>(), products);

            // Assert
            Assert.Equal(expectedSelectListItems, actualSelectListItems);
        }

        [Fact]
        public void BuildProductSelectListItems_EmptyProductList_ReturnsEmptySelectListItem()
        {
            // Arrange
            var products = Enumerable.Empty<Product>();
            var userSelectedProductIds = Enumerable.Empty<int>();
            var expectedSelectListItems = Enumerable.Empty<SelectListItem>();

            // Act
            var actualSelectListItems = baseHandler.BuildProductSelectListItems(userSelectedProductIds, products);

            // Assert
            Assert.Equal(expectedSelectListItems, actualSelectListItems);
        }

        [Fact]
        public void BuildProductSelectListItems_HasDataProductList_ReturnsCorrectSelectListItem()
        {
            // Arrange
            Func<int, string> getProductDisplayName = (id) => $"Product {id}";

            IEnumerable<Product> products = new List<Product>
            {
                new Product(1, "Product 1", getProductDisplayName),
                new Product(2, "Product 2", getProductDisplayName),
                new Product(3, "Product 3", getProductDisplayName)
            };

            var userSelectedProductIds = new List<int> { 1, 3 };
            IEnumerable<SelectListItem> expectedSelectListItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Product 1",
                    Value = "1",
                    Selected = true
                },
                new SelectListItem
                {
                    Text = "Product 2",
                    Value = "2",
                    Selected = false
                },
                new SelectListItem
                {
                    Text = "Product 3",
                    Value = "3",
                    Selected = true
                }
            };

            // Act
            var actualSelectListItems = baseHandler.BuildProductSelectListItems(userSelectedProductIds, products).ToList();

            // Assert
            Assert.True(compareLogic.Compare(expectedSelectListItems, actualSelectListItems).AreEqual);
        }
    }
}