namespace Alpha.BetLists.Site.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.BetLists.Site.Requests.MonitoringBets;
    using Alpha.BetLists.Site.ViewModels.MonitoringBets;
    using Alpha.NetThrottling.Mvc;
    using Alpha.Web.Filters;
    using MediatR;

    public class MonitoringBetsController : BetListBaseController
    {
        public MonitoringBetsController(IMediator mediator) : base(mediator)
        {
        }

        [DisableNetThrottling]
        public async Task<ActionResult> Index(MonitoringBetsQuery message)
        {
            var viewModel = await mediator.Send(message);

            if (viewModel.Filters.Any())
            {
                return View(viewModel);
            }

            return RedirectToAction(nameof(FilterConfiguration), new { welcome = true });
        }

        public Task<ActionResult> ManageFilters(ManageFiltersQuery message) => View(message);

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> UpdateFilter(UpdateFilterCommand message)
            => message.IsInternal ? JsonForbidden() : await Json(message);

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> DeleteFilters(DeleleFiltersCommand message)
            => message.IsInternal ? JsonForbidden() : await Json(message);

        public ActionResult FilterConfiguration(bool welcome = false)
            => View(new FilterConfigurationViewModel(welcome));

        public Task<ActionResult> FilterBySports(FilterBySportsQuery message)
            => Json(message, JsonRequestBehavior.AllowGet);

        [HttpPost]
        public Task<ActionResult> CheckUsernames(CheckUsernamesQuery message) => Json(message);

        public Task<ActionResult> GetMonitoredWatchlistMembers(GetMonitoredWatchlistMembersQuery message)
            => Json(message, JsonRequestBehavior.AllowGet);

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> AddFilter(AddFilterCommand message)
            => message.IsInternal ? JsonForbidden() : await Json(message);
    }
}