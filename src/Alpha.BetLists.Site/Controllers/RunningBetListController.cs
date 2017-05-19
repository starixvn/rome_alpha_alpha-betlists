namespace Alpha.BetLists.Site.Controllers
{
    using MediatR;
    using Requests.RunningBetList;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class RunningBetListController : BetListBaseController
    {
        public RunningBetListController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(RunningBetListQuery message)
            => await View(message);
    }
}