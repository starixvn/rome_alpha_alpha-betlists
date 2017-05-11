namespace Alpha.BetLists.Site.Controllers
{
    using MediatR;
    using Requests.Outstanding;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class OutstandingController : BetListBaseController
    {
        public OutstandingController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(OutstandingQuery message)
            => await View(message);
    }
}