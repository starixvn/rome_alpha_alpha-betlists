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

        public async Task<ActionResult> RNGCasino(RNGCasinoQuery message)
            => await View(message);
    }
}
