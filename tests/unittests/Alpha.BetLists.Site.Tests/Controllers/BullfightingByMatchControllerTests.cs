namespace Alpha.BetLists.Site.Tests.Controllers
{
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Requests.BullfightingByMatch;
    using Site.Controllers;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "BullfightingByMatch")]
    public class BullfightingByMatchControllerTests
    {
        private readonly IUser user;
        private readonly IMediator mediator;
        private readonly BullfightingByMatchController controller;

        public BullfightingByMatchControllerTests()
        {
            user = Substitute.For<IUser>();
            mediator = Substitute.For<IMediator>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            controller = new BullfightingByMatchController(mediator, user);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendBetListByBetTypeQuery()
        {
            // Arrange
            var message = new BullfightingByMatchQuery(user);

            // Act
            controller.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendBetListByBetTypeExcelQuery()
        {
            // Arrange
            var message = new BullfightingByMatchExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<BullfightingByMatchExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.Excel(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }
    }
}
