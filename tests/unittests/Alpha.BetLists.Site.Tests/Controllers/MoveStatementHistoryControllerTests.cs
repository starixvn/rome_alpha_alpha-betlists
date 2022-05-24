namespace Alpha.BetLists.Site.Tests.Controllers
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.Controllers;
    using Alpha.BetLists.Site.Requests.MoveStatementHistory;
    using MediatR;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "MoveStatementHistory")]
    public class MoveStatementHistoryControllerTests
    {
        private readonly IMediator mediator;
        private readonly MoveStatementHistoryController moveStatemetHistoryController;
        private readonly IUser user;

        public MoveStatementHistoryControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            moveStatemetHistoryController = new MoveStatementHistoryController(mediator);
        }

        [Fact]
        public void Index_Always_CallMediatorSendMoveStatementHistoryQuery()
        {
            // Arrange
            var message = new MoveStatementHistoryQuery(user);

            // Act
            moveStatemetHistoryController.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }
    }
}
