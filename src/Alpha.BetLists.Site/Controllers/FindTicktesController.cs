namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.BetLists.Site.Requests.FindTickets;
    using MediatR;

    public class FindTicketsController : BetListBaseController
    {
        public FindTicketsController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(FindTicketsQuery message)
            => await View(message);

        public async Task<ActionResult> BetData(FindTicketsQuery message)
            => await View("WinLossTableData", message);
    }
}