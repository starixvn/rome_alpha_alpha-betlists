namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests.MemberBetList;

    public class MemberBetListController : BetListBaseController
    {
        public MemberBetListController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Index(MemberBetListQuery message)
            => await View(message);
    }
}