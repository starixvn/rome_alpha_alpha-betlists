using Alpha.BetLists.Models;
using Alpha.BetLists.Site.Controllers;
using Alpha.BetLists.Site.Requests.WinLossByMatch;
using Alpha.BetLists.Site.ViewModels;
using MediatR;
using NSubstitute;
using Xunit;

namespace Alpha.BetLists.Site.Tests.Controllers
{
    [Trait("Category", "ParlayWinLossByMatch")]
    public class ParlayWinLossByMatchControllerTests
    {
        private readonly IMediator mediator;
        private readonly ParlayWinLossByMatchController winLossController;
        private readonly IUser user;

        public ParlayWinLossByMatchControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            winLossController = new ParlayWinLossByMatchController(mediator);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendParlayWinLossByMatchQuery()
        {
            // Arrange
            var message = new ParlayWinLossByMatchQuery(user);

            // Act
            winLossController.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendExportParlayWinLossByMatchQuery()
        {
            // Arrange
            var message = new ParlayWinLossByMatchExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<ParlayWinLossByMatchExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            winLossController.Excel(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }
    }
}
