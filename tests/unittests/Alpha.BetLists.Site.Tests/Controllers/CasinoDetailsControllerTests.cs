namespace Alpha.BetLists.Site.Tests.Controllers
{
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Requests.CasioDetails;
    using Site.Controllers;
    using Xunit;

    [Trait("Category", "CasinoDetails")]
    public class CasinoDetailsControllerTests
    {
        private readonly IUser user;
        private readonly IMediator mediator;
        private readonly CasinoDetailsController controller;

        public CasinoDetailsControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            controller = new CasinoDetailsController(mediator);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendDailyStatementQuery()
        {
            // Arrange
            var message = new CasinoDetailsQuery(user);

            // Act
            controller.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }
    }
}
