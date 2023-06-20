namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.BetLists.Site.Requests.MonitoringBets;
    using Alpha.NetThrottling.Mvc;
    using MediatR;

    public class MonitoringBetsController : BetListBaseController
    {
        public MonitoringBetsController(IMediator mediator) : base(mediator)
        {
        }

        [DisableNetThrottling]
        public Task<ActionResult> Index(MonitoringBetsQuery message) => View(message);
    }
}