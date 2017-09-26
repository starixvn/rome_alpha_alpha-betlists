namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.WinLossByMatch;

    public class ParlayWinLossByMatchController : BetListBaseController
    {
        public ParlayWinLossByMatchController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(ParlayWinLossByMatchQuery message)
            => await View(message);

        public async Task<FileContentResult> Excel(ParlayWinLossByMatchExcelQuery message)
            => await ExcelView(message);
    }
}