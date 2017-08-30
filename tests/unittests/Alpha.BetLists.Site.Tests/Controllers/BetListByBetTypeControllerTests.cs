namespace Alpha.BetLists.Site.Tests.Controllers
{
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Requests.BetListByBetType;
    using Site.Controllers;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "BetListByBetType")]
    public class BetListByBetTypeControllerTests
    {
        private readonly IUser user;
        private readonly IMediator mediator;
        private readonly BetListByBetTypeController betListByBetTypeController;

        public BetListByBetTypeControllerTests()
        {
            user = Substitute.For<IUser>();
            mediator = Substitute.For<IMediator>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            betListByBetTypeController = new BetListByBetTypeController(mediator);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendBetListByBetTypeQuery()
        {
            // Arrange
            var message = new BetListByBetTypeQuery(user);

            // Act
            betListByBetTypeController.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendBetListByBetTypeExcelQuery()
        {
            // Arrange
            var message = new BetListByBetTypeExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<BetListByBetTypeExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            betListByBetTypeController.Excel(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }
    }
}
