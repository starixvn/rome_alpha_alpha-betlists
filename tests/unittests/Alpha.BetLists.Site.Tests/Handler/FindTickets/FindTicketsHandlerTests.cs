using Alpha.BetLists.Site.Tests.Constants;

namespace Alpha.BetLists.Site.Tests.Handler.FindTickets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Extensions;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Handlers.FindTickets;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.FindTickets;
    using Services;
    using Site.ViewModels.FindTickets;
    using Xunit;

    [Trait("Category", "FindTicketsBetList")]
    [Collection("BetListCollection")]
    public class FindTicketsHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly FindTicketsHandler FindTicketsHandler;
        private readonly FindTicketsService FindTicketsService;
        private readonly FindTicketsQuery query;
        private readonly CompareLogic compareLogic;

        public FindTicketsHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            FindTicketsService = Substitute.For<FindTicketsService>();
            FindTicketsService.GetBetList(Arg.Any<FindTicketsParam>()).Returns(BetListDataTest.BetListData);
            var reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetMoveBetsDay().Returns(DefaultVariablesTest.MoveBetDate);

            query = new FindTicketsQuery(this.betlistFixture.User);

            FindTicketsHandler = new FindTicketsHandler(
                FindTicketsService,
                this.betlistFixture.HtmlBuilder,
                this.betlistFixture.TicketHelper,
                this.betlistFixture.MonitorService,
                this.betlistFixture.IpService,
                this.betlistFixture.IpInfoService);

            compareLogic = new CompareLogic(new ComparisonConfig
            {
                MembersToIgnore = new List<string> { "ResultsPath", "OldMainPath" }
            });
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 10)]
        public void Handle_Always_CallWriteLog(int custId, int expectedId)
        {
            // Arrange
            query.CustId = custId;

            // Act
            FindTicketsHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            betlistFixture.MonitorService.Received().WriteLog(
                Arg.Is<int>(cid => cid == expectedId),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(BetListDataTest.Tickets)));
        }

        [Fact]
        public void Handle_Always_CallGetBetListFromService()
        {
            // Arrange & Act
            FindTicketsHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            FindTicketsService.Received().GetBetList(Arg.Any<FindTicketsParam>());
        }

        [Fact]
        public void Hanlde_Always_ReturnFindTicketsViewModel()
        {
            // Arrange
            var expectedViewModel = new FindTicketsViewModel(
                query,
                BetListDataTest.BetListData,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            // Act
            var viewModel = FindTicketsHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(viewModel, expectedViewModel).AreEqual);
        }
    }
}