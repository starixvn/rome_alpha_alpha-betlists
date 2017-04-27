using System;
using Alpha.BetLists.Models;
using Alpha.BetLists.Params;
using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Handlers.WinLossByMatch;
using Alpha.BetLists.Site.Requests.WinLossByMatch;
using Alpha.BetLists.Site.Tests.Fixtures;
using KellermanSoftware.CompareNetObjects;
using NSubstitute;
using Xunit;

namespace Alpha.BetLists.Site.Tests.Handler.WinLossByMatch
{
    [Trait("Category", "WinLossByMatch")]
    [Collection("BetListCollection")]
    public class WinLossByMatchBaseHandlerTests
    {
        private readonly WinLossByMatchBaseHandler winLossBaseHandler;
        private readonly WinLossByMatchService winLossService;
        private readonly CompareLogic compareLogic;

        public WinLossByMatchBaseHandlerTests(BetListFixture betlistFixture)
        {
            winLossService = Substitute.For<WinLossByMatchService>();

            winLossBaseHandler = new WinLossByMatchHandler(
                winLossService,
                betlistFixture.MoniterService,
                betlistFixture.HtmlBuilder,
                betlistFixture.ExcelBuilder,
                betlistFixture.TicketHelper,
                betlistFixture.IpService);

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void WinLossService_Always_SetValueFromCtor()
        {
            // Act & Assert
            Assert.Equal(winLossBaseHandler.WinLossService, winLossService);
        }

        [Fact]
        public void CreateWinLossByMatchParam_Always_ReturnWinLossParam()
        {
            // Arrange
            var query = CreateWinLossByMatchQuery();

            var expectedParam = new WinLossByMatchParam(1, 2, 3, 4, "5", 1, "en", "CustName", DateTime.Today, DateTime.Today, UserLevel.Super);

            // Act
            var actualParam = winLossBaseHandler.CreateWinLossParam(query);

            // Assert
            Assert.True(compareLogic.Compare(expectedParam, actualParam).AreEqual);
        }

        private WinLossByMatchQuery CreateWinLossByMatchQuery()
        {
            var user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            return new WinLossByMatchQuery(user)
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
}
