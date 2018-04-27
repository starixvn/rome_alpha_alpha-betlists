namespace Alpha.BetLists.Site.Tests.Handler.ParlayWinLossByMatch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Handlers.ParlayWinLossByMatch;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.WinLossByMatch;
    using Services;
    using Site.ViewModels.ParlayWinLossByMatch;
    using Xunit;

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
                this.betlistFixture.MonitorService,
                this.betlistFixture.HtmlBuilder,
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
            betlistFixture.MonitorService.Received().WriteLog(
                Arg.Is<int>(cid => cid == expectedId),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(BetListDataTest.Tickets)));
        }

        [Fact]
        public void Handle_Always_CallGetBetListFromService()
        {
            // Arrange
            var query = CreateWinLossByMatchQuery();

            var expectedParam = new ParlayWinLossByMatchParam(1, 2, "en", DateTime.Today, DateTime.Today, DateTime.MinValue, UserLevel.Super);

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

        [Fact]
        public void CreateWinLossByMatchParam_Always_ReturnWinLossParam()
        {
            // Arrange
            user.LevelId.Returns(UserLevel.Agent.Value);
            var query = CreateWinLossByMatchQuery();
            var expectedParam = new ParlayWinLossByMatchParam(1, 2, "en", DateTime.Today, DateTime.Today, DateTime.MinValue, UserLevel.Super);

            // Act
            var actualParam = ParlayWinLossByMatchHandler.CreateWinLossParam(query);

            // Assert
            Assert.True(compareLogic.Compare(expectedParam, actualParam).AreEqual);
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