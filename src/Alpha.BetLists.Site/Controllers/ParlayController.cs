namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.Parlay;

    public class ParlayController : BetListBaseController
    {
        public ParlayController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> MixParlay(MixParlayQuery message)
            => await PartialView(message);

        public async Task<ActionResult> ComboParlay(ComboParlayQuery message)
            => await PartialView(message);

        public async Task<ActionResult> ComboParlayDetail(ComboParlayDetailQuery message)
            => await View(message);

        public async Task<ActionResult> ComboParlayDetailExcel(ComboParlayDetailExcelQuery message)
            => await ExcelView(message);
    }
}