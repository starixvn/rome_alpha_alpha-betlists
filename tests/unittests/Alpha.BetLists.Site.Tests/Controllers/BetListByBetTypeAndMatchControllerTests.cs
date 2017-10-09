namespace Alpha.BetLists.Site.Tests.Controllers
{
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Requests.BetListByBetTypeAndMatch;
    using Site.Controllers;
    using Site.ViewModels;
    using Site.ViewModels.BetListByBetTypeAndMatch;
    using Xunit;

    [Trait("Category", "BetListByBetTypeAndMatch")]
    public class BetListByBetTypeAndMatchControllerTests
    {
        private readonly IUser user;
        private readonly IMediator mediator;
        private readonly BetListByBetTypeAndMatchController controller;

        public BetListByBetTypeAndMatchControllerTests()
        {
            user = Substitute.For<IUser>();
            mediator = Substitute.For<IMediator>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            controller = new BetListByBetTypeAndMatchController(mediator);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendBetListByBetTypeAndMatchQuery()
        {
            // Arrange
            var message = new BetListByBetTypeAndMatchQuery(user);

            // Act
            controller.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send<BetListByBetTypeAndMatchViewModel>(message);
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendBetListByBetTypeAndMatchExcelQuery()
        {
            // Arrange
            var message = new BetListByBetTypeAndMatchExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<BetListByBetTypeAndMatchExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.Excel(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }
    }
}
