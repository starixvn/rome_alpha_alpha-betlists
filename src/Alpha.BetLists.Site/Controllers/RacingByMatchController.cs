namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.RacingByMatch;

    public class RacingByMatchController : BetListBaseController
    {
        public RacingByMatchController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(RacingByMatchQuery message)
            => await View(message);

        public async Task<FileContentResult> Excel(RacingByMatchExcelQuery message)
            => await ExcelView(message);
    }
}
