namespace Alpha.BetLists.Site.Tests.Handler.WinLossByMatch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Handlers.WinLossByMatch;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.WinLossByMatch;
    using Services;
    using Site.ViewModels.WinLossByMatch;
    using Xunit;

    [Trait("Category", "WinLossByMatch")]
    [Collection("BetListCollection")]
    public class WinLossByMatchHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly WinLossByMatchHandler winLossHandler;
        private readonly WinLossByMatchService winLossService;
        private readonly IUser user;
        private readonly CompareLogic compareLogic;

        public WinLossByMatchHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            winLossService = Substitute.For<WinLossByMatchService>();
            winLossService.GetBetList(Arg.Any<WinLossByMatchParam>()).Returns(BetListDataTest.BetListData);

            user = Substitute.For<IUser>();
            user.Id.Returns(2);
            user.LevelId.Returns(UserLevel.Agent.Value);

            winLossHandler = new WinLossByMatchHandler(
                winLossService,
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
        [InlineData(0, 2)]
        public void Handle_Always_CallWriteLog(int custId, int expectedId)
        {
            // Arrange
            var query = new WinLossByMatchQuery(user)
            {
                CustId = custId,
                CustLevelId = 2
            };

            // Act
            winLossHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            betlistFixture.MonitorService.Received().WriteLog(
                Arg.Is<int>(cid => cid == expectedId),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(BetListDataTest.Tickets)));
        }

        [Fact]
        public void Handle_Always_CallGetBetListFromService()
        {
            // Arrange
            var query = new WinLossByMatchQuery(user)
            {
                CustId = 1,
                CustLevelId = 2,
                CustName = "CustName",
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                Language = "en",

                LeagueId = 2,
                MatchId = 3,
                BetType = 4,
                Race = "5",
                LiveIndicator = 1
            };

            var expectedParam = new WinLossByMatchParam(1, 2, 3, 4, "5", 1, "en", "CustName", DateTime.Today, DateTime.Today, UserLevel.Agent);

            // Act
            winLossHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            winLossService.Received().GetBetList(Arg.Is<WinLossByMatchParam>(p => compareLogic.Compare(expectedParam, p).AreEqual));
        }

        [Fact]
        public void Hanlde_Always_ReturnWinLossByMatchViewModel()
        {
            // Arrange
            var query = CreateWinLossByMatchQuery();
            var expectedViewModel = new WinLossByMatchViewModel(
                query,
                BetListDataTest.BetListData,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            // Act
            var viewModel = winLossHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(viewModel, expectedViewModel).AreEqual);
        }

        [Fact]
        public void CreateWinLossByMatchParam_Always_ReturnWinLossParam()
        {
            // Arrange
            var query = CreateWinLossByMatchQuery();

            var expectedParam = new WinLossByMatchParam(1, 2, 3, 4, "5", 1, "en", "CustName", DateTime.Today, DateTime.Today, UserLevel.Super);

            // Act
            var actualParam = WinLossByMatchHandler.CreateWinLossParam(query);

            // Assert
            Assert.True(compareLogic.Compare(expectedParam, actualParam).AreEqual);
        }

        private WinLossByMatchQuery CreateWinLossByMatchQuery()
            => new WinLossByMatchQuery(user)
            {
                CustId = 1,
                ProductIds = "1,2",
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                Language = "en",

                LeagueId = 2,
                MatchId = 3,
                BetType = 4,
                Race = "5",
                LiveIndicator = 1,
                CustName = "CustName",
                CustLevelId = 4
            };
    }
}