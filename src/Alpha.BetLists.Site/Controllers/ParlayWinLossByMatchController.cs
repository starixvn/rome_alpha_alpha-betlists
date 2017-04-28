using System.Threading.Tasks;
using System.Web.Mvc;
using Alpha.BetLists.Site.Requests.WinLossByMatch;
using MediatR;

namespace Alpha.BetLists.Site.Controllers
{
    public class ParlayWinLossByMatchController : BetListBaseController
    {
        public ParlayWinLossByMatchController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(ParlayWinLossByMatchQuery message)
            => await View(message);
    }
}