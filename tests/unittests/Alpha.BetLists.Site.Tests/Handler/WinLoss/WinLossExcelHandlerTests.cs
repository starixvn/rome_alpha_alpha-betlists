namespace Alpha.BetLists.Site.Tests.Handler.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using BetLists.Tests.TestData;
    using Extensions;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Handlers.WinLoss;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Requests.WinLoss;
    using Services;
    using Xunit;

    [Trait("Category", "WinLossBetList")]
    [Collection("BetListCollection")]
    public class WinLossExcelHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly WinLossExcelHandler winLossExcelHandler;
        private readonly WinLossService winLossService;
        private readonly WinLossExcelQuery query;
        private readonly Workbook<ITicket> workbook;
        private readonly CompareLogic compareLogic;

        public WinLossExcelHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            winLossService = Substitute.For<WinLossService>();
            winLossService.GetBetList(Arg.Any<WinLossParam>()).Returns(BetListDataTest.BetListData);

            query = new WinLossExcelQuery(this.betlistFixture.User);
            InitQuery();

            workbook = Substitute.For<Workbook<ITicket>>();
            InitWorkbook();

            winLossExcelHandler = new WinLossExcelHandler(
                winLossService,
                this.betlistFixture.HtmlBuilder,
                this.betlistFixture.ExcelBuilder,
                this.betlistFixture.TicketHelper,
                this.betlistFixture.MoniterService,
                this.betlistFixture.IpService,
                workbook);

            winLossExcelHandler.MapPath = (path) => path;
            winLossExcelHandler.IsFileExist = (path) => true;

            compareLogic = new CompareLogic(new ComparisonConfig
            {
                MembersToIgnore = new List<string> { "ResultsPath", "OldMainPath" }
            });
        }

        private void InitWorkbook()
        {
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<ITicket>>(), Arg.Any<Mapper<ITicket>>()).Returns(workbook);
            workbook.Sheet(Arg.Any<string>()).Returns(workbook);
            workbook.Footer(Arg.Any<ITicket>(), Arg.Any<Mapper<ITicket>>()).Returns(workbook);
        }

        private void InitQuery()
        {
            query.CustId = 1;
            query.ProductIds = "1";
            query.FromDate = DateTime.Today;
            query.ToDate = DateTime.Today;
            query.Language = "en";
        }

        [Fact]
        public void Handle_Always_CallGetBetListFromService()
        {
            // Act
            winLossExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            winLossService.Received().GetBetList(Arg.Is<WinLossParam>(
                p => p.CustId == query.CustId
                && p.ProductIds.SequenceEqual(query.ProductIds.ToListInt())
                && p.FromDate == query.FromDate
                && p.ToDate == query.ToDate
                && p.Language == query.Language));
        }

        [Fact]
        public void Handle_Workbook_UseTemplate()
        {
            // Act
            winLossExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(
                t => t.TemplatePath == $"~/App_Data/ExcelTemplates/{query.ViewerLevel.DisplayName}.xls"
                && t.NoInfomationText == Resources.NoInformationIsAvailable
                && t.BodyStartIndex == 3 - 1
                && t.BodyEndIndex == 4 - 1
                && t.FooterEndIndex == 5 - 1));
        }

        [Fact]
        public void Handle_Workbook_Header()
        {
            // Act
            winLossExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Header(Arg.Is<Func<string, string>>(f => f == Resources.ResourceManager.GetString));
        }

        [Fact]
        public void Handle_Workbook_Title()
        {
            // Arrange
            var title = $"{Resources.memberBetList} {query.FromDate.ToShortDateString()} --> {query.ToDate.ToShortDateString()} - {query.CustName}";

            // Act
            winLossExcelHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            workbook.Received().Title(Arg.Is(title.ToUpperInvariant()));
        }
    }
}