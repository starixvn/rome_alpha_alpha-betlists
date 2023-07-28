using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Tests.Constants;

namespace Alpha.BetLists.Site.Tests.Controllers
{
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Requests.FindTickets;
    using Site.Controllers;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "FindTickets")]
    public class FindTicketsControllerTests
    {
        private readonly IMediator mediator;
        private readonly FindTicketsController FindTicketsController;
        private readonly IUser user;

        public FindTicketsControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);
            var reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetMoveBetsDay().Returns(DefaultVariablesTest.MoveBetDate);

            FindTicketsController = new FindTicketsController(mediator);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendFindTicketsQuery()
        {
            // Arrange
            var message = new FindTicketsQuery(user);

            // Act
            FindTicketsController.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }
    }
}