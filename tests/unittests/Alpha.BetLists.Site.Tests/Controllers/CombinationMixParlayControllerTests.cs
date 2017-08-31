namespace Alpha.BetLists.Site.Tests.Controllers
{
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Requests.CombinationMixParlay;
    using Site.Controllers;
    using Site.ViewModels;
    using Site.ViewModels.CombinationMixParlay;
    using Xunit;

    [Trait("Category", "CombinationMixParlay")]
    public class CombinationMixParlayControllerTests
    {
        private readonly IUser user;
        private readonly IMediator mediator;
        private readonly CombinationMixParlayController controller;

        public CombinationMixParlayControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            controller = new CombinationMixParlayController(mediator);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendCombinationMixParlayQuery()
        {
            // Arrange
            var message = new CombinationMixParlayQuery(user);

            // Act
            controller.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send<CombinationMixParlayViewModel>(message);
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendCombinationMixParlayExcelQuery()
        {
            // Arrange
            var message = new CombinationMixParlayExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<CombinationMixParlayExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.Excel(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }
    }
}
