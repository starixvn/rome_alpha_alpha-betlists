namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.WinLossByMatch;

    public class WinLossByMatchController : BetListBaseController
    {
        public WinLossByMatchController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(WinLossByMatchQuery message)
            => await View(message);

        public async Task<FileContentResult> Excel(WinLossByMatchExcelQuery message)
            => await ExcelView(message);
    }
}