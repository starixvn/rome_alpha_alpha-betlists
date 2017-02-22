namespace Alpha.BetLists.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.WinLoss;

    public class WinLossController : BetListBaseController
    {
        public WinLossController(IMediator mediator) : base(mediator)
        {
        }

        public ActionResult Index(WinLossQuery message)
            => View(message);
    }
}