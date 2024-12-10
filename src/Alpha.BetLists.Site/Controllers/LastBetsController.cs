namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.NetThrottling.Mvc;
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

        [DisableNetThrottling]
        public async Task<ActionResult> Indexxxxxxx(LastBetsQuery query) => await View("Index", query);

        [DisableNetThrottling]
        public async Task<ActionResult> BetDataaaaaaa(GetLastBetsQuery message)
        {
            var canViewLastBets = await mediator.Send(new CheckCanViewLastBetsQuery(viewer));

            if (canViewLastBets.ErrCode != ErrorCodeConstants.None)
            {
                return Json(canViewLastBets, JsonRequestBehavior.AllowGet);
            }

            return await View("BetData", message);
        }
    }
}