namespace Alpha.BetLists.Site.Tests.Controllers
{
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Requests.JackpotDetails;
    using Site.Controllers;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "JackpotDetails")]
    public class JackpotDetailsControllerTests
    {
        private readonly IUser user;
        private readonly IMediator mediator;
        private readonly JackpotDetailsController controller;

        public JackpotDetailsControllerTests()
        {
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            mediator = Substitute.For<IMediator>();
            controller = new JackpotDetailsController(mediator);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendJackpotDetailsQuery()
        {
            // Arrange
            var query = new JackpotDetailsQuery(user);

            // Act
            controller.Index(query).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(query);
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendJackpotDetailsExcelQuery()
        {
            // Arrange
            var message = new JackpotDetailsExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<JackpotDetailsExcelQuery>())
                .Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.Excel(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(message);
        }
    }
}