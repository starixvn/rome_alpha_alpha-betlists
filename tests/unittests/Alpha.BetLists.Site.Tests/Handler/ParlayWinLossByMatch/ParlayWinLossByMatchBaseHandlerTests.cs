using System;
using Alpha.BetLists.Models;
using Alpha.BetLists.Params;
using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Handlers.ParlayWinLossByMatch;
using Alpha.BetLists.Site.Requests.WinLossByMatch;
using Alpha.BetLists.Site.Tests.Fixtures;
using KellermanSoftware.CompareNetObjects;
using NSubstitute;
using Xunit;

namespace Alpha.BetLists.Site.Tests.Handler.ParlayWinLossByMatch
{
    [Trait("Category", "ParlayWinLossByMatch")]
    [Collection("BetListCollection")]
    public class ParlayWinLossByMatchBaseHandlerTests
    {
        private readonly ParlayWinLossByMatchBaseHandler winLossBaseHandler;
        private readonly ParlayWinLossByMatchService winLossService;
        private readonly CompareLogic compareLogic;

        public ParlayWinLossByMatchBaseHandlerTests(BetListFixture betlistFixture)
        {
            winLossService = Substitute.For<ParlayWinLossByMatchService>();

            winLossBaseHandler = new ParlayWinLossByMatchBaseHandler(
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
            var user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            var query = new ParlayWinLossByMatchQuery(user)
            {
                CustId = 1,
                ProductIds = "1,2",
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                Language = "en",
                BetType = 4,
                CustName = "CustName",
                CustLevelId = 4
            };

            var expectedParam = new ParlayWinLossByMatchParam(1, 4, "en", DateTime.Today, DateTime.Today, UserLevel.Super);

            // Act
            var actualParam = winLossBaseHandler.CreateWinLossParam(query);

            // Assert
            Assert.True(compareLogic.Compare(expectedParam, actualParam).AreEqual);
        }
    }
}
