using Alpha.BetLists.Models;
using Alpha.BetLists.Site.Controllers;
using Alpha.BetLists.Site.Requests.Lucky3;
using MediatR;
using NSubstitute;
using Xunit;

namespace Alpha.BetLists.Site.Tests.Controllers
{
    public class Lucky3ControllerTests
    {
        private readonly IMediator mediator;
        private readonly IUser user;
        private Lucky3Controller controller;

        public Lucky3ControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            controller = new Lucky3Controller(mediator);
        }

        [Fact]
        public void ViewDetail_Always_CallMediatorSend()
        {
            //// Arrange
            user.LevelId.Returns(UserLevel.Agent.Value);
            var query = new Lucky3DetailQuery(user);

            //// Act
            var act = controller.ViewDetail(query);

            //// Assert
            mediator.Received();
        }

        [Fact]
        public void ViewDetail_CallMediatorSend_CorrectQuery()
        {
            //// Arrange   
            user.LevelId.Returns(UserLevel.Agent.Value);
            var query = new Lucky3DetailQuery(user);

            //// Act
            var act = controller.ViewDetail(query);

            //// Assert
            mediator.Received().Send(query);
        }
    }
}
