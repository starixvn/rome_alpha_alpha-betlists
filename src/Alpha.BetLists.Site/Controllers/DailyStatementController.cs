namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.DailyStatement;

    public class DailyStatementController : BetListBaseController
    {
        public DailyStatementController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(DailyStatementQuery message)
            => await View(message);

        public async Task<FileContentResult> Excel(DailyStatementExcelQuery message)
            => await ExcelView(message);
    }
}