namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.RacingByMatch;
    using ViewModels.RacingByMatch;

    public class RacingByMatchController : BetListBaseController
    {
        public RacingByMatchController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(RacingByMatchQuery message)
            => await View<RacingByMatchViewModel>(message);

        public async Task<FileContentResult> Excel(RacingByMatchExcelQuery message)
            => await ExcelView(message);
    }
}
