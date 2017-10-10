namespace Alpha.BetLists.Site.Tests.Controllers
{
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Requests.CasinoDetails;
    using Site.Controllers;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "CasinoDetails")]
    public class CasinoDetailsControllerTests
    {
        private readonly IUser user;
        private readonly IMediator mediator;
        private readonly CasinoDetailsController controller;

        public CasinoDetailsControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            controller = new CasinoDetailsController(mediator);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendCasinoDetailsQuery()
        {
            // Arrange
            var message = new CasinoDetailsQuery(user);

            // Act
            controller.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendCasinoDetailsExcelQuery()
        {
            // Arrange
            var message = new CasinoDetailsExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<CasinoDetailsExcelQuery>())
                .Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.Excel(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(message);
        }
    }
}
