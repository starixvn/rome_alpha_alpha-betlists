namespace Alpha.BetLists.Site.Tests.Controllers
{
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Requests.MixParlay;
    using Site.Controllers;
    using Site.ViewModels;
    using Site.ViewModels.MixParlay;
    using Xunit;

    [Trait("Category", "MixParlay")]
    public class MixParlayControllerTests
    {
        private readonly IUser user;
        private readonly IMediator mediator;
        private readonly MixParlayController controller;

        public MixParlayControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            controller = new MixParlayController(mediator);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendMixParlayQuery()
        {
            // Arrange
            var message = new MixParlayQuery(user);

            // Act
            controller.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send<MixParlayViewModel>(message);
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendMixParlayExcelQuery()
        {
            // Arrange
            var message = new MixParlayExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<MixParlayExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.Excel(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }
    }
}
