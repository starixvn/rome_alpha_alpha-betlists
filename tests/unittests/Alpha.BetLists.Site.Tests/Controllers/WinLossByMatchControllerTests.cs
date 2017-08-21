namespace Alpha.BetLists.Site.Tests.Controllers
{
    using BetLists.Models;
    using MediatR;
    using NSubstitute;
    using Requests.WinLossByMatch;
    using Site.Controllers;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "WinLossByMatch")]
    public class WinLossByMatchControllerTests
    {
        private readonly IMediator mediator;
        private readonly WinLossByMatchController winLossController;
        private readonly IUser user;

        public WinLossByMatchControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            winLossController = new WinLossByMatchController(mediator);
        }
       
        [Fact]
        public void Index_Always_CallsMediatorSendWinLossByMatchQuery()
        {
            // Arrange
            var message = new WinLossByMatchQuery(user);

            // Act
            winLossController.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendExportWinLossByMatchQuery()
        {
            // Arrange
            var message = new WinLossByMatchExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<WinLossByMatchExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            winLossController.Excel(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }
    }
}