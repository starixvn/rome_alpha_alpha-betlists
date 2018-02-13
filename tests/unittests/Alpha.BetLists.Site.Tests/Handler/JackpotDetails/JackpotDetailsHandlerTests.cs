namespace Alpha.BetLists.Site.Tests.Handler.JackpotDetails
{
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fixtures;
    using Handlers.JackpotDetails;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.JackpotDetails;
    using Services;
    using Site.ViewModels.JackpotDetails;
    using Xunit;

    [Trait("Category", "JackpotDetails")]
    [Collection("BetListCollection")]
    public class JackpotDetailsHandlerTests
    {
        private readonly IUser user;
        private readonly BetListFixture betlistFixture;
        private readonly JackpotDetailsService jackpotDetailsService;
        private readonly JackpotDetailsHandler handler;

        public JackpotDetailsHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;

            user = betlistFixture.User;
            jackpotDetailsService = Substitute.For<JackpotDetailsService>();

            handler = new JackpotDetailsHandler(
                jackpotDetailsService,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper,
                betlistFixture.MonitorService,
                betlistFixture.IpService);
        }

        [Fact]
        public void Handle_Always_CallsGetJackpotDetailsFromService()
        {
            // Arrange
            var message = new JackpotDetailsQuery(user);

            // Act
            handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            jackpotDetailsService.Received().GetJackpotDetails(
                Arg.Is<JackpotDetailsParam>(
                    p => p.TransId == message.TransId
                        && p.CustId == message.CustId
                        && p.BetType == message.BetType));
        }

        [Fact]
        public void Handle_Always_ReturnsCasinoBetDetailsViewModel()
        {
            // Arrange
            var compareLogic = new CompareLogic();
            var message = new JackpotDetailsQuery(user);

            jackpotDetailsService.GetJackpotDetails(Arg.Any<JackpotDetailsParam>()).Returns(BetListDataTest.Tickets);

            var expectedViewModel = new JackpotDetailsViewModel(
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