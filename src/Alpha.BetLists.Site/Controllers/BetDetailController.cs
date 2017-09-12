namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.BetDetail;

    public class BetDetailController : BetListBaseController
    {
        public BetDetailController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> RNGCasino(RNGCasinoDetailQuery message)
            => await View(message);

        public async Task<ActionResult> AllBet(AllbetDetailQuery message)
            => await View(message);
    }
}
