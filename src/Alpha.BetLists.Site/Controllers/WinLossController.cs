namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.BetLists.Site.ViewModels.WinLoss;
    using MediatR;
    using Requests.WinLoss;

    public class WinLossController : BetListBaseController
    {
        public WinLossController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(WinLossQuery message)
            => await View(message);

        public async Task<FileContentResult> Excel(WinLossExcelQuery message)
            => await ExcelView(message);

        public async Task<ActionResult> IndexNew(WinLossIndexQuery message)
            => message.IsValid() ? await View<WinLossIndexViewModel>(message) : View(InvalidBetListView);

        public async Task<ActionResult> BetListWeekSelection(WinLossBetListWeekSelectionQuery message)
            => message.IsValid() ? await PartialView<WinLossBetListWeekSelectionViewModel>(message) : View(InvalidBetListView);

        public async Task<ActionResult> BetList(WinLossBetListQuery message)
            => message.IsValid() ? await PartialView(message) : View(InvalidBetListView);
    }
}