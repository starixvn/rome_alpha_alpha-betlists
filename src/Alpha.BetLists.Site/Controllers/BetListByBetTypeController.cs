namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.BetListByBetType;

    public class BetListByBetTypeController : BetListBaseController
    {
        public BetListByBetTypeController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(BetListByBetTypeQuery message)
            => await View(message);

        public async Task<FileContentResult> Excel(BetListByBetTypeExcelQuery message)
            => await ExcelView(message);
    }
}
