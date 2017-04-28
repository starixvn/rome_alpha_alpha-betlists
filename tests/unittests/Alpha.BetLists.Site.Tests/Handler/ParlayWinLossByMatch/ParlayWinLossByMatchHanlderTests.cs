using System;
using System.Collections.Generic;
using System.Linq;
using Alpha.BetLists.Models;
using Alpha.BetLists.Params;
using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Handlers.ParlayWinLossByMatch;
using Alpha.BetLists.Site.Requests.WinLossByMatch;
using Alpha.BetLists.Site.Tests.Fixtures;
using Alpha.BetLists.Site.ViewModels.ParlayWinLossByMatch;
using Alpha.BetLists.Tests.TestData;
using Fanex.BetList.Core.Entities;
using KellermanSoftware.CompareNetObjects;
using NSubstitute;
using Xunit;

namespace Alpha.BetLists.Site.Tests.Handler.ParlayWinLossByMatch
{
    [Trait("Category", "WinLossByMatch")]
    [Collection("BetListCollection")]
    public class ParlayWinLossByMatchHanlderTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly ParlayWinLossByMatchHandler winLossHandler;
        private readonly ParlayWinLossByMatchService winLossService;
        private readonly IUser user;
        private readonly CompareLogic compareLogic;

        public ParlayWinLossByMatchHanlderTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            winLossService = Substitute.For<ParlayWinLossByMatchService>();
            winLossService.GetBetList(Arg.Any<ParlayWinLossByMatchParam>()).Returns(BetListDataTest.Tickets);

            user = Substitute.For<IUser>();
            user.Id.Returns(2);
            user.LevelId.Returns(UserLevel.Agent.Value);

            winLossHandler = new ParlayWinLossByMatchHandler(
                winLossService,
                this.betlistFixture.MoniterService,
                this.betlistFixture.HtmlBuilder,
                this.betlistFixture.ExcelBuilder,
                this.betlistFixture.TicketHelper,
                this.betlistFixture.IpService);

            compareLogic = new CompareLogic(new ComparisonConfig
            {
                MembersToIgnore = new List<string> { "ResultsPath", "OldMainPath" }
            });
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 2)]
        public void Handle_Always_CallWriteLog(int custId, int expectedId)
        {
            // Arrange
            var query = new ParlayWinLossByMatchQuery(user)
            {
                CustId = custId,
                CustLevelId = 2
            };

            // Act
            winLossHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            betlistFixture.MoniterService.Received().WriteLog(
                Arg.Is<int>(cid => cid == expectedId),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(BetListDataTest.Tickets)));
        }

        [Fact]
        public void Handle_Always_CallGetBetListFromService()
        {
            // Arrange
            var query = CreateWinLossByMatchQuery();

            var expectedParam = new ParlayWinLossByMatchParam(1, 2, "en", DateTime.Today, DateTime.Today, UserLevel.Super);

            // Act
            winLossHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            winLossService.Received().GetBetList(Arg.Is<ParlayWinLossByMatchParam>(p => compareLogic.Compare(expectedParam, p).AreEqual));
        }

        [Fact]
        public void Hanlde_Always_ReturnWinLossByMatchViewModel()
        {
            // Arrange
            var query = CreateWinLossByMatchQuery();
            var expectedViewModel = new ParlayWinLossByMatchViewModel(
                query,
                new BetListData(BetListDataTest.Tickets, null),
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            // Act
            var viewModel = winLossHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(viewModel, expectedViewModel).AreEqual);
        }

        private ParlayWinLossByMatchQuery CreateWinLossByMatchQuery()
            => new ParlayWinLossByMatchQuery(user)
            {
                CustId = 1,
                CustLevelId = 4,
                BetType = 2,
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                Language = "en"
            };
    }
}
