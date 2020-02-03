namespace Alpha.BetLists.Site.Controllers
{
    using Alpha.BetLists.Site.Requests.RiskAnalysisIPMatching;
    using MediatR;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class RiskAnalysisIPMatchingController : BetListBaseController
    {
        public RiskAnalysisIPMatchingController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(RiskAnalysisIPMatchingQuery message)
            => await View(message);
    }
}