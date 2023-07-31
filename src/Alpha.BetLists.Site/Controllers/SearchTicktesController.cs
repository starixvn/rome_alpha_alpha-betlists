namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.BetLists.Site.Requests.SearchTickets;
    using MediatR;

    public class SearchTicketsController : BetListBaseController
    {
        public SearchTicketsController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(SearchTicketsQuery message)
            => await View(message);

        public async Task<ActionResult> BetData(SearchTicketsQuery message)
            => await View("WinLossTableData", message);
    }
}