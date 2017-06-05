using Alpha.BetLists.Site.Requests.CancelledBet;
using MediatR;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Alpha.BetLists.Site.Controllers
{
    public class CancelledBetController : BetListBaseController
    {
        public CancelledBetController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(CancelledBetQuery message)
            => await View(message);
    }
}