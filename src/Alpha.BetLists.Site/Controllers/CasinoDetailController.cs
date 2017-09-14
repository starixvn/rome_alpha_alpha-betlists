namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.CasioDetail;

    public class CasinoDetailController : BetListBaseController
    {
        public CasinoDetailController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(CasioDetailQuery message)
            => await View(message);
    }
}
