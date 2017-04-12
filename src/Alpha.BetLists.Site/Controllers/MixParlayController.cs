namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.Parlay;

    public class MixParlayController : BetListBaseController
    {
        public MixParlayController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(MixParlayQuery message)
            => await View(message);
    }
}