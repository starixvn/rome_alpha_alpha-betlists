namespace Alpha.BetLists.Site.Features.BookedBet
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.BetLists.Site.Controllers;
    using MediatR;

    public class BookedBetController : BetListBaseController
    {
        private const string BasePath = "~/Features/BookedBet/Views";
        public BookedBetController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(BookedBetQuery message)
            => await View($"{BasePath}/Index.cshtml", message);

        public async Task<ActionResult> GetData(BookedBetDataQuery message)
            => await View($"{BasePath}/BetList.cshtml", message);

        public async Task<FileContentResult> Excel(BookedBetExcelQuery message)
            => await ExcelView(message);
    }
}