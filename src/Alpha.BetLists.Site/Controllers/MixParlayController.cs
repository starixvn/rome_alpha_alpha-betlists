namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.MixParlay;
    using ViewModels.MixParlay;

    public class MixParlayController : BetListBaseController
    {
        public MixParlayController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(MixParlayQuery message)
            => await View<MixParlayViewModel>(message);

        public async Task<FileContentResult> Excel(MixParlayExcelQuery message)
            => await ExcelView(message);
    }
}
