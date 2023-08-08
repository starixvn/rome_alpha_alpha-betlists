using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Tests.Constants;

namespace Alpha.BetLists.Site.Tests.Controllers
{
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Requests.SearchTickets;
    using Site.Controllers;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "SearchTickets")]
    public class SearchTicketsControllerTests
    {
        private readonly IMediator mediator;
        private readonly SearchTicketsController SearchTicketsController;
        private readonly IUser user;

        public SearchTicketsControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);
            var reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(DefaultVariablesTest.BalanceUpDate);
            reportDateService.GetMoveBetsDay().Returns(DefaultVariablesTest.MoveBetDate);

            SearchTicketsController = new SearchTicketsController(mediator);
        }

        [Fact]
        public void BetData_Always_CallsMediatorSendSearchTicketsQuery()
        {
            // Arrange
            var message = new SearchTicketsQuery(user);

            // Act
            SearchTicketsController.BetData(message);

            // Assert
            mediator.Received().Send(message);
        }
    }
}