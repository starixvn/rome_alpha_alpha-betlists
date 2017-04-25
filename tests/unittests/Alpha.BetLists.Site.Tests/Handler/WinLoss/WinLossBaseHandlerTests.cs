namespace Alpha.BetLists.Site.Tests.Handler.WinLoss
{
    using System;
    using System.Linq;
    using BetLists.Models;
    using Extensions;
    using Fixtures;
    using Handlers.WinLoss;
    using NSubstitute;
    using Requests.WinLoss;
    using Services;
    using Xunit;

    [Trait("Category", "WinLossBetList")]
    [Collection("BetListCollection")]
    public class WinLossBaseHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly WinLossBaseHandler winLossBaseHandler;
        private readonly WinLossService winLossService;

        public WinLossBaseHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            winLossService = Substitute.For<WinLossService>();

            winLossBaseHandler = new WinLossBaseHandler(
                winLossService,
                this.betlistFixture.HtmlBuilder,
                this.betlistFixture.ExcelBuilder,
                this.betlistFixture.TicketHelper,
                this.betlistFixture.MoniterService,
                this.betlistFixture.IpService);
        }

        [Fact]
        public void WinLossService_Always_SetValueFromCtor()
        {
            // Act & Assert
            Assert.Equal(winLossBaseHandler.WinLossService, winLossService);
        }

        [Fact]
        public void CreateWinLossParam_Always_ReturnWinLossParam()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);
            var query = new WinLossQuery(user)
            {
                CustId = 1,
                ProductIds = "1,2",
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                Language = "en"
            };

            // Act
            var param = winLossBaseHandler.CreateWinLossParam(query);

            // Assert
            Assert.True(param.CustId == query.CustId
                && param.ProductIds.SequenceEqual(query.ProductIds.ToListInt())
                && param.FromDate == query.FromDate
                && param.ToDate == query.ToDate
                && param.Language == query.Language);
        }
    }
}