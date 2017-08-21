namespace Alpha.BetLists.Site.Tests.Controllers
{
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Requests.CorrectScore;
    using Site.Controllers;
    using Site.ViewModels;
    using Site.ViewModels.CorrectScore;
    using Xunit;

    [Trait("Category", "CorrectScore")]
    public class CorrectScoreControllerTests
    {
        private readonly IMediator mediator;
        private readonly IUser user;
        private readonly CorrectScoreController correctScoreController;

        public CorrectScoreControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            correctScoreController = new CorrectScoreController(mediator);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendCorrectScoreQuery()
        {
            // Arrange
            var message = new CorrectScoreQuery(user);

            // Act
            correctScoreController.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send<CorrectScoreViewModel>(message);
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendCorrectScoreExcelQuery()
        {
            // Arrange
            var message = new CorrectScoreExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<CorrectScoreExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            correctScoreController.Excel(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }
    }
}
