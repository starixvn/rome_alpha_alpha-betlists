namespace Alpha.BetLists.Site.Tests.Controllers
{
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Requests.RunningBetList;
    using Site.Controllers;
    using Xunit;

    [Trait("Category", "RunningBetList")]
    public class RunningBetListControllerTests
    {
        private readonly IMediator mediator;
        private readonly RunningBetListController runningBetListController;
        private readonly IUser user;

        public RunningBetListControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            runningBetListController = new RunningBetListController(mediator);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendRunningBetListQuery()
        {
            // Arrange
            var message = new RunningBetListQuery(user);

            // Act
            runningBetListController.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }
    }
}