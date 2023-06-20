namespace Alpha.BetLists.Site.Tests.Handler.MoveStatementHistory
{
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Params;
    using Alpha.BetLists.Site.Handlers.MoveStatementHistory;
    using Alpha.BetLists.Site.Tests.Fixtures;
    using NSubstitute;
    using Xunit;
    using Alpha.BetLists.Tests.TestData;
    using Alpha.BetLists.Site.Requests.MoveStatementHistory;
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using System.Linq;
    using Alpha.BetLists.Site.ViewModels.MoveStatementHistory;
    using KellermanSoftware.CompareNetObjects;
    using Alpha.BetLists.Models;

    [Trait("Category", "MoveStatementHistory")]
    [Collection("BetListCollection")]
    public class MoveStatementHistoryHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly MoveStatementHistoryHandler moveStatementHistoryHandler;
        private readonly MoveStatementHistoryService moveStatementHistoryService;
        private readonly MoveStatementHistoryQuery query;
        private readonly CompareLogic compareLogic;

        public MoveStatementHistoryHandlerTests(BetListFixture betListFixture)
        {
            betlistFixture = betListFixture;
            moveStatementHistoryService = Substitute.For<MoveStatementHistoryService>();
            moveStatementHistoryService.GetBetList(Arg.Any<MoveStatementHistoryParam>()).Returns(BetListDataTest.Tickets);

            query = new MoveStatementHistoryQuery(betlistFixture.User);

            moveStatementHistoryHandler = new MoveStatementHistoryHandler(
                moveStatementHistoryService,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper,
                betlistFixture.MonitorService,
                betlistFixture.IpService,
                betlistFixture.IpInfoService);

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
            moveStatementHistoryHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            betlistFixture.MonitorService.Received().WriteLog(
                Arg.Is<int>(cid => cid == expectedId),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(BetListDataTest.Tickets)));
        }

        [Fact]
        public void Handle_Always_CallGetBetListFromService()
        {
            // Arrange
            query.CustId = 1;
            query.TransId = 123456;

            // Act
            moveStatementHistoryHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            moveStatementHistoryService.Received().GetBetList(Arg.Is<MoveStatementHistoryParam>(
                p => p.CustId == query.CustId
                && p.TransId == query.TransId));
        }

        [Fact]
        public void Hanlde_Always_ReturnMoveStatementHistoryViewModel()
        {
            // Arrange
            var expectedViewModel = new MoveStatementHistoryViewModel(
                query,
                new BetListData(BetListDataTest.Tickets, null),
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper
                );

            // Act
            var viewModel = moveStatementHistoryHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(viewModel, expectedViewModel).AreEqual);
        }
    }
}
