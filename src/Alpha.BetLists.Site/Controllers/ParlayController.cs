namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.Parlay;

    public class ParlayController : BetListBaseController
    {
        public ParlayController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> MixParlay(MixParlayQuery message)
            => await View(message);

        public async Task<ActionResult> CombinationMixParlay(CombinationMixParlayQuery message)
            => await View(message);
    }
}