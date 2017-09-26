namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.BetLists.Site.Requests.CancelledBet;
    using MediatR;

    public class CancelledBetController : BetListBaseController
    {
        public CancelledBetController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(CancelledBetQuery message)
            => await View(message);

        public async Task<FileContentResult> Excel(CancelledBetExcelQuery message)
            => await ExcelView(message);
    }
}