namespace Alpha.BetLists.Site.Tests.Controllers
{
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Requests.JackpotDetails;
    using Site.Controllers;
    using Xunit;

    [Trait("Category", "JackpotDetails")]
    public class JackpotDetailsControllerTests
    {
        private readonly IUser user;
        private readonly IMediator mediator;
        private readonly JackpotDetailsController controller;

        public JackpotDetailsControllerTests()
        {
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            mediator = Substitute.For<IMediator>();
            controller = new JackpotDetailsController(mediator);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendJackpotDetailsQuery()
        {
            // Arrange
            var query = new JackpotDetailsQuery(user);

            // Act
            controller.Index(query).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(query);
        }
    }
}