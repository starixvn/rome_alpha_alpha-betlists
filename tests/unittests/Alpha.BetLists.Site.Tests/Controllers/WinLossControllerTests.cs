using Alpha.BetLists.Models;
using Alpha.BetLists.Site.Controllers;
using Alpha.BetLists.Site.Requests.WinLoss;
using Alpha.BetLists.Site.ViewModels;
using MediatR;
using NSubstitute;
using Xunit;

namespace Alpha.BetLists.Site.Tests.Controllers
{
    [Trait("Category", "WinLoss")]
    public class WinLossControllerTests
    {
        private readonly IMediator mediator;
        private readonly WinLossController winLossController;
        private readonly IUser user;

        public WinLossControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            winLossController = new WinLossController(mediator);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendWinLossQuery()
        {
            // Arrange
            var message = new WinLossQuery(user);

            // Act
            winLossController.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendExportWinLossQuery()
        {
            // Arrange
            var message = new WinLossExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<WinLossExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            winLossController.Excel(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }
    }
}