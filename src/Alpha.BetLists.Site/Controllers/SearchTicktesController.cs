namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.BetLists.Site.Requests.SearchTickets;
    using Alpha.BetLists.Site.ViewModels.SearchTickets;
    using MediatR;

    public class SearchTicketsController : BetListBaseController
    {
        public SearchTicketsController(IMediator mediator)
            : base(mediator)
        {
        }

        public ActionResult Index(SearchTicketsQuery message)
            => View(new SearchTicketsViewModel(message, null, null, null));

        public async Task<ActionResult> BetData(SearchTicketsQuery message)
            => await View("WinLossTableData", message);
    }
}