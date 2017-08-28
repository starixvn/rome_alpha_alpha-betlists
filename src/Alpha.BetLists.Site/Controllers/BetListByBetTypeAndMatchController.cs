namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.BetListByBetTypeAndMatch;
    using ViewModels.BetListByBetTypeAndMatch;

    public class BetListByBetTypeAndMatchController : BetListBaseController
    {
        public BetListByBetTypeAndMatchController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(BetListByBetTypeAndMatchQuery message)
            => await View<BetListByBetTypeAndMatchViewModel>(message);

        public async Task<FileContentResult> Excel(BetListByBetTypeAndMatchExcelQuery message)
            => await ExcelView(message);
    }
}
