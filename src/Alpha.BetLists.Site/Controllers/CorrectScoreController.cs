namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.CorrectScore;
    using ViewModels.CorrectScore;

    public class CorrectScoreController : BetListBaseController
    {
        public CorrectScoreController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(CorrectScoreQuery message)
            => await View<CorrectScoreViewModel>(message);

        public async Task<FileContentResult> Excel(CorrectScoreExcelQuery message)
            => await ExcelView(message);
    }
}
