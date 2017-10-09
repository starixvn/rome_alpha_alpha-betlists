namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.CasioDetails;

    public class CasinoDetailsController : BetListBaseController
    {
        public CasinoDetailsController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(CasinoDetailsQuery message)
            => await View(message);
    }
}
