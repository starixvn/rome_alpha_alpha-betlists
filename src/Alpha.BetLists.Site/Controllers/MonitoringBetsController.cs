namespace Alpha.BetLists.Site.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.BetLists.Configurations;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.Requests.MonitoringBets;
    using Alpha.BetLists.Site.ViewModels.MonitoringBets;
    using Alpha.NetThrottling.Mvc;
    using Alpha.Web.Models;
    using MediatR;

    [SessionState(SessionStateBehavior.Default)]
    public class MonitoringBetsController : BetListBaseController
    {
        private readonly IUser viewer;

        public MonitoringBetsController(IMediator mediator, IUser viewer) : base(mediator)
        {
            this.viewer = viewer;
        }

        [DisableNetThrottling]
        public async Task<ActionResult> Index(MonitoringBetsQuery message)
        {
            var viewModel = await mediator.Send(message);

            if (viewModel.Filters.Any())
            {
                return View(viewModel);
            }

            return RedirectPermanent($"~/{AppSettings.BetlistRouting}MonitoringBets/FilterConfiguration?welcome=true");
        }

        [HttpPost, DisableNetThrottling]
        public Task<ActionResult> BetData(GetLastFilteringBetsQuery message) => View(message);

        public Task<ActionResult> ManageFilters(ManageFiltersQuery message) => View(message);

        [HttpPost]
        public async Task<ActionResult> UpdateFilter(UpdateFilterCommand message)
            => Json(await ValidateAndDoAction(message), JsonRequestBehavior.DenyGet);

        [HttpPost]
        public async Task<ActionResult> DeleteFilters(DeleteFiltersCommand message)
        {
            IEnumerable<Result> results;
            if (message.IsInternal)
            {
                results = new[] { ForbiddenResult() };
            }
            else if (!message.IsValid())
            {
                results = new[] { BadRequestResult() };
            }
            else
            {
                results = await mediator.Send(message);
            }

            return Json(results, JsonRequestBehavior.DenyGet);
        }

        public ActionResult FilterConfiguration(FilterConfigurationQuery message)
            => View(new FilterConfigurationViewModel(message));

        public Task<ActionResult> FilterBySports(FilterBySportsQuery message)
            => Json(message, JsonRequestBehavior.AllowGet);

        public Task<ActionResult> GetMyWatchlistMembers(GetMyWatchlistMembersQuery message)
            => Json(message, JsonRequestBehavior.AllowGet);

        [HttpPost]
        public async Task<ActionResult> ApplyFilter(ApplyFilterCommand message)
            => Json(
                message.IsValid() ? await mediator.Send(message) : BadRequestResult(),
                JsonRequestBehavior.DenyGet);

        [HttpPost]
        public async Task<ActionResult> AddFilter(AddFilterCommand message)
            => Json(await ValidateAndDoAction(message), JsonRequestBehavior.DenyGet);

        [HttpPost]
        public async Task<ActionResult> AddCustomFilter(AddUnsavedFilterCommand message)
            => Json(await ValidateAndDoAction(message), JsonRequestBehavior.DenyGet);

        private async Task<Result> ValidateAndDoAction(AddUnsavedFilterCommand message)
        {
            if (message.IsInternal)
            {
                return ForbiddenResult();
            }

            return message.IsValid()
                ? await mediator.Send(message)
                : BadRequestResult();
        }
    }
}