namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.WinLoss;

    public class WinLossController : BetListBaseController
    {
        public WinLossController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(WinLossQuery message)
            => await View(message);
    }
}