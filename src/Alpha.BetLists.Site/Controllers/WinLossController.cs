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

        public async Task<ActionResult> IndexNew(WinLossNewQuery message)
            => message.IsValid() ? await View<WinLossNewViewModel>(message) : View(InvalidBetListView);

        public async Task<ActionResult> GetData(WinLossGetDataQuery message)
            => message.IsValid() ? await PartialView(message) : View(InvalidBetListView);
    }
}