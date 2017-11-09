namespace Alpha.BetLists.Site.Tests.Handler.BullfightingByMatch
{
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fixtures;
    using Handlers.BullfightingByMatch;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.BullfightingByMatch;
    using Services;
    using Site.ViewModels.BullfightingByMatch;
    using Xunit;

    [Trait("Category", "BullfightingByMatch")]
    [Collection("BetListCollection")]
    public class BullfightingByMatchHandlerTests
    {
        private readonly CompareLogic compareLogic;
        private readonly BetListFixture betlistFixture;
        private readonly BullfightingByMatchQuery message;        
        private readonly BullfightingByMatchService service;
        private readonly BullfightingByMatchHandler handler;

        public BullfightingByMatchHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            compareLogic = new CompareLogic();
            message = new BullfightingByMatchQuery(betlistFixture.User);
            
            service = Substitute.For<BullfightingByMatchService>();
            service.GetBetList(Arg.Any<BullfightingByMatchParam>()).Returns(BetListDataTest.Tickets);

            handler = new BullfightingByMatchHandler(
                service,                
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper,
                betlistFixture.IpService);
        }        

        [Fact]
        public void Handle_Always_CallGetBetListFromService()
        {
            // Arrange
            var param = new BullfightingByMatchParam(
                message.ViewerId,
                message.MatchId,
                message.ViewerLevel);

            // Act
            handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            service.Received().GetBetList(
                Arg.Is<BullfightingByMatchParam>(
                    p => compareLogic.Compare(param, p).AreEqual));
        }
        
        [Fact]
        public void Handle_Always_ReturnsBullfightingByMatchViewModel()
        {
            // Arrange
            var expectedViewModel = new BullfightingByMatchViewModel(
                message,
                new BetListData(BetListDataTest.Tickets, null),
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            // Act
            var actualViewModel = handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }
    }
}
