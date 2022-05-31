namespace Alpha.BetLists.Site.Controllers
{
    using Alpha.BetLists.Site.Requests.MoveStatementHistory;
    using MediatR;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class MoveStatementHistoryController : BetListBaseController
    {
        public MoveStatementHistoryController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(MoveStatementHistoryQuery message)
            => await View(message);
    }
}