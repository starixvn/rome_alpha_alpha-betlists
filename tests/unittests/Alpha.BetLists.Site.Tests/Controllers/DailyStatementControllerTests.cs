namespace Alpha.BetLists.Site.Tests.Controllers
{
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Requests.DailyStatement;
    using Site.Controllers;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "DailyStatement")]
    public class DailyStatementControllerTests
    {
        private readonly IUser user;
        private readonly IMediator mediator;
        private readonly DailyStatementController dailyStatementController;

        public DailyStatementControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            dailyStatementController = new DailyStatementController(mediator);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendDailyStatementQuery()
        {
            // Arrange
            var message = new DailyStatementQuery(user);

            // Act
            dailyStatementController.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendExportDailyStatementExcelQuery()
        {
            // Arrange
            var message = new DailyStatementExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<DailyStatementExcelQuery>())
                .Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            dailyStatementController.Excel(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }
    }
}
