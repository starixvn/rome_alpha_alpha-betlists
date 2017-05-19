namespace Alpha.BetLists.Site.Tests.Controllers
{
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Requests.Outstanding;
    using Site.Controllers;
    using Xunit;

    [Trait("Category", "Outstanding")]
    public class OutstandingControllerTests
    {
        private readonly IMediator mediator;
        private readonly OutstandingController outstandingController;
        private readonly IUser user;

        public OutstandingControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            outstandingController = new OutstandingController(mediator);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendOutstandingQuery()
        {
            // Arrange
            var message = new OutstandingQuery(user);

            // Act
            outstandingController.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }
    }
}