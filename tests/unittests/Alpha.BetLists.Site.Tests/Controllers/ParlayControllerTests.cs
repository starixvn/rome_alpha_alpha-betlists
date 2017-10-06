namespace Alpha.BetLists.Site.Tests.Controllers
{
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Requests.Parlay;
    using Site.Controllers;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "Parlay")]
    public class ParlayControllerTests
    {
        private readonly IUser user;
        private readonly IMediator mediator;
        private readonly ParlayController controller;

        public ParlayControllerTests()
        {
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);
            mediator = Substitute.For<IMediator>();

            controller = new ParlayController(mediator);
        }

        [Fact]
        public void MixParlay_Always_CallsMediatorSendMixParlayQuery()
        {
            // Arrange
            var message = new MixParlayQuery(user);

            // Act
            controller.MixParlay(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void ComboParlay_Always_CallsMediatorSendComboParlayQuery()
        {
            // Arrange
            var message = new ComboParlayQuery(user);

            // Act
            controller.ComboParlay(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void ComboParlayDetail_Always_CallsMediatorSendComboParlayDetailQuery()
        {
            // Arrange
            var message = new ComboParlayDetailQuery(user);

            // Act
            controller.ComboParlayDetail(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void ComboParlayDetailExcel_Always_CallsMediatorSendComboParlayDetailExcelQuery()
        {
            // Arrange
            var message = new ComboParlayDetailExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<ComboParlayDetailExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.ComboParlayDetailExcel(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(message);
        }
    }
}
