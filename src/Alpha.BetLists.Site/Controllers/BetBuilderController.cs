namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.BetBuilder;

    public class BetBuilderController : BetListBaseController
    {
        public BetBuilderController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> ViewDetail(BetBuilderDetailQuery message)
            => await PartialView(message);
    }
}