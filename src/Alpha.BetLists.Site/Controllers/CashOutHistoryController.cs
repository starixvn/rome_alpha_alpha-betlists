namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.BetLists.Site.Requests.CasinoDetails;
    using MediatR;

    public class CashOutHistoryController : BetListBaseController
    {
        public CashOutHistoryController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(CashOutHistoryQuery message)
            => await View(message);
    }
}