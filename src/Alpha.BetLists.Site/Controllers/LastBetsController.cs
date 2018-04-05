namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using BetLists.Constants;
    using BetLists.Models;
    using MediatR;
    using Requests.LastBets;

    public class LastBetsController : BetListBaseController
    {
        private readonly IUser viewer;

        public LastBetsController(IMediator mediator, IUser viewer) : base(mediator)
        {
            this.viewer = viewer;
        }

        public async Task<ActionResult> Index(LastBetsQuery query) => await View(query);

        public async Task<ActionResult> BetData(GetLastBetsQuery message)
        {
            var canViewLastBets = mediator.Send(new CheckCanViewLastBetsQuery(viewer)).GetAwaiter().GetResult();

            if (canViewLastBets.ErrCode != ErrorCodeConstants.None)
            {
                return Json(canViewLastBets, JsonRequestBehavior.AllowGet);
            }

            return await View(message);
        }        
    }
}