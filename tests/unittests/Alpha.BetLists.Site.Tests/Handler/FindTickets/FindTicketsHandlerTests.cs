using Alpha.BetLists.Site.Tests.Constants;

namespace Alpha.BetLists.Site.Tests.Handler.SearchTickets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Extensions;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Handlers.SearchTickets;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.SearchTickets;
    using Services;
    using Site.ViewModels.SearchTickets;
    using Xunit;

    [Trait("Category", "SearchTicketsBetList")]
    [Collection("BetListCollection")]
    public class SearchTicketsHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly SearchTicketsHandler SearchTicketsHandler;
        private readonly SearchTicketsService SearchTicketsService;
        private readonly SearchTicketsQuery query;
        private readonly CompareLogic compareLogic;

        public SearchTicketsHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            SearchTicketsService = Substitute.For<SearchTicketsService>();
            SearchTicketsService.GetBetList(Arg.Any<SearchTicketsParam>()).Returns(BetListDataTest.BetListData);
            var reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetMoveBetsDay().Returns(DefaultVariablesTest.MoveBetDate);

            query = new SearchTicketsQuery(this.betlistFixture.User);

            SearchTicketsHandler = new SearchTicketsHandler(
                SearchTicketsService,
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
            SearchTicketsHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            betlistFixture.MonitorService.Received().WriteLog(
                Arg.Is<int>(cid => cid == expectedId),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(BetListDataTest.Tickets)));
        }

        [Fact]
        public void Handle_Always_CallGetBetListFromService()
        {
            // Arrange & Act
            SearchTicketsHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            SearchTicketsService.Received().GetBetList(Arg.Any<SearchTicketsParam>());
        }

        [Fact]
        public void Hanlde_Always_ReturnSearchTicketsViewModel()
        {
            // Arrange
            var expectedViewModel = new SearchTicketsViewModel(
                query,
                BetListDataTest.BetListData,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            // Act
            var viewModel = SearchTicketsHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(viewModel, expectedViewModel).AreEqual);
        }
    }
}