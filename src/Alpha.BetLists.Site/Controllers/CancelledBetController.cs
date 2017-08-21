namespace Alpha.BetLists.Site.Controllers
{
    using Alpha.BetLists.Site.Requests.CancelledBet;
    using MediatR;
    using System.Threading.Tasks;
    using System.Web.Mvc;

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