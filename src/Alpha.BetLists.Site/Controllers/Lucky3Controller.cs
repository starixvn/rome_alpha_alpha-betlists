namespace Alpha.BetLists.Site.Controllers
{
    using MediatR;
    using Requests.Lucky3;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class Lucky3Controller : BetListBaseController
    {
        public Lucky3Controller(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> ViewDetail(Lucky3DetailQuery message)
            => await View(message);
    }
}