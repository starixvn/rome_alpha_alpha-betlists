namespace Alpha.BetLists.Site.Controllers
{
    using MediatR;
    using Requests.DailyStatement;
    using System.Threading.Tasks;
    using System.Web.Mvc;

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