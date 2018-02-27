namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.JackpotDetails;

    public class JackpotDetailsController : BetListBaseController
    {
        public JackpotDetailsController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(JackpotDetailsQuery message)
            => await View(message);

        public async Task<FileContentResult> Excel(JackpotDetailsExcelQuery message)
            => await ExcelView(message);
    }
}