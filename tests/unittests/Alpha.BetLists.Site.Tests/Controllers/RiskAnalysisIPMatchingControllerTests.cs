namespace Alpha.BetLists.Site.Tests.Controllers
{
    using Alpha.BetLists.Site.Requests.RiskAnalysisIPMatching;
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Site.Controllers;
    using Xunit;

    [Trait("Category", "RiskAnalysisIPMatching")]
    public class RiskAnalysisIPMatchingControllerTests
    {
        private readonly IMediator mediator;
        private readonly RiskAnalysisIPMatchingController controller;
        private readonly IUser user;

        public RiskAnalysisIPMatchingControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            controller = new RiskAnalysisIPMatchingController(mediator);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendRiskAnalysisIPMatchingQuery()
        {
            // Arrange
            var message = new RiskAnalysisIPMatchingQuery(user);

            // Act
            controller.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }
    }
}