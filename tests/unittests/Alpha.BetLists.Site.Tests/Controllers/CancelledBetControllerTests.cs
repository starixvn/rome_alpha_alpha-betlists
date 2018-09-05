namespace Alpha.BetLists.Site.Tests.Controllers
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.Controllers;
    using Alpha.BetLists.Site.Requests.CancelledBet;
    using Alpha.BetLists.Site.ViewModels;
    using Constants;
    using MediatR;
    using NSubstitute;
    using Services;
    using Xunit;

    [Trait("Category", "CancelledBet")]
    public class CancelledBetControllerTests
    {
        private readonly IMediator mediator;
        private readonly IUser user;
        private readonly CancelledBetController cancelledBetController;

        public CancelledBetControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            cancelledBetController = new CancelledBetController(mediator);

            var reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(DefaultVariablesTest.BalanceUpDate);
        }

        [Fact]
        public void Index_Always_CallMediatorToSendQuery()
        {
            //// Arrange
            var query = new CancelledBetQuery(user);

            //// Act
            cancelledBetController.Index(query).GetAwaiter().GetResult();

            //// Assert
            mediator.Received().Send(query);
        }

        [Fact]
        public void Excel_Always_CallMediatorToSendQuery()
        {
            //// Arrange
            var query = new CancelledBetExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<CancelledBetExcelQuery>()).Returns(
                new ExcelFileViewModel("Excel File", new byte[0]));

            //// Act
            cancelledBetController.Excel(query).GetAwaiter().GetResult();

            //// Assert
            mediator.Received().Send<ExcelFileViewModel>(query);
        }
    }
}
