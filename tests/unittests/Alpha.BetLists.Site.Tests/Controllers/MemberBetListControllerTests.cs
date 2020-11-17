namespace Alpha.BetLists.Site.Tests.Controllers
{
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Tests.Constants;
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Requests.MemberBetList;
    using Site.Controllers;
    using Xunit;

    [Trait("Category", "MemberBetList")]
    public class MemberBetListControllerTests
    {
        private readonly IMediator mediator;
        private readonly MemberBetListController memberBetListController;
        private readonly IUser user;

        public MemberBetListControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            memberBetListController = new MemberBetListController(mediator);
            var reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetMoveBetsDay().Returns(DefaultVariablesTest.MoveBetDate);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendMemberBetListQuery()
        {
            // Arrange
            var message = new MemberBetListQuery(user);

            // Act
            memberBetListController.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }
    }
}