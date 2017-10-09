namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.CombinationMixParlay;
    using ViewModels.CombinationMixParlay;

    public class CombinationMixParlayController : BetListBaseController
    {
        public CombinationMixParlayController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(CombinationMixParlayQuery message)
            => await View<CombinationMixParlayViewModel>(message);

        public async Task<FileContentResult> Excel(CombinationMixParlayExcelQuery message)
           => await ExcelView(message);
    }
}
