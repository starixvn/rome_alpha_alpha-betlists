namespace Alpha.BetLists.Site.Tests.Controllers
{
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Requests.RacingByMatch;
    using Site.Controllers;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "RacingByMatch")]
    public class RacingByMatchControllerTests
    {
        private readonly IUser user;
        private readonly IMediator mediator;
        private readonly RacingByMatchController controller;

        public RacingByMatchControllerTests()
        {
            user = Substitute.For<IUser>();
            mediator = Substitute.For<IMediator>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            controller = new RacingByMatchController(mediator);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendBetListByBetTypeQuery()
        {
            // Arrange
            var message = new RacingByMatchQuery(user);

            // Act
            controller.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendBetListByBetTypeExcelQuery()
        {
            // Arrange
            var message = new RacingByMatchExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<RacingByMatchExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.Excel(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }
    }
}
