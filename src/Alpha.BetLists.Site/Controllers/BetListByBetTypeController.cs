namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.BetListByBetType;

    public class BetListByBetTypeController : BetListBaseController
    {
        private const string viewName = "~/Views/BetListByBetType/Index.cshtml";

        public BetListByBetTypeController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(BetListByBetTypeQuery message)
            => await View(message);

        public async Task<ActionResult> CleanSheet(BetListByBetTypeQuery message)
            => await View(message, viewName);

        public async Task<ActionResult> DoubleChance(BetListByBetTypeQuery message)
            => await View(message, viewName);

        public async Task<ActionResult> BothNoneNeitherTeamToScore(BetListByBetTypeQuery message)
            => await View(message, viewName);

        public async Task<ActionResult> ToWinToNil(BetListByBetTypeQuery message)
            => await View(message, viewName);

        public async Task<ActionResult> ThreeWayHandicap(BetListByBetTypeQuery message)
            => await View(message, viewName);

        public async Task<FileContentResult> Excel(BetListByBetTypeExcelQuery message)
            => await ExcelView(message);

        public async Task<FileContentResult> CleanSheetExcel(BetListByBetTypeExcelQuery message)
            => await ExcelView(message);

        public async Task<FileContentResult> DoubleChanceExcel(BetListByBetTypeExcelQuery message)
            => await ExcelView(message);

        public async Task<FileContentResult> BothNoneNeitherTeamToScoreExcel(BetListByBetTypeExcelQuery message)
            => await ExcelView(message);

        public async Task<FileContentResult> ToWinToNilExcel(BetListByBetTypeExcelQuery message)
            => await ExcelView(message);

        public async Task<FileContentResult> ThreeWayHandicapExcel(BetListByBetTypeExcelQuery message)
            => await ExcelView(message);
    }
}
