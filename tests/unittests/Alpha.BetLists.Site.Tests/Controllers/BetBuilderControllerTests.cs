using Alpha.BetLists.Models;
using Alpha.BetLists.Site.Controllers;
using MediatR;
using NSubstitute;
using System.Web.Mvc;
using Xunit;

namespace Alpha.BetLists.Site.Tests.Controllers
{
    [Trait("Category", "BetBuilder")]
    public class BetBuilderControllerTests
    {
        private readonly IUser user;
        private readonly IMediator mediator;
        private readonly BetBuilderController controller;

        public BetBuilderControllerTests()
        {
            user = Substitute.For<IUser>();
            mediator = Substitute.For<IMediator>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            controller = new BetBuilderController(mediator);
        }

        [Fact]
        public void BetBuilder_ViewDetail_Always_ReturnPartialViewResult ()
        {
            // Arrange
            var act = controller.ViewDetail(new Requests.BetBuilder.BetBuilderDetailQuery()).GetAwaiter().GetResult();

            // Assert
            Assert.True(act is PartialViewResult);
        }
    }
}
