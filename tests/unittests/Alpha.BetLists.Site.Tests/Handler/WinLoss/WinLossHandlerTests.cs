namespace Alpha.BetLists.Site.Tests.Handler.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Extensions;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Handlers.WinLoss;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.WinLoss;
    using Services;
    using ViewModels.WinLoss;
    using Xunit;

    [Trait("Category", "WinLossBetList")]
    [Collection("BetListCollection")]
    public class WinLossHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly WinLossHandler winLossHandler;
        private readonly WinLossService winLossService;
        private readonly IUser user;
        private readonly WinLossQuery query;
        private readonly CompareLogic compareLogic;

        public WinLossHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            winLossService = Substitute.For<WinLossService>();
            winLossService.GetBetList(Arg.Any<WinLossParam>()).Returns(BetListDataTest.BetListData);

            user = Substitute.For<IUser>();
            user.Id.Returns(2);
            user.LevelId.Returns(UserLevel.Agent.Value);
            query = new WinLossQuery(user);

            winLossHandler = new WinLossHandler(
                winLossService,
                this.betlistFixture.HtmlBuilder,
                this.betlistFixture.ExcelBuilder,
                this.betlistFixture.TicketHelper,
                this.betlistFixture.MoniterService,
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
            query.CustId = custId;

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
            query.CustId = 1;
            query.ProductIds = "1";
            query.FromDate = DateTime.Today;
            query.ToDate = DateTime.Today;
            query.Language = "en";

            // Act
            winLossHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            winLossService.Received().GetBetList(Arg.Is<WinLossParam>(
                p => p.CustId == query.CustId
                && p.ProductIds.SequenceEqual(query.ProductIds.ToListInt())
                && p.FromDate == query.FromDate
                && p.ToDate == query.ToDate
                && p.Language == query.Language));
        }

        [Fact]
        public void Hanlde_Always_ReturnWinLossViewModel()
        {
            // Arrange
            var expectedViewModel = new WinLossViewModel(
                query,
                BetListDataTest.BetListData,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            // Act
            var viewModel = winLossHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(viewModel, expectedViewModel).AreEqual);
        }
    }
}